using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class ProductPhotoDAL : _BaseDAL, IProductDetailsDAL<ProductPhoto>
    {
        public ProductPhotoDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(ProductPhoto data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden) 
                                    values (@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden)
                                    select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("Photo", data.Photo);
                cmd.Parameters.AddWithValue("Description", data.Description);
                cmd.Parameters.AddWithValue("DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("IsHidden", data.IsHidden);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public int Count(int ProductID)
        {
            int count = 0;


            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from ProductPhotos
                                    where ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@ProductID", ProductID);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool Delete(int id, int productID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from ProductPhotos where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("PhotoID", id);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
        public int GetDisplayOrder(int id)
        {
            int order = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select DisplayOrder from ProductPhotos where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("PhotoID", id);
                order = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return order;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductPhoto Get(int id)
        {
            ProductPhoto data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ProductPhotos where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("PhotoID", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt32(dbReader["PhotoID"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        Description = Convert.ToString(dbReader["Description"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(dbReader["IsHidden"])
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
            throw new NotImplementedException();
        }

        public IList<ProductPhoto> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public IList<ProductPhoto> List(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                from ProductPhotos
                                where ProductID = @productID
                                order by DisplayOrder asc";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", productID);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt32(result["PhotoID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        Description = Convert.ToString(result["Description"]),
                        Photo = Convert.ToString(result["Photo"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(result["IsHidden"])
                    });
                }

                cn.Close();
            }
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(ProductPhoto data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update ProductPhotos
                    set ProductID = @ProductID, Description = @Description,
                    Photo = @Photo, DisplayOrder = @DisplayOrder, IsHidden = @IsHidden
                    where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("Description", data.Description);
                cmd.Parameters.AddWithValue("Photo", data.Photo);
                cmd.Parameters.AddWithValue("DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("IsHidden", data.IsHidden);
                cmd.Parameters.AddWithValue("PhotoID", data.PhotoID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="PhotoID"></param>
        /// <param name="DisplayOrder"></param>
        /// <returns></returns>
        public bool UpdateDisplayOrder(int productID, int PhotoID, int DisplayOrder)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                                  declare @currentOrder int
                                  set @currentOrder = (select DisplayOrder from ProductPhotos where PhotoID = @PhotoID)

                                  declare @count int
                                  set @count = (select count(*) from ProductPhotos where ProductID = @productID)

                                  if (@DisplayOrder = @count)
                                  begin
	                                  update ProductPhotos
	                                  set DisplayOrder = @DisplayOrder
	                                  where ProductID = @productID and PhotoID = @PhotoID

	                                  update ProductPhotos
	                                  set DisplayOrder = DisplayOrder - 1
	                                  where ProductID = @productID and PhotoID <> @PhotoID and DisplayOrder <= @DisplayOrder and DisplayOrder > 1
                                  end
                                  else
                                  begin
	                                  update ProductPhotos
	                                  set DisplayOrder = @DisplayOrder
	                                  where ProductID = @productID and PhotoID = @PhotoID

	                                  update ProductPhotos
	                                  set DisplayOrder = DisplayOrder + 1
	                                  where ProductID = @productID and DisplayOrder >= @DisplayOrder and PhotoID <> @PhotoID and DisplayOrder >= @DisplayOrder
                                  end";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("productID", productID);
                cmd.Parameters.AddWithValue("PhotoID", PhotoID);
                cmd.Parameters.AddWithValue("DisplayOrder", DisplayOrder);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            
            return result;
        }
    }
}
