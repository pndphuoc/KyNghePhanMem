using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;


namespace SV19T1081018.DataLayer
{
        //định nghĩa các phép xử lý dữ liệu liên quan đến loại hàng
    public interface ICategoryDAL
    {
 
        /// <summary>
        /// Lấy danh sách các loại hàng
        /// </summary>
        /// <returns></returns>
        IList<Category> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Lấy thông tin của một loại hàng dựa vào mã loại hàng
        /// </summary>
        /// <param name="categoryID">Mã loại hàng cần lấy thống tin</param>
        /// <returns></returns>
        Category Get(int categoryID);

        /// <summary>
        /// Bổ sung loại hàng, hàm trả về mã của loại hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Category data);
        /// <summary>
        /// Cập nhật thông tin
        /// </summary>P:\TestWebApp\HRSort\HRSoft.DataLayer\ICategoryDAL.cs
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Category data);
        /// <summary>
        /// XÓA 1 loại hàng dựa vào mã loại hàng, lưu ý không được xóa nếu loại hàng đã có mặt hàng liên quan
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        bool Delete(int categoryID);
        bool InUsed(int categoryID);
        int Count(string searchValue);
    }
}
