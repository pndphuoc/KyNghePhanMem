using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;

namespace SV19T1081018.DataLayer
{
    /// <summary>
    /// Định nghĩa các phép xử lý dữ liệu liên quan đến khách hàng
    /// </summary>
    public interface ICustomerDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm. Tên và địa chỉ cần tìm. Nếu rỗng thì hiển thị toàn bộ</param>
        /// <returns></returns>
        IList<Customer> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng khách hàng thỏa mãn điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin khách hàng dựa vào mã khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Customer Get(int customerID);
        /// <summary>
        /// Bổ sung khách hàng. Hàm trả về mã của khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Customer data);
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Customer data);
        /// <summary>
        /// Xóa 1 khách hàng dựa vào mã khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        bool Delete(int customerID);
        /// <summary>
        /// Kiểm tra xem 1 khách hàng hiện có các dữ liệu khác liên quan hay không (ví dụ: đang có đơn hàng)
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        bool InUsed(int customerID);

    }
}
