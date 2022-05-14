using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer.SQLServer
{
    /// <summary>
    /// Lớp cơ sở (lớp cha) cho các lớp liên quan đến xử lý đữ liệu trên SQL server
    /// </summary>
    public abstract class _BaseDAL
    {
        /// <summary>
        /// Chuỗi tham số kết nối
        /// </summary>
        protected string _connectionString;

        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối đến cơ sở dữ liệu
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = _connectionString;
            cn.Open();
            return cn;
        }
    }
}
