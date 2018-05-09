using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using University.Models.Data;

namespace University.App_Start
{
    public class SaveStatus
    {
    }
    public class XuLy : IJob
    {
        private UniversityEntities1 db = new UniversityEntities1();
        public void Execute(IJobExecutionContext context)
        {
        LopMonHoc lmonhoc = db.LopMonHocs.FirstOrDefault();
            if (DateTime.Now < lmonhoc.hanDangKy)
            {
                if (lmonhoc.soLuongDangKy >= 20 && lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                {
                    lmonhoc.trangThai = "Chấp nhận mở lớp";
                    

                        
                        db.SaveChanges();
                        
                    
                }
                else if (lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                {
                    lmonhoc.trangThai = "Chờ Sinh Viên Đăng ký";
                  

                       
                        db.SaveChanges();
                        
                    }
                


            }
            else if (DateTime.Now > lmonhoc.hanDangKy)
            {
                if (lmonhoc.soLuongDangKy < 20)
                {
                    lmonhoc.trangThai = "Hủy Lớp";
                    

                        db.SaveChanges();
                       
                    
                }
                else if (lmonhoc.soLuongDangKy > 20)
                {
                    lmonhoc.trangThai = "Khóa Lớp";
                    


                        db.SaveChanges();
                       
                    
                }


            }
            throw new NotImplementedException();
        }
    }
}