using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;

namespace SV19T1081018.Web.Models
{
    public class CaLamViecPaginationResult : BasePaginationResult
    {
        public List<CaLamViec> Data { get; set; }
    }
}
