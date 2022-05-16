using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DataLayer
{
    public interface IAccountDAL
    {
        NguoiDung Login(string username, string password);
        bool ChangePassword(string email, string newPassword);
        string getOldPassword(string email);
    }
}
