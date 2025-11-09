namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_ThanhToanLuong
    {
        public Guid Id { get; set; }

        public int IdNhanVien { get; set; }

        public double TienCong { get; set; }

        public double TienCom { get; set; }

        public double PCGiaoHang { get; set; }

        public double PCXangXe { get; set; }

        public double PCChucVu { get; set; }

        public double PCKhac { get; set; }

        public double Thuong { get; set; }

        public double KhauTruBH { get; set; }

        public double DaUngLuong { get; set; }

        public double ThucLinh { get; set; }

        public bool DaNhanLuong { get; set; }

        public int Thang { get; set; }

        public int Nam { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
