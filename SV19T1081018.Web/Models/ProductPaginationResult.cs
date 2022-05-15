using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV19T1081018.Web.Models
{
    public class ProductPaginationResult : BasePaginationResult
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public List<Product> Data { get; set; }
    }
}