using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductAttributeDAL : _BaseDAL, IProductDetailsDAL<ProductAttribute>
    {
        public ProductAttributeDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(ProductAttribute data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder) 
                                    values (@ProductID, @AttributeName, @AttributeValue, @DisplayOrder)
                                    select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("AttributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("DisplayOrder", data.DisplayOrder);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Count(int id)
        {
            int count = 0;


            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from ProductAttributes
                                    where ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@ProductID", id);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public bool Delete(int id, int ProductID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                                    declare @curDO int
                                    set @curDO = (select DisplayOrder from ProductAttributes where AttributeID = @AttributeID)

                                    delete from ProductAttributes where AttributeID = @AttributeID
                                    update ProductAttributes set DisplayOrder = DisplayOrder - 1 where DisplayOrder > @curDO and ProductID = @ProductID 
                                    ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("AttributeID", id);
                cmd.Parameters.AddWithValue("ProductID", ProductID);
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
        public ProductAttribute Get(int id)
        {
            ProductAttribute data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ProductAttributes where AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("AttributeID", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new ProductAttribute()
                    {
                        AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                        AttributeName = Convert.ToString(dbReader["AttributeName"]),
                        AttributeValue = Convert.ToString(dbReader["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"])
                    };
                }

                cn.Close();
            }
            return data;
        }

        public int GetDisplayOrder(int id)
        {
            int order = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select DisplayOrder from ProductAttributes where AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("AttributeID", id);
                order = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return order;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ProductAttribute> List(int id)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * 
                                from ProductAttributes
                                where ProductID = @productID
                                order by DisplayOrder asc";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", id);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new ProductAttribute()
                    {
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                        AttributeID = Convert.ToInt32(result["AttributeID"]),
                        AttributeValue = Convert.ToString(result["AttributeValue"]),
                        AttributeName = Convert.ToString(result["AttributeName"])
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
        public bool Update(ProductAttribute data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update ProductAttributes
                    set ProductID = @ProductID, AttributeName = @AttributeName,
                    AttributeValue = @AttributeValue, DisplayOrder = @DisplayOrder
                    where AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("AttributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("AttributeID", data.AttributeID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <param name="DisplayOrder"></param>
        /// <returns></returns>
        public bool UpdateDisplayOrder(int productID, int attributeID, int DisplayOrder)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"
                                  declare @currentOrder int
                                  set @currentOrder = (select DisplayOrder from ProductAttributes where AttributeID = @attributeID)

                                  declare @count int
                                  set @count = (select count(*) from ProductAttributes where ProductID = @productID)

                                  if (@DisplayOrder = @count)
                                  begin
	                                  update ProductAttributes
	                                  set DisplayOrder = @DisplayOrder
	                                  where ProductID = @productID and AttributeID = @attributeID

	                                  update ProductAttributes
	                                  set DisplayOrder = DisplayOrder - 1
	                                  where ProductID = @productID and AttributeID <> @attributeID and DisplayOrder <= @DisplayOrder and DisplayOrder > 1
                                  end
                                  else
                                  begin
	                                  update ProductAttributes
	                                  set DisplayOrder = @DisplayOrder
	                                  where ProductID = @productID and AttributeID = @attributeID

	                                  update ProductAttributes
	                                  set DisplayOrder = DisplayOrder + 1
	                                  where ProductID = @productID and DisplayOrder >= @DisplayOrder and AttributeID <> @attributeID and DisplayOrder >= @DisplayOrder
                                  end";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("productID", productID);
                cmd.Parameters.AddWithValue("attributeID", attributeID);
                cmd.Parameters.AddWithValue("DisplayOrder", DisplayOrder);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
