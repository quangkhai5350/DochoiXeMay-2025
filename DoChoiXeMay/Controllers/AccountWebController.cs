using DoChoiXeMay.Areas.Admin.Data;
using DoChoiXeMay.Filters;
using DoChoiXeMay.Models;
using DoChoiXeMay.Utils;
using MaHoa_GiaiMa_TaiKhoan;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DoChoiXeMay.Controllers
{
    public class AccountWebController : Controller
    {
        // GET: UserWeb
        Model1 dbc = new Model1();
        TaiKhoanInfo tk = new TaiKhoanInfo();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _ForgotPass()
        {
            return View("_ForgotPass");
        }
        [HttpPost]
        public ActionResult _ForgotPass(string UserName)
        {
            if (UserName != null) {
                var user_mem = dbc.UserTeks.FirstOrDefault(kh=>kh.UserName== UserName);
                if (user_mem != null) {
                    TaiKhoanInfo tk = new TaiKhoanInfo();
                    string pas = tk.DeCryptDotNetNukePassword(user_mem.Password.ToString(), "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", user_mem.PasswordSalt.ToString());
                    var kq = Mailer.Send(user_mem.EmailConnection, "Quên password được gửi từ tek-lightning", "password của bạn là: " + pas);
                    if (kq)
                    {
                        Session["ThongbaoLogin"] = "Đã gửi mật khẩu đến email " + user_mem.EmailConnection;
                        return Redirect("/Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Có lỗi !!! Không thể gửi mật khẩu đến email " + user_mem.EmailConnection + " !!!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi !!! User này không tồn tại !!!");
                }
                
            }
            return View();
        }
        [ProtectNV]
        public ActionResult _ChangeInFoStaff(int Id)
        {
            var model = dbc.NV_NhanVienTek.Find(Id);
            ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
            return View(model);
        }
        [ProtectNV]
        public ActionResult _ChangeInFoStaffJson(NV_NhanVienTek model,string PW, string PWM, string PWMM)
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
                model.NgayUpdate = DateTime.Now;
                dbc.Entry(model).State = EntityState.Modified;
                var kq= dbc.SaveChanges();
                var uid = int.Parse(Session["UserId"].ToString());
                if (kq >0)
                {
                    if (PW != "" && PWM != "" && PWMM != "")
                    {
                        var modelU = dbc.UserTeks.Find(uid);
                        string check_pass = tk.DeCryptDotNetNukePassword(modelU.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", modelU.PasswordSalt);//pass ma hoa
                        if (check_pass == PW && PWM == PWMM && PW != PWM)
                        {
                            string PasswordSalt = Convert.ToBase64String(tk.GenerateSalt()); //tạo chuổi salt ngẫu nhiên
                            string cipherPass = tk.EnCryptDotNetNukePassword(PWM, "", PasswordSalt);

                            modelU.Password = cipherPass;
                            modelU.PasswordSalt = PasswordSalt;
                            modelU.lastPasswordChangedate = DateTime.Now;
                            dbc.Entry(modelU).State = EntityState.Modified;
                            dbc.SaveChanges();
                            var nhatkyPass = XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Tự Update thông tin của mình và PassWord: " + model.HoTen + " Thành Công.", "");
                            Session["ThongbaoSthanhcong"] = "Update thông tin và PassWord: " + model.HoTen + " Thành Công.";
                            //return Json("Succeed", JsonRequestBehavior.AllowGet);
                            ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
                            return View("_ChangeInFoStaff", model);
                        }
                    }
                }
                Session["ThongbaoSthanhcong"] = "Update thông tin: " + model.HoTen + " Thành Công.";
                ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
            return View("_ChangeInFoStaff",model);
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                ModelState.AddModelError("", "Update Thất Bại !!!!" + loi);
                var model2 = dbc.NV_NhanVienTek.Find(model.Id);
                ViewBag.IdKhuVucThuongTru = new SelectList(dbc.KhuVucs.ToList(), "Id", "TenKhuvuc", model.IdKhuVucThuongTru);
                return View("_ChangeInFoStaff", model2);
            }
        }
        [ProtectKH]
        public ActionResult _ChangeInFo(int Id)
        {
            var model = dbc.Ser_ChiNhanh.Find(Id);
            return View(model);
        }
        [ProtectKH]
        public ActionResult ChangeInFoJson(int Id,string DaiDien, string SDT,string DiaChi, string Gmail
                                        , string PW, string PWM, string PWMM )
        {
            try
            {
                var model = dbc.Ser_ChiNhanh.Find(Id);
                model.DaiDien = DaiDien;
                model.SDT = SDT;
                model.DiaChi = DiaChi;
                model.Gmail = Gmail;
                dbc.Entry(model).State = EntityState.Modified;
                var kq= dbc.SaveChanges();
                var uid = int.Parse(Session["UserId"].ToString());
                if (kq > 0)
                {
                    if (PW !="" && PWM !="" && PWMM != "")
                    {
                        var modelU = dbc.UserTeks.Find(uid);
                        string check_pass = tk.DeCryptDotNetNukePassword(modelU.Password, "A872EDF100E1BC806C0E37F1B3FF9EA279F2F8FD378103CB", modelU.PasswordSalt);//pass ma hoa
                        if(check_pass == PW && PWM == PWMM && PW != PWM)
                        {
                            string PasswordSalt = Convert.ToBase64String(tk.GenerateSalt()); //tạo chuổi salt ngẫu nhiên
                            string cipherPass = tk.EnCryptDotNetNukePassword(PWM, "", PasswordSalt);

                            modelU.Password = cipherPass;
                            modelU.PasswordSalt = PasswordSalt;
                            modelU.lastPasswordChangedate = DateTime.Now;
                            dbc.Entry(model).State = EntityState.Modified;
                            dbc.SaveChanges();
                            var nhatkyPass = XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Tự Update thông tin chi nhánh của mình và PassWord: " + model.TenChiNhanh + " Thành Công.", "");
                            Session["ThongbaoUthanhcong"] = "Update thông tin chi nhánh và PassWord: " + model.TenChiNhanh + " Thành Công.";
                            return Json("Succeed", JsonRequestBehavior.AllowGet);
                        }
                    }
                    var nhatky = XuatNhapData.InsertNhatKy_Admin(dbc, uid, Session["quyen"].ToString()
                                , Session["UserName"].ToString(), "Tự Update thông tin chi nhánh của mình: " + model.TenChiNhanh + " Thành Công.", "");
                    Session["ThongbaoUthanhcong"] = "Update thông tin chi nhánh: " + model.TenChiNhanh + ", Thành Công.";
                    return Json("Succeed", JsonRequestBehavior.AllowGet);
                }
                Session["ThongbaoUthanhcong"] = "Có Lỗi, update thông tin chi nhánh: " + model.TenChiNhanh + ", Thất bại.";
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                string loi = ex.ToString();
                var model = dbc.Ser_ChiNhanh.Find(Id);
                Session["ThongbaoUthanhcong"] = "Có Lỗi, update thông tin chi nhánh: " + model.TenChiNhanh + ", Thất bại.";
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}