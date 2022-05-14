using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class ChucVuDAL : _BaseDAL, ICommonDAL<ChucVu>
    {
        public ChucVuDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(ChucVu data)
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

        public ChucVu Get(int id)
        {
            ChucVu data = new ChucVu();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from ChucVu where MaChucVu = @MaChucVu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaChucVu", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new ChucVu()
                    {
                        TenChucVu = Convert.ToString(dbReader["TenChucVu"]),
                        MaChucVu = Convert.ToInt32(dbReader["MaChucVu"]),
                        TienLuong = Convert.ToInt32(dbReader["TienLuong"])
                    };
                }
                return data;
            }
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public List<ChucVu> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Update(ChucVu data)
        {
            throw new NotImplementedException();
        }
    }
}
