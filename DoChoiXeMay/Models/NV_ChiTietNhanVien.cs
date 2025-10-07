namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_ChiTietNhanVien
    {
        public Guid Id { get; set; }

        public int IdNhanVien { get; set; }

        public double SoGioDaLam { get; set; }

        public double SoNgayNghi { get; set; }

        public double PhuCap { get; set; }

        public double DaUngLuong { get; set; }

        public double LuongNhanCuoi { get; set; }

        public bool DaNhanLuong { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
