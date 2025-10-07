namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_Vitrinhanvien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_Vitrinhanvien()
        {
            NV_NhanVienTek = new HashSet<NV_NhanVienTek>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string TenVitri { get; set; }

        public double MucLuong { get; set; }

        public int IdDonViTinh { get; set; }

        public double PhuCapChucVu { get; set; }

        public double PhuCapChucKhac { get; set; }

        public double XangXe { get; set; }

        public double TienCom { get; set; }

        public double TrangPhuc { get; set; }

        [StringLength(1000)]
        public string GhiChuThem { get; set; }

        public int SoNgayNghitrongtuan { get; set; }

        public int SoNgayNghitrongthang { get; set; }

        public virtual NV_DonViTinhLuong NV_DonViTinhLuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_NhanVienTek> NV_NhanVienTek { get; set; }
    }
}
