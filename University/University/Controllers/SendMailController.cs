using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using University.Models.Data;

namespace University.Controllers
{
    public class SendMailController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult PheDuyet(string tt, string nd, string email, string id)
        //{
            





        //    foreach (var obj in lst)
        //    {

        //        string mail = obj.email;
        //        string mamail = obj.maEmail;
        //        string matkhau = obj.matKhau;
        //        string noidung = obj.noiDung;


        //        if (mamail == "01")
        //        {
        //            string emailpheduyet = Convert.ToString(TempData["emailndtta1"]);

        //            try
        //            {

        //                WebMail.SmtpServer = "smtp.gmail.com";

        //                WebMail.SmtpPort = 587;
        //                WebMail.SmtpUseDefaultCredentials = true;

        //                //  ViewBag.test = t;
        //                WebMail.EnableSsl = true;


        //                WebMail.UserName = mail;
        //                WebMail.Password = matkhau;


        //                WebMail.From = mail;

        //                //Send email
        //                WebMail.Send(to: emailpheduyet, subject: "Mượn sách thành công", body: "Xin chào bạn  " + TempData["tenndttmn"] + "!" + "<br>" +
        //                    "Bạn vừa mượn thành công cuốn sách  " + TempData["tensachttmn"] + "<br>" + "Bạn có thể đến thư viện vào ngày   "
        //                + Session["ngaylamviec"] + noidung, isBodyHtml: true);

        //                ViewBag.Status = "Email Sent Successfully ";
        //            }
        //            catch (Exception)
        //            {
        //                ViewBag.Status = "Problem while sending email, Please check details.";

        //            }


        //        }



        //    }
        //    return RedirectToAction("loadsachtheongaythem", "TieuDe");
        //}
    }
}