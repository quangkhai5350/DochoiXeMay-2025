namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KyTonKho")]
    public partial class KyTonKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KyTonKho()
        {
            ChiTietTonKhoes = new HashSet<ChiTietTonKho>();
            KyXuatNhaps = new HashSet<KyXuatNhap>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKy { get; set; }

        [StringLength(200)]
        public string LuuKho { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        public DateTime NgayTao { get; set; }

        [StringLength(50)]
        public string STT { get; set; }

        public bool SuDung { get; set; }
        public bool HoanThanh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTonKho> ChiTietTonKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KyXuatNhap> KyXuatNhaps { get; set; }
    }
}
