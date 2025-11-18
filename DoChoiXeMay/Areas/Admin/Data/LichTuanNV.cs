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
    }
}