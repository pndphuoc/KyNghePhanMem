using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    /// <summary>
    /// Lưu trữ kết quả tìm kiếm phân trang liên quan đến khách hàng
    /// </summary>
    public class CustomerPaginationResult : BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }
}