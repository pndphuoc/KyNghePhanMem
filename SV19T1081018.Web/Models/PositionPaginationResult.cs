using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081018.DomainModel;

namespace SV19T1081018.Web.Models
{
    public class PositionPaginationResult : BasePaginationResult
    {
        public List<ChucVu> Data { get; set; }
    }
}
