using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    public class NhanVienPaginationResult : BasePaginationResult
    {
        public IList<NhanVien> Data { get; set; }
    }
}