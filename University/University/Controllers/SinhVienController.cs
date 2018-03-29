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
using PagedList;

namespace University.Controllers
{
    public class SinhVienController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();

        // GET: SinhVien
        public ActionResult Index()
        {
            var sinhViens = db.SinhViens.Include(s => s.Lop).Include(s => s.TaiKhoan);
            return View(sinhViens.ToList());
        }

        // GET: SinhVien/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }

        // GET: SinhVien/Create
        public ActionResult Create()
        {
            ViewBag.maLop = new SelectList(db.Lops, "maLop", "tenLop");
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau");
            return View();
        }

        // POST: SinhVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maSinhVien,tenSinhVien,queQuan,ngaySinh,namNhapHoc,maChuyenNganh,eMail,tenDangNhap,gioiTinh,trangThai,maLop")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                db.SinhViens.Add(sinhVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.maLop = new SelectList(db.Lops, "maLop", "tenLop", sinhVien.maLop);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", sinhVien.tenDangNhap);
            return View(sinhVien);
        }

        // GET: SinhVien/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.maLop = new SelectList(db.Lops, "maLop", "tenLop", sinhVien.maLop);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", sinhVien.tenDangNhap);
            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maSinhVien,tenSinhVien,queQuan,ngaySinh,namNhapHoc,maChuyenNganh,eMail,tenDangNhap,gioiTinh,trangThai,maLop")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sinhVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.maLop = new SelectList(db.Lops, "maLop", "tenLop", sinhVien.maLop);
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", sinhVien.tenDangNhap);
            return View(sinhVien);
        }

        // GET: SinhVien/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SinhVien sinhVien = db.SinhViens.Find(id);
            if (sinhVien == null)
            {
                return HttpNotFound();
            }
            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SinhVien sinhVien = db.SinhViens.Find(id);
            db.SinhViens.Remove(sinhVien);
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
        public ActionResult TTSV(int page = 1, int pageSize = 10)
        {
            string id = Session["UserName"].ToString();
          
            var list = from tk in db.TaiKhoans
                       join sv in db.SinhViens
                       on tk.tenDangNhap equals sv.tenDangNhap
                       join l in db.Lops
                       on sv.maLop equals l.maLop
                       join cn in db.ChuyenNganhs
                       on sv.maChuyenNganh equals cn.maChuyenNganh
                       select new ModelviewSinhVien()
                       {
                           masv = sv.maSinhVien,
                           tensv = sv.tenSinhVien,
                           tendangnhap = tk.tenDangNhap,
                           quequan = sv.queQuan,
                           ngaysinh = (DateTime)sv.ngaySinh,
                           tenlop = l.tenLop,
                           gioitinh = (Int32)sv.gioiTinh,
                           tennganh = cn.tenChuyenNganh,
                           Email = sv.eMail,
                           trangthai = sv.trangThai,
                           namnhaphoc = (Int32)sv.namNhapHoc
                       };







            return View(list.ToList().Where(x=>x.tendangnhap == id).ToPagedList(page, pageSize));

        }
        public ActionResult Xemdiem(int page = 1, int pageSize = 10)
        {

            string id = Session["UserName"].ToString();

            var listdiem = from tk in db.TaiKhoans
                       join sv in db.SinhViens
                       on tk.tenDangNhap equals sv.tenDangNhap
                       join bd in db.BangDiems
                       on sv.maSinhVien equals bd.maSinhVien
                       join lmh in db.LopMonHocs
                       on bd.maLopMonHoc equals lmh.maLopMonHoc
                       select new ModelViewDiem()
                       {
                           masv = sv.maSinhVien,
                           tensv = sv.tenSinhVien,
                           tendangnhap = tk.tenDangNhap,
                           ngaysinh = (DateTime)sv.ngaySinh,
                           gioitinh = (Int32)sv.gioiTinh,
                           giuaki = (Int32)bd.giuaKy,
                           cuoiki = (Int32)bd.cuoiKy,
                           thuhanh = (Int32)bd.thucHanh,
                           mamonhoc = lmh.maMonHoc


                       };
            return View(listdiem.ToList().Where(x=>x.tendangnhap == id).ToPagedList(page, pageSize));
        }
        public ActionResult SinhVien()
        {
          
            if(Session["UserName"]  == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }

            return View();
        }
    }
}
