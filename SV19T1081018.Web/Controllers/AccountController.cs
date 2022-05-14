using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var acc = BusinessLayer.AccountService.Login(username, password);
            if (acc!=null)
            {
                Session["Account"] = acc;
                System.Web.Security.FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.UserName = username;
            ViewBag.Message = "Đăng nhập thất bại";
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            var model = (NhanVien)Session["Account"];
            return View(model);
        }
        /// <summary>
        /// Hàm mã hóa mật khẩu
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string encodedPassword(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            password = encoded;
            return password;
        }
        /// <summary>
        /// Lưu mật khẩu mới
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="reNewPassword"></param>
        /// <returns></returns>
        //public ActionResult SaveNewPassword(string oldPassword, string newPassword, string reNewPassword)
        //{
        //    //Mã hóa các mật khẩu nhập vào
        //    oldPassword = encodedPassword(oldPassword);
        //    newPassword = encodedPassword(newPassword);
        //    reNewPassword = encodedPassword(reNewPassword);

        //    //Lấy session nhân viên 
        //    NguoiDung acc = Session["Account"] as NguoiDung;
        //    //Nếu không có thì get từ User.Identity.Name (email của nhân viên)
        //    if (acc==null)
        //    {
        //        acc = BusinessLayer.CommonDataService.GetEmployee(Convert.ToString(User.Identity.Name));
        //        Session["Account"] = acc;
        //    }
        //    //Xử lý dữ liệu đầu vào
        //    if (newPassword != reNewPassword)
        //        ModelState.AddModelError("NewPassword", "Mật khẩu xác thực không khớp");
           
        //    if (oldPassword != acc.Password)
        //        ModelState.AddModelError("Password", "Mật khẩu cũ không đúng");
        //    else
        //    {
        //        if (newPassword == acc.Password)
        //            ModelState.AddModelError("DuplicatedPassword", "Mật khẩu mới không được giống với mật khẩu cũ");
        //    }
        //    //Khi dữ liệu đầu vào lỗi
        //    if (!ModelState.IsValid)
        //    {
        //        return View("ChangePassword");
        //    }
        //    //Đổi mật khẩu
        //    bool result = BusinessLayer.AccountService.ChangePassword(User.Identity.Name, newPassword);
        //    //Gán session bằng Employee vừa đổi mật khẩu
        //    acc.Password = newPassword;
        //    Session["Account"] = acc;

        //    //Tạo session để nhảy alert đổi mật khẩu thành công 
        //    Session["Notify"] = 1;
        //    return RedirectToAction("Index", "Home");
        //}

    }
}