using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Models.Data;

namespace University.Controllers
{
    public class BangDiemController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();

        // GET: BangDiem
        public ActionResult Index()
        {
            var bangDiems = db.BangDiems.Include(b => b.LopMonHoc).Include(b => b.SinhVien);
            return View(bangDiems.ToList());
        }

        // GET: BangDiem/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // GET: BangDiem/Create
        public ActionResult Create()
        {
            ViewBag.maLopMonHoc = new SelectList(db.LopMonHocs, "maLopMonHoc", "tenLopMonHoc");
            ViewBag.maSinhVien = new SelectList(db.SinhViens, "maSinhVien", "tenSinhVien");
            return View();
        }

        // POST: BangDiem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maSinhVien,maLopMonHoc,trangThai,thucHanh,giuaKy,cuoiKy,thuongKy")] BangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.BangDiems.Add(bangDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maLopMonHoc = new SelectList(db.LopMonHocs, "maLopMonHoc", "tenLopMonHoc", bangDiem.maLopMonHoc);
            ViewBag.maSinhVien = new SelectList(db.SinhViens, "maSinhVien", "tenSinhVien", bangDiem.maSinhVien);
            return View(bangDiem);
        }

        // GET: BangDiem/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            ViewBag.maLopMonHoc = new SelectList(db.LopMonHocs, "maLopMonHoc", "tenLopMonHoc", bangDiem.maLopMonHoc);
            ViewBag.maSinhVien = new SelectList(db.SinhViens, "maSinhVien", "tenSinhVien", bangDiem.maSinhVien);
            return View(bangDiem);
        }

        // POST: BangDiem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maSinhVien,maLopMonHoc,trangThai,thucHanh,giuaKy,cuoiKy,thuongKy")] BangDiem bangDiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bangDiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maLopMonHoc = new SelectList(db.LopMonHocs, "maLopMonHoc", "tenLopMonHoc", bangDiem.maLopMonHoc);
            ViewBag.maSinhVien = new SelectList(db.SinhViens, "maSinhVien", "tenSinhVien", bangDiem.maSinhVien);
            return View(bangDiem);
        }

        // GET: BangDiem/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangDiem bangDiem = db.BangDiems.Find(id);
            if (bangDiem == null)
            {
                return HttpNotFound();
            }
            return View(bangDiem);
        }

        // POST: BangDiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BangDiem bangDiem = db.BangDiems.Find(id);
            db.BangDiems.Remove(bangDiem);
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
    }
}
