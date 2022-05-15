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
    [RoutePrefix("supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {

            PaginationSearchInput model = Session["SUPPLIER_SEARCH"] as PaginationSearchInput;

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
        /// <summary>
        /// Tìm kiếm nhà cung cấp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfSuppliers(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.SupplierPaginationResult model = new Models.SupplierPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["SUPPLIER_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tạo mới nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Supplier model = new Supplier()
            {
                SupplierID = 0
            };

            ViewBag.Title = "Thêm mới nhà cung cấp";
            return View(model);
        }
        [Route("edit/{supplierID}")]
        public ActionResult Edit(int SupplierID)
        {
            Supplier model = CommonDataService.GetSupplier(SupplierID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View("Create", model); 
        }
        /// <summary>
        /// Xóa 1 nhà cung cấp
        /// </summary>
        /// <param name="SupplierID"></param>
        /// <returns></returns>
        [Route("delete/{supplierID}")]
        public ActionResult Delete(int SupplierID)
        {

            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteSupplier(SupplierID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(SupplierID);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        /// <summary>
        /// Lưu thông tin 1 nhà cung cấp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Supplier model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.SupplierName))
                ModelState.AddModelError("SupplierName", "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên giao dịch của nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                model.Phone = "";
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";
            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.Country))
                model.Country = "";
            if (string.IsNullOrWhiteSpace(model.Address))
                model.Address = "";

            if (ModelState.IsValid)
            {
                if (model.SupplierID > 0)
                    CommonDataService.UpdateSupplier(model);
                else
                {
                    PaginationSearchInput input = new PaginationSearchInput();

                    input = new PaginationSearchInput()
                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = model.SupplierName
                    };
                    CommonDataService.AddSupplier(model);
                    Session["SUPPLIER_SEARCH"] = input;
                }
                return RedirectToAction("Index");

            }

            ViewBag.Title = model.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật thông tin nhà cung cấp";
            return View("Create", model);
        }
    }
}