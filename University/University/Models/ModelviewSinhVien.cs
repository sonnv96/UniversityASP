using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class ModelviewSinhVien
    {

        [DisplayName("Mã Sinh Viên")]
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        public string masv { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [DisplayName("Tên Đăng Nhập")]
        public string tendangnhap { get; set; }
        [DisplayName("Tên Sinh Viên")]
        [RegularExpression(@"([A-Z]{1}[a-z]+[\s]*)+", ErrorMessage = "Sai định dạng tên người dùng")]
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        public string tensv { get; set; }
        [DisplayName("Quê Quán")]
        public string quequan { get; set; }
        [DisplayName("Ngày Sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngaysinh { get; set; }
        [DisplayName("Tên Lớp")]
        public string tenlop { get; set; }
        [DisplayName("Giới Tính")]
        public string gioitinh { get; set; }
        [DisplayName("Trạng Thái")]
        public string trangthai { get; set; }
        [DisplayName("Năm Nhập Học")]
        [Range(1900, 3000, ErrorMessage = "Sai định dạng năm")]
        public int namnhaphoc { get; set; }
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Hình ảnh")]
        public string hinhanh { get; set; }
        [DisplayName("Tên Ngành")]
        public string tennganh { get; set; }

    }
}