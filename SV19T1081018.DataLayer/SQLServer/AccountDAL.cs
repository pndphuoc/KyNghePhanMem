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
        public string getOldPassword(string email)
        {
            string oldPassword = "";
            //using (SqlConnection cn = OpenConnection())
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandText = @"select Password from Employees where Email = @email";
            //    cmd.CommandType = System.Data.CommandType.Text;

            //    cmd.Connection = cn;
            //    cmd.Parameters.AddWithValue("email", email);

            //    oldPassword = Convert.ToString(cmd.ExecuteScalar());

            //    cn.Close();
            //}
            return oldPassword;
        }
        public bool ChangePassword(string email, string newPassword)
        {
            bool result = false;
            //using (SqlConnection cn = OpenConnection())
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandText = @"update Employees set Password = @password where Email = @email";
            //    cmd.CommandType = System.Data.CommandType.Text;

            //    cmd.Connection = cn;
            //    cmd.Parameters.AddWithValue("email", email);
            //    cmd.Parameters.AddWithValue("password", newPassword);
            //    //cmd.Parameters.AddWithValue("password", password);

            //    result = Convert.ToBoolean(cmd.ExecuteScalar());

            //    cn.Close();
            //}
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
