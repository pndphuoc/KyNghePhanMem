using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class NhanVienDAL : _BaseDAL, ICommonDAL<NhanVien>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public NhanVienDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// Thêm Nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int  Add(NhanVien data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into NhanVien(TenNhanVien,NgaySinh,Anh,GhiChu,SoDienThoai,isNam,NgayVaoLam,MaChucVu) values
                    (@tennhanvien, @ngaysinh, @anh,@ghichu, @sodienthoai,@isnam, @ngayvaolam,@machucvu) 
                    select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@tennhanvien", data.TenNhanVien);  
                cmd.Parameters.AddWithValue("@ngaysinh", data.NgaySinh);
                cmd.Parameters.AddWithValue("@anh", data.Anh);
                cmd.Parameters.AddWithValue("@ghichu", data.GhiChu);
                cmd.Parameters.AddWithValue("@sodienthoai", data.SoDienThoai);
                cmd.Parameters.AddWithValue("@ngayvaolam", data.NgayVaoLam);
                cmd.Parameters.AddWithValue("@machucvu", data.MaChucVu);
                cmd.Parameters.AddWithValue("@isnam", data.isNam);
                result = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// đếm số lượng nhân viên
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue = "")
        {
            int count = 0;

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from NhanVien
                                    where (@searchValue = N'')
                                        or(
                                                (TenNhanVien like @searchValue)
                                                or
                                                (SoDienThoai like @searchValue)
                                               
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
        /// <summary>
        /// Xoá một nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool Delete(int MaNhanVien)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from NhanVien where MaNhanVien = @manhanvien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("manhanvien", MaNhanVien);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// lấy một nhân viên khi click vào 'chỉnh sửa'
        /// </summary>
        public NhanVien Get(int MaNhanVien)
        {
            NhanVien data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from NhanVien where MaNhanVien = @manhanvien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("manhanvien", MaNhanVien);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new NhanVien()
                    {
                        MaNhanVien = Convert.ToInt32(dbReader["MaNhanVien"]),
                        TenNhanVien = Convert.ToString(dbReader["TenNhanVien"]),
                        NgaySinh = Convert.ToDateTime(dbReader["NgaySinh"]),
                        MaChucVu = Convert.ToInt32(dbReader["MaChucVu"]),
                        Anh = Convert.ToString(dbReader["Anh"]),
                        GhiChu = Convert.ToString(dbReader["GhiChu"]),
                        SoDienThoai= Convert.ToString(dbReader["SoDienThoai"]),
                        isNam = Convert.ToBoolean(dbReader["isNam"]),
                        NgayVaoLam = Convert.ToDateTime(dbReader["NgayVaoLam"])

                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>


        /// <summary>
        // kiểm tra xem nhân viên chưa được nhận lương thì không được xoá .
        //kiểm tra xem thời gian vào ca lớn hơn hiện tại thì không được xoá nhân viên vì nhân viên này đã đăng kí ca làm.
        /// </summary>
        public bool InUsed(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
              
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select case 
                                    when (exists(select * from Cong where MaNhanVien= @manhanvien)) or (exists (select * from Luong where MaNhanVien=@manhanvien)) then 1 else 0 end";

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@manhanvien", id);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
            }

            return result;
        }


        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        public IList<NhanVien> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<NhanVien> data = new List<NhanVien>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                from
                                    (
                                        select    *,
                                                row_number() over(order by MaNhanVien) as RowNumber
                                        from    NhanVien
                                        where  (@searchValue like N'') or (TenNhanVien like @searchValue)               
                                    ) as t
                                where  (@pagesize=0) or   t.RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize
                                order by t.RowNumber;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new NhanVien()
                    {
                        MaNhanVien = Convert.ToInt32(result["MaNhanVien"]),
                        TenNhanVien = Convert.ToString(result["TenNhanVien"]),
                        NgaySinh = Convert.ToDateTime(result["NgaySinh"]),
                        MaChucVu = Convert.ToInt32(result["MaChucVu"]),
                        Anh = Convert.ToString(result["Anh"]),
                        GhiChu = Convert.ToString(result["GhiChu"]),
                        SoDienThoai = Convert.ToString(result["SoDienThoai"]),
                        isNam = Convert.ToBoolean(result["isNam"]),
                        NgayVaoLam = Convert.ToDateTime(result["NgayvaoLam"]),

                    });
                }

                cn.Close();
            }
            return data;
        }

        public IList<NhanVien> List()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Cập nhật nhân viên 
        /// </summary>
        public bool Update(NhanVien data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update NhanVien
                    set TenNhanVien = @tennhanvien, NgaySinh = @ngaysinh, Anh = @anh,
                    GhiChu = @ghichu, MaChucVu = @machucvu, SoDienThoai = @sodienthoai,isNam=@isnam,NgayvaoLam=@ngayvaolam
                    where MaNhanVien = @manhanvien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@manhanvien", data.MaNhanVien);
                cmd.Parameters.AddWithValue("@tennhanvien", data.TenNhanVien);
                cmd.Parameters.AddWithValue("@ngaysinh", data.NgaySinh);
                cmd.Parameters.AddWithValue("@anh", data.Anh);
                cmd.Parameters.AddWithValue("@ghichu", data.GhiChu);
                cmd.Parameters.AddWithValue("@machucvu", data.MaChucVu);
                cmd.Parameters.AddWithValue("@sodienthoai", data.MaChucVu);
                cmd.Parameters.AddWithValue("@isNam", data.isNam);

                cmd.Parameters.AddWithValue("@ngayvaolam", data.NgayVaoLam);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }

      



      
    }
}
