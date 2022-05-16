using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081018.Web.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save(int Mathu, int MaCaLamViec, string TenNhanVien, int MaChucVu, int MaNhanVien, int? Xoa, int? Them)
        {
            LichLamViec data = new LichLamViec()
            {
                MaThu = Mathu,
                MaCaLamViec = MaCaLamViec,
                TenNhanVien = TenNhanVien,
                MaChucVu = MaChucVu,
                MaNhanVien = MaNhanVien
            };
            if (Them == 1)
            {
                if (CommonDataService.AddLichNV(data))
                {
                    Session["note"] = 1;
                    return RedirectToAction("index", "calendar");
                }
            }
            if (Xoa == 2)
            {
                if (CommonDataService.DeleteNV(data))
                {
                    Session["note"] = 2;
                    return RedirectToAction("index", "calendar");
                }
            }

            return RedirectToAction("index", "calendar");
        }
    }
}