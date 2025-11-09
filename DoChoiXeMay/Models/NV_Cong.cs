namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_Cong
    {
        public int Id { get; set; }

        public int IdNhanVien { get; set; }

        public double SoNgayCong { get; set; }

        public double SoNgayTangCa { get; set; }

        public double SoNgayLe { get; set; }

        public int SLCom { get; set; }

        public int SLGiaoHang { get; set; }

        public int SLHoTro { get; set; }

        public double SoGioCongThang { get; set; }

        public double SoGioTangCaThang { get; set; }

        public double SoGioLeThang { get; set; }

        public int Thang { get; set; }

        public int Nam { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(200)]
        public string GiaiThich { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
