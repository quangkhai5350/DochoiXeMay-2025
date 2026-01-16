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
        //public int numberCancelbyLichTuan(int idnhanvien, int yearht)
        //{
        //    var tuanht = GetTuanHT();
        //    var getLichtuan = _context.NV_LichTuanParTime.FirstOrDefault(kh => kh.Year == yearht
        //        && kh.IdNhanVien == idnhanvien && kh.SoTuanTrongNam == tuanht);
        //    if (getLichtuan != null)
        //    {
        //        if (getLichtuan.SangCN == false && getLichtuan.ChieuCN == false && getLichtuan.ToiCN == false &&
        //            getLichtuan.SangT2 == false && getLichtuan.ChieuT2 == false && getLichtuan.ToiT2 == false &&
        //            getLichtuan.SangT3 == false && getLichtuan.ChieuT3 == false && getLichtuan.ToiT3 == false &&
        //            getLichtuan.SangT4 == false && getLichtuan.ChieuT4 == false && getLichtuan.ToiT4 == false &&
        //            getLichtuan.SangT5 == false && getLichtuan.ChieuT5 == false && getLichtuan.ToiT5 == false &&
        //            getLichtuan.SangT6 == false && getLichtuan.ChieuT6 == false && getLichtuan.ToiT6 == false &&
        //            getLichtuan.SangT7 == false && getLichtuan.ChieuT7 == false && getLichtuan.ToiT7 == false)
        //        {
        //            return -1;
        //        }
        //    }
        //}
    }
}