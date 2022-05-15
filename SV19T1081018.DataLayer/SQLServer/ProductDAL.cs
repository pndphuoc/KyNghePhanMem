using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Products(ProductName, SupplierID, CategoryID, Unit, Price, Photo) 
                                    values (@ProductName, @SupplierID, @CategoryID, @Unit, @Price, @Photo)
                                    select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("Unit", data.Unit);
                cmd.Parameters.AddWithValue("Price", data.Price);
                cmd.Parameters.AddWithValue("Photo", data.Photo);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public int Count(string searchValue = "", int CategoryID=0, int SupplierID=0)
        {
            int count = 0;

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from Products as p
                                    where ((@searchValue = N'') or ((@categoryID = 0 ) or ( p.CategoryID = @categoryID ))
		and ((@supplierID = 0 ) or ( p.SupplierID = @supplierID ))
		and ((@searchValue = '' ) or ( p.ProductName like @searchValue)))";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@categoryID", CategoryID);
                cmd.Parameters.AddWithValue("@supplierID", SupplierID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"delete from ProductPhotos where ProductID = @ProductID
                                    delete from ProductAttributes where ProductID = @ProductID
                                    delete from Products where ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", id);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product Get(int id)
        {
            Product data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Products where ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new Product()
                    {
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        Price = Convert.ToInt32(dbReader["Price"]),
                        Unit = Convert.ToString(dbReader["Unit"])
                        
                    };
                }

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool InUsed(int id)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select case when exists(select * from OrderDetails where ProductID = @ProductID) then 1 else 0 end";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", id);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="CategoryID"></param>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int CategoryID = 0, int SupplierID = 0)
        {
            List<Product> data = new List<Product>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"exec searchProduct @page = @pageIN, @pageSize = @pageSizeIN, @searchValue= @searchValueIN, @categoryID = @categoryIDIN, @supplierID = @supplierIDIN";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@pageIN", page);
                cmd.Parameters.AddWithValue("@pageSizeIN", pageSize);
                cmd.Parameters.AddWithValue("@searchValueIN", searchValue);
                cmd.Parameters.AddWithValue("@categoryIDIN", CategoryID);
                cmd.Parameters.AddWithValue("@supplierIDIN", SupplierID);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Product()
                    {
                        ProductName = Convert.ToString(result["ProductName"]),
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Price = Convert.ToInt32(result["Price"]),
                        Unit = Convert.ToString(result["Unit"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Products
                    set ProductName = @productName, SupplierID = @supplierID, CategoryID = @categoryID,
                    Unit = @unit, Price = @price, Photo = @photo
                    where ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("productName", data.ProductName);
                cmd.Parameters.AddWithValue("supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("unit", data.Unit);
                cmd.Parameters.AddWithValue("price", data.Price);
                cmd.Parameters.AddWithValue("photo", data.Photo);
                cmd.Parameters.AddWithValue("productID", data.ProductID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
