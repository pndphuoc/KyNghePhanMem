using SV19T1081018.DataLayer;
using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.BusinessLayer
{
    public static class AccountService
    {
        private static readonly IAccountDAL accountDB;
        private static readonly INguoiDungDAL nguoiDungDB;
        static AccountService()
        {
            string provider = ConfigurationManager.ConnectionStrings["DB"].ProviderName;
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            switch (provider)
            {
                case "SQLServer":
                    accountDB = new DataLayer.SQLServer.AccountDAL(connectionString);
                    nguoiDungDB = new DataLayer.SQLServer.NguoiDungDAL(connectionString);
                    break;
            }
        }
        public static NguoiDung Login(string username, string password)
        {
            return accountDB.Login(username, password);
        }

        public static NguoiDung GetNguoiDung(string username)
        {
            return nguoiDungDB.Get(username);
        }
        public static bool ChangePassword(string Username, string password)
        {
            return accountDB.ChangePassword(Username, password);
        }
        public static string getOldPassword(string Username)
        {
            return accountDB.getOldPassword(Username);
        }


    }
}
