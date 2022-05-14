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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public NhanVien Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
