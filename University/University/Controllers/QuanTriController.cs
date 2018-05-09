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
    public class QuanTriController : Controller
    {
        private UniversityEntities1 db = new UniversityEntities1();

        // GET: QuanTri
        public ActionResult Index()
        {
            var quanTris = db.QuanTris.Include(q => q.TaiKhoan);
            return View(quanTris.ToList());
        }

        // GET: QuanTri/Details/5
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

        // GET: QuanTri/Create
        public ActionResult Create()
        {
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau");
            return View();
        }

        // POST: QuanTri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "maQuanTri,tenQuanTri,queQuan,ngaySinh,tenDangNhap,trangThai")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.QuanTris.Add(quanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // GET: QuanTri/Edit/5
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
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // POST: QuanTri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "maQuanTri,tenQuanTri,queQuan,ngaySinh,tenDangNhap,trangThai")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "matKhau", quanTri.tenDangNhap);
            return View(quanTri);
        }

        // GET: QuanTri/Delete/5
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

        // POST: QuanTri/Delete/5
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

        public ActionResult Admin()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }

            return View();
        }
        public ActionResult TKSinhVien()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else if ((string)Session["loaiTaiKhoan"] != "Admin")
            {
                TempData["phanquyen"] = "sondep";
                return RedirectToAction("Home", "TaiKhoans");
            }


            var listtksv = from tk in db.TaiKhoans
                           join sv in db.SinhViens
                           on tk.tenDangNhap equals sv.tenDangNhap
                           join l in db.Lops
                           on sv.maLop equals l.maLop
                           join cn in db.ChuyenNganhs
                           on sv.maChuyenNganh equals cn.maChuyenNganh


                           select new ModelViewTKSV()
                           {
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               tendangnhap = tk.tenDangNhap,
                               loaitaikhoan = tk.loaiTaiKhoan,

                               quequan = sv.queQuan,
                               ngaysinh = (DateTime)sv.ngaySinh,
                               tenlop = l.tenLop,
                               gioitinh = sv.gioiTinh,
                               tennganh = cn.tenChuyenNganh,
                               Email = sv.eMail,
                               trangthai = sv.trangThai,
                               namnhaphoc = (Int32)sv.namNhapHoc,
                               hinhanh = sv.hinhAnh



                           };
            return View(listtksv.ToList());
        }
        public ActionResult CTTK(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listtksv = from tk in db.TaiKhoans
                           join sv in db.SinhViens
                           on tk.tenDangNhap equals sv.tenDangNhap
                           join l in db.Lops
                           on sv.maLop equals l.maLop
                           join cn in db.ChuyenNganhs
                           on sv.maChuyenNganh equals cn.maChuyenNganh


                           select new ModelViewTKSV()
                           {
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               tendangnhap = tk.tenDangNhap,
                               loaitaikhoan = tk.loaiTaiKhoan,

                               quequan = sv.queQuan,
                               ngaysinh = (DateTime)sv.ngaySinh,
                               tenlop = l.tenLop,
                               gioitinh = sv.gioiTinh,
                               tennganh = cn.tenChuyenNganh,
                               Email = sv.eMail,
                               trangthai = sv.trangThai,
                               namnhaphoc = (Int32)sv.namNhapHoc,
                               hinhanh = sv.hinhAnh



                           };
            var s = listtksv.Where(x => x.masv == id).FirstOrDefault();
           
            if (s == null)
            {
                return HttpNotFound();
            }
            return View(s);
        }
        public ActionResult CreateNewStudent(ModelViewTKSV model)
        {
            TaiKhoan tk = new TaiKhoan();
            SinhVien sv = new SinhVien();
            sv.maSinhVien = model.masv;
            sv.tenSinhVien = model.tensv;
            sv.queQuan = model.quequan;
            sv.ngaySinh = model.ngaysinh;
            sv.namNhapHoc = model.namnhaphoc;
            sv.maChuyenNganh = model.machuyennganh;
            sv.maLop = "";
            sv.eMail = model.Email;
            tk.tenDangNhap = model.tendangnhap;
            tk.matKhau = model.matkhau;
            tk.loaiTaiKhoan = model.loaitaikhoan;
            sv.tenDangNhap = tk.tenDangNhap;
            sv.gioiTinh = model.gioitinh;
            sv.eMailPH = model.emailph;
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(tk);
                db.SinhViens.Add(sv);

                db.SaveChanges();
                return RedirectToAction("TKSinhVien");
            }
            return View(model);
           
            
        }
    }
}
