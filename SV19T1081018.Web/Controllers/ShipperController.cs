using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    [RoutePrefix("shipper")]
    public class ShipperController : Controller
    {
        // GET: Shipper
        public ActionResult Index()
        {
            PaginationSearchInput model = Session["SHIPPER_SEARCH"] as PaginationSearchInput;
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
            var data = BusinessLayer.CommonDataService.ListOfShippers(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.ShipperPaginationResult model = new Models.ShipperPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["SHIPPER_SEARCH"] = input;

            return View(model);
        }
        public ActionResult Create()
        {
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };

            ViewBag.Title = "Thêm mới nhân viên giao hàng";
            return View(model);
        }
        [Route("edit/{shipperID}")]
        public ActionResult Edit(int ShipperID)
        {
            Shipper model = CommonDataService.GetShipper(ShipperID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin nhân viên giao hàng";
            return View("Create", model);
        }
        [Route("delete/{shipperID}")]
        public ActionResult Delete(int ShipperID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteShipper(ShipperID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(ShipperID);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Shipper model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError("ShipperName", "Tên shipper không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại shipper không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ShipperID == 0 ? "Bổ sung shipper" : "Cập nhật thông tin shipper";
                return View("Create", model);
            }
            if (model.ShipperID > 0)
                CommonDataService.UpdateShipper(model);
            else
            {
                PaginationSearchInput input = new PaginationSearchInput();

                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = model.ShipperName
                };
                CommonDataService.AddShipper(model);
                Session["SHIPPER_SEARCH"] = input;
            }
            return RedirectToAction("Index");




        }
    }

}
