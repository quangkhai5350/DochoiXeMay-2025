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
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            Session["requestUri"] = "/Admin/ThongKe/Index";
            var begin = dbc.ChitietXuatNhaps.Where(kh => kh.Ten == "Xi Nhan Wave TNT BLOCKX G2 ZEN 1"
                                    && kh.KyXuatNhap.XuatNhap == true).ToList();
            var daban = begin.Where(kh => kh.KyXuatNhap.HangMau == false).ToList();
            var TrongDaBan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 5).ToList();
            var TrongDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 2).ToList();
            var trongDaBanShopee = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 3).ToList();
            var trongDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var trongDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 5 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false).ToList();
            
            var KhoiDaBan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 7).ToList();
            var KhoiDaBanTikTok = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 2).ToList();
            var KhoiDaBanShopee = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 3).ToList();
            var KhoiDaBanLeNSan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == true).ToList();
            var KhoiDaBanLSiNSan = begin.Where(kh => kh.KyXuatNhap.HangMau == false && kh.IDColor == 7 && kh.KyXuatNhap.IdSan == 1 && kh.KyXuatNhap.KhachLe == false).ToList();

            var MauDaXuat = begin.Where(kh => kh.KyXuatNhap.HangMau == true).ToList();
            var KhoiMauDaXuat = begin.Where(kh => kh.KyXuatNhap.HangMau == true && kh.IDColor == 7).ToList();
            var TrongMauDaXuat = begin.Where(kh => kh.KyXuatNhap.HangMau == true && kh.IDColor == 7).ToList();
            ViewBag.TongXiNhanGen1Tek = dbc.HangHoas.Where(kh => kh.Id == 55 || kh.Id == 56).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongTK = dbc.HangHoas.Where(kh => kh.Id == 56).Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiTK = dbc.HangHoas.Where(kh => kh.Id == 55).Sum(kh => kh.SoLuong);
            
            ViewBag.TongXiNhanGen1DaBan = daban ==null?0: daban.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongDaBan = TrongDaBan==null?0:TrongDaBan.Sum(kh => kh.SoLuong);

            ViewBag.TongXiNhanGen1TrongDaBanTikTok = TrongDaBanTikTok == null ? 0 : TrongDaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1trongDaBanShopee = trongDaBanShopee == null ? 0 : trongDaBanShopee.Sum(kh => kh.SoLuong);
            
            ViewBag.TongXiNhanGen1trongDaBanLeNSan = trongDaBanLeNSan == null ? 0 : trongDaBanLeNSan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1trongDaBanSiNSan = trongDaBanLSiNSan == null ? 0 : trongDaBanLSiNSan.Sum(kh => kh.SoLuong);


            ViewBag.TongXiNhanGen1KhoiDaBan = KhoiDaBan == null ? 0 : KhoiDaBan.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanTikTok = KhoiDaBanTikTok == null ? 0 : KhoiDaBanTikTok.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanShopee = KhoiDaBanShopee == null ? 0 : KhoiDaBanShopee.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiDaBanSiNSan = KhoiDaBanLSiNSan == null ? 0 : KhoiDaBanLSiNSan.Sum(kh => kh.SoLuong);

            ViewBag.TongXiNhanGen1KhoiDaBanLeNSan = KhoiDaBanLeNSan == null ? 0 : KhoiDaBanLeNSan.Sum(kh => kh.SoLuong);

            ViewBag.TongXiNhanGen1MauDaXuat = MauDaXuat == null ? 0 : MauDaXuat.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1KhoiMauDaXuat = KhoiMauDaXuat == null ? 0 : KhoiMauDaXuat.Sum(kh => kh.SoLuong);
            ViewBag.TongXiNhanGen1TrongMauDaXuat = TrongMauDaXuat == null ? 0 : TrongMauDaXuat.Sum(kh => kh.SoLuong);
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