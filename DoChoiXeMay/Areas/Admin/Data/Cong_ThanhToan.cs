using DoChoiXeMay.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoChoiXeMay.Areas.Admin.Data
{
    public class Cong_ThanhToan
    {
        Model1 _context;
        public Cong_ThanhToan()
        {
            _context = new Model1();
        }
        public bool InsertCong(string DBname,int idnv,double snc, double sntc, double snle,int slcom,int slgiaohang,
            int slhotro, double sgcongthang, double sgtcathang, double sglethang, int thang, int nam)
        {
            try
            {
                DateTime date1 = DateTime.Now;
                string date = date1.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_Cong] " +
                              "values(" + idnv + "," + snc + "," + sntc + ","+snle+","+slcom+","+slgiaohang+"" +
                              ","+slhotro+","+sgcongthang+","+sgtcathang+","+sglethang+","+thang+","+nam+ "" +
                              ",convert(datetime, '" + date + "', 120),N'Insert Auto')";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public bool UPdateCong(NV_Cong model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[NV_Cong] set " +
                "IdNhanVien=@IdNhanVien,SoNgayCong=@SoNgayCong,SoNgayTangCa=@SoNgayTangCa," +
                "SoNgayLe=@SoNgayLe,SLCom=@SLCom,SLGiaoHang=@SLGiaoHang,SLHoTro=@SLHoTro,SoGioCongThang=@SoGioCongThang," +
                "SoGioTangCaThang=@SoGioTangCaThang,SoGioLeThang=@SoGioLeThang,Thang=@Thang,Nam=@Nam,NgayUpdate=@NgayUpdate," +
                "GiaiThich=@GiaiThich where Id=@Id",
                new SqlParameter("@IdNhanVien", model.IdNhanVien),
                new SqlParameter("@SoNgayCong", model.SoNgayCong),
                new SqlParameter("@SoNgayTangCa", model.SoNgayTangCa),
                new SqlParameter("@SoNgayLe", model.SoNgayLe),
                new SqlParameter("@SLCom", model.SLCom),
                new SqlParameter("@SLGiaoHang", model.SLGiaoHang),
                new SqlParameter("@SLHoTro", model.SLHoTro),
                new SqlParameter("@SoGioCongThang", model.SoGioCongThang),
                new SqlParameter("@SoGioTangCaThang", model.SoGioTangCaThang),
                new SqlParameter("@SoGioLeThang", model.SoGioLeThang),
                new SqlParameter("@Thang", model.Thang),
                new SqlParameter("@Nam", model.Nam),
                new SqlParameter("@NgayUpdate", DateTime.Now),
                new SqlParameter("@GiaiThich", model.GiaiThich),
                new SqlParameter("@Id", model.Id));
                if (update > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertThanhToanLuong(string DBname, Guid Id, int idnv, double tiencong, double tiencom, double pcgiaohang
            , double pcxangxe, double pcchucvu, double pckhac, double thuong, double khautrubh,
            double Ungluong,double thuclinh, int thang, int nam)
        {
            try
            {

                DateTime date1 = DateTime.Now;
                string ngaytao = date1.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_ThanhToanLuong] " +
                              "values('" + Id.ToString() + "'," + idnv + "," + tiencong + "," + tiencom + "," + pcgiaohang + "," + pcxangxe + "" +
                              "," + pcchucvu + "," + pckhac + "," + thuong + "," + khautrubh + "," + Ungluong + "," + thuclinh + "" +
                              ",0,"+thang+ ","+nam+",convert(datetime, '" + ngaytao + "', 120),convert(datetime, '" + ngaytao + "', 120))";
                var insert_SVL = _context.Database.ExecuteSqlCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }

        }
        public bool UPdateThanhToanLuong(NV_ThanhToanLuong model, string DBname)
        {
            try
            {
                var update = _context.Database.ExecuteSqlCommand("update [" + DBname + "TechZone].[dbo].[NV_ThanhToanLuong] set " +
                "IdNhanVien=@IdNhanVien,TienCong=@TienCong,TienCom=@TienCom," +
                "PCGiaoHang=@PCGiaoHang,PCXangXe=@PCXangXe,PCChucVu=@PCChucVu,PCKhac=@PCKhac,Thuong=@Thuong," +
                "KhauTruBH=@KhauTruBH,DaUngLuong=@DaUngLuong,ThucLinh=@ThucLinh,DaNhanLuong=@DaNhanLuong" +
                "Thang=@Thang,Nam=@Nam,NgayTao=@NgayTao,NgayUpdate=@NgayUpdate where Id=@Id",
                new SqlParameter("@IdNhanVien", model.IdNhanVien),
                new SqlParameter("@TienCong", model.TienCong),
                new SqlParameter("@TienCom", model.TienCom),
                new SqlParameter("@PCGiaoHang", model.PCGiaoHang),
                new SqlParameter("@PCXangXe", model.PCXangXe),
                new SqlParameter("@PCChucVu", model.PCChucVu),
                new SqlParameter("@PCKhac", model.PCKhac),
                new SqlParameter("@Thuong", model.Thuong),
                new SqlParameter("@KhauTruBH", model.KhauTruBH),
                new SqlParameter("@DaUngLuong", model.DaUngLuong),
                new SqlParameter("@ThucLinh", model.ThucLinh),
                new SqlParameter("@DaNhanLuong", model.DaNhanLuong),
                new SqlParameter("@Thang", model.Thang),
                new SqlParameter("@Nam", model.Nam),
                new SqlParameter("@NgayTao", DateTime.Now),
                new SqlParameter("@NgayUpdate", DateTime.Now),
                new SqlParameter("@Id", model.Id));
                if (update > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                string loi = ex.ToString();
                return false;
            }
        }
        public bool InsertChiTietNangLuong(string DBname, int Idnv, int mucluong,int idhsl)
        {
            try
            {
                DateTime date1 = DateTime.Now;
                string ngaytao = date1.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "insert into [" + DBname + "TechZone].[dbo].[NV_ChiTietNangLuong] " +
                    "values(" + Idnv + "," + mucluong + "," + idhsl + ",convert(datetime, '" + ngaytao + "', 120)" +
                    ",1,N'Auto')";
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