using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

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
            // Lấy ngày hiện tại
            DateTime date = DateTime.Now;
            //// Lấy số ngày trong năm
            //int dayOfYear = date.DayOfYear;
            //// Tính số tuần. Cộng thêm 6 để làm tròn lên
            //int week = (dayOfYear + 6) / 7;

            CultureInfo ci = CultureInfo.CurrentCulture; // Sử dụng hiện tại của hệ thống
            Calendar cal = ci.Calendar;
            CalendarWeekRule rule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
            int weekNumber = cal.GetWeekOfYear(date, rule, firstDayOfWeek);

            ViewBag.TuanCu = weekNumber - 1;
            ViewBag.TuanHT = weekNumber;
            ViewBag.Tuanmoi = weekNumber + 1;
            ViewBag.Tuanmoihon = weekNumber + 2;
            return View();
        }
        public ActionResult GetListTinhGioCong(DateTime dtInput)
        {
            double giothang = 0;
            var IDnv = int.Parse(Session["idNhanVien"].ToString());
            var model = dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == IDnv && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh => kh.Day)
                        .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                var kqT = double.Parse((model[i].GioRaSang - model[i].GioVaoSang +
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours.ToString("0.00"));
                var kqTC = double.Parse(((model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa)).ToString("0.00"));
                var kqLe = double.Parse(((model[i].GioRaLe - model[i].GioVaoLe).TotalHours * float.Parse(Le)).ToString("0.00"));
                model[i].GhiChu = (kqT + kqTC + kqLe).ToString();
                giothang = giothang + kqT + kqTC + kqLe;
            }
            var nv = dbc.NV_NhanVienTek.Find(IDnv);
            ViewBag.Hoten = nv.HoTen;
            ViewBag.NgayGioCong = model;
            ViewBag.TongSoSoGioThang = giothang;
            return PartialView(model);
        }
        public ActionResult UpdateGioCong(string Id)
        {
            var gio = dbc.NV_GioCong.Find(new Guid(Id));
            return View(gio);
        }
        [HttpPost]
        public ActionResult UpdateGioCong(NV_GioCong model, TimeSpan VaoSang, TimeSpan RaSang, TimeSpan VaoChieu
            , TimeSpan RaChieu, TimeSpan VaoTangCa, TimeSpan RaTangCa, TimeSpan VaoLe, TimeSpan RaLe)
        {
            var ktthanhtoan = dbc.NV_ThanhToanLuong.FirstOrDefault(kh => kh.Thang == model.Month && kh.Nam == model.Year
                                    && kh.DaNhanLuong == true && kh.IdNhanVien == model.IdNhanVien);
            if (ktthanhtoan == null)
            {
                var kq = new Areas.Admin.Data.NhanVien().UpdateGioCong(model, VaoSang, RaSang, VaoChieu
                        , RaChieu, VaoTangCa, RaTangCa, VaoLe, RaLe);
                if (kq)
                {
                    Session["ThongbaoNV"] = "Update giờ ngày " + model.Day + " thành công.";
                    Session["ThongbaoNVLoi"] = "";
                    //Lay gio Cong
                    double kqgc = 0; double kqtc = 0; double kqle = 0;
                    var modelkt = dbc.NV_GioCong.Where(kh => kh.IdNhanVien == model.IdNhanVien && kh.Month == model.Month
                                && kh.Year == model.Year && kh.Day <= DateTime.Now.Day).ToList();
                    for (int i = 0; i < modelkt.Count(); i++)
                    {
                        kqgc = kqgc + double.Parse((modelkt[i].GioRaSang - modelkt[i].GioVaoSang +
                            (modelkt[i].GioRaChieu - modelkt[i].GioVaoChieu)).TotalHours.ToString("0.00"));
                        kqtc = kqtc + double.Parse((modelkt[i].GioRaTangCa - modelkt[i].GioVaoTangCa).TotalHours.ToString("0.00"));
                        kqle = kqle + double.Parse((modelkt[i].GioRaLe - modelkt[i].GioVaoLe).TotalHours.ToString("0.00"));
                    }
                    //kiểm tra bảng Công
                    //Chưa có thì Insert, có rồi thì update
                    var TinhCongPartimeAu = new Areas.Admin.Data.Cong_ThanhToan().TinhCongAutoPartime(model, kqgc, kqtc, kqle);
                    if (TinhCongPartimeAu == false)
                    {
                        Session["ThongbaoNV"] = "";
                        Session["ThongbaoNVLoi"] = "Có lỗi Update bảng Công !!!";
                        return RedirectToAction("IndexStaff");
                    }
                    //kiểm tra bảng thanh toan luong
                    //Chưa có thì Insert, có rồi thì update
                    var dvt = dbc.NV_NhanVienTek.Find(model.IdNhanVien).NV_Vitrinhanvien.DonViTinh;
                    var updateThanhToan = new Areas.Admin.Data.Cong_ThanhToan().ThanhToanLuongAuto(model.IdNhanVien
                                                    , model.Month, model.Year, dvt, kqgc, kqtc, kqle);
                    if (updateThanhToan == false)
                    {
                        Session["ThongbaoNV"] = "";
                        Session["ThongbaoNVLoi"] = "Có lỗi update bảng thanh toán lương !!!";
                        return RedirectToAction("IndexStaff");
                    }
                }
            }
            else
            {
                Session["ThongbaoNV"] = "Tháng " + model.Month + "," + model.NV_NhanVienTek.HoTen + " đã nhận lương, không thể thay đổi giờ !!!.";
                Session["ThongbaoNVLoi"] = "";
            }

            return RedirectToAction("IndexStaff");
        }
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult InsertAutoNgayThang(DateTime dtInput)
        {
            var Id = int.Parse(Session["idNhanVien"].ToString());
            //(chưa nghỉ việc && Ngày vào cty <= dtInput) && dtInput.Month <= thanght && dtInput.Year<=namht
            if (dtInput.Month <= DateTime.Now.Month && dtInput.Year <= DateTime.Now.Year)
            {
                var ktnv = dbc.NV_NhanVienTek.FirstOrDefault(kh => kh.DaNghiViec == false
                && kh.NgayTao.Month <= dtInput.Month && kh.Id == Id);
                if (ktnv != null)
                {
                    var ser = new Areas.Admin.Data.NhanVien().InsertNgayGioCongAuto(DBname, dtInput, Id);
                    return Json(ser, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetListTuan(int tuanht=0)
        {
            //Lấy số tuần hiện tại trong năm :IndexStaff
            DateTime date = DateTime.Now;
            var Id = int.Parse(Session["idNhanVien"].ToString());
            for(int i = tuanht - 1; i <= tuanht + 2; i++)
            {
                //Tìm Số tuần i Theo year, không có thì Insert
                var gettuan = dbc.NV_LichTuanParTime.FirstOrDefault(kh => kh.Year == date.Year && kh.SoTuanTrongNam == i && kh.IdNhanVien == Id);
                if (gettuan == null)
                {
                    var kq = new Areas.Admin.Data.LichTuanNV().InsertLichTuanAuto(DBname, Id, i);
                    if(kq==false)break;
                }
                
            }
            var model = dbc.NV_LichTuanParTime.Where(kh => kh.Year == date.Year
                            && kh.SoTuanTrongNam == tuanht && kh.IdNhanVien == Id)
                            .ToList();
            ViewBag.GetLichTuan = model;
            return PartialView(model);
        }
        public ActionResult UpdateLichTuan(int week = 0)
        {
            DateTime date = DateTime.Now;
            var Id = int.Parse(Session["idNhanVien"].ToString());
            var update = dbc.NV_LichTuanParTime.FirstOrDefault(kh => kh.SoTuanTrongNam == week && kh.IdNhanVien == Id
                        && kh.Year == date.Year);
            if(update != null)
            {
                var model = dbc.NV_LichTuanParTime.Find(update.Id);
                return View(model);
            }
            Session["ThongbaoNV"] = "";
            Session["ThongbaoNVLoi"] = "Có lỗi update bảng Lịch Tuần !!!";
            return RedirectToAction("IndexStaff");
        }
        [HttpPost]
        public ActionResult UpdateLichTuan(NV_LichTuanParTime model)
        {
            try
            {
                model.Ngay = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                var kq = dbc.SaveChanges();
                Session["ThongbaoLT"] = "Update Lịch Tuần Thứ "+model.SoTuanTrongNam+ " ,thành công.";
                Session["ThongbaoLTLoi"] = "";
                return RedirectToAction("IndexStaff");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update Thất Bại !!!!" + loi);
                return View("", model);
            }
            
        }
    }
}