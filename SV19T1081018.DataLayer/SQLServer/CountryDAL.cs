using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class CountryDAL : _BaseDAL, ICommonDAL<Country>
    {
        public CountryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Country data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Country Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public IList<Country> List(int page=1, int pageSize=10, string searchValue="")
        {
            List<Country> data = new List<Country>();


            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Countries";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Country()
                    {
                        CountryName = Convert.ToString(result["CountryName"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(Country data)
        {
            throw new NotImplementedException();
        }
    }
}
