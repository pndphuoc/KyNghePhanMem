using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class LichLamViecDAL : _BaseDAL, ILichLamViecDAL
    {
        public LichLamViecDAL(string connectionString) : base(connectionString)
        {
        }

        public bool Add(LichLamViec data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into LichLamViec(MaCaLamViec,MaThu,TenNhanVien,MaChucVu,MaNhanVien) 
                            values (@MaCaLamViec,@MaThu,@TenNhanVien,@MaChucVu,@MaNhanVien) 
                            select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaThu", data.MaThu);
                cmd.Parameters.AddWithValue("@MaCaLamViec", data.MaCaLamViec);
                cmd.Parameters.AddWithValue("@MaChucVu", data.MaChucVu);
                cmd.Parameters.AddWithValue("@TenNhanVien", data.TenNhanVien);
                cmd.Parameters.AddWithValue("@MaNhanVien", data.MaNhanVien);

                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public int Check(int MaThu, int MaCaLamViec, int MaNhanVien)
        {
            int count = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) from NhanVien as n
                                    join LichLamViec as l on l.TenNhanVien=n.TenNhanVien
                                    where MaThu=@MaThu and MaCaLamViec=@MaCaLamViec and n.MaNhanVien=@MaNhanVien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaCaLamViec", MaCaLamViec);
                cmd.Parameters.AddWithValue("@MaThu", MaThu);
                cmd.Parameters.AddWithValue("@MaNhanVien", MaNhanVien);

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }

        public bool Delete(LichLamViec data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE  FROM LichLamViec where MaThu=@MaThu and MaCaLamViec=@MaCaLamViec and MaNhanVien=@MaNhanVien";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaThu", data.MaThu);
                cmd.Parameters.AddWithValue("@MaCaLamViec", data.MaCaLamViec);
                cmd.Parameters.AddWithValue("@MaNhanVien", data.MaNhanVien);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public IList<LichLamViec> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<LichLamViec> data = new List<LichLamViec>();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from LichLamViec where MaCaLamViec=@MaCaLamViec and MaThu=@MaThu";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@MaCaLamViec", page);
                cmd.Parameters.AddWithValue("@MaThu", pageSize);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new LichLamViec()
                    {
                        MaCaLamViec = Convert.ToInt32(result["MaCaLamViec"]),
                        MaThu = Convert.ToInt32(result["MaThu"]),
                        TenNhanVien = Convert.ToString(result["TenNhanVien"]),
                        MaChucVu = Convert.ToInt32(result["MaChucVu"]),
                        MaNhanVien = Convert.ToInt32(result["MaNhanVien"])

                    });
                }


                cn.Close();
            }
            return data;
        }
    }
}
