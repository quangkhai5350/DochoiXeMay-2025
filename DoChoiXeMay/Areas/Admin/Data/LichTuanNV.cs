using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class LichTuanNV
    {
        Model1 _context;
        public LichTuanNV()
        {
            _context = new Model1();
        }
        public bool InsertLichTuanAuto(string DBname,int IdNV,int tuanht)
        {
            try
            {
                int year = DateTime.Now.Year;
                var Id = Guid.NewGuid();
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_LichTuanParTime] " +
                              "values(N'" + Id.ToString() + "',"+tuanht+"," + IdNV + ",0,0,0,0,0," +
                              "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,"+year+",GETDATE())";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        //lấy Ngày (Không xài)
        public static DateTime GetDateFromWeekAndDay(int year, int weekNumber, int dayOfWeek)
        {
            //Thứ 2==1, CN ==0
            DateTime firstDayOfFirstWeek = new DateTime(year, 1, 1);
            while (firstDayOfFirstWeek.DayOfWeek != DayOfWeek.Monday) // Giả sử tuần bắt đầu từ Thứ Hai
            {
                firstDayOfFirstWeek = firstDayOfFirstWeek.AddDays(-1);
            }
            // Đơn giản hơn, dùng phương pháp tìm đầu tuần 1 (Thứ 2):
            DateTime startOfTargetWeek = firstDayOfFirstWeek.AddDays((weekNumber - 1) * 7);

            // 3. Tính ngày cụ thể trong tuần
            // Chuyển đổi DayOfWeek sang số ngày cần cộng (0 = Thứ 2, 1 = Thứ 3...)
            int daysToAdd = dayOfWeek - (int)DayOfWeek.Monday; // Nếu dayOfWeek là Monday=0, Tuesday=1...
            if (daysToAdd < 0) daysToAdd += 7; // Xử lý trường hợp ngày đầu tuần là Chủ Nhật (0)

            DateTime targetDate = startOfTargetWeek.AddDays(daysToAdd);

            return targetDate;
        }
        public static int GetTuanHT()
        {
            DateTime date = DateTime.Now;
            CultureInfo ci = CultureInfo.CurrentCulture; // Sử dụng hiện tại của hệ thống
            Calendar cal = ci.Calendar;
            CalendarWeekRule rule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;
            return cal.GetWeekOfYear(date, rule, firstDayOfWeek);
        }

        public int numberAcbyLichTuan(int idnhanvien, int yearht)
        {
            var tuanht = GetTuanHT();
            int dayht = DateTime.Now.Day;
            var getLichtuan = _context.NV_LichTuanParTime.FirstOrDefault(kh => kh.Year == yearht
                && kh.IdNhanVien == idnhanvien && kh.SoTuanTrongNam == tuanht);
            if (getLichtuan != null)
            {
                //(Sunday is 0, Monday is 1)
                var kk = (int)DateTime.Now.DayOfWeek;
                if(kk==0 && getLichtuan.SangCN==true)
                {
                    return 1;//9h sáng
                }else if(kk == 0 && getLichtuan.ChieuCN == true)
                {
                    return 2;//1h chiều
                }
                else if (kk == 0 && getLichtuan.ChieuCN == true)
                {
                    return 3;//6h tối
                }
                else if (kk == 1 && getLichtuan.SangT2 == true)
                {
                    return 1;
                }
                else if (kk == 1 && getLichtuan.ChieuT2 == true)
                {
                    return 2;
                }
                else if (kk == 1 && getLichtuan.ToiT2 == true)
                {
                    return 3;
                }
                else if (kk == 2 && getLichtuan.SangT3 == true)
                {
                    return 1;
                }
                else if (kk == 2 && getLichtuan.ChieuT3 == true)
                {
                    return 2;
                }
                else if (kk == 2 && getLichtuan.ToiT3 == true)
                {
                    return 3;
                }
                else if (kk == 3 && getLichtuan.SangT4 == true)
                {
                    return 1;
                }
                else if (kk == 3 && getLichtuan.ChieuT4 == true)
                {
                    return 2;
                }
                else if (kk == 3 && getLichtuan.ToiT4 == true)
                {
                    return 3;
                }
                else if (kk == 4 && getLichtuan.SangT5 == true)
                {
                    return 1;
                }
                else if (kk == 4 && getLichtuan.ChieuT5 == true)
                {
                    return 2;
                }
                else if (kk == 4 && getLichtuan.ToiT5 == true)
                {
                    return 3;
                }
                else if (kk == 5 && getLichtuan.SangT6 == true)
                {
                    return 1;
                }
                else if (kk == 5 && getLichtuan.ChieuT6 == true)
                {
                    return 2;
                }
                else if (kk == 5 && getLichtuan.ToiT6 == true)
                {
                    return 3;
                }
                else if (kk == 6 && getLichtuan.SangT7 == true)
                {
                    return 1;
                }
                else if (kk == 6 && getLichtuan.ChieuT7 == true)
                {
                    return 2;
                }
                else if (kk == 6 && getLichtuan.ToiT7 == true)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}