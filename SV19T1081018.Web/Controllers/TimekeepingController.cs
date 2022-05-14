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
            PaginationSearchInput model = Session["NHANVIEN_SEARCH"] as PaginationSearchInput;

            if (model == null)
            {
                model = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };
            }
            return View(model);
        }

        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfNhanVien(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.NhanVienPaginationResult model = new Models.NhanVienPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["NHANVIEN_SEARCH"] = input;

            return View(model);
        }
    }
}