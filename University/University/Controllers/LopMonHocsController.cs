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
    public class LopMonHocsController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: LopMonHocs
        public ActionResult Index()
        {
            var lopMonHocs = db.LopMonHocs.Include(l => l.GiangVien).Include(l => l.MonHoc);
            return View(lopMonHocs.ToList());
        }

        // GET: LopMonHocs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            if (lopMonHoc == null)
            {
                return HttpNotFound();
            }
            return View(lopMonHoc);
        }

        // GET: LopMonHocs/Create
        public ActionResult Create()
        {
            ViewBag.maGiangVien = new SelectList(db.GiangViens, "maGiangVien", "tenGiangVien");
            ViewBag.maMonHoc = new SelectList(db.MonHocs, "maMonHoc", "tenMonHoc");
            return View();
        }

        // POST: LopMonHocs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maLop,maMonHoc,soLuongToiDa,soLuongDangKy,maGiangVien,trangThai")] LopMonHoc lopMonHoc)
        {
            if (ModelState.IsValid)
            {
                db.LopMonHocs.Add(lopMonHoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maGiangVien = new SelectList(db.GiangViens, "maGiangVien", "tenGiangVien", lopMonHoc.maGiangVien);
            ViewBag.maMonHoc = new SelectList(db.MonHocs, "maMonHoc", "tenMonHoc", lopMonHoc.maMonHoc);
            return View(lopMonHoc);
        }

        // GET: LopMonHocs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            if (lopMonHoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.maGiangVien = new SelectList(db.GiangViens, "maGiangVien", "tenGiangVien", lopMonHoc.maGiangVien);
            ViewBag.maMonHoc = new SelectList(db.MonHocs, "maMonHoc", "tenMonHoc", lopMonHoc.maMonHoc);
            return View(lopMonHoc);
        }

        // POST: LopMonHocs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maLop,maMonHoc,soLuongToiDa,soLuongDangKy,maGiangVien,trangThai")] LopMonHoc lopMonHoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lopMonHoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maGiangVien = new SelectList(db.GiangViens, "maGiangVien", "tenGiangVien", lopMonHoc.maGiangVien);
            ViewBag.maMonHoc = new SelectList(db.MonHocs, "maMonHoc", "tenMonHoc", lopMonHoc.maMonHoc);
            return View(lopMonHoc);
        }

        // GET: LopMonHocs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            if (lopMonHoc == null)
            {
                return HttpNotFound();
            }
            return View(lopMonHoc);
        }

        // POST: LopMonHocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            db.LopMonHocs.Remove(lopMonHoc);
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
