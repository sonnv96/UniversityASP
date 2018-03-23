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
    public class QuanTrisController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: QuanTris
        public ActionResult Index()
        {
            var quanTris = db.QuanTris.Include(q => q.BaiViet).Include(q => q.TaiKhoan);
            return View(quanTris.ToList());
        }

        // GET: QuanTris/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // GET: QuanTris/Create
        public ActionResult Create()
        {
            ViewBag.maBaiViet = new SelectList(db.BaiViets, "maBaiViet", "tieuDe");
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau");
            return View();
        }

        // POST: QuanTris/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maQuanTri,tenQuanTri,queQuan,ngaySinh,tenDangNhap,maBaiViet")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.QuanTris.Add(quanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maBaiViet = new SelectList(db.BaiViets, "maBaiViet", "tieuDe", quanTri.maBaiViet);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // GET: QuanTris/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            ViewBag.maBaiViet = new SelectList(db.BaiViets, "maBaiViet", "tieuDe", quanTri.maBaiViet);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // POST: QuanTris/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maQuanTri,tenQuanTri,queQuan,ngaySinh,tenDangNhap,maBaiViet")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maBaiViet = new SelectList(db.BaiViets, "maBaiViet", "tieuDe", quanTri.maBaiViet);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // GET: QuanTris/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: QuanTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuanTri quanTri = db.QuanTris.Find(id);
            db.QuanTris.Remove(quanTri);
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
