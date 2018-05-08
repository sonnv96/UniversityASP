using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class ModelViewLichHoc
    {
        public string masv { get; set; }
        public string tendangnhap { get; set; }
        public string tensv { get; set; }
        public string hinhanh { get; set; }
        public int   namhoc { get; set; }
        public int hocki { get; set; }
        public int ngayhoc { get; set; }
        public string monhoc { get; set; }
        public string tiethoc { get; set; }
        public string giangvien { get; set; }
        public string phonghoc { get; set; }
        public DateTime? ngaythi { get; set; }
    }
}