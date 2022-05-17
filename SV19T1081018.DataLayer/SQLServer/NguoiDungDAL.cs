using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class NguoiDungDAL : _BaseDAL, INguoiDungDAL
    {
        public NguoiDungDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(NguoiDung data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into NguoiDung (HoTen, Username, Password, isChuCuaHang, Anh) values (@HoTen, @Username, @Password, @isChuCuaHang, @Anh)";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@HoTen", data.HoTen);
                cmd.Parameters.AddWithValue("@isChuCuaHang", data.isChuCuaHang);
                cmd.Parameters.AddWithValue("@Anh", data.Anh);
                cmd.Parameters.AddWithValue("@Username", data.Username);
                cmd.Parameters.AddWithValue("@Password", data.Password);

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
                                    from NguoiDung
                                    where (@searchValue = N'')
                                        or(HoTen like @searchValue)";
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
                cmd.CommandText = "delete from NguoiDung where MaNguoiDung = @MaNguoiDung";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaNguoiDung", id);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public NguoiDung Get(int id)
        {
            NguoiDung data = new NguoiDung();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from NguoiDung where MaNguoiDung = @MaNguoiDung";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaNguoiDung", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new NguoiDung()
                    {
                        MaNguoiDung = Convert.ToInt32(dbReader["MaNguoiDung"]),
                        HoTen = Convert.ToString(dbReader["HoTen"]),
                        Username = Convert.ToString(dbReader["Username"]),
                        isChuCuaHang = Convert.ToBoolean(dbReader["isChuCuaHang"]),
                        Password = Convert.ToString(dbReader["Password"]),
                        Anh = Convert.ToString(dbReader["Anh"])
                    };
                }

                cn.Close();
            }
            return data;
        }


        public NguoiDung Get(string username)
        {
            NguoiDung data = new NguoiDung();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from NguoiDung where Username = @Username";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@Username", username);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new NguoiDung()
                    {
                        MaNguoiDung = Convert.ToInt32(dbReader["MaNguoiDung"]),
                        HoTen = Convert.ToString(dbReader["HoTen"]),
                        Username = Convert.ToString(dbReader["Username"]),
                        isChuCuaHang = Convert.ToBoolean(dbReader["isChuCuaHang"]),
                        Password = Convert.ToString(dbReader["Password"]),
                        Anh = Convert.ToString(dbReader["Anh"])
                    };
                }

                cn.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public bool isEmailUser(string email)
        {
            throw new NotImplementedException();
        }

        public List<NguoiDung> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<NguoiDung> data = new List<NguoiDung>();
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
	                                 select *, row_number() over(order by MaNguoiDung) as RowNumber
	                                from    NguoiDUng
	                                where  (@searchValue  like N'') or (HoTen like @searchValue )) as t
                                where (@pageSize) = 0 or (t.RowNumber between(@page -1) *@pageSize + 1 and @page *@pageSize)
                                order by t.RowNumber";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@p", page);
                cmd.Parameters.AddWithValue("@ps", pageSize);
                cmd.Parameters.AddWithValue("@value", searchValue==null?"":searchValue);
                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new NguoiDung()
                    {
                        MaNguoiDung = Convert.ToInt32(result["MaNguoiDung"]),
                        HoTen = Convert.ToString(result["HoTen"]),
                        Username = Convert.ToString(result["Username"]),
                        isChuCuaHang = Convert.ToBoolean(result["isChuCuaHang"]),
                        Password = Convert.ToString(result["Password"]),
                        Anh = Convert.ToString(result["Anh"])
                    });
                }
                cn.Close();
            }
            return data;
        }

        public bool Update(NguoiDung data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update NguoiDung
                    set HoTen = @HoTen, isChuCuaHang = @isChuCuaHang, Anh = @Anh
                    where MaNguoiDung = @MaNguoiDung";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaNguoiDung", data.MaNguoiDung);
                cmd.Parameters.AddWithValue("@HoTen", data.HoTen);
                cmd.Parameters.AddWithValue("@isChuCuaHang", data.isChuCuaHang);
                cmd.Parameters.AddWithValue("@Anh", data.Anh);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
