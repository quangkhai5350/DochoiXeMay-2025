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
            NV_ChiTietNangLuong = new HashSet<NV_ChiTietNangLuong>();
            NV_Cong = new HashSet<NV_Cong>();
            NV_GioCong = new HashSet<NV_GioCong>();
            NV_LichTuanParTime = new HashSet<NV_LichTuanParTime>();
            NV_ThanhToanLuong = new HashSet<NV_ThanhToanLuong>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; }

        [Required]
        [StringLength(100)]
        public string CCCD { get; set; }

        public int IdKhuVucThuongTru { get; set; }

        [Required]
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

        [StringLength(200)]
        public string TinHoc { get; set; }

        [Required]
        [StringLength(50)]
        public string Sdt { get; set; }

        public int IdVitrinhanvien { get; set; }

        public bool ThuViec { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime NgayUpdate { get; set; }

        public bool DaNghiViec { get; set; }

        [StringLength(50)]
        public string STT { get; set; }

        public int IdUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ChiTietNangLuong> NV_ChiTietNangLuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_Cong> NV_Cong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_GioCong> NV_GioCong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_LichTuanParTime> NV_LichTuanParTime { get; set; }

        public virtual NV_Vitrinhanvien NV_Vitrinhanvien { get; set; }

        public virtual UserTek UserTek { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ThanhToanLuong> NV_ThanhToanLuong { get; set; }
    }
}
