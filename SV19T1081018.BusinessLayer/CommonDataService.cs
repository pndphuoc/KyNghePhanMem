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
        private static readonly ILichLamViecDAL lichlamviecDB;
        private static readonly ICommonDAL<ChucVu> chucVuDB;
        private static readonly ICommonDAL<CaLamViec> caLamViecDB;
        private static readonly INguoiDungDAL nguoiDungDB;
        static CommonDataService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            switch (provider)
            {
                case "SQLServer":
                    //employeeDB = new DataLayer.SQLServer.EmployeeDAL(connectionString);
                    nhanVienDB = new DataLayer.SQLServer.NhanVienDAL(connectionString);
                    lichlamviecDB = new DataLayer.SQLServer.LichLamViecDAL(connectionString);
                    congDB = new DataLayer.SQLServer.CongDAL(connectionString);
                    chucVuDB = new DataLayer.SQLServer.ChucVuDAL(connectionString);
                    caLamViecDB = new DataLayer.SQLServer.CaLamViecDAL(connectionString);
                    nguoiDungDB = new DataLayer.SQLServer.NguoiDungDAL(connectionString);
                    break;
            }
        }
        #region Nhân viên
        public static List<NhanVien> ListOfNhanVien(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = nhanVienDB.Count(searchValue);
            return nhanVienDB.List(page, pageSize, searchValue);
        }
        public static List<NhanVien> ListOfNhanVien()
        {
            return nhanVienDB.List();
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
            return chucVuDB.List();

        }
        /// <summary>
        /// lay danh sach cac chuc vu. tim kiem chuc vu duoi dang phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<ChucVu> ListOfChucVu(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = chucVuDB.Count(searchValue);
            return chucVuDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// Xoa chuc vu
        /// </summary>
        /// <param name="machucvu"></param>
        /// <returns></returns>
        public static bool DeleteChucVu(int machucvu)
        {
            if (chucVuDB.InUsed(machucvu))
            {
                return false;
            }
            return chucVuDB.Delete(machucvu);
        }


        /// <summary>
        /// Them chuc vu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddChucVu(ChucVu data)
        {
            return chucVuDB.Add(data);
        }


        /// <summary>
        /// kiem tra chuc vu co lien quan den truong du lieu khac hay khong
        /// </summary>
        /// <param name="machucvu"></param>
        /// <returns></returns>
        public static bool InUsed(int machucvu)
        {
            return chucVuDB.InUsed(machucvu);
        }
        public static bool UpdateChucVu(ChucVu data)
        {
            return chucVuDB.Update(data);
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
        public static List<CaLamViec> ListOfCaLamViec(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = caLamViecDB.Count(searchValue);
            return caLamViecDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Xoa ca lam viec
        /// </summary>
        /// <param name="machucvu"></param>
        /// <returns></returns>
        public static bool DeleteCaLamViec(int macalamviec)
        {
            if (caLamViecDB.InUsed(macalamviec))
            {
                return false;
            }
            return caLamViecDB.Delete(macalamviec);
        }


        /// <summary>
        /// Them ca lam viec
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCaLamViec(CaLamViec data)
        {
            return caLamViecDB.Add(data);
        }


        /// <summary>
        /// Kiem tra ca lam viec co lien quan den truong du lieu khac hay khong
        /// </summary>
        /// <param name="macalamviec"></param>
        /// <returns></returns>
        public static bool InUsedCaLamViec(int macalamviec)
        {
            return caLamViecDB.InUsed(macalamviec);
        }


        /// <summary>
        /// cap nhap thong tin ca lam viec
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCaLamViec(CaLamViec data)
        {
            return caLamViecDB.Update(data);
        }

        #endregion

        #region Lịch làm việc
        public static List<LichLamViec> GetList(int MaCaLamViec, int MaThu)
        {
            return lichlamviecDB.List(MaCaLamViec, MaThu, "").ToList();
        }
        public static int CheckNV(int MaThu, int MaCaLamViec, int MaNhanVien)
        {
            return lichlamviecDB.Check(MaThu, MaCaLamViec, MaNhanVien);
        }
        public static bool AddLichNV(LichLamViec data)
        {
            return lichlamviecDB.Add(data);
        }
        public static bool DeleteNV(LichLamViec data)
        {
            return lichlamviecDB.Delete(data);
        }
        #endregion

        #region
        public static NguoiDung GetNguoiDung(int MaNguoiDung)
        {
            return nguoiDungDB.Get(MaNguoiDung);
        }
        #endregion
    }
}
