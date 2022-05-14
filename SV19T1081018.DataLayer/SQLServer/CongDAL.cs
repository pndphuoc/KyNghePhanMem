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

        public Cong GetCongTheoNgay(DateTime Ngay)
        {
            throw new NotImplementedException();
        }

        public Cong GetCongTheoNhanVien(int MaNhanVien)
        {
            throw new NotImplementedException();
        }

        public Cong GetCongTheoThang(int Thang)
        {
            throw new NotImplementedException();
        }

        public Cong GetCongTungThangCuaNhanVien(int MaNhanVien, int Thang)
        {
            throw new NotImplementedException();
        }
    }
}
