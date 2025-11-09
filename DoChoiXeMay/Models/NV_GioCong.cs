namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_GioCong
    {
        public Guid Id { get; set; }

        public int IdNhanVien { get; set; }

        public DateTime GioVaoSang { get; set; }

        public DateTime GioRaSang { get; set; }

        public DateTime GioVaoChieu { get; set; }

        public DateTime GioRaChieu { get; set; }

        public DateTime GioVaoTangCa { get; set; }

        public DateTime GioRaTangCa { get; set; }

        public DateTime GioVaoLe { get; set; }

        public DateTime GioRaLe { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
