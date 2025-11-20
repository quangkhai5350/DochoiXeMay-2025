using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DoChoiXeMay.Controllers
{
    public class LookController : Controller
    {
        // GET: Look
        Model1 dbc = new Model1();
        static String TangCa = ConfigurationManager.AppSettings["TangCa"];
        static String Le = ConfigurationManager.AppSettings["Le"];
        public ActionResult Partime()
        {
            // Lấy ngày hiện tại
            DateTime date = DateTime.Now;
            CultureInfo ci = CultureInfo.CurrentCulture; // Sử dụng hiện tại của hệ thống
            Calendar cal = ci.Calendar;
            CalendarWeekRule rule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
            int weekNumber = cal.GetWeekOfYear(date, rule, firstDayOfWeek);

            ViewBag.TuanCu = weekNumber - 1;
            ViewBag.TuanHT = weekNumber;
            ViewBag.Tuanmoi = weekNumber + 1;
            ViewBag.Tuanmoihon = weekNumber + 2;
            ViewBag.Idnhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.DonViTinh == "Gio"
                            && kh.DaNghiViec == false), "Id", "HoTen");
            return View();
        }
        public ActionResult GetListTinhGioCong(DateTime dtInput, int Id = 0)
        {
            double giothang = 0;
            var nv = dbc.NV_NhanVienTek.Find(Id);
            var model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                var kqT = (model[i].GioRaSang - model[i].GioVaoSang +
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours;
                var kqTC = (model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa);
                var kqLe = (model[i].GioRaLe - model[i].GioVaoLe).TotalHours * float.Parse(Le);
                model[i].GhiChu = (kqT + kqTC + kqLe).ToString("0.00");
                giothang = giothang + kqT + kqTC + kqLe;
            }
            ViewBag.NgayGioCong = model;
            ViewBag.Hoten = nv.HoTen;
            ViewBag.TongSoSoGioThang = giothang;
            return PartialView(model);
        }
        public ActionResult GetListTuan(int tuanht = 0)
        {
            //Lấy số tuần hiện tại trong năm :IndexStaff
            DateTime date = DateTime.Now;
            var model = dbc.NV_LichTuanParTime.Where(kh => kh.Year == date.Year
                            && kh.SoTuanTrongNam == tuanht)
                            .ToList();
            ViewBag.GetLichTuan = model;
            return PartialView(model);
        }
    }
}