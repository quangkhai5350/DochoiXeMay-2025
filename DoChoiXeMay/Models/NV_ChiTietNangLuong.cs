namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_ChiTietNangLuong
    {
        public int Id { get; set; }

        public int IdNhanVien { get; set; }

        public int MucLuong { get; set; }

        public int IdHSG { get; set; }

        public DateTime NgayApDung { get; set; }
        public bool SuDung { get; set; }
        [StringLength(500)]
        public string Ghichu { get; set; }
        public virtual NV_HeSoGio NV_HeSoGio { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
