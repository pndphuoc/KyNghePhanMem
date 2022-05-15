using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SV19T1081018.BusinessLayer;
using SV19T1081018.DomainModel;
using System.Web.Mvc;

namespace SV19T1081018.Web
{
    /// <summary>
    /// Danh sách toàn bộ quốc gia
    /// </summary>
    public static class SelectListHelper
    {
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn quốc gia --"
            });
            foreach (var c in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.CountryName,
                    Text = c.CountryName
                });
            }
            return list;
        }

        public static List<SelectListItem> Categories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn loại hàng --"
            });
            foreach (var c in CommonDataService.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {
                    Value = Convert.ToString(c.CategoryID),
                    Text = c.CategoryName
                });
            }
            return list;
        }
    }
}