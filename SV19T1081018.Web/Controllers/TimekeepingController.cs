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
            return View();
        }
    }
}