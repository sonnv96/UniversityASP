using System;
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
    public class GiangVienController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();

        // GET: GiangVien
        public ActionResult Index()
        {
            var giangViens = db.GiangViens.Include(g => g.TaiKhoan);
            return View(giangViens.ToList());
        }

        // GET: GiangVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // GET: GiangVien/Create
        public ActionResult Create()
        {
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau");
            return View();
        }

        // POST: GiangVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maGiangVien,tenGiangVien,queQuan,ngaySinh,namBatDau,eMail,tenDangNhap,trangThai")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                db.GiangViens.Add(giangVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", giangVien.tenDangNhap);
            return View(giangVien);
        }

        // GET: GiangVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", giangVien.tenDangNhap);
            return View(giangVien);
        }

        // POST: GiangVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maGiangVien,tenGiangVien,queQuan,ngaySinh,namBatDau,eMail,tenDangNhap,trangThai")] GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giangVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", giangVien.tenDangNhap);
            return View(giangVien);
        }

        // GET: GiangVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiangVien giangVien = db.GiangViens.Find(id);
            if (giangVien == null)
            {
                return HttpNotFound();
            }
            return View(giangVien);
        }

        // POST: GiangVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GiangVien giangVien = db.GiangViens.Find(id);
            db.GiangViens.Remove(giangVien);
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

        public ActionResult GiangVien()
        {

            if (Session["UserName"] == null && Session["MaGV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }

            return View();
        }
        public ActionResult XemMonHoc(int nam = 0, int hocki = 0)
        {
            string ten = Session["UserName"].ToString();
            var listgiangday = from gv in db.GiangViens
                           join lmh in db.LopMonHocs
                           on gv.maGiangVien equals lmh.maGiangVien
                           join tk in db.TaiKhoans
                           on gv.tenDangNhap equals tk.tenDangNhap
                           join mh in db.MonHocs
                           on lmh.maMonHoc equals mh.maMonHoc
                           
                           select new ModelViewXemMonHoc()
                           {
                              tengiangvien = gv.tenGiangVien,
                              tendangnhap = tk.tenDangNhap,
                              tenlophp = lmh.tenLopMonHoc,
                              tenmonhoc = mh.tenMonHoc,
                              namhoc = (Int32)lmh.namHoc,
                              hocki = (Int32)lmh.hocKy,
                              ngayhoc = (Int32)lmh.ngayHoc,
                              phonghoc = lmh.phongHoc,
                              tiethoc = lmh.tietHoc
                           };
            var get = listgiangday.ToList();



            // Tạo SelectList
            SelectList cateList = new SelectList(get, "namhoc", "namhoc");
            SelectList cateList2 = new SelectList(get, "hocki", "hocki");


            // Set vào ViewBag
            ViewBag.namhoc = cateList;
            ViewBag.hocki = cateList2;



            return View(listgiangday.ToList().Where( x=> x.namhoc == nam && x.hocki == hocki));

        }
    }
}
