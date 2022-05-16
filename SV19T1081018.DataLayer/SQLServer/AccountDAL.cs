using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class AccountDAL : _BaseDAL, IAccountDAL
    {
        public AccountDAL(string connectionString) : base(connectionString)
        {
        }
        public string getOldPassword(string Username)
        {
            string oldPassword = "";
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select Password from NguoiDung where Username = @username";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("username", Username);


                oldPassword = Convert.ToString(cmd.ExecuteScalar());

                cn.Close();
            }
            return oldPassword;
        }
        public bool ChangePassword(string Username, string newPassword)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update NguoiDung set Password = @password where Username = @username";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("username", Username);
                cmd.Parameters.AddWithValue("password", newPassword);
                //cmd.Parameters.AddWithValue("password", password);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

        public NguoiDung Login(string username, string password)
        {
            NguoiDung data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from NguoiDung where Username = @Username and Password = @password";
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("Username", username);
                cmd.Parameters.AddWithValue("password", password);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new NguoiDung()
                    {
                        MaNguoiDung = Convert.ToInt32(dbReader["MaNguoiDung"]),
                        HoTen = Convert.ToString(dbReader["HoTen"]),
                        isChuCuaHang = Convert.ToBoolean(dbReader["isChuCuaHang"]),
                        Username = Convert.ToString(dbReader["Username"]),
                        Password = Convert.ToString(dbReader["Password"])
                    };
                }

                cn.Close();
            }
            return data;
        }

    }
}
