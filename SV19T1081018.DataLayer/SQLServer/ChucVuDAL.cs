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
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ChucVu> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// lấy danh sách chức vụ để hiển thị và phục vụ trong phần thêm và sửa thông tin Nhân Viên .
        /// </summary>
        public IList<ChucVu> List()
        {
            List<ChucVu> data = new List<ChucVu>();


            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                  from ChucVu";


                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ChucVu()
                    {
                        MaChucVu = Convert.ToInt32(result["MaChucVu"]),
                        TenChucVu =Convert.ToString(result["TenChucVu"]),
                        TienLuong = Convert.ToInt32(result["TienLuong"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(ChucVu data)
        {
            throw new NotImplementedException();
        }
    }
}
