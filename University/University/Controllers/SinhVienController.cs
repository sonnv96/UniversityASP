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
            ViewBag.tenDangNhap = new SelectList(db.TaiKhoans, "tenDangNhap", "tenDangNhap", sinhVien.tenDangNhap);
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
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");

            }
            else
            {
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

                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get = list.Where(x => x.tendangnhap == ten).FirstOrDefault();

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
                    return View(list.ToList().Where(x => x.tendangnhap == ten));
                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get = list.Where(x => x.masv == id).FirstOrDefault();

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

                    return View(list.ToList().Where(x => x.masv == id));

                }









            }

        }
        public ActionResult Xemdiem()
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {


                var listdiem = from tk in db.TaiKhoans
                               join sv in db.SinhViens
                               on tk.tenDangNhap equals sv.tenDangNhap
                               join bd in db.BangDiems
                               on sv.maSinhVien equals bd.maSinhVien
                               join lmh in db.LopMonHocs
                               on bd.maLopMonHoc equals lmh.maLopMonHoc
                               join mh in db.MonHocs
                               on lmh.maMonHoc equals mh.maMonHoc
                               select new ModelViewDiem()
                               {
                                   masv = sv.maSinhVien,
                                   tensv = sv.tenSinhVien,
                                   tendangnhap = tk.tenDangNhap,
                                   ngaysinh = (DateTime?)sv.ngaySinh,
                                   gioitinh = sv.gioiTinh,
                                   thuongki = (Decimal?)bd.thuongKy,
                                   giuaki = (Decimal?)bd.giuaKy,
                                   cuoiki = (Decimal?)bd.cuoiKy,
                                   thuhanh = (Decimal?)bd.thucHanh,
                                   mamonhoc = lmh.maMonHoc,
                                   hinhanh = sv.hinhAnh,
                                   nam = (Int32)lmh.namHoc,
                                   hocki = (Int32)lmh.hocKy,
                                   tenmonhoc = mh.tenMonHoc,
                                   namnhaphoc = (Int32)sv.namNhapHoc



                               };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get = listdiem.Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get.tensv;
                    ViewBag.hinhanh = get.hinhanh;
                    ViewBag.masv = get.masv;
                    ViewBag.gioitinh = get.gioitinh;
                    var a = get.namnhaphoc;
                    var namnhat = get.namnhaphoc;
                    var namhai = get.namnhaphoc + 1;
                    var namba = get.namnhaphoc + 2;
                    var nambon = get.namnhaphoc + 4;
                    ViewBag.namnhat = get.namnhaphoc;
                    ViewBag.namhai = get.namnhaphoc + 1;
                    ViewBag.namba = get.namnhaphoc + 2;
                    ViewBag.namtu = get.namnhaphoc + 3;
                    ViewBag.diemtk = (((get.thuongki * 2) + (get.giuaki * 3) + (get.cuoiki * 5)) / 5 + get.thuhanh) / 3;
                    ViewBag.namnhathk1 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namnhat && x.hocki == 1).ToList();
                    ViewBag.namnhathk2 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namnhat && x.hocki == 2).ToList();
                    ViewBag.namhaihk1 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namhai && x.hocki == 1).ToList();
                    ViewBag.namhaihk2 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namhai && x.hocki == 2).ToList();
                    ViewBag.nambahk1 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namba && x.hocki == 1).ToList();
                    ViewBag.nambahk2 = listdiem.Where(x => x.tendangnhap == ten && x.nam == namba && x.hocki == 2).ToList();
                    ViewBag.namtuhk1 = listdiem.Where(x => x.tendangnhap == ten && x.nam == nambon && x.hocki == 1).ToList();
                    ViewBag.namtuhk2 = listdiem.Where(x => x.tendangnhap == ten && x.nam == nambon && x.hocki == 2).ToList();







                    return View(listdiem.Where(x => x.tendangnhap == ten).OrderByDescending(x => x.nam).OrderByDescending(x => x.hocki).ToList());
                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get = listdiem.Where(x => x.masv == id).FirstOrDefault();
                    ViewBag.tensv = get.tensv;
                    ViewBag.hinhanh = get.hinhanh;
                    ViewBag.masv = get.masv;
                    ViewBag.gioitinh = get.gioitinh;







                    return View(listdiem.Where(x => x.masv == id).OrderByDescending(x => x.nam).OrderByDescending(x => x.hocki).ToList());
                }
            }
        }
        public ActionResult SinhVien()
        {

            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else if ((string)Session["loaiTaiKhoan"] != "SinhVien")
            {
                TempData["phanquyen"] = "sondep";
                return RedirectToAction("Home", "TaiKhoans");
            }


            return View();


        }
        public ActionResult XemThoiKhoaBieu(int page = 1, int pageSize = 10, int nam = 0, int hocki = 0)
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {


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
                                  hinhanh = sv.hinhAnh



                              };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get2 = listtkb.ToList().Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    var masv = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.ToList().Where(x => x.tendangnhap == ten).ToList();
                    var a = db.st_hocky(masv);
                    var b = db.st_namhoc(masv);


                    // Tạo SelectList
                    SelectList cateList = new SelectList(b, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a, "hocKy", "hocKy");


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


                    ViewBag.thu2 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 2).ToList();
                    ViewBag.thu3 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 3).ToList();
                    ViewBag.thu4 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 4).ToList();
                    ViewBag.thu5 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 5).ToList();
                    ViewBag.thu6 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 6).ToList();
                    ViewBag.thu7 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 7).ToList();
                    ViewBag.thu8 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 8).ToList();
                    ViewBag.thu2count = ViewBag.thu2.Count + 1;
                    ViewBag.thu3count = ViewBag.thu3.Count + 1;
                    ViewBag.thu4count = ViewBag.thu4.Count + 1;
                    ViewBag.thu5count = ViewBag.thu5.Count + 1;
                    ViewBag.thu6count = ViewBag.thu6.Count + 1;
                    ViewBag.thu7count = ViewBag.thu7.Count + 1;
                    ViewBag.thu8count = ViewBag.thu8.Count + 1;
                    return View(listtkb.ToList().Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki).ToPagedList(page, pageSize));

                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get2 = listtkb.ToList().Where(x => x.masv == id).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.masv == id).ToList();
                    var masv = get2.masv;
                    var a = db.st_hocky(masv);
                    var b = db.st_namhoc(masv);



                    // Tạo SelectList
                    SelectList cateList = new SelectList(get, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(get, "hocKy", "hocKy");


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

                    ViewBag.thu2 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 2).ToList();
                    ViewBag.thu3 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 3).ToList();
                    ViewBag.thu4 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 4).ToList();
                    ViewBag.thu5 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 5).ToList();
                    ViewBag.thu6 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 6).ToList();
                    ViewBag.thu7 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 7).ToList();
                    ViewBag.thu8 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 8).ToList();
                    ViewBag.thu2count = ViewBag.thu2.Count + 1;
                    ViewBag.thu3count = ViewBag.thu3.Count + 1;
                    ViewBag.thu4count = ViewBag.thu4.Count + 1;
                    ViewBag.thu5count = ViewBag.thu5.Count + 1;
                    ViewBag.thu6count = ViewBag.thu6.Count + 1;
                    ViewBag.thu7count = ViewBag.thu7.Count + 1;
                    ViewBag.thu8count = ViewBag.thu8.Count + 1;
                    return View(listtkb.ToList().Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki).ToPagedList(page, pageSize));

                }
            }
        }
        //public ActionRe  sult DangKiHPSV()
        //{
        //    if (Session["UserName"] == null && Session["MaSV"] == null)
        //    {
        //        return RedirectToAction("DangNhap", "TaiKhoans");
        //    }
        //    return View();
        //}

        [HttpPost]
        public PartialViewResult GetallMonhoc()
        {




            System.Threading.Thread.Sleep(3000); //DEMO ONLY
            int nam = Convert.ToInt32(Request.Form["NamHoc"]);
            int hocki = Convert.ToInt32(Request.Form["HocKi"]);

            var listmh = from lmh in db.LopMonHocs
                         join mh in db.MonHocs
                         on lmh.maMonHoc equals mh.maMonHoc
                         join gv in db.GiangViens
                         on lmh.maGiangVien equals gv.maGiangVien
                         join bd in db.BangDiems
                         on lmh.maLopMonHoc equals bd.maLopMonHoc
                         join sv in db.SinhViens
                         on bd.maSinhVien equals sv.maSinhVien
                         join tk in db.TaiKhoans
                         on sv.tenDangNhap equals tk.tenDangNhap
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
                             hocki = (Int32)lmh.hocKy,
                             tendn = tk.tenDangNhap,
                             masv = sv.maSinhVien

                         };
            if (Session["UserName"] != null)
            {
                string ten = Session["UserName"].ToString();
                var lst = listmh.Where(x => x.hocki == hocki && x.namhoc == nam).ToList();


                return PartialView("_GetallMonhoc", lst);

            }
            string id = Session["MaSV"].ToString();
            var lst2 = listmh.Where(x => x.hocki == hocki && x.namhoc == nam).ToList();


            return PartialView("_GetallMonhoc", lst2);

        }

        public PartialViewResult ChitietMon()
        {
            System.Threading.Thread.Sleep(3000); //DEMO ONLY
            string monhoc = Request.Form["mon"];

            var listmh = from lmh in db.LopMonHocs
                         join mh in db.MonHocs
                         on lmh.maMonHoc equals mh.maMonHoc
                         join gv in db.GiangViens
                         on lmh.maGiangVien equals gv.maGiangVien
                         join bd in db.BangDiems
                         on lmh.maLopMonHoc equals bd.maLopMonHoc
                         join sv in db.SinhViens
                         on bd.maSinhVien equals sv.maSinhVien
                         join tk in db.TaiKhoans
                         on sv.tenDangNhap equals tk.tenDangNhap
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
                             hocki = (Int32)lmh.hocKy,
                             tendn = tk.tenDangNhap,
                             masv = sv.maSinhVien

                         };
            var a = listmh.Where(x => x.monhoc == monhoc).ToList();


            return PartialView("_ChitietMon", a);
        }

        public ActionResult DangKiHPSV2(string trangthaidk, int namhoc = 0, int hocki = 0)
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
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
                             join tk in db.TaiKhoans
                             on sv.tenDangNhap equals tk.tenDangNhap
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
                                 hocki = (Int32)lmh.hocKy,
                                 tendn = tk.tenDangNhap,
                                 masv = sv.maSinhVien,
                                 mamonhoc = mh.maMonHoc

                             };


                var listlmh = from lmh in db.LopMonHocs
                              join mh in db.MonHocs
                              on lmh.maMonHoc equals mh.maMonHoc
                              join gv in db.GiangViens
                              on lmh.maGiangVien equals gv.maGiangVien



                              select new ModelViewDSMHDK()
                              {
                                  malophocphan = lmh.maLopMonHoc,
                                  monhoc = mh.tenMonHoc,
                                  tinchi = (Int32)mh.soTinChi,
                                  soluong = (Int32)lmh.soLuongToiDa,
                                  soluongdangki = (Int32)lmh.soLuongDangKy,
                                  tinhtrang = lmh.trangThai,
                                  tengiangvien = gv.tenGiangVien,
                                  mamonhoc = mh.maMonHoc,
                                  phonghoc = lmh.phongHoc,
                                  tiethoc = lmh.tietHoc,
                                  ngayhoc = (Int32)lmh.ngayHoc,
                                  namhoc = (Int32)lmh.namHoc,
                                  hocki = (Int32)lmh.hocKy
                              };

                if (Session["UserName"] != null)
                {
                    var lstlmh = listlmh.ToList();




                    string ten = Session["UserName"].ToString();
                    var get2 = listmh.Where(x => x.tendn == ten).FirstOrDefault();
                    var masv1 = get2.masv;
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);


                    var b = from mm in lstlmh select mm.mamonhoc;



                    //var mamonhoctheosv = get2.mamonhoc.ToArray();

                    var gett = listmh.ToList();

                    var get = listmh.ToList().Where(x => x.tendn == ten).ToList();


                    //                    select* from MonHoc t
                    //where t.maMonHoc not in 
                    //(
                    //select distinct mh.maMonHoc from SinhVien sv
                    // join BangDiem bd on sv.maSinhVien = bd.maSinhVien
                    // join LopMonHoc lmh on bd.maLopMonHoc = lmh.maLopMonHoc
                    // join MonHoc mh on lmh.maMonHoc = mh.maMonHoc
                    //)

                    var a = db.st_MHChuaDK1(masv1, namhoc, hocki);
                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    if (trangthaidk == "dkmoi")
                    {
                        return View(a);
                    }
                    else if (trangthaidk == "hoclai")
                    {
                        return RedirectToAction("DangKiHPSV3");
                    }




                    return View(a);


                }




                else
                {
                    var lstlmh = listlmh.ToList();




                    string ten = Session["MaSV"].ToString();
                    var get2 = listmh.Where(x => x.masv == ten).FirstOrDefault();
                    var masv1 = get2.masv;
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);






                    //var mamonhoctheosv = get2.mamonhoc.ToArray();

                    var gett = listmh.ToList();

                    var get = listmh.ToList().Where(x => x.masv == ten).ToList();



                    var a = db.st_MHChuaDK1(masv1, namhoc, hocki);
                    var b = db.st_hoclai(masv1, namhoc, hocki);
                    TempData["b"] = b;
                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");




                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    if (trangthaidk == "dkmoi")
                    {
                        return View(a);
                    }
                    else if (trangthaidk == "hoclai")
                    {
                        return RedirectToAction("DangKiHPSV3");
                    }






                    return View(a);

                }


            }




        }
        public ActionResult DangKiHPSV3(string trangthaidk, int namhoc = 0, int hocki = 0)
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
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
                             join tk in db.TaiKhoans
                             on sv.tenDangNhap equals tk.tenDangNhap
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
                                 hocki = (Int32)lmh.hocKy,
                                 tendn = tk.tenDangNhap,
                                 masv = sv.maSinhVien,
                                 mamonhoc = mh.maMonHoc

                             };


                var listlmh = from lmh in db.LopMonHocs
                              join mh in db.MonHocs
                              on lmh.maMonHoc equals mh.maMonHoc
                              join gv in db.GiangViens
                              on lmh.maGiangVien equals gv.maGiangVien



                              select new ModelViewDSMHDK()
                              {
                                  malophocphan = lmh.maLopMonHoc,
                                  monhoc = mh.tenMonHoc,
                                  tinchi = (Int32)mh.soTinChi,
                                  soluong = (Int32)lmh.soLuongToiDa,
                                  soluongdangki = (Int32)lmh.soLuongDangKy,
                                  tinhtrang = lmh.trangThai,
                                  tengiangvien = gv.tenGiangVien,
                                  mamonhoc = mh.maMonHoc,
                                  phonghoc = lmh.phongHoc,
                                  tiethoc = lmh.tietHoc,
                                  ngayhoc = (Int32)lmh.ngayHoc,
                                  namhoc = (Int32)lmh.namHoc,
                                  hocki = (Int32)lmh.hocKy
                              };

                if (Session["UserName"] != null)
                {
                    var lstlmh = listlmh.ToList();




                    string ten = Session["UserName"].ToString();
                    var get2 = listmh.Where(x => x.tendn == ten).FirstOrDefault();
                    var masv1 = get2.masv;
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);






                    //var mamonhoctheosv = get2.mamonhoc.ToArray();

                    var gett = listmh.ToList();

                    var get = listmh.ToList().Where(x => x.tendn == ten).ToList();


                    //                    select* from MonHoc t
                    //where t.maMonHoc not in 
                    //(
                    //select distinct mh.maMonHoc from SinhVien sv
                    // join BangDiem bd on sv.maSinhVien = bd.maSinhVien
                    // join LopMonHoc lmh on bd.maLopMonHoc = lmh.maLopMonHoc
                    // join MonHoc mh on lmh.maMonHoc = mh.maMonHoc
                    //)

                    var a = db.st_MHChuaDK1(masv1, namhoc, hocki);
                    var b = db.st_hoclai(masv1, namhoc, hocki);
                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    if (trangthaidk == "hoclai")
                    {
                        return View(b);
                    }
                    else if (trangthaidk == "dkmoi")
                    {
                        return RedirectToAction("DangKiHPSV2");
                    }




                    return View(b);


                }




                else
                {
                    var lstlmh = listlmh.ToList();




                    string ten = Session["MaSV"].ToString();
                    var get2 = listmh.Where(x => x.masv == ten).FirstOrDefault();
                    var masv1 = get2.masv;
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);






                    //var mamonhoctheosv = get2.mamonhoc.ToArray();

                    var gett = listmh.ToList();

                    var get = listmh.ToList().Where(x => x.masv == ten).ToList();



                    var a = db.st_MHChuaDK1(masv1, namhoc, hocki);
                    var b = db.st_hoclai(masv1, namhoc, hocki);

                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");




                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    if (trangthaidk == "hoclai")
                    {
                        return View(b);
                    }
                    else if (trangthaidk == "dkmoi")
                    {
                        return RedirectToAction("DangKiHPSV2");
                    }






                    return View(b);

                }


            }
        }
        //public ActionResult DangKiHPSV3(string trangthaidk, int namhoc = 0, int hocki = 0)
        //{
        //    if (Session["UserName"] == null && Session["MaGV"] == null)
        //    {
        //        return RedirectToAction("DangNhap", "TaiKhoans");
        //    }
        //    else
        //    {


        //        var listmh = from lmh in db.LopMonHocs
        //                     join mh in db.MonHocs
        //                     on lmh.maMonHoc equals mh.maMonHoc
        //                     join gv in db.GiangViens
        //                     on lmh.maGiangVien equals gv.maGiangVien
        //                     join bd in db.BangDiems
        //                     on lmh.maLopMonHoc equals bd.maLopMonHoc
        //                     join sv in db.SinhViens
        //                     on bd.maSinhVien equals sv.maSinhVien
        //                     join tk in db.TaiKhoans
        //                     on sv.tenDangNhap equals tk.tenDangNhap
        //                     join l in db.Lops
        //                     on sv.maLop equals l.maLop
        //                     select new ModelViewDKMon()
        //                     {
        //                         malophocphan = lmh.maLopMonHoc,
        //                         monhoc = mh.tenMonHoc,
        //                         tinchi = (Int32)mh.soTinChi,
        //                         soluong = (Int32)lmh.soLuongToiDa,
        //                         soluongdangki = (Int32)lmh.soLuongDangKy,
        //                         tinhtrang = lmh.trangThai,
        //                         tengiangvien = gv.tenGiangVien,
        //                         lopdangki = l.tenLop,
        //                         phonghoc = lmh.phongHoc,
        //                         tiethoc = lmh.tietHoc,
        //                         ngayhoc = (Int32)lmh.ngayHoc,
        //                         namhoc = (Int32)lmh.namHoc,
        //                         hocki = (Int32)lmh.hocKy,
        //                         tendn = tk.tenDangNhap,
        //                         masv = sv.maSinhVien,
        //                         mamonhoc = mh.maMonHoc,
        //                         magv = gv.maGiangVien


        //                     };


        //        var listlmh = from lmh in db.LopMonHocs
        //                      join mh in db.MonHocs
        //                      on lmh.maMonHoc equals mh.maMonHoc
        //                      join gv in db.GiangViens
        //                      on lmh.maGiangVien equals gv.maGiangVien



        //                      select new ModelViewDSMHDK()
        //                      {
        //                          malophocphan = lmh.maLopMonHoc,
        //                          monhoc = mh.tenMonHoc,
        //                          tinchi = (Int32)mh.soTinChi,
        //                          soluong = (Int32)lmh.soLuongToiDa,
        //                          soluongdangki = (Int32)lmh.soLuongDangKy,
        //                          tinhtrang = lmh.trangThai,
        //                          tengiangvien = gv.tenGiangVien,
        //                          mamonhoc = mh.maMonHoc,
        //                          phonghoc = lmh.phongHoc,
        //                          magv = gv.maGiangVien,
        //                          tiethoc = lmh.tietHoc,
        //                          ngayhoc = (Int32)lmh.ngayHoc,
        //                          namhoc = (Int32)lmh.namHoc,
        //                          hocki = (Int32)lmh.hocKy

        //                      };

        //        if (Session["MaGV"] != null)
        //        {

        //        }
        //        else
        //        {
        //            return View();
        //        }

        //    }
        //}
        [HttpGet]
        public PartialViewResult DSMonDKPartial(string id)
        {

            System.Threading.Thread.Sleep(3000);
            var lopMonHoc = db.LopMonHocs.Find(id);
            string mamonh = lopMonHoc.maLopMonHoc;
            var lstmh = from lmh in db.LopMonHocs

                        join gv in db.GiangViens
                        on lmh.maGiangVien equals gv.maGiangVien

                        select new ModelViewDKMon()
                        {
                            malophocphan = lmh.maLopMonHoc,

                            mamonhoc = lmh.maMonHoc,
                            soluong = (Int32)lmh.soLuongToiDa,
                            soluongdangki = (Int32)lmh.soLuongDangKy,
                            tinhtrang = lmh.trangThai,
                            tengiangvien = gv.tenGiangVien,
                            tenlophp = lmh.tenLopMonHoc,
                            phonghoc = lmh.phongHoc,
                            tiethoc = lmh.tietHoc,

                            ngayhoc = (Int32)lmh.ngayHoc,
                            namhoc = (Int32)lmh.namHoc,
                            hocki = (Int32)lmh.hocKy



                        };

            var lst1 = lstmh.Where(x => x.malophocphan == mamonh).FirstOrDefault();
            var lst = db.LopMonHocs.Where(x => x.maLopMonHoc == mamonh).ToList();
            ViewBag.tengv = lst1.tengiangvien;

            return PartialView("DSMonDKPartial", lst);
        }
        public PartialViewResult DSMonDK2()
        {



            return PartialView("DSMonDK2", null);
        }
        public PartialViewResult DSMonDK(string id)
        {
            System.Threading.Thread.Sleep(3000);
            MonHoc monHoc = db.MonHocs.Find(id);
            string mamonh = monHoc.maMonHoc;

            var lst = db.LopMonHocs.Where(x => x.maMonHoc == mamonh).ToList();
            if (lst.Count == 0)
            {
                return PartialView("DSMonDK2");
            }
            return PartialView("DSMonDK", lst);
        }
        public ActionResult DangKyMon(string id)
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
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
                             join tk in db.TaiKhoans
                             on sv.tenDangNhap equals tk.tenDangNhap
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
                                 hocki = (Int32)lmh.hocKy,
                                 tendn = tk.tenDangNhap,
                                 masv = sv.maSinhVien

                             };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get2 = listmh.Where(x => x.tendn == ten).FirstOrDefault();
                    string masv = get2.masv;
                    LopMonHoc lmonhoc = db.LopMonHocs.Find(id);
                    BangDiem bangDiem = new BangDiem();
                    bangDiem.maLopMonHoc = lmonhoc.maLopMonHoc;
                    bangDiem.maSinhVien = masv;
                    bangDiem.thucHanh = null;
                    bangDiem.thuongKy = null;
                    bangDiem.giuaKy = null;
                    bangDiem.cuoiKy = null;
                    bangDiem.trangThai = "";
                    lmonhoc.soLuongDangKy += 1;
                    if (DateTime.Now < lmonhoc.hanDangKy)
                    {
                        if (lmonhoc.soLuongDangKy >= 20 && lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                        {
                            lmonhoc.trangThai = "Chấp nhận mở lớp";
                            if (ModelState.IsValid)
                            {

                                db.BangDiems.Add(bangDiem);
                                db.SaveChanges();
                                TempData["dangkitc"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }
                        else if (lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                        {
                            lmonhoc.trangThai = "Chờ Sinh Viên Đăng ký";
                            if (ModelState.IsValid)
                            {

                                db.BangDiems.Add(bangDiem);
                                db.SaveChanges();
                                TempData["dangkitc"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }


                    }
                    else if (DateTime.Now > lmonhoc.hanDangKy)
                    {
                        if (lmonhoc.soLuongDangKy < 20)
                        {
                            lmonhoc.trangThai = "Hủy Lớp";
                            if (ModelState.IsValid)
                            {


                                db.SaveChanges();
                                TempData["dangkitb"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }
                        else if (lmonhoc.soLuongDangKy > 20)
                        {
                            lmonhoc.trangThai = "Khóa Lớp";
                            if (ModelState.IsValid)
                            {


                                db.SaveChanges();
                                TempData["dangkitb"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }


                    }



                    //SinhVien sv = new SinhVien();

                    //sv.BangDiems = new List<BangDiem>
                    //{
                    //    new BangDiem{ cuoiKy=9},
                    //    new BangDiem{ cuoiKy=9}
                    //};
                    //db.SinhViens.Add(sv);
                    //db.SaveChanges();

                }
                else
                {
                    string ten = Session["MaSV"].ToString();
                    var get2 = listmh.Where(x => x.masv == ten).FirstOrDefault();
                    string masv = get2.masv;
                    LopMonHoc lmonhoc = db.LopMonHocs.Find(id);
                    BangDiem bangDiem = new BangDiem();
                    bangDiem.maLopMonHoc = lmonhoc.maLopMonHoc;
                    bangDiem.maSinhVien = masv;
                    bangDiem.thucHanh = null;
                    bangDiem.thuongKy = null;
                    bangDiem.giuaKy = null;
                    bangDiem.cuoiKy = null;
                    bangDiem.trangThai = "";
                    lmonhoc.soLuongDangKy += 1;
                    if (DateTime.Now < lmonhoc.hanDangKy)
                    {
                        if (lmonhoc.soLuongDangKy >= 20 && lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                        {
                            lmonhoc.trangThai = "Chấp nhận mở lớp";
                            if (ModelState.IsValid)
                            {

                                db.BangDiems.Add(bangDiem);
                                db.SaveChanges();
                                TempData["dangkitc"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }
                        else if (lmonhoc.soLuongDangKy < lmonhoc.soLuongToiDa)
                        {
                            lmonhoc.trangThai = "Chờ Sinh Viên Đăng ký";
                            if (ModelState.IsValid)
                            {

                                db.BangDiems.Add(bangDiem);
                                db.SaveChanges();
                                TempData["dangkitc"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }


                    }
                    else if (DateTime.Now > lmonhoc.hanDangKy)
                    {
                        if (lmonhoc.soLuongDangKy < 20)
                        {
                            lmonhoc.trangThai = "Hủy Lớp";
                            if (ModelState.IsValid)
                            {


                                db.SaveChanges();
                                TempData["dangkitb"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }
                        else if (lmonhoc.soLuongDangKy > 20)
                        {
                            lmonhoc.trangThai = "Khóa Lớp";
                            if (ModelState.IsValid)
                            {


                                db.SaveChanges();
                                TempData["dangkitb"] = "sơn";
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }


                    }
                }
                return RedirectToAction("DangKiHPSV2");



            }

        }
        public JsonResult Get(int nam, int hocki)
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
                         join tk in db.TaiKhoans
                         on sv.tenDangNhap equals tk.tenDangNhap
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
                             hocki = (Int32)lmh.hocKy,
                             tendn = tk.tenDangNhap,
                             masv = sv.maSinhVien

                         };
            if (Session["UserName"] != null)
            {
                string ten = Session["UserName"].ToString();
                var lst = listmh.ToList();

                return Json(lst, JsonRequestBehavior.AllowGet);


            }
            string id = Session["MaSV"].ToString();
            var lst2 = listmh.Where(x => x.hocki == hocki && x.namhoc == nam && x.masv == id).ToList();




            return Json(lst2, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get2(int id)
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
                         join tk in db.TaiKhoans
                         on sv.tenDangNhap equals tk.tenDangNhap
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
                             hocki = (Int32)lmh.hocKy,
                             tendn = tk.tenDangNhap,
                             masv = sv.maSinhVien

                         };
            var Student = listmh.ToList().Find(x => x.mamonhoc.Equals(id));
            return Json(Student, JsonRequestBehavior.AllowGet);
        }

        public ActionResult XemDSLop()
        {

            return View(db.Lops.ToList());

        }
        public ActionResult XemSVtheoLop(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listdssv = from sv in db.SinhViens
                           join l in db.Lops
                           on sv.maLop equals l.maLop
                           join cn in db.ChuyenNganhs
                           on sv.maChuyenNganh equals cn.maChuyenNganh
                           select new ModelViewDSSV()
                           {
                               malop = l.maLop,
                               tenlop = l.tenLop,
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               chuyennganh = cn.tenChuyenNganh
                           };
            var slst = listdssv.Where(x => x.malop == id).ToList();



            if (slst.Count == 0)
            {
                ViewBag.Thongbao = "Không có sinh viên ở lớp này";
                return View(slst);
            }
            return View(slst);
        }
        public ActionResult XemLichThi(int nam = 0, int hocki = 0)
        {

            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {


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
                                  hinhanh = sv.hinhAnh,
                                  ngaythi = (DateTime?)lmh.ngayThi




                              };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get2 = listtkb.Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    var masv1 = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.tendangnhap == ten).ToList();
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);



                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    ViewBag.thu2 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 2).ToList();
                    ViewBag.thu3 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 3).ToList();
                    ViewBag.thu4 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 4).ToList();
                    ViewBag.thu5 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 5).ToList();
                    ViewBag.thu6 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 6).ToList();
                    ViewBag.thu7 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 7).ToList();
                    ViewBag.thu8 = listtkb.Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 8).ToList();
                    ViewBag.thu2count = ViewBag.thu2.Count + 1;
                    ViewBag.thu3count = ViewBag.thu3.Count + 1;
                    ViewBag.thu4count = ViewBag.thu4.Count + 1;
                    ViewBag.thu5count = ViewBag.thu5.Count + 1;
                    ViewBag.thu6count = ViewBag.thu6.Count + 1;
                    ViewBag.thu7count = ViewBag.thu7.Count + 1;
                    ViewBag.thu8count = ViewBag.thu8.Count + 1;
                    return View(listtkb.ToList().Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki));
                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get2 = listtkb.ToList().Where(x => x.masv == id).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    var masv1 = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.masv == id).ToList();
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);



                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;

                    ViewBag.thu2 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 2).ToList();
                    ViewBag.thu3 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 3).ToList();
                    ViewBag.thu4 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 4).ToList();
                    ViewBag.thu5 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 5).ToList();
                    ViewBag.thu6 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 6).ToList();
                    ViewBag.thu7 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 7).ToList();
                    ViewBag.thu8 = listtkb.Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki && x.ngayhoc == 8).ToList();
                    ViewBag.thu2count = ViewBag.thu2.Count + 1;
                    ViewBag.thu3count = ViewBag.thu3.Count + 1;
                    ViewBag.thu4count = ViewBag.thu4.Count + 1;
                    ViewBag.thu5count = ViewBag.thu5.Count + 1;
                    ViewBag.thu6count = ViewBag.thu6.Count + 1;
                    ViewBag.thu7count = ViewBag.thu7.Count + 1;
                    ViewBag.thu8count = ViewBag.thu8.Count + 1;

                    return View(listtkb.ToList().Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki));

                }


            }



        }
        public ActionResult DSLopHP()
        {
            if (Session["UserName"] == null && Session["MaGV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {
                var listgiangday = from gv in db.GiangViens
                                   join lmh in db.LopMonHocs
                                   on gv.maGiangVien equals lmh.maGiangVien
                                   join tk in db.TaiKhoans
                                   on gv.tenDangNhap equals tk.tenDangNhap


                                   select new ModelViewXemMonHoc()
                                   {
                                       tengiangvien = gv.tenGiangVien,
                                       tendangnhap = tk.tenDangNhap,
                                       tenlophp = lmh.tenLopMonHoc,
                                       malopmonhoc = lmh.maLopMonHoc,


                                       phonghoc = lmh.phongHoc,
                                       tiethoc = lmh.tietHoc,
                                       magv = gv.maGiangVien
                                   };
                if (Session["UserName"] != null)
                {
                    string tendn = Session["UserName"].ToString();
                    return View(listgiangday.Where(x => x.tendangnhap == tendn).ToList());
                }
                else
                {
                    string id = Session["MaGV"].ToString();
                    return View(listgiangday.Where(x => x.magv == id).ToList());
                }
            }

        }
        public ActionResult DSLopHP2()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {
                var listgiangday = from gv in db.GiangViens
                                   join lmh in db.LopMonHocs
                                   on gv.maGiangVien equals lmh.maGiangVien
                                   join tk in db.TaiKhoans
                                   on gv.tenDangNhap equals tk.tenDangNhap


                                   select new ModelViewXemMonHoc()
                                   {
                                       tengiangvien = gv.tenGiangVien,
                                       tendangnhap = tk.tenDangNhap,
                                       tenlophp = lmh.tenLopMonHoc,
                                       malopmonhoc = lmh.maLopMonHoc,


                                       phonghoc = lmh.phongHoc,
                                       tiethoc = lmh.tietHoc,
                                       magv = gv.maGiangVien
                                   };


                string tendn = Session["UserName"].ToString();
                return View(listgiangday.ToList());



            }

        }
        public ActionResult XemSVtheoLopHP(string id, List<BangDiem> lst)
        {
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            TempData["malmhoc"] = lopMonHoc.maLopMonHoc;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listdssv = from sv in db.SinhViens
                           join bd in db.BangDiems
                           on sv.maSinhVien equals bd.maSinhVien
                           join lmh in db.LopMonHocs
                           on bd.maLopMonHoc equals lmh.maLopMonHoc
                           select new ModelViewDSSV()
                           {
                               malmh = lmh.maLopMonHoc,
                               tenlmh = lmh.tenLopMonHoc,
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               thuongky = (Decimal?)bd.thuongKy,
                               giuaky = (Decimal?)bd.giuaKy,
                               cuoiky = (Decimal?)bd.cuoiKy,
                               thuchanh = (Decimal?)bd.thucHanh


                           };

            var s = listdssv.Where(x => x.malmh == id && x.thuongky == null && x.giuaky == null && x.cuoiky == null && x.thuchanh == null).ToList();



            if (s == null)
            {
                return HttpNotFound();
            }
            return View(s);
        }
        public ActionResult XemSVtheoLopHP2(string id, List<BangDiem> lst)
        {
            LopMonHoc lopMonHoc = db.LopMonHocs.Find(id);
            TempData["malmhoc"] = lopMonHoc.maLopMonHoc;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listdssv = from sv in db.SinhViens
                           join bd in db.BangDiems
                           on sv.maSinhVien equals bd.maSinhVien
                           join lmh in db.LopMonHocs
                           on bd.maLopMonHoc equals lmh.maLopMonHoc
                           select new ModelViewDSSV()
                           {
                               malmh = lmh.maLopMonHoc,
                               tenlmh = lmh.tenLopMonHoc,
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               thuongky = (Decimal)bd.thuongKy,
                               giuaky = (Decimal)bd.giuaKy,
                               cuoiky = (Decimal)bd.cuoiKy,
                               thuchanh = (Decimal)bd.thucHanh


                           };

            var s = listdssv.Where(x => x.malmh == id).ToList();



            if (s == null)
            {
                return HttpNotFound();
            }
            return View(s);
        }

        public ActionResult NhapDiem(List<ModelViewDSSV> model)
        {
            var listdssv = from sv in db.SinhViens
                           join bd in db.BangDiems
                           on sv.maSinhVien equals bd.maSinhVien
                           join lmh in db.LopMonHocs
                           on bd.maLopMonHoc equals lmh.maLopMonHoc
                           select new ModelViewDSSV()
                           {
                               malmh = lmh.maLopMonHoc,
                               tenlmh = lmh.tenLopMonHoc,
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               thuongky = (Decimal)bd.thuongKy,
                               giuaky = (Decimal)bd.giuaKy,
                               cuoiky = (Decimal)bd.cuoiKy,
                               thuchanh = (Decimal)bd.thucHanh


                           };
            var a = TempData["malmhoc"];
            var s = listdssv.Where(x => x.malmh == a.ToString()).ToList();



            foreach (var i in model)
            {



                var b = TempData["malmhoc"];
                var c = db.BangDiems.Where(x => x.maSinhVien == i.masv && x.maLopMonHoc == b.ToString()).FirstOrDefault();


                c.thuongKy = i.thuongky;

                c.giuaKy = i.giuaky;
                c.cuoiKy = i.cuoiky;
                c.thucHanh = i.thuchanh;
                db.SaveChanges();

            }





            return RedirectToAction("DSLopHP");
        }
        public ActionResult NhapDiem2(List<ModelViewDSSV> model)
        {
            var listdssv = from sv in db.SinhViens
                           join bd in db.BangDiems
                           on sv.maSinhVien equals bd.maSinhVien
                           join lmh in db.LopMonHocs
                           on bd.maLopMonHoc equals lmh.maLopMonHoc
                           select new ModelViewDSSV()
                           {
                               malmh = lmh.maLopMonHoc,
                               tenlmh = lmh.tenLopMonHoc,
                               masv = sv.maSinhVien,
                               tensv = sv.tenSinhVien,
                               thuongky = (Decimal)bd.thuongKy,
                               giuaky = (Decimal)bd.giuaKy,
                               cuoiky = (Decimal)bd.cuoiKy,
                               thuchanh = (Decimal)bd.thucHanh


                           };
            var a = TempData["malmhoc"];
            var s = listdssv.Where(x => x.malmh == a.ToString()).ToList();



            foreach (var i in model)
            {



                var b = TempData["malmhoc"];
                var c = db.BangDiems.Where(x => x.maSinhVien == i.masv && x.maLopMonHoc == b.ToString()).FirstOrDefault();


                c.thuongKy = i.thuongky;

                c.giuaKy = i.giuaky;
                c.cuoiKy = i.cuoiky;
                c.thucHanh = i.thuchanh;
                db.SaveChanges();

            }





            return RedirectToAction("DSLopHP2");
        }
        public ActionResult HuyHP(int namhoc = 0, int hocki = 0)
        {
            if (Session["UserName"] == null && Session["MaSV"] == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoans");
            }
            else
            {


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
                                  hinhanh = sv.hinhAnh,
                                  ngaythi = (DateTime?)lmh.ngayThi




                              };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get2 = listtkb.Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    var masv1 = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.tendangnhap == ten).ToList();
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);



                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;
                    var xx = db.st_temp1(masv1, namhoc, hocki);



                    return View(xx);
                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get2 = listtkb.ToList().Where(x => x.masv == id).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    var masv1 = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.masv == id).ToList();
                    var a2 = db.st_hocky(masv1);
                    var b2 = db.st_namhoc(masv1);



                    SelectList cateList = new SelectList(b2, "namHoc", "namHoc");
                    SelectList cateList2 = new SelectList(a2, "hocKy", "hocKy");


                    var xx = db.st_temp1(masv1, namhoc, hocki);
                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    return View(xx);
                }

            }
        }
        public ActionResult HuyHP2(string id)
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
        public ActionResult HuyHP2(int id)
        {
            BangDiem bangDiem = db.BangDiems.Find(id);
            db.BangDiems.Remove(bangDiem);
            db.SaveChanges();
            return RedirectToAction("HuyHP");
           
        }
    }
}


        
