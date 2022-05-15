using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface IProductDAL : ICommonDAL<Product>
    {
        /// <summary>
        /// Lấy danh sách mặt hàng theo các tiêu chí 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        IList<Product> List(int page = 1, int pageSize = 10, string searchValue = "", int CategoryID=0, int SupplierID=0);
        /// <summary>
        /// Đếm số lượng mặt hàng 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int CategoryID = 0, int SupplierID = 0);
    }
}
