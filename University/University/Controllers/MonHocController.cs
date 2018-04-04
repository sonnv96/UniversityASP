﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Models.Data;
using University.Models;

namespace University.Controllers
{
    public class MonHocController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();
        Random rd = new Random();
        // GET: MonHoc
        public ActionResult Index()
        {
            var monHocs = db.MonHocs.Include(m => m.ChuyenNganh);
            return View(monHocs.ToList());
        }

        // GET: MonHoc/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        // GET: MonHoc/Create
        public ActionResult Create()
        {
            ViewBag.maChuyenNganh = new SelectList(db.ChuyenNganhs, "maChuyenNganh", "tenChuyenNganh");
            return View();
        }

        // POST: MonHoc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maMonHoc,tenMonHoc,maChuyenNganh,soTinChi,trangThai")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.MonHocs.Add(monHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maChuyenNganh = new SelectList(db.ChuyenNganhs, "maChuyenNganh", "tenChuyenNganh", monHoc.maChuyenNganh);
            return View(monHoc);
        }

        // GET: MonHoc/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.maChuyenNganh = new SelectList(db.ChuyenNganhs, "maChuyenNganh", "tenChuyenNganh", monHoc.maChuyenNganh);
            return View(monHoc);
        }

        // POST: MonHoc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maMonHoc,tenMonHoc,maChuyenNganh,soTinChi,trangThai")] MonHoc monHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maChuyenNganh = new SelectList(db.ChuyenNganhs, "maChuyenNganh", "tenChuyenNganh", monHoc.maChuyenNganh);
            return View(monHoc);
        }

        // GET: MonHoc/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonHoc monHoc = db.MonHocs.Find(id);
            if (monHoc == null)
            {
                return HttpNotFound();
            }
            return View(monHoc);
        }

        // POST: MonHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MonHoc monHoc = db.MonHocs.Find(id);
            db.MonHocs.Remove(monHoc);
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
        public ActionResult MoLop(string mamon)
        {
            var a = db.MonHocs.Where(x => x.maMonHoc == mamon).FirstOrDefault();
            
            TempData["tenmonhoc"] = a.tenMonHoc;

            return RedirectToAction("MoDKMon", "MonHoc");
        }
        public ActionResult MoDKMon()
        {

            var b = TempData["a"];

            LopMonHoc lopmonhoc = new LopMonHoc();
            lopmonhoc.maMonHoc = 
           
            lopmonhoc.maLopMonHoc = "P" + rd.Next(100000).ToString();
            lopmonhoc.tenLopMonHoc = "S" + rd.Next(10000).ToString();
           

            lopmonhoc.soLuongToiDa = 40;
            lopmonhoc.soLuongDangKy = 0;
        
            lopmonhoc.namHoc = null;
            lopmonhoc.hocKy = null;
            lopmonhoc.phongHoc = null;
            lopmonhoc.tietHoc = "";
            lopmonhoc.trangThai = "dangchodangki";
            db.LopMonHocs.Add(lopmonhoc);
            db.SaveChanges();


            //var listmh = from lmh in db.LopMonHocs
            //             join mh in db.MonHocs
            //             on lmh.maMonHoc equals mh.maMonHoc
            //             join gv in db.GiangViens
            //             on lmh.maGiangVien equals gv.maGiangVien
            //             join bd in db.BangDiems
            //             on lmh.maLopMonHoc equals bd.maLopMonHoc
            //             join sv in db.SinhViens
            //             on bd.maSinhVien equals sv.maSinhVien
            //             join l in db.Lops
            //             on sv.maLop equals l.maLop
            //             select new ModelViewDKMon()
            //             {
            //                 malophocphan = lmh.maLopMonHoc,
            //                 monhoc = mh.tenMonHoc,
            //                 tinchi = (Int32)mh.soTinChi,
            //                 soluong = (Int32)lmh.soLuongToiDa,
            //                 soluongdangki = (Int32)lmh.soLuongDangKy,
            //                 tinhtrang = lmh.trangThai,
            //                 tengiangvien = gv.tenGiangVien,
            //                 lopdangki = l.tenLop,
            //                 phonghoc = lmh.phongHoc,
            //                 tiethoc = lmh.tietHoc,
            //                 ngayhoc = (Int32)lmh.ngayHoc,
            //                 namhoc = (Int32)lmh.namHoc,
            //                 hocki = (Int32)lmh.hocKy

            //             };




            //lopmonhoc.malophocphan = "P" + rd.Next(100000).ToString();
            //lopmonhoc.monhoc = a.tenMonHoc;
            //lopmonhoc.tinchi = (Int32)a.soTinChi;
            //lopmonhoc.soluong = 40;
            //lopmonhoc.soluongdangki = 0;
            //lopmonhoc.tinhtrang = "Dangchodangki";
            //lopmonhoc.tengiangvien = c.tenGiangVien;

            //lopmonhoc.phonghoc = b.phongHoc;
            ////xử lí sau
            //lopmonhoc.tiethoc = b.tietHoc;
            //lopmonhoc.ngayhoc =(Int32)b.ngayHoc;
            //lopmonhoc.namhoc = nam;
            //lopmonhoc.hocki = hocki;
            //mdkm.Add(lopmonhoc);+













            return View();
        }
        public ActionResult DKiMon(int hocki, int nam)
        {
            var listmh = from lmh in db.LopMonHocs
                         join mh in db.MonHocs
                         on lmh.maMonHoc equals mh.maMonHoc
                         join gv in db.GiangViens
                         on lmh.maGiangVien equals gv.maGiangVien
                         join bd in db.BangDiems
                         on lmh.maLopMonHoc equals bd.maLopMonHoc
                         join sv in db.SinhViens
                         on bd.maSinhVien equals sv.maSinhVien
                         join l in db.Lops
                         on sv.maLop equals l.maLop
                         select new ModelViewDKMon()
                         {
                             malophocphan = lmh.maLopMonHoc,
                             monhoc = mh.tenMonHoc,
                             tinchi = (Int32)mh.soTinChi,
                             soluong = (Int32)lmh.soLuongToiDa,
                             soluongdangki = (Int32)lmh.soLuongDangKy,
                             tinhtrang = lmh.trangThai,
                             tengiangvien = gv.tenGiangVien,
                             lopdangki = l.tenLop,
                             phonghoc = lmh.phongHoc,
                             tiethoc = lmh.tietHoc,
                             ngayhoc = (Int32)lmh.ngayHoc,
                             namhoc = (Int32)lmh.namHoc,
                             hocki = (Int32)lmh.hocKy

                         };

            return View(listmh.Where(x => x.namhoc == nam && x.hocki == hocki).ToList());
        }
    }
}
