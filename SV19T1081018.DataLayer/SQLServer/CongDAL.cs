using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class CongDAL : _BaseDAL, ICongDAL
    {
        public CongDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Cong data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Cong(MaNhanVien, MaCaLamViec, ThoiGianVaoCa, ThoiGianKetThuc, status, Ngay) 
                                    values (@MaNhanVien, @MaCaLamViec, @ThoiGianVaoCa, @ThoiGianKetThuc, @status, @Ngay) select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaNhanVien", data.MaNhanVien);
                cmd.Parameters.AddWithValue("MaCaLamViec", data.MaCaLamViec);
                cmd.Parameters.AddWithValue("ThoiGianVaoCa", data.ThoiGianVaoCa);
                cmd.Parameters.AddWithValue("ThoiGianKetThuc", data.ThoiGianKetThuc);
                cmd.Parameters.AddWithValue("status", data.status);
                cmd.Parameters.AddWithValue("Ngay", data.Ngay);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

        public int CountCongTungThangCuaNhanVien(int MaNhanVien, int Thang, int Nam)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*) from Cong where MaNhanVien = @MaNhanVien and MONTH(Ngay) = @Thang and YEAR(Ngay) = @Nam";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaNhanVien", MaNhanVien);
                cmd.Parameters.AddWithValue("Thang", Thang);
                cmd.Parameters.AddWithValue("Nam", Nam);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }

        public bool Delete(int MaCong)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from Cong where MaCong = @MaCong";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaCong", MaCong);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public Cong GetCong(int MaCong)
        {
            Cong c = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Cong  where MaCong = @MaCong";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaCong", MaCong);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    c = new Cong()
                    {
                        MaNhanVien = Convert.ToInt32(dbReader["MaNhanVien"]),
                        MaCaLamViec = Convert.ToInt32(dbReader["MaCaLamViec"]),
                        MaCong = Convert.ToInt32(dbReader["MaCong"]),
                        ThoiGianVaoCa = Convert.ToString(dbReader["ThoiGianVaoCa"]),
                        ThoiGianKetThuc = dbReader.IsDBNull(dbReader.GetOrdinal("ThoiGianKetThuc")) == true ? Convert.ToString(dbReader["ThoiGianVaoCa"]) : Convert.ToString(dbReader["ThoiGianKetThuc"]),
                        status = Convert.ToBoolean(dbReader["status"]),
                        Ngay = Convert.ToDateTime(dbReader["Ngay"])
                    };
                }
                cn.Close();
            }
            return c;
        }


        public List<Cong> GetCongTheoNgay(int MaNhanVien, int Ngay, int Thang, int Nam)
        {
             List<Cong> data = new List<Cong>();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from Cong where MaNhanVien = @MaNhanVien and MONTH(Ngay) = @Thang and YEAR(Ngay) = @Nam and DAY(Ngay) = @Ngay";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("MaNhanVien", MaNhanVien);
                cmd.Parameters.AddWithValue("Thang", Thang);
                cmd.Parameters.AddWithValue("Nam", Nam);
                cmd.Parameters.AddWithValue("Ngay", Ngay);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Cong()
                    {
                        MaNhanVien = Convert.ToInt32(result["MaNhanVien"]),
                        MaCaLamViec = Convert.ToInt32(result["MaCaLamViec"]),
                        MaCong = Convert.ToInt32(result["MaCong"]),
                        ThoiGianVaoCa = Convert.ToString(result["ThoiGianVaoCa"]),
                        ThoiGianKetThuc = result.IsDBNull(result.GetOrdinal("ThoiGianKetThuc")) == true ? Convert.ToString(result["ThoiGianVaoCa"]): Convert.ToString(result["ThoiGianKetThuc"]),
                        status = Convert.ToBoolean(result["status"]),
                        Ngay = Convert.ToDateTime(result["Ngay"])

                    });
                }
            }
            return data;
        }


        public bool Update(Cong cong)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Cong
                    set ThoiGianVaoCa = @ThoiGianVaoCa, ThoiGianKetThuc = @ThoiGianKetThuc, status = @status
                    where MaCong = @MaCong";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ThoiGianVaoCa", cong.ThoiGianVaoCa);
                cmd.Parameters.AddWithValue("ThoiGianKetThuc", cong.ThoiGianKetThuc);
                cmd.Parameters.AddWithValue("status", cong.status);
                cmd.Parameters.AddWithValue("MaCong", cong.MaCong);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
