using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
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
                              "values(N'" + hoten + "',0,'" + cccd + "',27,N'',N'',N'Tocdai.png',N''," +
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
    }
}