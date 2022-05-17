using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        [Route("delete/{MaCong}/{MaNhanVien}/{Thang}/{Nam}")]
        public ActionResult Delete(int MaCong, int MaNhanVien, int Thang, int Nam)
        {

            CommonDataService.DeleteCong(MaCong);

            return RedirectToAction($"Edit/{MaNhanVien}/{Thang}/{Nam}");
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
        [Route("createcong/{MaNhanVien}/{Nam}/{Thang}/{Ngay}")]
        public ActionResult CreateCong(int MaNhanVien, int Nam, int Thang, int Ngay)
        {
            DateTime date = new DateTime(Nam, Thang, Ngay);
            Cong model = new Cong()
            {
                MaCong = 0,
                MaNhanVien = MaNhanVien,
                Ngay = date
            };

            ViewBag.Title = "Thêm công";
            return View(model);
        }

        public ActionResult CreateCong(Cong model)
        {
            ViewBag.Title = "Thêm công";
            return View(model);
        }


        [Route("editcong/{MaNhanVien}/{MaCong}")]
        public ActionResult EditCong(int MaNhanVien, int MaCong)
        {
            Cong model = new Cong();
            model = BusinessLayer.CommonDataService.GetCong(MaCong);
            return View("CreateCong", model);
        }
        [HttpPost]
        public ActionResult Save(Cong model)
        {
            if(model.ThoiGianVaoCa.Length<7)
            if (model.ThoiGianVaoCa == null)
            {
                ModelState.AddModelError("ThoiGianVaoCa", "Thời gian vào ca không được để trống");
            }
            if (model.ThoiGianKetThuc == null)
            {
                ModelState.AddModelError("ThoiGianKetThuc", "Thời gian kết thúc không được để trống");
            }
            if (!ModelState.IsValid)
            {
                return View("CreateCong", model);
            }
            if (model.MaCong > 0)
                CommonDataService.UpdateCong(model);
            else
            {
                CommonDataService.AddCong(model);
            }
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
            return RedirectToAction($"Edit/{model.MaNhanVien}/{model.Ngay.Month}/{model.Ngay.Year}");
        }
    }
}