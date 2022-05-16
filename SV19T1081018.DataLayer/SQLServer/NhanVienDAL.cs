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
        public NhanVienDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(NhanVien data)
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
                                        or(TenNhanVien like @searchValue)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from NhanVien where MaNhanVien = @manhanvien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("manhanvien", id);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public NhanVien Get(int id)
        {
            NhanVien data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from NhanVien where MaNhanVien = @manhanvien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("manhanvien", id);

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
                        SoDienThoai = Convert.ToString(dbReader["SoDienThoai"]),
                        isNam = Convert.ToBoolean(dbReader["isNam"]),
                        NgayVaoLam = Convert.ToDateTime(dbReader["NgayVaoLam"])

                    };
                }

                cn.Close();
            }

            return data;
        }

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

        public List<NhanVien> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<NhanVien> data = new List<NhanVien>();

            string searchValueEmail = searchValue;
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"declare @searchValue nvarchar(MAX), @pageSize int, @page int
                                set @searchValue = @value
                                set @pageSize = @ps
                                set @page = @p
                                select *
                                from
                                (
	                                 select *, row_number() over(order by MaNhanVien) as RowNumber
	                                from    NhanVien
	                                where  (@searchValue  like N'') or (TenNhanVien like @searchValue )) as t
                                where (@pageSize) = 0 or (t.RowNumber between(@page -1) *@pageSize + 1 and @page *@pageSize)
                                order by t.RowNumber";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@p", page);
                cmd.Parameters.AddWithValue("@ps", pageSize);
                cmd.Parameters.AddWithValue("@value", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new NhanVien()
                    {
                        MaNhanVien = Convert.ToInt32(result["MaNhanVien"]),
                        TenNhanVien = Convert.ToString(result["TenNhanVien"]),
                        MaChucVu = Convert.ToInt32(result["MaChucVu"]),
                        SoDienThoai = Convert.ToString(result["SoDienThoai"]),
                        Anh = Convert.ToString(result["Anh"]),
                        NgaySinh = Convert.ToDateTime(result["NgaySinh"]),
                        GhiChu = Convert.ToString(result["GhiChu"]),
                        isNam = Convert.ToBoolean(result["isNam"]),
                        NgayVaoLam = Convert.ToDateTime(result["NgayVaoLam"])
                    });
                }

                cn.Close();
            }
            return data;
        }

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
                cmd.Parameters.AddWithValue("@sodienthoai", data.SoDienThoai);
                cmd.Parameters.AddWithValue("@isNam", data.isNam);

                cmd.Parameters.AddWithValue("@ngayvaolam", data.NgayVaoLam);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
