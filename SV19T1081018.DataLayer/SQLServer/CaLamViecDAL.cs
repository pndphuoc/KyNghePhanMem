using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class CaLamViecDAL : _BaseDAL, ICommonDAL<CaLamViec>
    {
        public CaLamViecDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(CaLamViec data)
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

        public CaLamViec Get(int id)
        {
            CaLamViec data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from CaLamViec where MaCaLamViec = @MaCaLamViec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaCaLamViec", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new CaLamViec()
                    {
                        MaCaLamViec = Convert.ToInt32(dbReader["MaCaLamViec"]),
                        TenCaLamViec = Convert.ToString(dbReader["TenCaLamViec"]),
                        ThoiGianBatDau = Convert.ToString(dbReader["ThoiGianBatDau"]),
                        ThoiGianKetThuc = Convert.ToString(dbReader["ThoiGianKetThuc"])

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

        public List<CaLamViec> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<CaLamViec> data = new List<CaLamViec>();

            string searchValueEmail = searchValue;
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from CaLamViec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new CaLamViec()
                    {
                        MaCaLamViec = Convert.ToInt32(result["MaCaLamViec"]),
                        TenCaLamViec = Convert.ToString(result["TenCaLamViec"]),
                        ThoiGianBatDau = Convert.ToString(result["ThoiGianBatDau"]),
                        ThoiGianKetThuc = Convert.ToString(result["ThoiGianKetThuc"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(CaLamViec data)
        {
            throw new NotImplementedException();
        }
    }
}
