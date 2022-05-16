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
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("Position")]
    public class PositionController : Controller
    {
        // GET: ChucVu


        /// <summary>
        /// Giao dien tim kiem
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //trong session luon luu dieu kien tim kiem vua thuc hien
            Models.PaginationSearchInput model = Session["CHUCVU_SEARCH"] as Models.PaginationSearchInput;
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
            var data = BusinessLayer.CommonDataService.ListOfChucVu(Input.Page, Input.PageSize, Input.SearchValue, out rowCount);
            Models.PositionPaginationResult model = new Models.PositionPaginationResult()
            {
                Page = Input.Page,
                PageSize = Input.PageSize,
                SearchValue = Input.SearchValue,
                RowCount = rowCount,
                Data = data
            };
            Session["CHUCVU_SEARCH"] = Input;
            return View(model);
        }



        /// <summary>
        /// them moi khach hang
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ChucVu model = new ChucVu()
            {
                MaChucVu = 0
            };

            ViewBag.Title = "Thêm mới chuc vu";
            return View(model);
        }


        /// <summary>
        /// sua khach hang
        /// </summary>
        /// <param name="machucvu"></param>
        /// <returns></returns>
        [Route("edit/{machucvu}")]
        public ActionResult Edit(int machucvu)
        {
            ChucVu model = CommonDataService.GetChucVu(machucvu);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin chucvu";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="machucvu"></param>S
        /// <returns></returns>
        [Route("delete/{machucvu}")]
        public ActionResult Delete(int machucvu)
        {
            var model = CommonDataService.GetChucVu(machucvu);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                if (CommonDataService.InUsed(machucvu))
                {
                    ViewBag.noti = 1;
                    return View(model);
                }

                else
                {
                    CommonDataService.DeleteChucVu(machucvu);
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
        public ActionResult Save(ChucVu model)
        {
            // TODO : Kiem tra du lieu dau vao
            if (string.IsNullOrWhiteSpace(model.TenChucVu))
            {
                ModelState.AddModelError("TenChucVu", "Tên chức vụ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(Convert.ToString(model.TienLuong)))
            {
                ModelState.AddModelError("TienLuong", " Tiền lương không được để trống ");
            }

            if (ModelState.IsValid)
            {
                if (model.MaChucVu > 0)
                    CommonDataService.UpdateChucVu(model);
                else
                {
                    PaginationSearchInput input = new PaginationSearchInput()

                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = model.TenChucVu
                    };
                    Session["CHUCVU_SEARCH"] = input;

                    CommonDataService.AddChucVu(model);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Title = model.MaChucVu == 0 ? "Bổ sung chuc vu" : "cap nhat chuc vu";
            return View("Create", model);
        }




    }


}
