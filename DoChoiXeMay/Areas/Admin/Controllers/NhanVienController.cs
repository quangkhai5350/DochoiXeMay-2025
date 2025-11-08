using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

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
            var model = dbc.NV_NhanVienTek
                .OrderByDescending(kh => kh.DaNghiViec)
                .ThenByDescending(kh => kh.ThuViec)
                .ThenByDescending(kh => kh.IdVitrinhanvien)
                .ThenByDescending(kh => kh.HoTen).ToList();

            for (int i = 0; i < model.Count(); i++)
            {
                model[i].STT = (i + 1).ToString();
            }
            ViewBag.GetListNV = model;
            return PartialView();
        }
        public ActionResult TinhGioCong()
        {
            Session["requestUri"] = "/Admin/NhanVien/TinhGioCong";
            ViewBag.Idnhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.Id == 2 
                            && kh.DaNghiViec==false), "Id", "HoTen");
            return View();
        }
        public ActionResult EditGioCong(string id)
        {
            var model = dbc.NV_GioCong.Find(new Guid(id));
            return PartialView(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditGioCong(NV_GioCong model, TimeSpan VaoSang, TimeSpan RaSang, TimeSpan VaoChieu
            , TimeSpan RaChieu, TimeSpan VaoTangCa, TimeSpan RaTangCa, TimeSpan VaoLe, TimeSpan RaLe)
        {
            //đã thanh toán lương, thì không cho update giờ
            //Update giờ thì thêm 1 dòng vào bảng Công (hoặc thay đổi)
            //Update giờ thì thêm 1 dòng vào bảng thanh toán lương (hoặc thay đổi)
            var kq = new Data.NhanVien().UpdateGioCong(model, VaoSang, RaSang, VaoChieu
            , RaChieu, VaoTangCa, RaTangCa, VaoLe, RaLe);
            if (kq)
            {
                Session["ThongBaoGioCongTEK"] = "Update giờ ngày "+model.Day+" thành công.";
                Session["ThongBaoGioCongTEKLoi"] = "";
            }
            else
            {
                Session["ThongBaoGioCongTEK"] = "";
                Session["ThongBaoGioCongTEKLoi"] = "Có Lỗi,Update giờ ngày "+model.Day+" không thành công !!!.";
            }
            //tro lai trang truoc do 
            var requestUri = Session["requestUri"] as string;
            if (requestUri != null)
            {
                return Redirect(requestUri);
            }
            return RedirectToAction("TinhGioCong");
        }
        static String TangCa = ConfigurationManager.AppSettings["TangCa"];
        static String Le = ConfigurationManager.AppSettings["Le"];
        public ActionResult GetListTinhGioCong(DateTime dtInput, int Id = 0)
        {
            var nv = dbc.NV_NhanVienTek.Find(Id);
            var model=dbc.NV_GioCong.Where(kh => kh.Month == dtInput.Month && kh.Year == dtInput.Year
                                && kh.IdNhanVien == Id && kh.Day <= DateTime.Now.Day)
                        .OrderByDescending(kh=>kh.Day)
                        .ToList();
            for(int i=0; i<model.Count(); i++)
            {
                var kqT = (model[i].GioRaSang - model[i].GioVaoSang+
                    (model[i].GioRaChieu - model[i].GioVaoChieu)).TotalHours;
                var kqTC = (model[i].GioRaTangCa - model[i].GioVaoTangCa).TotalHours * float.Parse(TangCa);
                var kqLe = (model[i].GioRaTangCaLe - model[i].GioVaoTangCaLe).TotalHours * float.Parse(Le);
                model[i].GhiChu = (kqT+kqTC + kqLe).ToString("0.00");
            }
            ViewBag.NgayGioCong = model;
            ViewBag.Hoten = nv.HoTen;
            return PartialView(model);
        }
        public ActionResult InsertAutoNgayThang(DateTime dtInput, int Id=0)
        {
            //(chưa nghỉ việc && Ngày vào cty <= dtInput) && dtInput.Month <= thanght && dtInput.Year<=namht
            if(dtInput.Month <= DateTime.Now.Month && dtInput.Year<= DateTime.Now.Year)
            {
                var ktnv = dbc.NV_NhanVienTek.FirstOrDefault(kh => kh.DaNghiViec == false
                && kh.NgayTao.Month <= dtInput.Month && kh.Id ==Id);
                if (ktnv != null) {
                    var ser = new Data.NhanVien().InsertNgayGioCongAuto(DBname, dtInput, Id);
                    return Json(ser, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult AddNhanVienAuto()
        {
            var kq = new Data.NhanVien().InsertNhanVienAuto(DBname);
            if (kq)
            {
                Session["ThongBaoNhanVienTEKLoi"] = "";
                Session["ThongBaoNhanVienTEK"] = "Thêm mới nhân viên thành công, cần update để sử dụng.";
                return RedirectToAction("ListNhanVien");
            }
            else
            {
                Session["ThongBaoNhanVienTEK"] = "";
                Session["ThongBaoNhanVienTEKLoi"] = "Thêm mới nhân viên thất bại !!!.";
                return RedirectToAction("ListNhanVien");
            }
        }
        public ActionResult DeleteNhanVien(int Id)
        {
            try
            {
                var model = dbc.NV_NhanVienTek.Find(Id);
                if (model.DaNghiViec)
                {
                    dbc.NV_NhanVienTek.Remove(model);
                    dbc.SaveChanges();
                    Session["ThongBaoNhanVienTEKLoi"] = "";
                    Session["ThongBaoNhanVienTEK"] = "Delete nhân viên thành công";
                }
                else
                {
                    Session["ThongBaoNhanVienTEKLoi"] = "Nhân viên Chưa nghỉ việc, Delete nhân viên Không thành công !!!";
                    Session["ThongBaoNhanVienTEK"] = "";
                }
                //Nhân Viên chưa nghỉ việc thì không cho delete


                return RedirectToAction("ListNhanVien");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                Session["ThongBaoNhanVienTEK"] = "";
                Session["ThongBaoNhanVienTEKLoi"] = "Delete nhân viên thất bại !!!.";
                return RedirectToAction("ListNhanVien");
            }

        }
        public ActionResult UpdateNhanVien(int Id)
        {
            var model = dbc.NV_NhanVienTek.Find(Id);
            ViewBag.IdVitrinhanvien = new SelectList(dbc.NV_Vitrinhanvien.ToList(), "Id", "TenVitri", model.IdVitrinhanvien);
            ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateNhanVien(NV_NhanVienTek model)
        {
            try
            {
                var file1 = Request.Files["Dinhkem1"];
                var file2 = Request.Files["Dinhkem2"];
                var file3 = Request.Files["Dinhkem3"];
                if (file1.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgTeK/", model.HinhDaiDien);
                    model.HinhDaiDien = XstringAdmin.saveFile(file1, "imgTeK/");
                }
                if (file2.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgTeK/", model.HinhCanCuocTruoc);
                    model.HinhCanCuocTruoc = XstringAdmin.saveFile(file2, "imgTeK/");
                }
                if (file3.ContentLength > 0)
                {
                    //Xoa hinh cu
                    bool xoahinhcu = XstringAdmin.Xoahinhcu("imgTeK/", model.HinhCanCuocSau);
                    model.HinhCanCuocSau = XstringAdmin.saveFile(file3, "imgTeK/");
                }
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoNhanVienTEKLoi"] = "";
                Session["ThongBaoNhanVienTEK"] = "Update nhân viên " + model.HoTen + " thành công.";
                //SMS hệ thống
                var sms = "Update thông tin nhân viên " + model.HoTen + ", thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListNhanVien");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ViewBag.IdVitrinhanvien = new SelectList(dbc.NV_Vitrinhanvien.ToList(), "Id", "TenVitri", model.IdVitrinhanvien);
                ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                return View(model);
            }
        }
        public ActionResult ViTriNhanVien()
        {
            Session["requestUri"] = "/Admin/NhanVien/ViTriNhanVien";
            ViewBag.ListVitri = dbc.NV_Vitrinhanvien.ToList();
            return View();
        }
        public ActionResult GetViTriNhanVien()
        {
            var model = dbc.NV_Vitrinhanvien.ToList();
            ViewBag.ListVitri = model;
            return PartialView();
        }
        public ActionResult AddVTNVAuto()
        {
            var kq = new Data.NhanVien().InsertViTriNVAuto(DBname);
            if (kq)
            {
                Session["ThongBaoVTNVTEKLoi"] = "";
                Session["ThongBaoVTNVTEK"] = "Thêm mới vị trí nhân viên thành công, cần update để sử dụng.";
                return RedirectToAction("ViTriNhanVien");
            }
            else
            {
                Session["ThongBaoVTNVTEK"] = "";
                Session["ThongBaoVTNVTEKLoi"] = "Thêm mới vị trí nhân viên thất bại !!!.";
                return RedirectToAction("ViTriNhanVien");
            }
        }
        public ActionResult UpdateViTriNhanVien(int Id)
        {
            var model = dbc.NV_Vitrinhanvien.Find(Id);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateViTriNhanVien(NV_Vitrinhanvien model)
        {
            try
            {
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                Session["ThongBaoVTNVTEKLoi"] = "";
                Session["ThongBaoVTNVTEK"] = "Update vị trí " + model.TenVitri + " thành công.";
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ViTriNhanVien");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                return View(model);
            }
        }
    }
}