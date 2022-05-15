using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into Customers(CustomerName, ContactName, Address, City, Country, PostalCode) values (@customerName, @ContactName, @Address, @City, @Country, @PostalCode) " +
                    "               select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("Address", data.Address);
                cmd.Parameters.AddWithValue("City", data.City);
                cmd.Parameters.AddWithValue("Country", data.Country);
                cmd.Parameters.AddWithValue("PostalCode", data.PostalCode);

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
                                    from Customers
                                    where (@searchValue = N'')
                                        or(
                                                (CustomerName like @searchValue)
                                                or
                                                (ContactName like @searchValue)
                                                or
                                                (Address like @searchValue)
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from Customers where CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("customerID", id);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public Customer Get(int id)
        {
            Customer data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Customers where CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("customerID", id);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new Customer()
                    {
                        CustomerID = Convert.ToInt32(dbReader["CustomerID"]),
                        CustomerName = Convert.ToString(dbReader["CustomerName"]),
                        Address = Convert.ToString(dbReader["Address"]),
                        ContactName = Convert.ToString(dbReader["ContactName"]),
                        PostalCode = Convert.ToString(dbReader["PostalCode"]),
                        City = Convert.ToString(dbReader["City"]),
                        Country = Convert.ToString(dbReader["Country"])
                    };
                }

                cn.Close();
            }

            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select case when exists(select * from Orders where CustomerId = @customerID) then 1 else 0 end";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("customerID", id);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }

        public IList<Customer> List(int page=1, int pageSize=0, string searchValue="")
        {
            List<Customer> data = new List<Customer>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select *,
                                                    row_number() over(order by CustomerName) as RowNumber
                                            from Customers
                                            where (@searchValue = N'')
                                                or(
                                                        (CustomerName like @searchValue)
                                                        or
                                                        (ContactName like @searchValue)
                                                        or
                                                        (Address like @searchValue)
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
                    data.Add(new Customer()
                    {
                        CustomerID = Convert.ToInt32(result["CustomerID"]),
                        CustomerName = Convert.ToString(result["CustomerName"]),
                        Address = Convert.ToString(result["Address"]),
                        ContactName = Convert.ToString(result["ContactName"]),
                        PostalCode = Convert.ToString(result["PostalCode"]),
                        City = Convert.ToString(result["City"]),
                        Country = Convert.ToString(result["Country"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Customers
                    set CustomerName = @customerName, ContactName = @contactName, Address = @address,
                    City = @city, Country = @country, PostalCode = @postalCode
                    where CustomerID = @customerID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("customerName", data.CustomerName);
                cmd.Parameters.AddWithValue("contactName", data.ContactName);
                cmd.Parameters.AddWithValue("address", data.Address);
                cmd.Parameters.AddWithValue("city", data.City);
                cmd.Parameters.AddWithValue("country", data.Country);
                cmd.Parameters.AddWithValue("postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("customerID", data.CustomerID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }

}
