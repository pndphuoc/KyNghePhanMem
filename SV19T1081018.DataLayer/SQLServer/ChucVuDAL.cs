using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class ChucVuDAL : _BaseDAL, ICommonDAL<ChucVu>
    {
        public ChucVuDAL(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Them Chuc vu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(ChucVu data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into ChucVu(TenChucVu, TienLuong) values (@TenChucVu, @TienLuong) 
                             select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("TenChucVu", data.TenChucVu);
                cmd.Parameters.AddWithValue("TienLuong", data.TienLuong);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }


        /// <summary>
        /// Dem chuc vu
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Count(string searchValue = "")
        {
            int count = 0;

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from ChucVu
                                    where (@searchValue = N'')
                                        or(
                                                (TenChucVu like @searchValue)
                                                or
                                                (TienLuong like @searchValue)
                                                
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }


        /// <summary>
        /// Xoa chuc vu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(int machucvu)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from ChucVu where MaChucVu = @machucvu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("machucvu", machucvu);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }


        /// <summary>
        /// Lay thong tin chuc vu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ChucVu Get(int machucvu)
        {
            ChucVu data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ChucVu where MaChucVu = @machucvu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("machucvu", machucvu);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new ChucVu()
                    {
                        MaChucVu = Convert.ToInt32(dbReader["MaChucVu"]),
                        TenChucVu = Convert.ToString(dbReader["TenChucVu"]),
                        TienLuong = Convert.ToInt32(dbReader["TienLuong"])

                    };
                }

                cn.Close();
            }

            return data;
        }


        /// <summary>
        /// Kiem tra chuc vu co lien quan den truong du lieu khac hay ko
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool InUsed(int machucvu)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from ChucVu as cv join NhanVien as nv on cv.MaChucVu = nv.MaChucVu
                                    where cv.MaChucvu = @machucvu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@machucvu", machucvu);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }


        /// <summary>
        /// Lay danh sach chuc vu, tim kiem chuc vu duoi dang phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ChucVu> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<ChucVu> data = new List<ChucVu>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select *,
                                                    row_number() over(order by MaChucVu) as RowNumber
                                            from ChucVu
                                            where (@searchValue = N'')
                                                or(
                                                        (TenChucVu like @searchValue)
                                                        or
                                                        (TienLuong like @searchValue)
                                                        
                                                    )
                                        ) as t
                                    where (@pageSize=0) or (t.RowNumber between(@page -1) *@pageSize + 1 and @page *@pageSize)
                                    order by t.RowNumber; ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ChucVu()
                    {
                        MaChucVu = Convert.ToInt32(result["MaChucVu"]),
                        TenChucVu = Convert.ToString(result["TenChucVu"]),
                        TienLuong = Convert.ToInt32(result["TienLuong"])

                    });
                }

                cn.Close();
            }
            return data;
        }


        /// <summary>
        /// Cap nhat chuc vu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Update(ChucVu data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update ChucVu 
                    set TenChucVu = @tenchucvu, TienLuong = @tienluong
                    where MaChucVu = @machucvu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("tenchucvu", data.TenChucVu);
                cmd.Parameters.AddWithValue("tienluong", data.TienLuong);
                cmd.Parameters.AddWithValue("machucvu", data.MaChucVu);


                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
