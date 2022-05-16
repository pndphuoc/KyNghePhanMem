using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using SV19T1081018.Web.Models;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Shift")]
    public class ShiftController : Controller
    {
        // GET: ChucVu


        /// <summary>
        /// Giao dien tim kiem
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //trong session luon luu dieu kien tim kiem vua thuc hien
            Models.PaginationSearchInput model = Session["CALAMVIEC_SEARCH"] as Models.PaginationSearchInput;
            if (model == null)
            {
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };
            }
            return View(model);
        }


        /// <summary>
        /// Tim kiem
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput Input)
        {
            int rowCount = 0;
            var data = BusinessLayer.CommonDataService.ListOfCaLamViec(Input.Page, Input.PageSize, Input.SearchValue, out rowCount);
            Models.CaLamViecPaginationResult model = new Models.CaLamViecPaginationResult()
            {
                Page = Input.Page,
                PageSize = Input.PageSize,
                SearchValue = Input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["CALAMVIEC_SEARCH"] = Input;
            return View(model);
        }



        /// <summary>
        /// them moi ca lam viec
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            CaLamViec model = new CaLamViec()
            {
                MaCaLamViec = 0
            };

            ViewBag.Title = "Thêm mới ca lam viec";
            return View(model);
        }


        /// <summary>
        /// sua ca lam viec
        /// </summary>
        /// <param name="machucvu"></param>
        /// <returns></returns>
        [Route("edit/{macalamviec}")]
        public ActionResult Edit(int macalamviec)
        {
            CaLamViec model = CommonDataService.GetCaLamViec(macalamviec);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin ca lam viec";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machucvu"></param>S
        /// <returns></returns>
        [Route("delete/{macalamviec}")]
        public ActionResult Delete(int macalamviec)
        {
            var model = CommonDataService.GetCaLamViec(macalamviec);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                if (CommonDataService.InUsedCaLamViec(macalamviec))
                {
                    ViewBag.mess = 1;
                    return View(model);
                }

                else
                {
                    CommonDataService.DeleteCaLamViec(macalamviec);
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        /// <summary>
        /// luu du lieu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        public ActionResult Save(CaLamViec model)
        {
            // TODO : Kiem tra du lieu dau vao
            if (string.IsNullOrWhiteSpace(model.TenCaLamViec))
            {
                ModelState.AddModelError("TenCaLamViec", "Tên chức vụ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.ThoiGianBatDau)))
            {
                ModelState.AddModelError("ThoiGianBatDau", " không được để trống ");
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.ThoiGianKetThuc)))
            {
                ModelState.AddModelError("ThoiGianKetThuc", " không được để trống ");
            }

            if (ModelState.IsValid)
            {
                if (model.MaCaLamViec > 0)
                    CommonDataService.UpdateCaLamViec(model);
                else
                {
                    PaginationSearchInput input = new PaginationSearchInput()

                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = model.TenCaLamViec
                    };
                    Session["CALAMVIEC_SEARCH"] = input;

                    CommonDataService.AddCaLamViec(model);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Title = model.MaCaLamViec == 0 ? "Bổ sung ca lam viec" : "cap nhat ca lam viec";
            return View("Create", model);
        }
    }
}
