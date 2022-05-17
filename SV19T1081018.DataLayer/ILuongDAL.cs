using SV19T1081018.DataLayer.SQLServer;
using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface ILuongDAL

    {
        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="page">trang cần xem</param>
        /// <param name="pageSize">Số dòng mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm</param>
        /// <returns></returns>
        IList<Luong> List(int page = 1, int pageSize = 0, string searchValue = "", int Thang = 5, int Nam = 2022);
        /// <summary>
        /// đếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int Thang = 5, int Nam = 2022);
        /// <summary>
        /// Trả lương
        /// </summary>
        /// <param name="MaLuong"></param>
        /// <returns></returns>
        bool TraLuong(int MaLuong);
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="MaLuong"></param>
        /// <returns></returns>
        List<TienThuongPhat> GetThuongPhat(int MaLuong);
        int Add(TienThuongPhat data);
        bool Update(TienThuongPhat data);
        bool Delete(int MaTienThuongPhat);
        TienThuongPhat Get(int MaTienThuongPhat);
    }
}
