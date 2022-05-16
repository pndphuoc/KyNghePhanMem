using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    [RoutePrefix("employee")]
    public class EmployeeController : Controller
    {
        // GET: vào trang nhân viên
        public ActionResult Index()
        {   // phương thức lấy danh sách nhân viên rồi trả về cho trang 
            PaginationSearchInput model = Session["EMPLOYEE_SEARCH"] as PaginationSearchInput;

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
        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            //Phương thức được lấy từ commondataservice
            var data = BusinessLayer.CommonDataService.ListOfNhanVien(input.Page, input.PageSize, input.SearchValue, out rowCount);
            // phương thức dùng để đếm số hàng trong mỗi lần phân trang
            Models.NhanVienPaginationResult model = new Models.NhanVienPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["EMPLOYEE_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Chuyển vào trang tạo mới một nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            NhanVien model = new NhanVien()
            {
                MaNhanVien = 0,
                NgaySinh = DateTime.Now.Date,
                NgayVaoLam = DateTime.Today

            };

            ViewBag.Title = "Thêm mới nhân viên";
            return View(model);
        }
        /// <summary>
        ///chuyển vào trang Chỉnh sửa 1 nhân viên
        /// </summary>
        /// <returns></returns>
        [Route("edit/{MaNhanVien}")]
        public ActionResult Edit(int MaNhanVien)
        {
            NhanVien model = CommonDataService.GetNhanVien(MaNhanVien);
            //trả về  trang view create một model
            return View("Create", model);
        }





        /// <summary>
        /// Xoá một nhân viên
        /// </summary>
        /// <returns></returns>
        [Route("delete/{MaNhanVien}")]
        public ActionResult Delete(int MaNhanVien)
        {   //nếu được truyền bằng phương thức 'post' thì người dùng đã vào trang thông tin xoá nhân viên và muốn xoá nhân viên đó
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.XoaNhanVien(MaNhanVien);
                return RedirectToAction("Index");
            }
            //nếu không phải được truyền bằng phương thức 'post'
            //người Dùng nhấn vào biểu tượng "xoá" và được chuyển đến trang thông tin xoá nhân viên
            var model = CommonDataService.GetNhanVien(MaNhanVien);
            if (model == null)
                return RedirectToAction("Index");
            //trả về view có tên trùng với tên phương thức Delete
            return View(model);
        }
        /// <summary>
        /// Lưu thông tin 1 nhân viên cho cả hai trường hợp chỉnh sửa và thêm nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="BirthDateString"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(NhanVien model, string BirthDateString, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                ///Xử lý nhập ngày sinh
                DateTime birthday = DateTime.ParseExact(BirthDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ");
            }

            // xử lí các trường hợp người dùng không nhập các thông tin cần thiết
            if (string.IsNullOrWhiteSpace(model.TenNhanVien))
                ModelState.AddModelError("TenNhanVien", "Họ tên nhân viên không được để trống");

            ////if (model.BirthDate.)
            ////    model.BirthDate = DateTime.Today;
            if (string.IsNullOrWhiteSpace(model.GhiChu))
                model.GhiChu = "";
            if (string.IsNullOrWhiteSpace(model.Anh))
                model.Anh = "";
            //kiểm tra số điện thoại được người dùng nhập vào có hợp lệ hay không
            if (string.IsNullOrWhiteSpace(model.SoDienThoai))
                ModelState.AddModelError("SoDienThoai", "Số Điện Thoại không hợp lệ");
            else
            {
                if (model.SoDienThoai.Length > 10)
                {
                    ModelState.AddModelError("SoDienThoai", "Số Điện Thoại phải dưới 10 chữ số");

                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.MaNhanVien == 0 ? "Bổ sung nhân viên " : "Cập nhật thông tin nhân viên";
                return View("Create", model);

            }
            ////Xử lý ảnh
            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/employees");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Anh = $"/Images/Employees/{fileName}";
            }
            /// set ngày bắt đầu vào làm là ngày nhân  viên được tạo

            /// nếu mã nhân viên trả về lớn hơn 0 thì người dùng đang muốn cập nhật lại thông tin của nhân viên
            if (model.MaNhanVien > 0)
                CommonDataService.CapNhatNhanVien(model);
            /// nếu mã nhân viên trả về nhỏ hơn 0 thì người dùng đang muốn thêm nhân viên
            else
            {

                PaginationSearchInput input = new PaginationSearchInput();
                CommonDataService.ThemNhanVien(model);
                // trả về danh sách số nhân viên vừa được thêm
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = model.TenNhanVien
                };
                Session["EMPLOYEE_SEARCH"] = input;
            }
            // trả đến action index
            return RedirectToAction("Index");
        }
    }
}