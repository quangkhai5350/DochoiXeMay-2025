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
            var model =dbc.NV_NhanVienTek
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
            ViewBag.IdVitrinhanvien = new SelectList(dbc.NV_NhanVienTek.Where(kh => kh.NV_Vitrinhanvien.Id==2), "Id", "HoTen");
            return View();
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
    }
}