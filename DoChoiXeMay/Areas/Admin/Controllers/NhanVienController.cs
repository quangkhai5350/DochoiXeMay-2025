using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class NhanVienController : Controller
    {
        // GET: Admin/NhanVien
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListNhanVien()
        {
            Session["requestUri"] = "/Admin/NhanVien/ListNhanVien";
            return View();
        }
        public ActionResult GetListNhanVien()
        {
            ViewBag.GetListNV = dbc.NV_NhanVienTek
                .OrderByDescending(kh => kh.DaNghiViec)
                .ThenByDescending(kh => kh.ThuViec)
                .ThenByDescending(kh => kh.IdVitrinhanvien)
                .ThenByDescending(kh => kh.HoTen).ToList();

            return PartialView();
        }
        public ActionResult TinhGioCong()
        {
            ViewBag.IdVitrinhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.Id==2), "Id", "HoTen");
            return View();
        }
    }
}