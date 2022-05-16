using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    public class EmployeePaginationResult : BasePaginationResult
    {
        public List<NhanVien> Data { get; set; }
    }
}