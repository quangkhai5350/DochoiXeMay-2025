namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietTonKho")]
    public partial class ChiTietTonKho
    {
        public int Id { get; set; }

        public int IdKyTonKho { get; set; }

        [Required]
        [StringLength(200)]
        public string TenHang { get; set; }

        public int TonDauKy { get; set; }

        public int DaRap { get; set; }

        public int CoLoi { get; set; }

        public int ChuaRap { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public bool SanPham { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        [StringLength(20)]
        public string STT { get; set; }

        public int ParentId { get; set; }

        public int IDMF { get; set; }

        public int IDColor { get; set; }

        public int IDSize { get; set; }

        public virtual Color Color { get; set; }

        public virtual KyTonKho KyTonKho { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual Size Size { get; set; }
    }
}
