namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangDoiTra")]
    public partial class HangDoiTra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HangDoiTra()
        {
            ChitietXuatNhaps = new HashSet<ChitietXuatNhap>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; }

        [StringLength(200)]
        public string GiaiThich { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChitietXuatNhap> ChitietXuatNhaps { get; set; }
    }
}
