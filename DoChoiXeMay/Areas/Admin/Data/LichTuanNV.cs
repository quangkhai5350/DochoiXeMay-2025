using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
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
                
                //string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_NhanVienTek] " +
                //              "values(N'" + hoten + "',0,'" + cccd + "',27,N'',N'',N'',N''," +
                //              "N'',N'',N'',N'',N'',N'0987654321',2,1,GETDATE(),GETDATE(),1,''," + IDU + ")";
                //var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
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