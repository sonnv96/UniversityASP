﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using University.Models.Data;
using University.Models;

namespace University.Controllers
{
    public class TaiKhoansController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();

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
        public ActionResult DangNhap()
        {

            return View();
        }


        [HttpPost]

        public ActionResult DangNhap(TaiKhoan taikhoan, string tenDangNhap, string matKhau)
        {
            if (ModelState.IsValid)
            {
                using (UniversityEntities1 db = new UniversityEntities1())
                {

                    var list = from tk in db.TaiKhoans
                               join sv in db.SinhViens
                               on tk.tenDangNhap equals sv.tenDangNhap
                             
                               select new ModelViewTaiKhoan()
                               {
                                   masv = sv.maSinhVien,
                                   
                                   tendangnhap = tk.tenDangNhap,
                                   matkhau = tk.matKhau,
                                   loaitk = tk.loaiTaiKhoan
                               };
                    var list2 = from tk in db.TaiKhoans
                               join gv in db.GiangViens
                               on tk.tenDangNhap equals gv.tenDangNhap

                               select new ModelViewTaiKhoan()
                               {
                                  magv = gv.maGiangVien,

                                   tendangnhap = tk.tenDangNhap,
                                   matkhau = tk.matKhau,
                                   loaitk = tk.loaiTaiKhoan
                               };
                    var obj = db.TaiKhoans.Where(a => a.tenDangNhap.Equals(taikhoan.tenDangNhap) && a.matKhau.Equals(taikhoan.matKhau)).FirstOrDefault() ;
                    var obj2 = list.Where(a => a.masv == tenDangNhap && a.matkhau == matKhau).FirstOrDefault();
                    var objmagv = list2.Where(a => a.magv == tenDangNhap && a.matkhau == matKhau).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserName"] = obj.tenDangNhap.ToString();
                        Session["loaiTaiKhoan"] = obj.loaiTaiKhoan.ToString();


                        if (obj.loaiTaiKhoan == "SinhVien")
                        {
                            return RedirectToAction("SinhVien", "SinhVien");
                        }
                        else if (obj.loaiTaiKhoan == "GiangVien")
                        {
                            return RedirectToAction("GiangVien", "GiangVien");
                        }
                        else if (obj.loaiTaiKhoan == "Admin")
                        {
                            return RedirectToAction("Index", "MonHoc");
                        }


                        return RedirectToAction("XemDiem", "SinhVien");
                    }
                    else if (obj2 != null)
                    {
                        Session["MaSV"] = obj2.masv.ToString();
                        Session["loaiTaiKhoan"] = obj2.loaitk.ToString();
                      

                        if (obj2.loaitk == "SinhVien")
                        {
                            return RedirectToAction("SinhVien", "SinhVien");
                        }
                      
                    }
                    else if(objmagv !=null)
                    {
                        Session["MaGV"] = objmagv.magv.ToString();
                        Session["loaiTaiKhoan"] = objmagv.loaitk.ToString();
                      


                       
                         if (objmagv.loaitk == "GiangVien")
                        {
                            return RedirectToAction("GiangVien", "GiangVien");
                        }
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
            return RedirectToAction("DangNhap", "TaiKhoans");
        }
        public ActionResult Home()
        {
            
            return View();
        }


    }
}
