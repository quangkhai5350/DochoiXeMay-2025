namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_HeSoGio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_HeSoGio()
        {
            NV_Luong = new HashSet<NV_ChiTietNangLuong>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; }

        public double HeSo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ChiTietNangLuong> NV_Luong { get; set; }
    }
}
