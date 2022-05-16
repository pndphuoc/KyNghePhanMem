using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class CaLamViecDAL : _BaseDAL, ICommonDAL<CaLamViec>
    {
        public CaLamViecDAL(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Them ca lam viec
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(CaLamViec data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into CaLamViec(TenCaLamViec, ThoiGianBatDau, ThoiGianKetThuc) values (@TenCaLamViec, @ThoiGianBatDau, @ThoiGianKetThuc) 
                             select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("TenCaLamViec", data.TenCaLamViec);
                cmd.Parameters.AddWithValue("ThoiGianBatDau", data.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("ThoiGianKetThuc", data.ThoiGianKetThuc);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }


        /// <summary>
        /// Dem ca lam viec
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
                                    from CaLamViec
                                    where (@searchValue = N'')
                                        or(
                                                (TenCaLamViec like @searchValue) 
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
        /// Xoa ca lam viec
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(int macalamviec)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from CaLamViec where MaCaLamViec = @macalamviec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("macalamviec", macalamviec);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }


        /// <summary>
        /// Lay thong tin ca lam viec
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CaLamViec Get(int macalamviec)
        {
            CaLamViec data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from CaLamViec where MaCaLamViec = @macalamviec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("macalamviec", macalamviec);

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


        /// <summary>
        /// Kiem tra ca lam viec co lien quan den truong du lieu khac hay ko
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool InUsed(int macalamviec)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                    from CaLamViec as clv join Cong as c on clv.MaCaLamViec = c.MaCaLamViec
                                    where clv.MaCaLamViec = @macalamviec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@macalamviec", macalamviec);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }


        /// <summary>
        /// Lay danh sach ca lam viec, tim kiem chuc vu duoi dang phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<CaLamViec> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<CaLamViec> data = new List<CaLamViec>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select *,
                                                    row_number() over(order by MaCaLamViec) as RowNumber
                                            from CaLamViec
                                            where (@searchValue = N'')
                                                or(
                                                        (TenCaLamViec like @searchValue)
                                                       
                                                        
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


        /// <summary>
        /// Cap nhat ca lam viec
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Update(CaLamViec data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update CaLamViec 
                    set TenCaLamViec = @tencalamviec, ThoiGianBatDau = @thoigianbatdau,
                        ThoiGianKetThuc = @thoigianketthuc
                    where MaCaLamViec = @macalamviec";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;


                cmd.Parameters.AddWithValue("macalamviec", data.MaCaLamViec);
                cmd.Parameters.AddWithValue("tencalamviec", data.TenCaLamViec);
                cmd.Parameters.AddWithValue("thoigianbatdau", data.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("thoigianketthuc", data.ThoiGianKetThuc);


                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
