using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    public class EmployeeDAL : _BaseDAL, IEmployeeDAL
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Employee data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"insert into Employees(LastName, FirstName, BirthDate, Photo, Notes, Email) 
                //            values (@LastName, @FirstName, @BirthDate, @Photo, @Notes, @Email) 
                //            select @@identity;";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public int Count(string searchValue="")
        {
            int count = 0;

            if (searchValue != "")
                searchValue = "%" + searchValue + "%";

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"select count(*)
                //                    from Employees
                //                    where (@searchValue = N'')
                //                        or(
                //                                (FirstName like @searchValue)
                //                                or
                //                                (LastName like @searchValue)
                //                                or
                //                                (Email = @searchValue)
                //                            )";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("@searchValue", searchValue);

                //count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool Delete(int employeeID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "delete from Employees where EmployeeID = @employeeID";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("employeeID", employeeID);
                //result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public Employee Get(int employeeID)
        {
            Employee data = null;

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "select * from Employees where EmployeeID = @employeeID";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("employeeID", employeeID);

                //var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                //if (dbReader.Read())
                //{
                //    data = new Employee()
                //    {
                //        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                //        LastName = Convert.ToString(dbReader["LastName"]),
                //        FirstName = Convert.ToString(dbReader["FirstName"]),
                //        BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                //        Photo = Convert.ToString(dbReader["Photo"]),
                //        Notes = Convert.ToString(dbReader["Notes"]),
                //        Email = Convert.ToString(dbReader["Email"]),
                //        Password = Convert.ToString(dbReader["Password"])
                //    };
                //}

                cn.Close();
            }

            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public bool InUsed(int employeeID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"select count(*) from Employees as e join Orders as o on o.EmployeeID = e.EmployeeID where e.EmployeeID = @employeeID";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("employeeID", employeeID);

                //result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

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
        /// <returns></returns>
        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();

            //string searchValueEmail = searchValue;
            //if (searchValue != "")
            //    searchValue = "%" + searchValue + "%";
            //using (SqlConnection cn = OpenConnection())
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    cmd.CommandText = @"declare @searchValue nvarchar(MAX), @pageSize int, @page int
            //                    set @searchValue = @value
            //                    set @pageSize = @ps
            //                    set @page = @p
            //                    select *
            //                    from
            //                    (
	           //                      select    *, row_number() over(order by EmployeeID) as RowNumber
	           //                     from    Employees
	           //                     where  (@searchValue like N'') or (FirstName + ' ' + LastName like @searchValue) or
	           //                     (Email = @searchValue)) as t
            //                    where (@pageSize) = 0 or (t.RowNumber between(@page -1) *@pageSize + 1 and @page *@pageSize)
            //                    order by t.RowNumber;
            //                        ";
            //    cmd.CommandType = System.Data.CommandType.Text;
            //    cmd.Connection = cn;

            //    cmd.Parameters.AddWithValue("@p", page);
            //    cmd.Parameters.AddWithValue("@ps", pageSize);
            //    cmd.Parameters.AddWithValue("@value", searchValue);

            //    var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            //    while (result.Read())
            //    {
            //        data.Add(new Employee()
            //        {
            //            EmployeeID = Convert.ToInt32(result["EmployeeID"]),
            //            LastName = Convert.ToString(result["LastName"]),
            //            FirstName = Convert.ToString(result["FirstName"]),
            //            Photo = Convert.ToString(result["Photo"]),
            //            BirthDate = Convert.ToDateTime(result["BirthDate"]),
            //            Notes = Convert.ToString(result["Notes"]),
            //            Email = Convert.ToString(result["Email"]),
            //            Password = Convert.ToString(result["Password"])
            //        });
            //    }

            //    cn.Close();
            //}
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Employee data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"update Employees
                //    set LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate,
                //    Photo = @Photo, Notes = @Notes, Email = @Email
                //    where EmployeeID = @EmployeeID";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;


                //result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }

        public bool isEmailUser(string email)
        {
            int count = 0;

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = @"select count(*) from Employees where Email = @email";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("@email", email);

                //count = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }
            return count == 1;
        }

        public Employee Get(string email)
        {
            Employee data = null;

            using (SqlConnection cn = OpenConnection())
            {
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "select * from Employees where Email = @Email";
                //cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Connection = cn;

                //cmd.Parameters.AddWithValue("Email", email);

                //var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                //if (dbReader.Read())
                //{
                //    data = new Employee()
                //    {
                //        EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                //        LastName = Convert.ToString(dbReader["LastName"]),
                //        FirstName = Convert.ToString(dbReader["FirstName"]),
                //        BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                //        Photo = Convert.ToString(dbReader["Photo"]),
                //        Notes = Convert.ToString(dbReader["Notes"]),
                //        Email = Convert.ToString(dbReader["Email"]),
                //        Password = Convert.ToString(dbReader["Password"])
                //    };
                //}

                cn.Close();
            }

            return data;
        }
    }
}
