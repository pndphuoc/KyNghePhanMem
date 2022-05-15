using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface IShipperDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên giao hangf dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        IList<Shipper> List(int page, int pageSize, string searchValue);
        /// <summary>
        /// Đếm số lượng nhân viên giao hangf thỏa mãn điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin nhân viên giao hangf
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        Shipper Get(int shipperID);
        /// <summary>
        /// Bổ sung nhân viên, trả về mã của nhân viên giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Shipper data);
        /// <summary>
        /// Cập nhật thông tin nhân viên giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Shipper data);
        /// <summary>
        /// Xóa 1 nhân viên dựa vào mã nhân viên giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        bool Delete(int shipperID);
        /// <summary>
        /// Kiểm tra nhân viên giao hàng có các dữ liệu khác liên quan hay không
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        bool InUsed(int shipperID);
    }
}
