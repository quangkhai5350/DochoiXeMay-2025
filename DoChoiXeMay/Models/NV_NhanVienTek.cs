namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_NhanVienTek
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_NhanVienTek()
        {
            NV_ChiTietNhanVien = new HashSet<NV_ChiTietNhanVien>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(100)]
        public string CanCuocCongDan { get; set; }

        public int IdKhuVucThuongTru { get; set; }

        [StringLength(500)]
        public string DiaChiThuongTru { get; set; }

        [StringLength(500)]
        public string DiaChiHienTai { get; set; }

        [StringLength(200)]
        public string HinhDaiDien { get; set; }

        [StringLength(200)]
        public string HinhCanCuocTruoc { get; set; }

        [StringLength(200)]
        public string HinhCanCuocSau { get; set; }

        [StringLength(500)]
        public string CongViec { get; set; }

        [StringLength(500)]
        public string BangCapChinh { get; set; }

        [StringLength(500)]
        public string BangCapPhu { get; set; }

        [StringLength(50)]
        public string Sdt { get; set; }

        public int IdVitrinhanvien { get; set; }

        public bool ThuViec { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public bool DaNghiViec { get; set; }

        [StringLength(50)]
        public string STT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ChiTietNhanVien> NV_ChiTietNhanVien { get; set; }

        public virtual NV_Vitrinhanvien NV_Vitrinhanvien { get; set; }
    }
}
