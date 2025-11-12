using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class Cong_ThanhToanController : Controller
    {
        // GET: Admin/Cong_ThanhToan
        Model1 dbc = new Model1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListThanhToan()
        {
            Session["requestUri"] = "/Admin/Cong_ThanhToan/ListThanhToan";
            return View();
        }
        public ActionResult GetListThanhToan()
        {
            ViewBag.GetThanhToan = dbc.NV_ThanhToanLuong
                .OrderByDescending(kh => kh.NV_NhanVienTek.DaNghiViec)
                .ThenByDescending(kh => kh.NV_NhanVienTek.NV_Vitrinhanvien.DonViTinh)
                .ThenByDescending(kh => kh.ThucLinh)
                .ThenByDescending(kh => kh.NV_NhanVienTek.NgayTao)
                .ToList();
            return PartialView();
        }
        public ActionResult ListCong()
        {
            Session["requestUri"] = "/Admin/Cong_ThanhToan/ListCong";
            var addcongauto = new Data.Cong_ThanhToan().AddCongAutoChinhThuc();
            if(addcongauto == false)
            {
                Session["ThongBaoCongTEKLoi"] = "Có Lỗi Insert Công NV Chính Thức!!!";
                Session["ThongBaoCongTEK"] = "";
            }
            return View();
        }
        public ActionResult GetListCong() { 
            ViewBag.GetListCong = dbc.NV_Cong
                .OrderByDescending(kh => kh.NV_NhanVienTek.DaNghiViec)
                .ThenByDescending(kh=>kh.NV_NhanVienTek.NV_Vitrinhanvien.DonViTinh)
                .ThenByDescending(kh=>kh.NV_NhanVienTek.Id)
                .ThenByDescending(kh=>kh.NV_NhanVienTek.NgayTao)
                .ToList();
            return PartialView();
        }
        public ActionResult UpdateThanhToanLuong(Guid Id)
        {
            var model = dbc.NV_ThanhToanLuong.Find(Id);
            Session["HotenTT"] = model.NV_NhanVienTek.HoTen;
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateThanhToanLuong(NV_ThanhToanLuong model)
        {
            try
            {
                model.ThucLinh= model.TienCong + model.TienCom + model.PCGiaoHang + model.PCXangXe + model.PCChucVu 
                    + model.PCKhac + model.Thuong - model.KhauTruBH - model.DaUngLuong + model.PCKhac;
                model.NgayUpdate = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();

                Session["ThongBaoThanhToanTEK"] = "Update Lương nv: " + Session["HotenTT"].ToString() + ", thành công.";
                Session["ThongBaoThanhToanTEKLoi"] = "";
                //SMS hệ thống
                var sms = "Update Lương nv: " + Session["HotenTT"].ToString() + ", thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListThanhToan");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                var model1 = dbc.NV_ThanhToanLuong.Find(model.Id);
                return View(model1);
            }
        }
        public ActionResult UpdateCong(int Id)
        {
            var model = dbc.NV_Cong.Find(Id);
            Session["Hotennv"] = model.NV_NhanVienTek.HoTen;
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateCong(NV_Cong model)
        {
            try
            {
                model.NgayUpdate = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                dbc.SaveChanges();
                //Update thanh Toán Lương Auto
                var dvt = dbc.NV_NhanVienTek.Find(model.IdNhanVien).NV_Vitrinhanvien.DonViTinh;
                var updateThanhToan = new Data.Cong_ThanhToan().ThanhToanLuongAuto(model.IdNhanVien,model.Thang
                        ,model.Nam, dvt, model.SoGioCongThang, model.SoGioTangCaThang, model.SoGioLeThang);

                Session["ThongBaoCongTEK"] = "Update Công nv " + Session["Hotennv"].ToString() + ", thành công.";
                Session["ThongBaoCongTEKLoi"] = "";
                //SMS hệ thống
                var sms = "Update Công nv " + Session["Hotennv"].ToString() + ", thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("ListCong");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                ModelState.AddModelError("", "Update Thất Bại !!!!" + message);
                var model1 = dbc.NV_Cong.Find(model.Id);
                return View(model1);
            }
        }
        
    }
}