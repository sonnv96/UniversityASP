//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace University.Models.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class GiangVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiangVien()
        {
            this.LopMonHocs = new HashSet<LopMonHoc>();
        }
    
        public string maGiangVien { get; set; }
        public string tenGiangVien { get; set; }
        public string queQuan { get; set; }
        public Nullable<System.DateTime> ngaySinh { get; set; }
        public Nullable<int> namBatDau { get; set; }
        public string eMail { get; set; }
        public string tenDangNhap { get; set; }
        public string trangThai { get; set; }
    
        public virtual TaiKhoan TaiKhoan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopMonHoc> LopMonHocs { get; set; }
    }
}
