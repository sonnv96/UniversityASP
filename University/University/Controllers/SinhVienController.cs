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
        public ActionResult TTSV()
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
                           gioitinh = sv.gioiTinh,
                           tennganh = cn.tenChuyenNganh,
                           Email = sv.eMail,
                           trangthai = sv.trangThai,
                           namnhaphoc = (Int32)sv.namNhapHoc,
                             hinhanh = sv.hinhAnh
                             
                       };
            var get = list.Where(x => x.tendangnhap == id).FirstOrDefault();
            ViewBag.tensv = get.tensv;
            ViewBag.hinhanh = get.hinhanh;
            ViewBag.trangthai = get.trangthai;
            ViewBag.masv = get.masv;
            ViewBag.email = get.Email;
            ViewBag.gioitinh = get.ngaysinh;
            ViewBag.ngaysinh = get.ngaysinh;
            ViewBag.quequan = get.quequan;
            ViewBag.tenlop = get.tenlop;
            ViewBag.nganh = get.tennganh;
            ViewBag.namnhaphoc = get.namnhaphoc;







            return View(list.ToList().Where(x => x.tendangnhap == id));

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
                           gioitinh = sv.gioiTinh,
                           giuaki = (Decimal)bd.giuaKy,
                           cuoiki = (Decimal)bd.cuoiKy,
                           thuhanh = (Decimal)bd.thucHanh,
                           mamonhoc = lmh.maMonHoc,
                           hinhanh = sv.hinhAnh,
                           nam = (Int32)lmh.namHoc,
                           hocki = (Int32)lmh.hocKy
                           
                           

                       };
            var get = listdiem.Where(x => x.tendangnhap == id).FirstOrDefault();
            ViewBag.tensv = get.tensv;
            ViewBag.hinhanh = get.hinhanh;
            ViewBag.masv = get.masv;
            ViewBag.gioitinh = get.gioitinh;
    
       





            return View(listdiem.ToList().Where(x=>x.tendangnhap == id).OrderByDescending(x=>x.nam).OrderByDescending(x=>x.hocki).ToPagedList(page, pageSize));
        }
        public ActionResult SinhVien()
        {
          
            if(Session["UserName"]  == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }

            return View();
        }
        public ActionResult XemThoiKhoaBieu(int page = 1, int pageSize = 10, int nam = 0, int hocki = 0 )
        {

            string id = Session["UserName"].ToString();
          
            var listtkb = from tk in db.TaiKhoans
                           join sv in db.SinhViens
                           on tk.tenDangNhap equals sv.tenDangNhap
                           join bd in db.BangDiems
                           on sv.maSinhVien equals bd.maSinhVien
                           join lmh in db.LopMonHocs
                           on bd.maLopMonHoc equals lmh.maLopMonHoc
                           join gv in db.GiangViens
                           on lmh.maGiangVien equals gv.maGiangVien
                           join mh in db.MonHocs
                           on lmh.maMonHoc equals mh.maMonHoc
                           select new ModelViewLichHoc()
                           {
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               tendangnhap = tk.tenDangNhap,
                               namhoc = (Int32)lmh.namHoc,
                               hocki = (Int32)lmh.hocKy,
                               ngayhoc = (Int32)lmh.ngayHoc,
                               monhoc = mh.tenMonHoc,
                               tiethoc = lmh.tietHoc,
                               giangvien = gv.tenGiangVien,
                               phonghoc = lmh.phongHoc,
                              


                           };
            var get = listtkb.ToList().Where(x=>x.tendangnhap == id).ToList();
           
            
            // Tạo SelectList
            SelectList cateList = new SelectList(get, "namhoc", "namhoc");
            SelectList cateList2 = new SelectList(get, "hocki", "hocki");


            // Set vào ViewBag
            ViewBag.namhoc = cateList;
            ViewBag.hocki = cateList2;
            //IEnumerable<ModelViewLichHoc> values =

            //          Enum.GetValues(typeof(ModelViewLichHoc))

            //          .Cast<ModelViewLichHoc>();

            //IEnumerable<SelectListItem> items =

            //    from value in values

            //    select new SelectListItem

            //    {

            //        Text = value.ToString(),

            //        Value = value.ToString(),

            //        Selected = value == ,

            //    };

            //ViewBag.MovieType = items;




            //ViewBag.Day = items;


            return View(listtkb.ToList().Where(x => x.tendangnhap == id && x.namhoc == nam && x.hocki ==hocki).ToPagedList(page, pageSize));
        }

    }
}
