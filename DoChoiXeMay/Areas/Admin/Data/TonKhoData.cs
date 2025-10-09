using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static QRCoder.PayloadGenerator;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class TonKhoData
    {
        Model1 _context = new Model1();
        public List<ChiTietTonKho> GetListTKhoNVLByKy(int idKytonkho)
        {
            var model = _context.ChiTietTonKhoes.Where(kh => kh.IdKyTonKho == idKytonkho && kh.SanPham==false)
                    .OrderByDescending(kh => kh.Id)
                    .ToList();
            for (int i = 0; i < model.Count(); i++)
            {
                model[i].STT = (i + 1).ToString();
            }
            return model;
        }
        public bool InsertTonKhoAotu(int IdKy, string DBname)
        {
            try
            {
                string sql = "insert into [" + DBname + "TechZone].[dbo].[ChiTietTonKho] " +
                                            "values(" + IdKy + ",'Auto tên 1',0,0,0,0,GETDATE(),GETDATE(),0,'','',0)";
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