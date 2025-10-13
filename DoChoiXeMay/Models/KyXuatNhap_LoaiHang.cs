namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KyXuatNhap_LoaiHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KyXuatNhap_LoaiHang()
        {
            KyXuatNhaps = new HashSet<KyXuatNhap>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string TenLoai { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KyXuatNhap> KyXuatNhaps { get; set; }
    }
}
