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

        public TimeSpan GioVaoSang { get; set; }

        public TimeSpan GioRaSang { get; set; }

        public TimeSpan GioVaoChieu { get; set; }

        public TimeSpan GioRaChieu { get; set; }

        public TimeSpan GioVaoTangCa { get; set; }

        public TimeSpan GioRaTangCa { get; set; }

        public TimeSpan GioVaoTangCaLe { get; set; }

        public TimeSpan GioRaTangCaLe { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(100)]
        public string GhiChu { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
