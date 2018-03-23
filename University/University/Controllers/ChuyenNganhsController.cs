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
    public class ChuyenNganhsController : Controller
    {
        private UniversityEntities db = new UniversityEntities();

        // GET: ChuyenNganhs
        public ActionResult Index()
        {
            var chuyenNganhs = db.ChuyenNganhs.Include(c => c.NganhHoc);
            return View(chuyenNganhs.ToList());
        }

        // GET: ChuyenNganhs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = db.ChuyenNganhs.Find(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            return View(chuyenNganh);
        }

        // GET: ChuyenNganhs/Create
        public ActionResult Create()
        {
            ViewBag.maNganh = new SelectList(db.NganhHocs, "maNganh", "tenNganh");
            return View();
        }

        // POST: ChuyenNganhs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maChuyenNganh,tenChuyenNganh,maNganh")] ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                db.ChuyenNganhs.Add(chuyenNganh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maNganh = new SelectList(db.NganhHocs, "maNganh", "tenNganh", chuyenNganh.maNganh);
            return View(chuyenNganh);
        }

        // GET: ChuyenNganhs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = db.ChuyenNganhs.Find(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.maNganh = new SelectList(db.NganhHocs, "maNganh", "tenNganh", chuyenNganh.maNganh);
            return View(chuyenNganh);
        }

        // POST: ChuyenNganhs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maChuyenNganh,tenChuyenNganh,maNganh")] ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenNganh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maNganh = new SelectList(db.NganhHocs, "maNganh", "tenNganh", chuyenNganh.maNganh);
            return View(chuyenNganh);
        }

        // GET: ChuyenNganhs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenNganh chuyenNganh = db.ChuyenNganhs.Find(id);
            if (chuyenNganh == null)
            {
                return HttpNotFound();
            }
            return View(chuyenNganh);
        }

        // POST: ChuyenNganhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ChuyenNganh chuyenNganh = db.ChuyenNganhs.Find(id);
            db.ChuyenNganhs.Remove(chuyenNganh);
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
