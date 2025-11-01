namespace DoChoiXeMay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NV_Cong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NV_Cong()
        {
            NV_ThanhToanLuong = new HashSet<NV_ThanhToanLuong>();
        }

        public int Id { get; set; }

        public double SoNgayCong { get; set; }

        public double SoNgayTangCa { get; set; }

        public double SoNgayLeTangCa { get; set; }

        public int SLCom { get; set; }

        public int SLComTangCa { get; set; }

        public int SLGiaoHang { get; set; }

        public double NghiHL { get; set; }

        public double SoGioCongThang { get; set; }

        public double SoGioTangCaThang { get; set; }

        public double SoGioTangCaLeThang { get; set; }

        public int Thang { get; set; }

        public int Nam { get; set; }

        public DateTime NgayUpdate { get; set; }

        [StringLength(200)]
        public string GiaiThich { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NV_ThanhToanLuong> NV_ThanhToanLuong { get; set; }
    }
}
