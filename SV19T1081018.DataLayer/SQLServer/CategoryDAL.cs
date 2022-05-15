using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SV19T1081018.DomainModel;


namespace SV19T1081018.DataLayer.SQLServer
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        private string connectionString;

        public CategoryDAL(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Category data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into Categories(CategoryName, Description) values (@categoryName, @description) select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("categoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("description", data.Description);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }

        public int Count(string searchValue="")
        {
            int count = 0;

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select count(*)
                                    from Categories
                                    where (@searchValue = N'')
                                        or(
                                                (CategoryName like @searchValue)
                                                or
                                                (Description like @searchValue)
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }

        public bool Delete(int categoryID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from Categories where CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("categoryID", categoryID);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public Category Get(int categoryID)
        {
            Category data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Categories where CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("categoryID", categoryID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new Category()
                    {
                        CategoryID = Convert.ToInt32(dbReader["CategoryId"]),
                        Description = Convert.ToString(dbReader["Description"]),
                        CategoryName = Convert.ToString(dbReader["CategoryName"]),
                    };
                }

                cn.Close();
            }

            return data;
        }

        public bool InUsed(int categoryID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from Categories as c join Products as p on c.CategoryID = p.CategoryID where c.CategoryID = @categoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("categoryID", categoryID);

                result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

                cn.Close();
            }

            return result;
        }

        public IList<Category> List(int page=1, int pageSize=0, string searchValue="")
        {
            List<Category> data = new List<Category>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select *,
                                                    row_number() over(order by CategoryID) as RowNumber
                                            from Categories
                                            where (@searchValue = N'')
                                                or(
                                                        (CategoryName like @searchValue)
                                                        or
                                                        (Description like @searchValue)
                                                    )
                                        ) as t
                                    where (@pageSize) = 0 or (t.RowNumber between(@page -1) *@pageSize + 1 and @page *@pageSize)
                                    order by t.RowNumber; ";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (result.Read())
                {
                    data.Add(new Category()
                    {
                        CategoryID = Convert.ToInt32(result["CategoryID"]),
                        CategoryName = Convert.ToString(result["CategoryName"]),
                        Description = Convert.ToString(result["Description"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(Category data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Categories
                    set CategoryName = @categoryName, Description = @description
                    where CategoryID = @CategoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("categoryName", data.CategoryName);
                cmd.Parameters.AddWithValue("description", data.Description);
                cmd.Parameters.AddWithValue("CategoryID", data.CategoryID);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
