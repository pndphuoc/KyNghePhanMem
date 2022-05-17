using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;
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
    [RoutePrefix("Account")]
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PaginationSearchInput model = Session["ACCOUNT_SEARCH"] as PaginationSearchInput;

            if (model == null)
            {
                model = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };
            }
            // trả về một model cho trang view có tên cùng tên với Index trong employee 
            return View(model);
        }

        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            //Phương thức được lấy từ commondataservice
            var data = BusinessLayer.CommonDataService.ListOfNguoiDung(input.Page, input.PageSize, input.SearchValue, out rowCount);
            // phương thức dùng để đếm số hàng trong mỗi lần phân trang
            Models.NguoiDungPaginationResult model = new Models.NguoiDungPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["ACCOUNT_SEARCH"] = input;

            return View(model);
        }

        public ActionResult Create()
        {
            NguoiDung model = new NguoiDung()
            {
                MaNguoiDung = 0
            };

            ViewBag.Title = "Thêm mới tài khoản";
            return View(model);
        }
        [Route("edit/{MaNguoiDung}")]
        public ActionResult Edit(int MaNguoiDung)
        {
            NguoiDung model = CommonDataService.GetNguoiDung(MaNguoiDung);
            return View("Create", model);
        }
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
        [Route("account/EditInfomation/{MaNhanVien}")]
        public ActionResult EditInfomation(int MaNhanVien)
        {
            NguoiDung model = SV19T1081018.BusinessLayer.CommonDataService.GetNguoiDung(MaNhanVien);
            if (model == null)
                return RedirectToAction("Index");
            ViewBag.Title = "Thông Tin Cá Nhân";
            return View(model);
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
        public ActionResult Save(string oldPassword, string newPassword, string reNewPassword, NguoiDung model, HttpPostedFileBase uploadPhoto)
        {

            //Lấy session nhân viên 
            NguoiDung acc = Session["Account"] as NguoiDung;
            if(model!=null)
            {
                if (uploadPhoto != null)
                {
                    string path = Server.MapPath("~/images/employees");
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string uploadFilePath = System.IO.Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(uploadFilePath);
                    model.Anh = $"/Images/Employees/{fileName}";
                }
                if (model.MaNguoiDung > 0)
                {
                    CommonDataService.UpdateNguoiDung(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    PaginationSearchInput input = new PaginationSearchInput();
                    CommonDataService.AddNguoiDung(model);

                    input = new PaginationSearchInput()
                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = model.HoTen
                    };
                    Session["ACCOUNT_SEARCH"] = input;
                    return RedirectToAction("Index");
                }
            }


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