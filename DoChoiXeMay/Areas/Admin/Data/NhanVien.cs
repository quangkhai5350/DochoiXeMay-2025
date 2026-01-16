using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class NhanVien
    {
        Model1 _context;
        public NhanVien()
        {
            _context = new Model1();
        }
        public bool InsertNhanVienAuto(string DBname)
        {
            try
            {
                var UN = new Data.ActiveData().InsertUserAotu();
                var IDU = _context.UserTeks.FirstOrDefault(kh => kh.UserName == UN).Id;
                var hoten = "TeK Auto";
                var cccd = "1111111111";
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_NhanVienTek] " +
                              "values(N'" + hoten + "',0,'" + cccd + "',27,N'',N'',N'',N''," +
                              "N'',N'',N'',N'',N'',N'0987654321',2,1,GETDATE(),GETDATE(),1,'',"+IDU+")";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        //đang làm 6/11
        public bool InsertNgayGioCongAuto(string DBname, DateTime dtInput, int NV)
        {
            try
            {
                //Tổng số ngày trong tháng;
                DateTime dtResult = dtInput;
                dtResult = dtResult.AddMonths(1);
                dtResult = dtResult.AddDays(-(dtResult.Day));
                var kq= dtResult.Day;
                //Duyệt vòng lặp từ ngày + 1 đến ngày cuối tháng ==> Insert bảng giờ công
                var checkngaygio = _context.NV_GioCong.Where(kh=>kh.Month==dtInput.Month && kh.Year==dtInput.Year 
                                && kh.IdNhanVien==NV).ToList();
                if(checkngaygio.Count() < kq)
                {
                    var y = dtInput.Year; var m= dtInput.Month;
                    for (int i = checkngaygio.Count() + 1; i < kq+1; i++)
                    {
                        var Id = Guid.NewGuid();
                        DateTime date1 = DateTime.Now;
                        string date = date1.ToString("yyyy-MM-dd HH:mm:ss");
                        DateTime date2= new DateTime(y, m, i,0,0,0);
                        string date3 = date2.ToString("yyyy-MM-dd HH:mm:ss");
                        string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_GioCong] " +
                                      "values(N'" + Id.ToString() + "'," + NV + "" +
                                      ",convert(datetime,'" + date3 + "',120),convert(datetime,'" + date3 + "',120)" +
                                      ",convert(datetime,'" + date3 + "',120),convert(datetime,'" + date3 + "',120)" +
                                      ",convert(datetime,'" + date3 + "',120),convert(datetime,'" + date3 + "',120)" +
                                      ",convert(datetime,'" + date3 + "',120),convert(datetime,'" + date3 + "',120)" +
                                      "," + i + "," + dtInput.Month + "," + dtInput.Year + "" +
                                      ",convert(datetime, '" + date + "', 120),'')";
                        var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertViTriNVAuto(string DBname)
        {
            try
            {
                var tenvitri = "TeK Auto";
                var dvt = "Gio";
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_Vitrinhanvien] "+
                    "values(N'"+tenvitri+"','"+dvt+"',0,0,0,'',1,4)";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public int DiemDanh(string Id)
        {
            try
            {
                var timeday = DateTime.Now;
                int h = timeday.Hour;
                int m = timeday.Minute;
                int s = timeday.Second;
                var giocong = _context.NV_GioCong.Find(new Guid(Id));
                var kqCS = double.Parse((giocong.GioRaSang - giocong.GioVaoSang).TotalHours.ToString("0.00"));
                var kqCC = double.Parse((giocong.GioRaChieu - giocong.GioVaoChieu).TotalHours.ToString("0.00"));
                var kqTC = double.Parse(((giocong.GioRaTangCa - giocong.GioVaoTangCa).TotalHours).ToString("0.00"));
                var kqLe = double.Parse(((giocong.GioRaLe - giocong.GioVaoLe).TotalHours).ToString("0.00"));

                var checkdiemdanh = kqCS + kqCC + kqTC + kqLe;
                if (checkdiemdanh == 0)
                {
                    //numberCancelbyLichTuan thời điểm không cho điểm danh
                    //chưa điểm danh
                    var kqq = UpdateGioCongDiemDanh(giocong, 1);
                    if (kqq) return 1; 
                }else if(kqLe != 0)
                {
                    return 0;//đã update lễ (không dd)
                }
                else if(kqTC > 0)
                {
                    return 0;//đã hoàn thành tăng ca (không dd)
                }
                else if (kqLe == 0 && kqTC < 0)
                {
                    //Chưa up lễ, Chưa hoàn thành tăng ca ==>kết thúc tăng ca 6
                    var kqq = UpdateGioCongDiemDanh(giocong, 6);
                    if (kqq) return 6;
                }
                else if(kqLe == 0 && kqTC == 0 && kqCC > 0)
                {
                    //Chưa diem danh tang ca, nhưng đã hoàn thành buổi chiều==>bắt đầu tăng ca 5
                    ////chưa đến 6 h chiều, không cho check
                    if (h < 18)
                    {
                        //////var kqq = UpdateGioCongDiemDanh(giocong, 4);
                        //////if (kqq) return 4;
                        return 0;
                    }
                    else
                    {
                        var kqq = UpdateGioCongDiemDanh(giocong, 5);
                        if (kqq) return 5;
                    }
                }
                else if(kqLe == 0 && kqTC ==0 && kqCC < 0)
                {
                    //Chưa diem danh tang ca, Chưa hoàn thành buổi chiều ==>kết thúc chiều 4
                    var kqq = UpdateGioCongDiemDanh(giocong, 4);
                    if (kqq) return 4;
                }
                else if (kqLe == 0 && kqTC == 0 && kqCC == 0 && kqCS >0)
                {
                    //Chưa diem danh tang ca, đã hoàn thành buổi sáng ==> bắt đầu chiều 3
                    ////chưa đến 1 h chiều, không cho check
                    if (h < 13)
                    {
                        //////var kqq = UpdateGioCongDiemDanh(giocong, 2);
                        //////if (kqq) return 2;
                        return 0;
                    }
                    else
                    {
                        var kqq = UpdateGioCongDiemDanh(giocong, 3);
                        if (kqq) return 3;
                    }
                }
                else if (kqLe == 0 && kqTC == 0 && kqCC == 0 && kqCS < 0)
                {
                    //Chưa diem danh tang ca, chưa hoàn thành buổi sáng ==> kết thúc sang 2
                    var kqq = UpdateGioCongDiemDanh(giocong, 2);
                    if (kqq) return 2;
                }
                return 0;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return 0;
            }
        }
        public bool UpdateGioCongDiemDanh(NV_GioCong model, int stt)
        {
            try
            {
                var timeday = DateTime.Now;
                int h = timeday.Hour;
                int m = timeday.Minute;
                int s = timeday.Second;
                if (stt == 6)
                {
                    model.GioRaTangCa = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                }
                else if (stt == 5)
                {
                    model.GioVaoTangCa = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                }
                else if (stt == 4)
                {
                    model.GioRaChieu = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                }
                else if (stt == 3)
                {
                    model.GioVaoChieu = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                }
                else if (stt == 2)
                {
                    model.GioRaSang = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                }
                else if (stt == 1)
                {
                    if (h < 12)
                    {
                        model.GioVaoSang = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                    }else if (h >= 12 && h < 18)
                    {
                        model.GioVaoChieu = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                    }
                    else //Tăng Ca chiều bắt đầu lúc 18h
                    {
                        model.GioVaoTangCa = new DateTime(model.Year, model.Month, model.Day, h, m, s);
                    }

                }
                
                model.NgayUpdate = DateTime.Now;
                model.GhiChu = "Check in/out";
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool UpdateGioCong(NV_GioCong model, TimeSpan VaoSang, TimeSpan RaSang, TimeSpan VaoChieu
            , TimeSpan RaChieu, TimeSpan VaoTangCa, TimeSpan RaTangCa, TimeSpan VaoLe, TimeSpan RaLe)
        {
            try
            {
                model.GioVaoSang = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var VS = model.GioVaoSang.Add(VaoSang);
                model.GioRaSang = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var RS = model.GioRaSang.Add(RaSang);
                model.GioVaoChieu = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var VC = model.GioVaoChieu.Add(VaoChieu);
                model.GioRaChieu = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var RC = model.GioRaChieu.Add(RaChieu);
                model.GioVaoTangCa = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var VTC = model.GioVaoTangCa.Add(VaoTangCa);
                model.GioRaTangCa = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var RTC = model.GioRaTangCa.Add(RaTangCa);
                model.GioVaoLe = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var VL = model.GioVaoLe.Add(VaoLe);
                model.GioRaLe = new DateTime(model.Year, model.Month, model.Day, 0, 0, 0);
                var RL = model.GioRaLe.Add(RaLe);

                model.GioVaoSang = VS;
                model.GioRaSang = RS;
                model.GioVaoChieu = VC;
                model.GioRaChieu = RC;
                model.GioVaoTangCa = VTC;
                model.GioRaTangCa = RTC;
                model.GioVaoLe = VL;
                model.GioRaLe = RL;
                model.NgayUpdate = DateTime.Now;
                model.GhiChu = "Update";
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        
    }
}