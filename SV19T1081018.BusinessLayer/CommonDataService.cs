using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DataLayer;
using SV19T1081018.DomainModel;
using System.Configuration;


namespace SV19T1081018.BusinessLayer
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu chung
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<NhanVien> nhanVienDB;
        private static readonly ICongDAL congDB;
        private static readonly ICommonDAL<ChucVu> chucVuDB;
        static CommonDataService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            switch (provider)
            {
                case "SQLServer":
                    //employeeDB = new DataLayer.SQLServer.EmployeeDAL(connectionString);
                    nhanVienDB = new DataLayer.SQLServer.NhanVienDAL(connectionString);
                    congDB = new DataLayer.SQLServer.CongDAL(connectionString);
                    chucVuDB = new DataLayer.SQLServer.ChucVuDAL(connectionString);
                    break;
            }
        }

        public static List<NhanVien> ListOfNhanVien(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = nhanVienDB.Count(searchValue);
            return nhanVienDB.List(page, pageSize, searchValue);
        }

        public static int CountCongTungThangCuaNhanVien(int MaNhanVien, int Thang, int Nam)
        {
            return congDB.CountCongTungThangCuaNhanVien(MaNhanVien, Thang, Nam);
        }
        public static ChucVu GetChucVu(int MaChucVu)
        {
            return chucVuDB.Get(MaChucVu);
        }
    }
}
