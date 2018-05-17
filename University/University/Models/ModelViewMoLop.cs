using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class ModelViewMoLop
    {
       
        [DisplayName("Mã Giảng Viên")]
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        public string magiangvien { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [DisplayName("Tiết học")]
        public string tiethoc { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [DisplayName("Phòng học")]
        public string phonghoc { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [Range(1, 3, ErrorMessage = "Nhập học kỳ 1, 2 hoặc 3")]
        [DisplayName("Học kỳ")]
        public int hocki { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [Range(1900, 3000, ErrorMessage = "Sai định dạng năm")]
        [DisplayName("Năm học")]
        public int namhoc { get; set; }
        [Required(ErrorMessage = "Trường này bắt buộc nhập")]
        [DisplayName("Ngày học")]
        [Range(2, 8, ErrorMessage = "Sai định dạng ngày học, từ thứ 2 đến chủ nhật")]
        public int ngayhoc { get; set; }
    }
}