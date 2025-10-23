namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_Luong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_Luong()
        {
            NV_ThanhToanLuong = new HashSet<NV_ThanhToanLuong>();
        }

        public int Id { get; set; }

        public int MucLuong { get; set; }

        public double HSL { get; set; }

        public int IdHSG { get; set; }

        public DateTime NgayApDung { get; set; }

        public virtual NV_HeSoGio NV_HeSoGio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ThanhToanLuong> NV_ThanhToanLuong { get; set; }
    }
}
