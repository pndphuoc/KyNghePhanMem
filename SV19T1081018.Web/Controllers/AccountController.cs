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
            if (acc != null)
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
            var model = (NguoiDung)Session["Account"];
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
        [HttpPost]
        public ActionResult Save(string oldPassword, string newPassword, string reNewPassword)
        {
            //Mã hóa các mật khẩu nhập vào
            //oldPassword = encodedPassword(oldPassword);
            //newPassword = encodedPassword(newPassword);
            //reNewPassword = encodedPassword(reNewPassword);

            //Lấy session nhân viên 
            NguoiDung acc = Session["Account"] as NguoiDung;
            //Nếu không có thì get từ User.Identity.Name (email của nhân viên)           
            if (acc == null)
            {
                acc = BusinessLayer.AccountService.GetNguoiDung(Convert.ToString(User.Identity.Name));
                Session["Account"] = acc;
            }
            if (newPassword != reNewPassword)
                ModelState.AddModelError("NewPassword", "Nhập Lại Mật Khẩu");
            if (oldPassword != acc.Password)
                ModelState.AddModelError("Password", "Mật khẩu cũ không đúng");
            else
            {
                if (newPassword == acc.Password)
                    ModelState.AddModelError("DuPassword", "Mật khẩu mới không được giống với mật khẩu cũ");
            }
            if (!ModelState.IsValid)
            {
                return View("ChangePassword");
            }
            bool result = BusinessLayer.AccountService.ChangePassword(User.Identity.Name, newPassword);
            acc.Password = newPassword;
            Session["Account"] = acc;
            return RedirectToAction("Index", "Home");
        }
    }
}