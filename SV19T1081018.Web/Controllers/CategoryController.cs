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
    [RoutePrefix("category")]
    public class CategoryController : Controller
    {
        // GET: Category
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PaginationSearchInput model = Session["CATEGORY_SEARCH"] as PaginationSearchInput;

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
        /// Tìm kiếm loại hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfCategories(input.Page, input.PageSize, input.SearchValue, out rowCount);
            Models.CategoryPaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["CATEGORY_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tạo mới loại hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Category model = new Category()
            {
                CategoryID = 0
            };
            ViewBag.Title = "Tạo mới loại hàng";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("edit/{categoryID}")]
        public ActionResult Edit(int categoryID)
        {
            Category model = CommonDataService.GetCategory(categoryID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin loại hàng";
            return View("Create", model);
        }
        /// <summary>
        /// Xóa 1 loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("delete/{categoryID}")]
        public ActionResult Delete(int categoryID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(categoryID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(categoryID);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        /// <summary>
        /// Lưu một loại hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Category model)
        {
            //TODO: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                ModelState.AddModelError("CategoryName", "Tên loại hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Description))
                model.Description = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
                return View("Create", model);
            }
            if (model.CategoryID > 0)
                CommonDataService.UpdateCategory(model);
            else
            {
                PaginationSearchInput input = new PaginationSearchInput();

                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = model.CategoryName
                };
                CommonDataService.AddCategory(model);
                Session["CATEGORY_SEARCH"] = input;
            }
            return RedirectToAction("Index");


        }
    }
}