namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_DonViTinhLuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_DonViTinhLuong()
        {
            NV_Vitrinhanvien = new HashSet<NV_Vitrinhanvien>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDVT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_Vitrinhanvien> NV_Vitrinhanvien { get; set; }
    }
}
