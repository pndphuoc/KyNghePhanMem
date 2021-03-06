using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface INguoiDungDAL
    {
        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm, để rỗng nếu không tìm kiếm</param>
        /// <returns></returns>
        List<NguoiDung> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Đếm số dòng kết quả khi tìm kiếm
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Lấy thông tin của đối tượng 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NguoiDung Get(int id);
        NguoiDung Get(string email);
        /// <summary>
        /// Thêm một đối tượng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(NguoiDung data);
        /// <summary>
        /// Cập nhật thông tin một đối tượng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(NguoiDung data);
        /// <summary>
        /// Xóa một đối tượng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Kiểm tra đối tượng đã được sử dụng hay chưa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
        /// <summary>
        /// Kiểm tra email đã được sử dụng hay chưa
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool isEmailUser(string email);
    }
}
