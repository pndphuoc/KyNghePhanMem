using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV19T1081018.DomainModel;
using SV19T1081018.BusinessLayer;
using SV19T1081018.Web.Models;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Income")]
    public class IncomeController : Controller
    {

        // GET: Category
        /// <summary>
        /// Giao diện tìm kiếm
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? Thang, int? Nam)
        {

            LuongSearchValue model = Session["LUONG_SEARCH"] as LuongSearchValue;
            if (model == null)
            {
                model = new LuongSearchValue()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    Thang = DateTime.Now.Month,
                    Nam = DateTime.Now.Year
                };
            }
            if (Thang != null || Nam != null)
            {
                model = new LuongSearchValue()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    Thang = Convert.ToInt32(Thang),
                    Nam = Convert.ToInt32(Nam)
                };
            }
            return View(model);


        }
        /// <summary>
        /// Tìm kiếm phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.LuongSearchValue input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfLuong(input.Page,
                                                        input.PageSize,
                                                        input.SearchValue,
                                                        input.Thang,
                                                        input.Nam,
                                                        out rowCount);
            Models.LuongPaginationResult model = new Models.LuongPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                Thang = input.Thang,
                Nam = input.Nam,
                RowCount = rowCount,
                Data = data
            };
            Session["LUONG_SEARCH"] = input;
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaLuong"></param>
        /// <returns></returns>
        [Route("TraLuong/{MaLuong}")]
        public ActionResult TraLuong(int MaLuong)
        {
            CommonDataService.TraLuong(MaLuong);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("ThuongPhat/{MaLuong}")]
        public ActionResult ThuongPhat(int MaLuong)
        {
            var data = CommonDataService.GetThuongPhat(MaLuong);
            Models.ThuongPhatPaginationResult model = new Models.ThuongPhatPaginationResult()
            {
                MaLuong = MaLuong,
                Data = data
            };
            return View(model);
        }
        /// <summary>
        /// thêm chỉnh sửa xóa dữ liệu mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("SetThuongPhat/{method}/{MaLuong}/{MaTienThuongPhat?}")]
        public ActionResult SetThuongPhat(string method, int MaLuong, int? MaTienThuongPhat)
        {

            TienThuongPhat model = new TienThuongPhat()
            {
                MaLuong = MaLuong
            };
            switch (method)
            {
                case "add":
                    model.MaTienThuongPhat = 0;
                    ViewBag.Title = "Bổ sung mức thưởng phạt";
                    break;
                case "edit":
                    model = CommonDataService.Get(MaTienThuongPhat);
                    ViewBag.Title = "Thay đổi mức thưởng phạt";
                    break;
                case "delete":
                    CommonDataService.Delete(MaTienThuongPhat);
                    return RedirectToAction("ThuongPhat", new { MaLuong = MaLuong });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// Lưu dữ liệu 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveThuongPhat(TienThuongPhat model)
        {
            if (string.IsNullOrWhiteSpace(model.LyDo))
                ModelState.AddModelError("LyDo", "Lý do không được để trống");
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.SoTienPhat)))
                ModelState.AddModelError("SoTienPhat", "Số tiền phạt không được để trống");


            if (ModelState.IsValid)
            {
                if (model.MaTienThuongPhat > 0)
                    CommonDataService.Update(model);
                else
                    CommonDataService.Add(model);


                return RedirectToAction("ThuongPhat", new { MaLuong = model.MaLuong });

            }
            ViewBag.Title = model.MaTienThuongPhat == 0 ? "Bổ sung thưởng phạt không thành công" : "Cập nhật thưởng phạt không thành công";
            return View("ThuongPhat");

        }
    }
}