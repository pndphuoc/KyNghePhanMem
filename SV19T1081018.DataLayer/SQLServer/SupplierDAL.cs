using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }
        public int Add(Supplier data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Suppliers(SupplierName, ContactName, Address, City, Country, PostalCode, Phone) 
                        values (@SupplierName, @ContactName, @Address, @City, @Country, @PostalCode, @Phone) 
                        select @@identity;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("SupplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("ContactName", data.ContactName);
                cmd.Parameters.AddWithValue("Address", data.Address);
                cmd.Parameters.AddWithValue("City", data.City);
                cmd.Parameters.AddWithValue("Country", data.Country);
                cmd.Parameters.AddWithValue("PostalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("Phone", data.Phone);
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
                                    from Suppliers
                                    where (@searchValue = N'')
                                        or(
                                                (SupplierName like @searchValue)
                                                or
                                                (ContactName like @searchValue)
                                                or
                                                (Address like @searchValue)
                                                or
                                                (Phone like @searchValue)
                                            )";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);

                count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }

        public bool Delete(int supplierID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from Suppliers where SupplierID = @SupplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("SupplierID", supplierID);
                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public Supplier Get(int supplierID)
        {
            Supplier data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Suppliers where SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("SupplierID", supplierID);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (dbReader.Read())
                {
                    data = new Supplier()
                    {
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        SupplierName = Convert.ToString(dbReader["SupplierName"]),
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

        public bool InUsed(int supplierID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) from Suppliers as s join Products as p on s.SupplierID = p.SupplierID where s.SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("supplierID", supplierID);

                result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

                cn.Close();
            }

            return result;
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data = new List<Supplier>();

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select *
                                    from
                                        (
                                            select *,
                                                    row_number() over(order by SupplierID) as RowNumber
                                            from Suppliers
                                            where (@searchValue = N'')
                                                or(
                                                        (SupplierName like @searchValue)
                                                        or
                                                        (ContactName like @searchValue)
                                                        or
                                                        (Address like @searchValue)
or
                                                        (Phone like @searchValue)
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
                    data.Add(new Supplier()
                    {
                        SupplierID = Convert.ToInt32(result["SupplierID"]),
                        SupplierName = Convert.ToString(result["SupplierName"]),
                        ContactName =Convert.ToString(result["ContactName"]),
                        Address = Convert.ToString(result["Address"]),
                        City = Convert.ToString(result["City"]),
                        Country = Convert.ToString(result["Country"]),
                        Phone = Convert.ToString(result["Phone"]),
                        PostalCode = Convert.ToString(result["PostalCode"])
                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(Supplier data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"update Suppliers 
                    set SupplierName = @supplierName, ContactName = @contactName, Address = @address,
                    City = @city, Country = @country, PostalCode = @postalCode, Phone = @Phone
                    where SupplierID = @supplierID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("supplierName", data.SupplierName);
                cmd.Parameters.AddWithValue("contactName", data.ContactName);
                cmd.Parameters.AddWithValue("address", data.Address);
                cmd.Parameters.AddWithValue("city", data.City);
                cmd.Parameters.AddWithValue("country", data.Country);
                cmd.Parameters.AddWithValue("postalCode", data.PostalCode);
                cmd.Parameters.AddWithValue("Phone", data.Phone);
                cmd.Parameters.AddWithValue("supplierID", data.SupplierID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
