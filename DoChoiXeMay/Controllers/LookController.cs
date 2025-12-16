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
            ViewBag.TuanCu4 = weekNumber - 4;
            ViewBag.TuanCu3 = weekNumber - 3;
            ViewBag.TuanCu2 = weekNumber - 2;
            ViewBag.TuanCu1 = weekNumber - 1;
            ViewBag.TuanHT = weekNumber;
            ViewBag.Tuanmoi = weekNumber + 1;
            ViewBag.Tuanmoihon = weekNumber + 2;
            ViewBag.Idnhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.DonViTinh == "Gio"
                            && kh.DaNghiViec == false), "Id", "HoTen");
            //16thang12
            ViewBag.IdSan = new SelectList(dbc.SanThuongMais.Where(kh => kh.SuDung == true), "Id", "TenSan");
            ViewBag.IdLoaiHangXN = new SelectList(dbc.KyXuatNhap_LoaiHang.ToList(), "Id", "TenLoai");
            ViewBag.TrongTon = dbc.HangHoas.Where(kh => kh.Id == 56).Sum(kh => kh.SoLuong);
            ViewBag.KhoiTon = dbc.HangHoas.Where(kh => kh.Id == 55).Sum(kh => kh.SoLuong);
            return View();
        }
        public ActionResult GetListKyXNTeK(string ngay = "", string strk = "", int idLHXN = 0, int IdSan = 0, int Iddoitra = 0, int PageNo = 0, int PageSize = 8, int UserId = 0)
        {
            strk = strk.ToLower().Trim();
            ViewBag.KyXNTeK = new Areas.Admin.Data.XuatNhapData().getXuatNhapTek(ngay, strk, idLHXN, IdSan, Iddoitra, PageNo, PageSize, UserId);
            return PartialView();
        }
        public ActionResult GetPageCountXNTek(string ngay = "", string strk = "", int idLHXN = 0, int IdSan = 0, int Iddoitra = 0, int PageSize = 8, int UserId = 0)
        {
            var num = new Areas.Admin.Data.XuatNhapData().GetPageCountXuatNhapTek(ngay, strk, idLHXN, IdSan, Iddoitra, UserId);
            var pageCount = Math.Ceiling(1.0 * num / PageSize);
            return Json(pageCount, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadLoaiKyXN()
        {
            var IdLoaiHangXN = dbc.KyXuatNhap_LoaiHang.
                            Select(kh => new { id = kh.Id, ten = kh.TenLoai });

            return Json(IdLoaiHangXN, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadSanTM()
        {
            var IdSan = dbc.SanThuongMais.Where(kh => kh.SuDung == true).
                            Select(kh => new { id = kh.Id, ten = kh.TenSan });

            return Json(IdSan, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTinhGioCong(DateTime dtInput, int Id = 0)
        {
            double giothang = 0;
            DateTime date = DateTime.Now;
            List<NV_GioCong> model = new List<NV_GioCong>();
            var nv = dbc.NV_NhanVienTek.Find(Id);
            if (dtInput.Month == date.Month && dtInput.Year == date.Year)
            {
                model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            }
            else
            {
                model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            }
            for (int i = 0; i < model.Count(); i++)
            {
                var kqT = double.Parse((model[i].GioRaSang - model[i].GioVaoSang +
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours.ToString("0.00"));
                var kqTC = double.Parse(((model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa)).ToString("0.00"));
                var kqLe = double.Parse(((model[i].GioRaLe - model[i].GioVaoLe).TotalHours * float.Parse(Le)).ToString("0.00"));
                model[i].GhiChu = (kqT + kqTC + kqLe).ToString();
                kqT = kqT > 0?kqT: 0;
                kqTC = kqTC > 0?kqTC: 0;
                kqLe = kqLe > 0?kqLe: 0;
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
                            .OrderByDescending(kh=>kh.NV_NhanVienTek.HoTen)
                            .ToList();
            ViewBag.GetLichTuan = model;
            return PartialView(model);
        }
    }
}