using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface IProductDetailsDAL<T> where T : class
    {
        /// <summary>
        /// Lấy danh sách đối tượng dựa vào mã mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<T> List(int id);
        /// <summary>
        /// Đếm số lượng thuộc tính/ảnh của từng mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        int Count(int productID);
        /// <summary>
        /// Lấy thông tin của đối tượng 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);
        /// <summary>
        /// Thêm một đối tượng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cập nhật thông tin một đối tượng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa 1 đối tượng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool Delete(int id, int productID);

        bool InUsed(int id);

        int GetDisplayOrder(int id);
        /// <summary>
        /// Cập nhật thứ tự hiển thị toàn bộ đối tượng của mặt hàng khi có 1 đối tượng thay đổi thứ tự hiển thị
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <param name="DisplayOrder"></param>
        /// <returns></returns>
        bool UpdateDisplayOrder(int productID, int attributeID, int DisplayOrder);
    }
}
