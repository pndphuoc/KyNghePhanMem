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
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? supplierID, int? categoryID)
        {
            ProductPaginationSearchInput model = Session["PRODUCT_SEARCH"] as ProductPaginationSearchInput;


            if (model == null)
            {

                model = new ProductPaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0
                };

            }

            if (supplierID != null || categoryID != null)
            {
                model = new ProductPaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    CategoryID = categoryID != null ? (int)categoryID : 0,
                    SupplierID = supplierID != null ? (int)supplierID : 0,
                };
            }
            return View(model);
        }
        /// <summary>
        /// Tìm kiếm một mặt hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.ProductPaginationSearchInput input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfProduct(input.Page, input.PageSize, input.SearchValue, input.CategoryID, input.SupplierID, out rowCount);
            Models.ProductPaginationResult model = new Models.ProductPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                RowCount = rowCount,
                Data = data
            };

            Session["PRODUCT_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// Tạo mới một mặt hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Product model = new Product()
            {
                ProductID = 0
            };

            ViewBag.Title = "Bổ sung mặt hàng";
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa thông tin mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("edit/{productID}")]
        public ActionResult Edit(int productID)
        {
            Product model = CommonDataService.GetProduct(productID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin sản phẩm";
            return View("Edit", model);
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID}")]
        public ActionResult Delete(int productID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteProduct(productID);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetProduct(productID);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        /// <summary>
        /// Lưu thông tin mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public ActionResult Save(Product model, HttpPostedFileBase uploadPhoto)
        {
            //Xử lý ảnh
            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/product");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/product/{fileName}";
            }
            if (string.IsNullOrWhiteSpace(model.ProductName))
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
            if (model.SupplierID == 0)
                ModelState.AddModelError("SupplierID", "Nhà cung cấp không được để trống");
            if (model.CategoryID == 0)
                ModelState.AddModelError("CategoryID", "Loại hàng không được để trống");
            if (model.Price == 0 || string.IsNullOrWhiteSpace(model.Price.ToString()))
                ModelState.AddModelError("Price", "Giá không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung sản phẩm " : "Cập nhật thông tin sản phẩm";
                return View("Create", model);

            }
            if (model.ProductID > 0)
                CommonDataService.UpdateProduct(model);
            else
            {
                ProductPaginationSearchInput input = new ProductPaginationSearchInput();

                input = new ProductPaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = model.ProductName,
                    CategoryID = model.CategoryID,
                    SupplierID = model.SupplierID
                };
                CommonDataService.AddProduct(model);
                Session["PRODUCT_SEARCH"] = input;
            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Lưu thông tin ảnh của mặt hàng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>
        public ActionResult SavePhoto(ProductPhoto model, HttpPostedFileBase uploadPhoto)
        {
            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/product");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/product/{fileName}";
            }
            else if (model.Photo == null)
            {
                ModelState.AddModelError("Photo", "Phải tải ảnh lên trước");
            }

            //Đếm số lượng ảnh của mặt hàng
            int count = BusinessLayer.CommonDataService.CountPhotos(model.ProductID);



            if (string.IsNullOrWhiteSpace(model.Description))
                model.Description = "";

            if (!ModelState.IsValid)
                return View("Photo", model);

            if (model.PhotoID == 0)
            {
                if (model.DisplayOrder == 0 || model.DisplayOrder - count > 1)
                    model.DisplayOrder = count + 1;
                CommonDataService.UpdatePhotoDisplayOrder(model.ProductID, model.PhotoID, model.DisplayOrder);
                CommonDataService.AddProductPhoto(model);
            }
            else
            {
                if (model.DisplayOrder != (int)Session["DisplayOrderOfPhoto"])
                {
                    if (model.DisplayOrder == 0 || model.DisplayOrder > count)
                        model.DisplayOrder = count;
                    CommonDataService.UpdatePhotoDisplayOrder(model.ProductID, model.PhotoID, model.DisplayOrder);
                    Session["DisplayOrderOfPhoto"] = null;
                }
                CommonDataService.UpdateProductPhoto(model);
            }
            return RedirectToAction("edit/" + model.ProductID);
        }
        /// <summary>
        /// Các thao tác với ảnh của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="PhotoID"></param>
        /// <returns></returns>
        [Route("photo/{method}/{productID}/{photoID?}")]
        public ActionResult Photo(string method, int productID, int? PhotoID)
        {
            ProductPhoto model = new ProductPhoto();
            //return Json(new { mt = method, ProductID = productID }, JsonRequestBehavior.AllowGet);
            switch (method)
            {
                case "add":
                    model.PhotoID = 0;
                    model.ProductID = productID;
                    ViewBag.Title = "Bổ sung ảnh";
                    break;
                case "edit":
                    model = BusinessLayer.CommonDataService.GetProductPhoto((int)PhotoID);
                    Session["DisplayOrderOfPhoto"] = model.DisplayOrder;
                    ViewBag.Title = "Thay đổi ảnh";
                    break;
                case "delete":
                    CommonDataService.DeleteProductPhoto((int)PhotoID, productID);
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        /// <summary>
        /// Lưu thông tin thuộc tính của mặt hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SaveAttribute(ProductAttribute model)
        {
            if (string.IsNullOrWhiteSpace(model.AttributeName))
                model.AttributeName = "";

            int count = BusinessLayer.CommonDataService.CountAttributes(model.ProductID);
         
            if (model.AttributeID == 0)
            {
                if (model.DisplayOrder == 0 || model.DisplayOrder - count > 1)
                    model.DisplayOrder = count + 1;
                CommonDataService.UpdateAttributeDisplayOrder(model.ProductID, model.AttributeID, model.DisplayOrder);
                CommonDataService.AddProductAttribute(model);
            }
            else
            {
                if (model.DisplayOrder != (int)Session["DisplayOrderOfAttribute"])
                {
                    if (model.DisplayOrder == 0 || model.DisplayOrder > count)
                        model.DisplayOrder = count;
                    CommonDataService.UpdateAttributeDisplayOrder(model.ProductID, model.AttributeID, model.DisplayOrder);
                    Session["DisplayOrderOfAttribute"] = null;
                }
                CommonDataService.UpdateProductAttribute(model);
            }
            return RedirectToAction("Edit", new { productID = model.ProductID });
        }

        /// <summary>
        /// Thao tác với thuộc tính của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method, int productID, int? attributeID)
        {
            ProductAttribute model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    model.AttributeID = 0;
                    model.ProductID = productID;
                    ViewBag.Title = "Bổ sung thuộc tính";
                    break;
                case "edit":
                    model = BusinessLayer.CommonDataService.GetProductAttribute((int)attributeID);
                    ViewBag.Title = "Thay đổi thuộc tính";
                    Session["DisplayOrderOfAttribute"] = model.DisplayOrder;
                    break;
                case "delete":
                    BusinessLayer.CommonDataService.DeleteProductAttribute((int)attributeID, productID);
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}