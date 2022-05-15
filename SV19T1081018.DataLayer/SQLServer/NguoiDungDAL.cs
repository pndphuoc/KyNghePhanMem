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
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
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
                        Password = Convert.ToString(dbReader["Password"])
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
                        Password = Convert.ToString(dbReader["Password"])
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

        public IList<NguoiDung> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Update(NguoiDung data)
        {
            throw new NotImplementedException();
        }
    }
}
