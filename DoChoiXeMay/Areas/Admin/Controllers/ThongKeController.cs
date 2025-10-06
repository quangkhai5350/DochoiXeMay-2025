using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            Session["requestUri"] = "/Admin/ThongKe/Index";
            return View();
        }
        public ActionResult GetListKyTonKho()
        {
            List<KyTonKho> model = new List<KyTonKho>();
            model = dbc.KyTonKhoes.Where(kh=>kh.Id>1).OrderBy(kh => kh.Id).ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].STT = (i + 1).ToString();
            }
            ViewBag.KyTonKho = model.OrderByDescending(kh => kh.Id).ToList();
            
            
            return PartialView();
        }
        public ActionResult InsertKyTonKho()
        {
            try
            {
                KyTonKho model = new KyTonKho();
                model.TenKy = "Kỳ Auto";
                model.LuuKho = "Auto Kho Số 1";
                model.GhiChu = "";
                model.NgayTao =DateTime.Now;
                model.SuDung = false;
                dbc.KyTonKhoes.Add(model);
                dbc.SaveChanges();
                Session["ThongBaoKyTonKhoOK"] = "Auto thêm mới kỳ tồn kho thành công, cần update để sử dụng.";
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["ThongBaoKyTonKhoLoi"] = "Auto thêm mới bị Lỗi: "+ex.Message;
                string message = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertChiTietKyTonKho(int id)
        {
            try
            {
                var kq = new Data.TonKhoData().InsertTonKhoAotu(id,DBname);
                if (kq)
                {
                    Session["ThongBaoKyTonKhoOK"] = "Auto thêm mới CT kỳ tồn kho thành công, cần update để sử dụng.";

                }
                //tro lai trang truoc do 
                var requestUri = Session["requestUri"] as string;
                if (requestUri != null)
                {
                    return Redirect(requestUri);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Session["ThongBaoKyTonKhoLoi"] = "Auto thêm mới Chi Tiết Kỳ bị Lỗi: " + ex.Message;
                string message = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateChiTietKyTonKho(int id)
        {
            var model = dbc.KyTonKhoes.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateChiTietKyTonKho(KyTonKho modelTK)
        {
            KyTonKho model = new KyTonKho();
            model = modelTK;

            return View(model);
        }
    }
}