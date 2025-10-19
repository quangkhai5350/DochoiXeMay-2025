namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TonKhoCap")]
    public partial class TonKhoCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TonKhoCap()
        {
            ChiTietTonKhoes = new HashSet<ChiTietTonKho>();
        }

        public int Id { get; set; }
        public string Ten { get; set; }
        public int Cap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietTonKho> ChiTietTonKhoes { get; set; }
    }
}
