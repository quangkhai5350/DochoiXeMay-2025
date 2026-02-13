namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_LichTuanParTime
    {
        public Guid Id { get; set; }

        public int SoTuanTrongNam { get; set; }

        public int IdNhanVien { get; set; }

        public bool SangT2 { get; set; }

        public bool ChieuT2 { get; set; }

        public bool ToiT2 { get; set; }

        public bool SangT3 { get; set; }

        public bool ChieuT3 { get; set; }

        public bool ToiT3 { get; set; }

        public bool SangT4 { get; set; }

        public bool ChieuT4 { get; set; }

        public bool ToiT4 { get; set; }

        public bool SangT5 { get; set; }

        public bool ChieuT5 { get; set; }

        public bool ToiT5 { get; set; }

        public bool SangT6 { get; set; }

        public bool ChieuT6 { get; set; }

        public bool ToiT6 { get; set; }

        public bool SangT7 { get; set; }

        public bool ChieuT7 { get; set; }

        public bool ToiT7 { get; set; }

        public bool SangCN { get; set; }

        public bool ChieuCN { get; set; }

        public bool ToiCN { get; set; }

        public int Year { get; set; }

        public DateTime Ngay { get; set; }

        public virtual NV_NhanVienTek NV_NhanVienTek { get; set; }
    }
}
