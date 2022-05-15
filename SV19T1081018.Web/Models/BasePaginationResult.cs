using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    /// <summary>
    /// Lớp cơ sở (lớp cha) của các lớp dùng để lưu giữ kết quả liên quan đến tìm kiếm, phân trang
    /// </summary>
    public abstract class BasePaginationResult
    {
        /// <summary>
        /// trang cần xem (trang hiện tại)
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// số dòng hiển thị trên 1 trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// giá trị tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Tổng số dòng dữ liệu
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int p = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    p += 1;
                return p;
            }
        }
    }
}