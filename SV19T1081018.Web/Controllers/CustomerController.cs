using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV19T1081018.Web.Models;
namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// Giao diện tìm kiếm
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PaginationSearchInput model = Session["CUSTOMER_SEARCH"] as PaginationSearchInput;


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
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfCustomers(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.CustomerPaginationResult model = new Models.CustomerPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["CUSTOMER_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tạo mới 1 khách hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Customer model = new Customer()
            {
                CustomerID = 0
            };

            ViewBag.Title = "Thêm mới khách hàng";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa một khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [Route("edit/{customerID}")]
        public ActionResult Edit(int customerID)
        {
            Customer model = CommonDataService.GetCustomer(customerID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// Xóa một khách hàng
        /// </summary>
        /// <param name="customerID"></param>S
        /// <returns></returns>
        [Route("delete/{customerID}")]
        public ActionResult Delete(int customerID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(customerID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Customer model)
        {
            PaginationSearchInput odel = Session["CUSTOMER_SEARCH"] as PaginationSearchInput;
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.CustomerName))
                ModelState.AddModelError("CustomerName", "Tên khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError("ContactName", "Tên giao dịch của khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ của khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Country))
                ModelState.AddModelError("Country", "Phải chọn quốc gia");
            if (string.IsNullOrWhiteSpace(model.City))
                model.City = "";
            if (string.IsNullOrWhiteSpace(model.PostalCode))
                model.PostalCode = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CustomerID == 0 ? "Bổ sung khách hàng " : "Cập nhật thông tin khách hàng";
                return View("Create", model);

            }
            if (model.CustomerID > 0)
                CommonDataService.UpdateCustomer(model);
            else
            {
                PaginationSearchInput input = new PaginationSearchInput();

                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = model.CustomerName
                };

                CommonDataService.AddCustomer(model);
                Session["CUSTOMER_SEARCH"] = input;
            }
            return RedirectToAction("Index");

        }
    }
}