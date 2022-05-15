using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface ISupplierDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm. Tên và địa chỉ cần tìm. Nếu rỗng thì hiển thị toàn bộ</param>
        /// <returns></returns>
        IList<Supplier> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng nhà cung cấp thỏa mãn điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin nhà cung cấp dựa vào mã khách hàng
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Bổ sung nhà cung cấp. Hàm trả về mã của nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Supplier data);
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa 1 nhà cung cấp dựa vào mã nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool Delete(int supplierID);
        /// <summary>
        /// Kiểm tra xem 1 nhà cung cấp hiện có các dữ liệu khác liên quan hay không
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool InUsed(int supplierID);
    }
}
