using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class LuongDAL : _BaseDAL, ILuongDAL
    {
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public LuongDAL(string connectionString) : base(connectionString)
        {

        }



        public int Count(string searchValue, int Thang = 0, int Nam = 0)
        {
            int count = 0;

            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }

            using (SqlConnection cn = OpenConnection())
            {
                //Tạo command
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from Luong as l
                                    right join NhanVien as nv on l.MaNhanVien=nv.MaNhanVien
                                    where ((@thang = 0 ) or ( l.thang = @thang ))
		                                            and ((@nam = 0 ) or ( l.nam = @nam ))
		                                            and ((@searchValue = '' ) or ( nv.TenNhanVien like @searchValue))";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@thang", Thang);
                cmd.Parameters.AddWithValue("@nam", Nam);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return count;
        }
        /// <summary>
        /// phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplieriD"></param>
        /// <returns></returns>
        public IList<Luong> List(int page = 1, int pageSize = 10, string searchValue = "", int Thang = 0, int Nam = 0)
        {
            List<Luong> data = new List<Luong>();

            if (searchValue != "")
            {
                searchValue = "%" + searchValue + "%";
            }

            List<int> ListMaNhanVien = new List<int>();
            // Tạo và mở kết nối
            using (SqlConnection cn = OpenConnection())
            {
                //Tạo command
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"select * from NhanVien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    ListMaNhanVien.Add(Convert.ToInt32(result["MaNhanVien"]));
                }
                cn.Close();
            }
            using (SqlConnection cn = OpenConnection())
            {


                foreach (var i in ListMaNhanVien)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = @"declare @thang int, @nam int, @manv int
                                    set @manv = @MaNhanVien
                                    set @thang = @ThangInput
                                    set @nam = @NamInput

                                    if (select sum(DATEDIFF ( HOUR , ThoiGianVaoCa,ThoiGianKetThuc)) as temp
                                    from Cong where MaNhanVien = @manv and MONTH(Ngay) = @thang and YEAR(Ngay) = @nam) is null 
                                    begin
	                                    if (select TongLuong from Luong where MaNhanVien = @manv and Thang = @thang and Nam = @nam) is null
	                                    begin
		                                    if ((select MONTH(NgayVaoLam) from NhanVien where MaNhanVien = @manv) <= @thang) and ((select Year(NgayVaoLam) from NhanVien where MaNhanVien = @manv) <= @nam)
		                                    begin
			                                    insert into Luong (MaNhanVien, Nam, Thang, TongLuong, DaNhan) values (@manv, @nam, @thang, 0, 0)
		                                    end
	                                    end
                                    end
                                    else
                                    begin
	                                    if not exists (select * from Luong where MaNhanVien = @manv and Thang = @thang and Nam = @nam)
	                                    begin
		                                    insert into Luong (MaNhanVien, Nam, Thang, TongLuong, DaNhan) values (@manv, @nam, @thang, 0, 0)
	                                    end
	                                    else
	                                    begin
		                                    update Luong set TongLuong = (select sum(DATEDIFF ( HOUR , ThoiGianVaoCa,ThoiGianKetThuc)) as temp
                                    from Cong where MaNhanVien = @manv and MONTH(Ngay) = @thang and YEAR(Ngay) = @nam)  * (select cv.TienLuong from NhanVien as nv join ChucVu as cv on nv.MaChucVu = cv.MaChucVu where nv.MaNhanVien = @manv)
		                                    + (select isnull(sum(SoTienPhat),0) from TienThuongPhat as ttp join Luong as l on l.MaLuong = ttp.MaLuong where l.Thang = @thang and l.Nam = @nam and l.MaNhanVien = @manv)
		                                    where MaNhanVien = @manv
	                                    end
                                    end";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = cn;

                    cmd.Parameters.AddWithValue("MaNhanVien", i);
                    cmd.Parameters.AddWithValue("ThangInput", Thang);
                    cmd.Parameters.AddWithValue("NamInput", Nam);
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                           select l.MaLuong , nv.MaNhanVien , nv.TenNhanVien ,l.Thang,l.Nam,l.TongLuong,l.DaNhan,
                                          row_number() over (order by nv.TenNhanVien) as RowNumber
                                            from Luong as l
                                            right join NhanVien as nv on l.MaNhanVien=nv.MaNhanVien
                                                where ((@thang = 0 ) or ( l.thang = @thang ))
                                          and ((@nam = 0 ) or ( l.nam = @nam ))
                                          and ((@searchValue = '' ) or ( nv.TenNhanVien like @searchValue))
                                        ) as t
                                    where (@pageSize = 0) or (t.RowNumber between(@page - 1) * @pageSize + 1 and @page * @pageSize)
                                    order by t.RowNumber; ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@thang", Thang);
                cmd.Parameters.AddWithValue("@nam", Nam);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Luong()
                    {
                        MaLuong = Convert.ToInt32(result["MaLuong"]),
                        MaNhanVien = Convert.ToInt32(result["MaNhanVien"]),
                        TenNhanVien = Convert.ToString(result["TenNhanVien"]),
                        Thang = Convert.ToInt32(result["Thang"]),
                        Nam = Convert.ToInt32(result["Nam"]),
                        TongLuong = Convert.ToInt32(result["TongLuong"]),
                        DaNhan = Convert.ToBoolean(result["DaNhan"])
                    });
                }
                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaLuong"></param>
        /// <returns></returns>
        public bool TraLuong(int MaLuong)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Luong set DaNhan=1 where MaLuong = @maLuong";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@maLuong", MaLuong);

                result = cmd.ExecuteNonQuery() > 0;



                cn.Close();
            }

            return result;
        }
        public List<TienThuongPhat> GetThuongPhat(int MaLuong)
        {
            List<TienThuongPhat> data = new List<TienThuongPhat>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from TienThuongPhat where MaLuong = @MaLuong";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("MaLuong", MaLuong);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new TienThuongPhat()
                    {
                        MaTienThuongPhat = Convert.ToInt32(result["MaTienThuongPhat"]),
                        LyDo = Convert.ToString(result["LyDo"]),
                        SoTienPhat = Convert.ToInt32(result["SoTienPhat"]),
                        MaLuong = Convert.ToInt32(result["MaLuong"])


                    });
                }
                cn.Close();
            }
            return data;
        }

        public TienThuongPhat Get(int MaTienThuongPhat)
        {
            TienThuongPhat data = new TienThuongPhat();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from TienThuongPhat where MaTienThuongPhat = @MaTienThuongPhat";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("MaTienThuongPhat", MaTienThuongPhat);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data = new TienThuongPhat()
                    {
                        MaTienThuongPhat = Convert.ToInt32(result["MaTienThuongPhat"]),
                        LyDo = Convert.ToString(result["LyDo"]),
                        SoTienPhat = Convert.ToInt32(result["SoTienPhat"]),
                        MaLuong = Convert.ToInt32(result["MaLuong"])


                    };
                }
                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(TienThuongPhat data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update TienThuongPhat
                                set LyDo=@LyDo,
                                SoTienPhat=@SoTienPhat,
                                MaLuong=@MaLuong
                                where MaTienThuongPhat=@MaTienThuongPhat";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@LyDo", data.LyDo);
                cmd.Parameters.AddWithValue("@SoTienPhat", data.SoTienPhat);
                cmd.Parameters.AddWithValue("@MaLuong", data.MaLuong);
                cmd.Parameters.AddWithValue("@MaTienThuongPhat", data.MaTienThuongPhat);

                result = cmd.ExecuteNonQuery() > 0;



                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(TienThuongPhat data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @" insert into TienThuongPhat(LyDo,SoTienPhat,MaLuong) 
                                     values(@LyDo,@SoTienPhat,@MaLuong)
                                        select @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@LyDo", data.LyDo);
                cmd.Parameters.AddWithValue("@SoTienPhat", data.SoTienPhat);
                cmd.Parameters.AddWithValue("@MaLuong", data.MaLuong);

                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        public bool Delete(int MaTienThuongPhat)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"delete from TienThuongPhat where MaTienThuongPhat = @MaTienThuongPhat";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaTienThuongPhat", MaTienThuongPhat);

                result = cmd.ExecuteNonQuery() > 0;



                cn.Close();
            }

            return result;
        }
    }

}
