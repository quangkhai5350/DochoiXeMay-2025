using Antlr.Runtime.Misc;
using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Windows;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator;

namespace DoChoiXeMay.Areas.Admin.Controllers
{
    [Protect]
    public class SerialController : Controller
    {
        // GET: Admin/Serial
        Model1 dbc = new Model1();
        string DBname = ConfigurationManager.AppSettings["DBname"];
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListSerialChuaIn()
        {
            //Auto active Khách Lẻ mua quá 15 ngày
            DateTime ngayHienTai = DateTime.Today;
            var ListChuaActive = dbc.ChitietXuatNhaps.Where(kh =>kh.KyXuatNhap.KhachLe==true && kh.KyXuatNhap.UPush==true &&( kh.DaActive == null || kh.DaActive != "ActiveAuto")
               && kh.SerialHop !=null && kh.SerialSP !=null && kh.IdDoiTra == 1 && DbFunctions.DiffDays(kh.NgayAuto,ngayHienTai) > 14).ToList();
            
            foreach (var kh in ListChuaActive)
            {
                var ky = dbc.KyXuatNhaps.Find(kh.IdKy);
                var kqAc= AotuActive(ky.TenKy, kh.SerialHop, kh.SerialSP, "AutoActive", "", "AutoActive");
                if (kqAc)
                {
                    kh.DaActive = "ActiveAuto";
                    var kqAuto = new XuatNhapData().UPdateChiTietKy(kh,DBname);
                }
            }
            Session["requestUri"] = "/Admin/Serial/ListSerialChuaIn";
            ViewBag.TotalSerialSPchuaIn = dbc.Ser_sp.Where(kh=>kh.DaIn==false).Count();
            ViewBag.TotalSerialBoXchuaIn = dbc.Ser_box.Where(kh => kh.DaIn == false).Count();
            return View();
        }
        public bool AotuActive(string Tenkh = "", string sBOX = "", string sSP = "", string gmail = "", string sdt = "", string khuvuc = "")
        {
            var serial = dbc.Ser_sp.FirstOrDefault(kh => kh.SerialSP == sSP && kh.DaIn == true && kh.Sudung == false);
            var serialb = dbc.Ser_box.FirstOrDefault(kh => kh.Serial == sBOX && kh.DaIn == true && kh.Sudung == false);
            
                if (serial != null)
                {
                    if (serialb != null)
                    {
                        var Ac = new ActiveData().InsertKichHoatBH(
                            serialb.Id.ToString(), serial.Id.ToString(), 1, gmail, Tenkh, sdt, khuvuc);
                        if (Ac > -1)
                        {
                            var kqBox = new SerialData().UpdateSer_box(serialb.Id.ToString(),
                                true, true, serialb.NgayUpdate, "Active");
                            var kqSP = new SerialData().UpdateSer_SP(serial.Id.ToString(),
                                true, true, serial.NgayUpdate, serial.HangTangKhongBan);
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
        }
        public ActionResult ListSerialDaIn()
        {
            Session["requestUri"] = "/Admin/Serial/ListSerialDaIn";
            ViewBag.TotalSerialSPDaIn = dbc.Ser_sp.Where(kh => kh.DaIn == true && kh.Sudung==false).Count();
            ViewBag.TotalSerialBoXDaIn = dbc.Ser_box.Where(kh => kh.DaIn == true && kh.Sudung == false).Count();
            ViewBag.IdKho = new SelectList(dbc.Khoes.Where(kh=>kh.SuDung==true).ToList(), "Id", "TenKho");
            return View();
        }
        
        public ActionResult AddNewSerial()
        {
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name",5);
            ViewBag.IdColor = new SelectList(dbc.Colors.ToList(), "Id", "TenColor");
            ViewBag.IdSize = new SelectList(dbc.Sizes.ToList(), "Id", "TenSize");
            ViewBag.Idver = new SelectList(dbc.Versions.ToList(), "Id", "VerName");
            ViewBag.IdLoai = new SelectList(dbc.Ser_LoaiHang.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewSerial(Ser_sp SerSP,int SoSerialN, string soloSTT)
        {
            ViewBag.IDMF = new SelectList(dbc.Manufacturers.Where(kh => kh.Sudung == true), "Id", "Name", 5);
            ViewBag.IdColor = new SelectList(dbc.Colors.ToList(), "Id", "TenColor");
            ViewBag.IdSize = new SelectList(dbc.Sizes.ToList(), "Id", "TenSize");
            ViewBag.Idver = new SelectList(dbc.Versions.ToList(), "Id", "VerName");
            ViewBag.IdLoai = new SelectList(dbc.Ser_LoaiHang.ToList(), "Id", "Name");
            try
            {
                //tạo S/N Box
                string loai = dbc.Ser_LoaiHang.Find(SerSP.IdLoai).Viettat;
                for (int i = 0; i < SoSerialN; i++)
                {
                    //loSanxuat =>ngaythang // loaihang = NEW
                    SerSP.LoSanXuat = soloSTT;
                    string SNB = loai + Utils.XString.MakeAotuSN(5);
                    var kt = dbc.Ser_box.Where(kh => kh.Serial.Contains(SNB)).ToList();
                    if (kt.Count() > 0)
                    {
                        //Nếu S/N bị trùng, random 50 lần
                        for (int j = 0; j < 50; j++)
                        {
                            SNB = loai + Utils.XString.MakeAotuSN(5);
                            var ktt = dbc.Ser_box.Where(kh => kh.Serial.Contains(SNB)).ToList();
                            if (ktt.Count() == 0)
                            {
                                SerSP.SerialSP = SNB;
                                break;
                            }
                        }
                    }
                    else
                    {
                        SerSP.SerialSP = SNB;
                        //string sn1 = new Data.SerialData().getImgtextBOX(SNB, true);
                        //string qr = new Data.SerialData().getQRcode(SNB);
                        //SerSP.QRcode = new Data.SerialData().getMergeImg(sn1,qr);
                    }
                    SerSP.QRcode = "";
                    SerSP.Stt = (i + 1).ToString();

                    var kq = new Data.SerialData().InsertSer_Box(SerSP.IdLoai, SerSP.NgayUpdate.ToString(), SerSP.LoSanXuat, SerSP.SerialSP, SerSP.Sudung, SerSP.Stt, SerSP.Ghichu, SerSP.QRcode);
                    if (kq == false)
                    {
                        Session["ThongBaoSerialBoxchuaIn"] = "Có lỗi Insert Serial Box.";
                        break;
                    }
                }
                //tạo S/N SP
                for (int i = 0; i < SoSerialN; i++)
                {
                    //loSanxuat =>ngaythang // 5 ký tự Random
                    SerSP.LoSanXuat = soloSTT;
                    string SN = Utils.XString.MakeAotuSN(5);
                    var kt = dbc.Ser_sp.Where(kh => kh.SerialSP.Contains(SN)).ToList();
                    if (kt.Count() > 0)
                    {
                        //Nếu S/N bị trùng, random 50 lần
                        for (int j = 0; j < 50; j++) {
                            SN= Utils.XString.MakeAotuSN(5);
                            kt = dbc.Ser_sp.Where(kh => kh.SerialSP.Contains(SN)).ToList();
                            if (kt.Count() == 0)
                            {
                                SerSP.SerialSP = SN;
                                break;
                            }
                        }
                    }
                    else
                    {
                        SerSP.SerialSP = SN;
                    }
                    SerSP.QRcode = "";
                    SerSP.Stt = (i + 1).ToString();
                    //Lấy IdSerBox mới tạo
                    var SerBoxChuaIn = dbc.Ser_box.FirstOrDefault(kh => kh.DaIn == false && kh.Stt==SerSP.Stt
                        && dbc.Ser_sp.FirstOrDefault(kk=>kk.IdSerBox==kh.Id)==null);
                    SerSP.IdSerBox = SerBoxChuaIn.Id;
                    var kq = new Data.SerialData().InsertSer_sp(SerSP);
                    if (kq==false)
                    {
                        Session["ThongBaoSerialSPchuaIn"] = "Có lỗi Insert Serial SP.";
                        break;
                    }
                }
                
                Session["ThongBaoSerialSPchuaIn"] = "Tạo mới thành công "+SoSerialN+" Serial SP.";
                Session["ThongBaoSerialBoxchuaIn"] = "Tạo mới thành công " + SoSerialN + " Serial Box.";
                //SMS hệ thống
                string sms = " đã tạo mới thành công " + SoSerialN + " Serial SP và Box.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
            }
            catch (Exception ex) {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Thêm mới Thất Bại !!!!!!!!!!. Có lỗi hệ thống");
                return View();
            }
            return RedirectToAction("ListSerialChuaIn");
        }
        public ActionResult InSNmoitaoBox(int bang=0, int soluong=0, string mayin="")
        {
            try
            {
                if (bang == 2)
                {
                    //29thang12
                    //var SN = dbc.Ser_box.Where(kh => kh.DaIn == false).OrderBy(kh => kh.NgayTao)
                    //    .Skip(0)
                    //    .Take(soluong)
                    //    .ToList();
                    //var countImg = Math.Ceiling(1.0 * SN.Count() / 2);

                    //InFromDataNew(null, SN, mayin);
                    //Session["ThongBaoSerialBoxchuaIn"] = "Đã in "+soluong+" serial, thành công.";
                    ////SMS hệ thống
                    //string sms = " đã in " + soluong + " serial Box, thành công.";
                    //new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    //    , Session["quyen"].ToString(), sms);
                }
                else
                {
                    var SN = dbc.Ser_sp.Where(kh => kh.DaIn == false).OrderBy(kh => kh.NgayTao)
                        .Skip(0)
                        .Take(soluong)
                        .ToList();
                    var countImg = Math.Ceiling(1.0 * SN.Count() / 2);

                    InFromDataNew(SN, null, mayin);
                    Session["ThongBaoSerialSPchuaIn"] = "Đã in " + soluong + " serial, thành công.";
                    //SMS hệ thống
                    string sms = " đã in " + soluong + " serial SP, thành công.";
                    new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                        , Session["quyen"].ToString(), sms);
                }
                return RedirectToAction("ListSerialChuaIn");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoSerialBoxchuaIn"] = "In thất bại !!!!!!!!!!. Có lỗi hệ thống";
                return RedirectToAction("ListSerialChuaIn");
            }
        }
        void InFromDataNew(List<Ser_sp> sp, List<Ser_box> box, string mayin)
        {
            for (int i = 0; i < sp.Count(); i++)
            {
                string sn1 = new Data.SerialData().getImgtextBOX(sp[i].SerialSP, false);
                string qr1 = new Data.SerialData().getQRcode(sp[i].SerialSP);
                Session["Img641"] = new Data.SerialData().getMergeImg(sn1, qr1);
                string sn2 = new Data.SerialData().getImgtextBOX(sp[i].Ser_box.Serial, false);
                string qr2 = new Data.SerialData().getQRcode(sp[i].Ser_box.Serial);
                Session["Img642"] = new Data.SerialData().getMergeImg(sn2, qr2);
                

                PrintDocument pd = new PrintDocument();
                //pd.DefaultPageSettings.Landscape = false;
                pd.DefaultPageSettings.Margins.Left = 4;
                pd.DefaultPageSettings.Margins.Top = 2;
                pd.DefaultPageSettings.Margins.Right = 0;
                pd.DefaultPageSettings.Margins.Bottom = 5;
                pd.OriginAtMargins = true;
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

                pd.PrinterSettings.PrinterName = mayin;
                pd.Print();
                Session.Remove("Img641"); Session.Remove("Img642");
                string sql = "Update [" + DBname + "TechZone].[dbo].[Ser_sp] set DaIn=1 where Id = '" + sp[i].Id + "'";
                var UpdateSerSP = dbc.Database.ExecuteSqlCommand(sql);
                //
                string sqlBox = "Update [" + DBname + "TechZone].[dbo].[Ser_box] set DaIn=1 where Id = '" + sp[i].Ser_box.Id + "'";
                var UpdateSerBox = dbc.Database.ExecuteSqlCommand(sqlBox);
            }
        }
        //void InFromData(List<Ser_sp> sp, List<Ser_box> box, string mayin)
        //{
        //    double countImg;
        //    if (box != null)
        //    {
        //        //Tính xem có bao nhiêu cặp?
        //        countImg = Math.Ceiling(1.0 * box.Count() / 2);
        //        for (int i = 0; i < countImg; i++)
        //        {
        //            var SNIMG = box.Skip(i * 2).Take(2).ToList();
        //            if (SNIMG.Count() == 2)
        //            {
        //                string sn1 = new Data.SerialData().getImgtextBOX(SNIMG[0].Serial, true);
        //                string qr1 = new Data.SerialData().getQRcode(SNIMG[0].Serial);
        //                Session["Img641"] = new Data.SerialData().getMergeImg(sn1, qr1);
        //                string sn2 = new Data.SerialData().getImgtextBOX(SNIMG[1].Serial, true);
        //                string qr2 = new Data.SerialData().getQRcode(SNIMG[1].Serial);
        //                Session["Img642"] = new Data.SerialData().getMergeImg(sn2, qr2);
        //                //Session["Img643"] = SNIMG[2].QRcode;
        //            }
        //            else if (SNIMG.Count() == 1)
        //            {
        //                string sn1 = new Data.SerialData().getImgtextBOX(SNIMG[0].Serial, true);
        //                string qr1 = new Data.SerialData().getQRcode(SNIMG[0].Serial);
        //                Session["Img641"] = new Data.SerialData().getMergeImg(sn1, qr1);
        //                Session["Img642"] = "NOSERIALNUMBER";
        //            }

        //            PrintDocument pd = new PrintDocument();

        //            //pd.DefaultPageSettings.Landscape = false;
        //            pd.DefaultPageSettings.Margins.Left = 4;
        //            pd.DefaultPageSettings.Margins.Top = 2;
        //            pd.DefaultPageSettings.Margins.Right = 0;
        //            pd.DefaultPageSettings.Margins.Bottom = 5;
        //            pd.OriginAtMargins = true;
        //            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

        //            pd.PrinterSettings.PrinterName = mayin;
        //            pd.Print();
        //            Session.Remove("Img641"); Session.Remove("Img642");
        //            if (SNIMG.Count() == 2)
        //            {
        //                string sql = "Update [" + DBname + "TechZone].[dbo].[Ser_box] set DaIn=1 where Id = '" + SNIMG[0].Id + "' or Id= '" + SNIMG[1].Id + "'";
        //                var UpdateSerBox = dbc.Database.ExecuteSqlCommand(sql);
        //            }
        //            else if (SNIMG.Count() == 1)
        //            {
        //                string sql = "Update [" + DBname + "TechZone].[dbo].[Ser_box] set DaIn=1 where Id = '" + SNIMG[0].Id + "'";
        //                var UpdateSerBox = dbc.Database.ExecuteSqlCommand(sql);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        countImg = Math.Ceiling(1.0 * sp.Count() / 2);
        //        for (int i = 0; i < countImg; i++)
        //        {
        //            var SNIMG = sp.Skip(i * 2).Take(2).ToList();
        //            if (SNIMG.Count() == 2)
        //            {
        //                string sn1 = new Data.SerialData().getImgtextBOX(SNIMG[0].SerialSP, false);
        //                string qr1 = new Data.SerialData().getQRcode(SNIMG[0].SerialSP);
        //                Session["Img641"] = new Data.SerialData().getMergeImg(sn1, qr1);
        //                string sn2 = new Data.SerialData().getImgtextBOX(SNIMG[1].SerialSP, false);
        //                string qr2 = new Data.SerialData().getQRcode(SNIMG[1].SerialSP);
        //                Session["Img642"] = new Data.SerialData().getMergeImg(sn2, qr2);
        //                //Session["Img643"] = SNIMG[2].QRcode;
        //            }
        //            else if (SNIMG.Count() == 1)
        //            {
        //                string sn1 = new Data.SerialData().getImgtextBOX(SNIMG[0].SerialSP, false);
        //                string qr1 = new Data.SerialData().getQRcode(SNIMG[0].SerialSP);
        //                Session["Img641"] = new Data.SerialData().getMergeImg(sn1,qr1);
        //                Session["Img642"] = "NOSERIALNUMBER";
        //            }

        //            PrintDocument pd = new PrintDocument();
        //            //pd.DefaultPageSettings.Landscape = false;
        //            pd.DefaultPageSettings.Margins.Left = 4;
        //            pd.DefaultPageSettings.Margins.Top = 2;
        //            pd.DefaultPageSettings.Margins.Right = 0;
        //            pd.DefaultPageSettings.Margins.Bottom = 5;
        //            pd.OriginAtMargins = true;
        //            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

        //            pd.PrinterSettings.PrinterName = mayin;
        //            pd.Print();
        //            Session.Remove("Img641"); Session.Remove("Img642");
        //            if (SNIMG.Count() == 2)
        //            {
        //                string sql = "Update [" + DBname + "TechZone].[dbo].[Ser_sp] set DaIn=1 where Id = '" + SNIMG[0].Id + "' or Id= '" + SNIMG[1].Id + "'";
        //                var UpdateSerBox = dbc.Database.ExecuteSqlCommand(sql);
        //            }
        //            else if (SNIMG.Count() == 1)
        //            {
        //                string sql = "Update [" + DBname + "TechZone].[dbo].[Ser_sp] set DaIn=1 where Id = '" + SNIMG[0].Id + "'";
        //                var UpdateSerBox = dbc.Database.ExecuteSqlCommand(sql);
        //            }
        //        }
        //    }
        //}
        void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            if(Session["Img641"] != null && Session["Img642"]!=null)
            {
                //string imgscale = new Data.SerialData().getMerge3Img
                //    (Session["Img641"].ToString(), Session["Img642"].ToString(), Session["Img643"].ToString());
                string imgscale = new Data.SerialData().getMerge2Img
                    (Session["Img641"].ToString(), Session["Img642"].ToString());
                Image img64 = new Data.SerialData().getScaleImg2(imgscale);
                ev.Graphics.DrawImage(img64, ev.MarginBounds);
            }
        }
        public ActionResult DeleteSerialSP()
        {
            try
            {
                var kt = dbc.Ser_sp.Where(kh=>kh.DaIn==false).ToList();
                if (kt.Count() == 0)
                {
                    Session["ThongBaoSerialSPchuaIn"] = "Không còn S/N SP nào chưa in để Xóa.";
                    return RedirectToAction("ListSerialChuaIn");
                }
                string sql = "DELETE  FROM [" + DBname + "TechZone].[dbo].[Ser_sp] where DaIn=0";
                var XoaChitietXN = dbc.Database.ExecuteSqlCommand(sql);
                Session["ThongBaoSerialSPchuaIn"] = "Xóa tất cả S/N SP thành công.";
                //SMS hệ thống
                string sms = "  đã Xóa tất cả S/N SP chưa In, thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                return RedirectToAction("ListSerialChuaIn");
            }
            catch (Exception ex) {
                string loi = ex.ToString();
                Session["ThongBaoSerialSPchuaIn"] = "Có Lỗi hệ thống khi xóa S/N SP !!!";
                return RedirectToAction("ListSerialChuaIn");
            }
        }
        public ActionResult XoaserialSP(string serialsp)
        {
            try
            {
                if (serialsp.Length == 11)
                {
                    var sp = dbc.Ser_sp.FirstOrDefault(kh => kh.SerialSP == serialsp && kh.Sudung == false && kh.DaIn==true);
                    if (sp == null)
                    {
                        Session["ThongBaoSerialSPDaIn"] = "Serial này không tồn tại, hoặc đã kích hoạt, không Xóa Được !!!";
                        return RedirectToAction("ListSerialDaIn");
                    }
                    else
                    {
                        string sql = "DELETE  FROM [" + DBname + "TechZone].[dbo].[Ser_sp] where SerialSP='"+sp.SerialSP+"'";
                        var Xoaserialsp = dbc.Database.ExecuteSqlCommand(sql);
                        Session["ThongBaoSerialSPDaIn"] = "Xóa thành công serial: " + serialsp + ".";
                        //SMS hệ thống
                        string sms = " đã Xóa thành công serial: " + serialsp + ".";
                        new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                            , Session["quyen"].ToString(), sms);
                        return RedirectToAction("ListSerialDaIn");
                    }
                }
                else if(serialsp.Length == 14)
                {
                    var box = dbc.Ser_box.FirstOrDefault(kh => kh.Serial == serialsp && kh.Sudung == false && kh.DaIn == true);
                    if (box == null)
                    {
                        Session["ThongBaoSerialBoxDaIn"] = "Serial này không tồn tại, hoặc đã kích hoạt, không Xóa Được !!!";
                        return RedirectToAction("ListSerialDaIn");
                    }
                    else
                    {
                        string sql = "DELETE  FROM [" + DBname + "TechZone].[dbo].[Ser_box] where Serial='" + box.Serial + "'";
                        var Xoaserialsp = dbc.Database.ExecuteSqlCommand(sql);
                        Session["ThongBaoSerialBoxDaIn"] = "Xóa thành công serial: "+ serialsp+".";
                        //SMS hệ thống
                        string sms = " đã Xóa thành công serial: " + serialsp + ".";
                        new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                            , Session["quyen"].ToString(), sms);
                        return RedirectToAction("ListSerialDaIn");
                    }
                    
                }
                Session["ThongBaoSerialSPDaIn"] = "Serial này không tồn tại !!!";
                Session["ThongBaoSerialBoxDaIn"] = "Serial này không tồn tại !!!";
                return RedirectToAction("ListSerialDaIn");
            }
            catch (Exception ex) {
                string loi = ex.ToString();
                Session["ThongBaoSerialSPDaIn"] = "Có Lỗi hệ thống khi xóa S/N SP !!!";
                return RedirectToAction("ListSerialDaIn");
            }
        }
        public ActionResult DeleteSerialBox()
        {
            try
            {
                var kt = dbc.Ser_box.Where(kh => kh.DaIn == false).ToList();
                if (kt.Count() == 0)
                {
                    Session["ThongBaoSerialBoxchuaIn"] = "Không còn S/N Box nào chưa in để Xóa.";
                    return RedirectToAction("ListSerialChuaIn");
                }
                string sql = "DELETE  FROM [" + DBname + "TechZone].[dbo].[Ser_box] where DaIn=0";
                var XoaChitietXN = dbc.Database.ExecuteSqlCommand(sql);
                Session["ThongBaoSerialBoxchuaIn"] = "Xóa tất cả S/N Box thành công.";
                //SMS hệ thống
                string sms = " đã Xóa tất cả S/N Box thành công.";
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                return RedirectToAction("ListSerialChuaIn");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoSerialBoxchuaIn"] = "Có Lỗi hệ thống khi xóa S/N Box !!!";
                return RedirectToAction("ListSerialChuaIn");
            }
        }
        public ActionResult GetListSer_SP()
        {
            ViewBag.SerSPchuaIn = dbc.Ser_sp.Where(kh => kh.DaIn == false).OrderBy(kh => kh.NgayTao).ToList();
            return PartialView();
        }
        public ActionResult GetListSer_Box()
        {
            ViewBag.SerBoxchuaIn = dbc.Ser_box.Where(kh => kh.DaIn == false).OrderBy(kh => kh.NgayTao).ToList();
            return PartialView();
        }
        public ActionResult GetListSer_SPDaIn(string Ngay="", string KeywordsTT="")
        {

            var list = dbc.Ser_sp.Where(kh => kh.DaIn == true && kh.Sudung == false)
                        .OrderByDescending(kh => kh.NgayTao)
                        .Take(1).Single();
            if (Ngay == "")
            {
                var model = new Data.SerialData().listSerSPDaInChuaAc(list.NgayTao,KeywordsTT);
                ViewBag.SerSPDaIn=model;
            }
            else
            {
                var ngayIn = DateTime.Parse(Ngay);
                ViewBag.SerSPDaIn = new Data.SerialData().listSerSPDaInChuaAc(ngayIn, KeywordsTT);
            }
            return PartialView();
        }
        public ActionResult GetCountChuaActi(string Ngay = "",string KeywordsTT="")
        {
            var count = 0;
            var list = dbc.Ser_sp.Where(kh => kh.DaIn == true && kh.Sudung == false)
                        .OrderByDescending(kh => kh.NgayTao)
                        .Take(1).Single();
            if (Ngay == "")
            {
                count = new Data.SerialData().listSerSPDaInChuaAc(list.NgayTao,KeywordsTT).Count();
            }
            else {
                var ngayIn = DateTime.Parse(Ngay);
                count = new Data.SerialData().listSerSPDaInChuaAc(ngayIn, KeywordsTT).Count();
            }
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChuyenKhoserialSP(string serialsp, int idkho)
        {
            try
            {
                var modelK = dbc.Khoes.Find(idkho);
                if (serialsp.Length == 11)
                {
                    var sp = dbc.Ser_sp.FirstOrDefault(kh => kh.SerialSP == serialsp && kh.Sudung == false && kh.DaIn == true);
                    if (sp == null)
                    {
                        Session["ThongBaoSerialSPDaIn"] = "Serial này không tồn tại, hoặc đã kích hoạt!!!";
                        return RedirectToAction("ListSerialDaIn");
                    }
                    else
                    {
                        if (sp.IdKho == idkho)
                        {
                            Session["ThongBaoSerialSPDaIn"] = "Serial này đã ở kho "+modelK.TenKho+", Không thể chuyển!!!";
                            return RedirectToAction("ListSerialDaIn");
                        }
                        else
                        {
                            var modelsp = dbc.Ser_sp.Find(sp.Id);
                            modelsp.IdKho = idkho;
                            modelsp.NgayUpdate = DateTime.Now;
                            dbc.Entry(modelsp).State = EntityState.Modified;
                            var kq = dbc.SaveChanges();
                        }
                    }
                }
                else if (serialsp.Length == 14)
                {
                    var modelsp = dbc.Ser_box.FirstOrDefault(kh => kh.Serial == serialsp);
                    var box = dbc.Ser_sp.FirstOrDefault(kh => kh.IdSerBox == modelsp.Id && kh.Sudung == false && kh.DaIn == true);
                    if (box == null)
                    {
                        Session["ThongBaoSerialBoxDaIn"] = "Serial này không tồn tại, hoặc đã kích hoạt!!!";
                        return RedirectToAction("ListSerialDaIn");
                    }
                    else
                    {
                        if (box.IdKho == idkho)
                        {
                            Session["ThongBaoSerialSPDaIn"] = "Serial này đã ở kho " + modelK.TenKho + ", Không thể chuyển!!!";
                            return RedirectToAction("ListSerialDaIn");
                        }
                        else
                        {
                            var modelbox = dbc.Ser_sp.Find(box.Id);
                            modelbox.IdKho = idkho;
                            modelbox.NgayUpdate = DateTime.Now;
                            dbc.Entry(modelbox).State = EntityState.Modified;
                            var kq = dbc.SaveChanges();
                        }
                    }

                }
                Session["ThongBaoSerialSPDaIn"] = "Chuyển thành công serial: " + serialsp + " đến kho: " + modelK.TenKho;
                //SMS hệ thống
                string sms = " đã Chuyển thành công serial: " + serialsp + " đến kho: " + modelK.TenKho;
                new Data.UserData().SMSvaNhatKy(dbc, Session["UserId"].ToString(), Session["UserName"].ToString()
                    , Session["quyen"].ToString(), sms);
                return RedirectToAction("ListSerialDaIn");
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                Session["ThongBaoSerialSPDaIn"] = "Có Lỗi hệ thống khi Chuyển Kho S/N SP !!!";
                return RedirectToAction("ListSerialDaIn");
            }
        }
    }
}