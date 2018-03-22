using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using University.Models.Data;

namespace University.Controllers
{
    public class TaiKhoansController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: TaiKhoans
        public ActionResult Index()
        {
            return View(db.TaiKhoans.ToList());
        }

        // GET: TaiKhoans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenDangNhap,matKhau,loaiTaiKhoan")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tenDangNhap,matKhau,loaiTaiKhoan")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]

        public ActionResult Login(TaiKhoan taikhoan)
        {
            if (ModelState.IsValid)
            {
                using (UniversityEntities db = new UniversityEntities())
                {
                    var obj = db.TaiKhoans.Where(a => a.tenDangNhap.Equals(taikhoan.tenDangNhap) && a.matKhau.Equals(taikhoan.matKhau)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserName"] = obj.tenDangNhap.ToString();
                        Session["loaiTaiKhoan"] = obj.loaiTaiKhoan.ToString();


                        if (obj.loaiTaiKhoan == 1)
                        {
                            return RedirectToAction("Index", "");
                        }
                        else if (obj.loaiTaiKhoan == 2)
                        {
                            return RedirectToAction("Index", "");
                        }

                        return RedirectToAction("loadtatca", "TieuDe");
                    }
                    else
                    {
                        TempData["taikhoan"] = "abc";
                    }
                }
            }
            return View(taikhoan);
        }
       
        public ActionResult LogOff()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "TaiKhoan");
        }

    }
}
