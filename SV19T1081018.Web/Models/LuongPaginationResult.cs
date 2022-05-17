using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    public class LuongPaginationResult : BasePaginationResult
    {
        public List<Luong> Data { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }

}