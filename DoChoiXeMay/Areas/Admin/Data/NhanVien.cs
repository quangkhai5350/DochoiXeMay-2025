using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                var hoten = "TeK Auto";
                var cccd = "1111111111";
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_NhanVienTek] " +
                              "values(N'" + hoten + "',0,'" + cccd + "',27,N'',N'',N'',N''," +
                              "N'',N'',N'',N'',N'',N'0987654321',2,1,GETDATE(),GETDATE(),1,'')";
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