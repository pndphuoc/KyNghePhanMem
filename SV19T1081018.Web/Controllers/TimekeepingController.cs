using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081018.Web.Controllers.NewFolder1
{
    [Authorize]
    [RoutePrefix("timekeeping")]
    public class TimekeepingController : Controller
    {
        public ActionResult Index()
        {
            CongPaginationSearchInput model = Session["NHANVIEN_CONG_SEARCH"] as CongPaginationSearchInput;

            if (model == null)
            {
                model = new CongPaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    Thang = DateTime.Now.Month,
                    Nam = DateTime.Now.Year
                };
            }
            return View(model);
        }

        public ActionResult Search(Models.CongPaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfNhanVien(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.CongPaginationResult model = new Models.CongPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Thang = input.Thang,
                Nam = input.Nam,
                Data = data
            };

            Session["NHANVIEN_CONG_SEARCH"] = input;

            return View(model);
        }
        [Route("edit/{MaNhanVien}/{Thang}/{Nam}")]
        public ActionResult Edit(int MaNhanVien, int Thang, int Nam)
        {
            ViewBag.Thang = Thang;
            ViewBag.Nam = Nam;
            ViewBag.MaNhanVien = MaNhanVien;
            ViewData["Thang"] = Thang;
            ViewData["Nam"] = Nam;
            ViewData["MaNhanVien"] = MaNhanVien;
            return View();
        }
        [Route("createcong/{MaNhanVien}/{Ngay}/{Thang}/{Nam}")]
        public ActionResult CreateCong(int MaNhanVien, int Ngay, int Thang, int Nam)
        {
            Cong model = new Cong()
            {
                MaCong = 0,
                MaNhanVien = MaNhanVien

            };

            ViewBag.Title = "Thêm công";
            return View(model);
        }


        [Route("editcong/{MaNhanVien}/{MaCong}")]
        public ActionResult EditCong (int MaNhanVien, int MaCong)
        {
            Cong model = new Cong();
            model = BusinessLayer.CommonDataService.GetCong(MaCong);
            return View("CreateCong", model);
        }

        public ActionResult Save(Cong model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.MaCong == 0 ? "Bổ sung công làm việc" : "Cập nhật công của nhân viên";
                return View("Create", model);

            }
            if (model.MaCong > 0)
                CommonDataService.UpdateCong(model);
            //else
            //{
            //    PaginationSearchInput input = new PaginationSearchInput();

            //    input = new PaginationSearchInput()
            //    {
            //        Page = 1,
            //        PageSize = 10,
            //        SearchValue = model.CustomerName
            //    };

            //    CommonDataService.AddCustomer(model);
            //    Session["CUSTOMER_SEARCH"] = input;
            //}
            return RedirectToAction($"Edit/{model.MaNhanVien}/{model.ThoiGianVaoCa.Month}/{model.ThoiGianVaoCa.Year}");
        }
    }
}