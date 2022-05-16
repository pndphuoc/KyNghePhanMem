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
        private static readonly ICommonDAL<CaLamViec> caLamViecDB;
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
                    caLamViecDB = new DataLayer.SQLServer.CaLamViecDAL(connectionString);
                    break;
            }
        }
        #region Nhân viên
        public static List<NhanVien> ListOfNhanVien(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = nhanVienDB.Count(searchValue);
            return nhanVienDB.List(page, pageSize, searchValue);
        }
        public static NhanVien GetNhanVien(int MaNhanVien)
        {
            return nhanVienDB.Get(MaNhanVien);
        }
        public static bool XoaNhanVien(int customerID)
        {
            return nhanVienDB.Delete(customerID);
        }
        public static int ThemNhanVien(NhanVien data)
        {
            return nhanVienDB.Add(data);
        }
        public static bool CapNhatNhanVien(NhanVien data)
        {
            return nhanVienDB.Update(data);
        }
        public static Boolean InusedNhanVien(int id)
        {
            return nhanVienDB.InUsed(id);

        }
        #endregion

        #region Chức vụ
        public static ChucVu GetChucVu(int MaChucVu)
        {
            return chucVuDB.Get(MaChucVu);
        }
        public static List<ChucVu> ListOfChucVu()
        {
            return chucVuDB.List().ToList();

        }
        #endregion

        #region Công
        public static int CountCongTungThangCuaNhanVien(int MaNhanVien, int Thang, int Nam)
        {
            return congDB.CountCongTungThangCuaNhanVien(MaNhanVien, Thang, Nam);
        }
        public static List<Cong> GetCongTheoNgay(int MaNhanVien, int Ngay, int Thang, int Nam)
        {
            return congDB.GetCongTheoNgay(MaNhanVien, Ngay, Thang, Nam);
        }
        public static Cong GetCong(int MaCong)
        {
            return congDB.GetCong(MaCong);
        }
        public static bool UpdateCong(Cong cong)
        {
            return congDB.Update(cong);
        }
        public static int AddCong(Cong data)
        {
            return congDB.Add(data);
        }
        public static bool DeleteCong(int MaCong)
        {
            return congDB.Delete(MaCong);
        }
        #endregion

        #region Ca làm việc
        public static CaLamViec GetCaLamViec(int MaCaLamViec)
        {
            return caLamViecDB.Get(MaCaLamViec);
        }

        public static List<CaLamViec> ListCaLamViec()
        {
            return caLamViecDB.List();
        }
        #endregion
    }
}
