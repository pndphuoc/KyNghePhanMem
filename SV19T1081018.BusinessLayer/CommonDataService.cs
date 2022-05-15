using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DataLayer;
using SV19T1081018.DomainModel;
using System.Configuration;
using SV19T1081018.DataLayer;
using SV19T1081018.BusinessLayer;

namespace SV19T1081018.BusinessLayer
{
    /// <summary>
    /// Cung cấp các chức năng xử lý dữ liệu chung
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<NhanVien> nhanvienDB;
        private static readonly ICommonDAL<ChucVu> chucvuDB;
        static CommonDataService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            switch (provider)
            {
                case "SQLServer":
                    //employeeDB = new DataLayer.SQLServer.EmployeeDAL(connectionString);
                    nhanvienDB = new DataLayer.SQLServer.NhanVienDAL(connectionString);
                    chucvuDB = new DataLayer.SQLServer.ChucVuDAL(connectionString);
                    break;
            }
        }
        public static List<NhanVien> DanhSachNhanVien(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = nhanvienDB.Count(searchValue);
            return nhanvienDB.List(page, pageSize, searchValue).ToList();
        }
        public static NhanVien GetNhanVien(int customerID)
        {
            return nhanvienDB.Get(customerID);
        }
        public static bool XoaNhanVien(int customerID)
        {
            return nhanvienDB.Delete(customerID);
        }
        public static int ThemNhanVien(NhanVien data)
        {
            return nhanvienDB.Add(data);
        }
        public static bool CapNhatNhanVien(NhanVien data)
        {
            return nhanvienDB.Update(data);
        }
        public static List<ChucVu> DanhSachChucVu()
        {
            return chucvuDB.List().ToList();

        }
        public static Boolean Inused(int id)
        {
            return nhanvienDB.InUsed(id);

        }
    }
}
