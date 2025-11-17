using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Controllers
{
    [ProtectNV]
    public class StaffTeKController : Controller
    {
        // GET: StaffTeK
        Model1 dbc = new Model1();
        static String TangCa = ConfigurationManager.AppSettings["TangCa"];
        static String Le = ConfigurationManager.AppSettings["Le"];
        public ActionResult IndexStaff()
        {
            Session["requestUri"] = "/StaffTeK/IndexStaff";
            var IDnv = int.Parse(Session["idNhanVien"].ToString());
            var nv = dbc.NV_NhanVienTek.Find(IDnv);
            ViewBag.Hoten = nv.HoTen;
            return View();
        }
        public ActionResult GetListTinhGioCong(DateTime dtInput)
        {
            var IDnv = int.Parse(Session["idNhanVien"].ToString());
            var model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == IDnv && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                var kqT = (model[i].GioRaSang - model[i].GioVaoSang +
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours;
                var kqTC = (model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa);
                var kqLe = (model[i].GioRaLe - model[i].GioVaoLe).TotalHours * float.Parse(Le);
                model[i].GhiChu = (kqT + kqTC + kqLe).ToString("0.00");
            }
            var nv = dbc.NV_NhanVienTek.Find(IDnv);
            ViewBag.Hoten = nv.HoTen;
            ViewBag.NgayGioCong = model;
            
            //ViewBag.GetListCong = dbc.NV_Cong.Where(kh=>kh.IdNhanVien== int.Parse(Session["idNhanVien"].ToString())
            //    && kh.NV_NhanVienTek.NV_Vitrinhanvien.DonViTinh=="Gio")
            //    .OrderByDescending(kh => kh.NV_NhanVienTek.DaNghiViec)
            //    .ThenByDescending(kh => kh.NV_NhanVienTek.NgayTao)
            //    .ToList();
            return PartialView(model);
        }
    }
}