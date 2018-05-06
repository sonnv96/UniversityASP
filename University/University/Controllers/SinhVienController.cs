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
                                   ngaysinh = (DateTime)sv.ngaySinh,
                                   gioitinh = sv.gioiTinh,
                                   giuaki = (Decimal)bd.giuaKy,
                                   cuoiki = (Decimal)bd.cuoiKy,
                                   thuhanh = (Decimal)bd.thucHanh,
                                   mamonhoc = lmh.maMonHoc,
                                   hinhanh = sv.hinhAnh,
                                   nam = (Int32)lmh.namHoc,
                                   hocki = (Int32)lmh.hocKy,
                                   tenmonhoc = mh.tenMonHoc



                               };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get = listdiem.Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get.tensv;
                    ViewBag.hinhanh = get.hinhanh;
                    ViewBag.masv = get.masv;
                    ViewBag.gioitinh = get.gioitinh;







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
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.ToList().Where(x => x.tendangnhap == ten).ToList();



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

                    var a = db.MM(masv1, namhoc, hocki);
                    SelectList cateList = new SelectList(get, "namhoc", "namhoc");
                    SelectList cateList2 = new SelectList(get, "hocki", "hocki");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;


                    if (trangthaidk == "dkmoi")
                    {
                        return View(a);
                    }
                    else if (trangthaidk == "hoclai")
                    {

                    }



                    //if (trangthaidk == "dkmoi" )
                    //{
                    //    return View(lopmh.Where(x => x.namhoc == namhoc && x.hocki == hocki && x.mamonhoc != a || x.malophocphan != b).ToList());
                    //}

                    //else if (trangthaidk == "hoclai")
                    //{
                    //    return View(listmh.ToList().Where(x => x.namhoc == namhoc && x.hocki == hocki && x.mamonhoc != mamonhoctheosv));
                    //}
                    //else if (trangthaidk == "caithien")
                    //{
                    //    return View(listmh.ToList().Where(x => x.namhoc == namhoc && x.hocki == hocki && x.mamonhoc != mamonhoctheosv));
                    //}


                    return View(a);
                }




                else
                {
                    string id = Session["MaSV"].ToString();
                    var get2 = listmh.ToList().Where(x => x.masv == id).FirstOrDefault();
                    //ViewBag.tensv = get2.tensv;
                    //ViewBag.masv = get2.masv;
                    //ViewBag.hinhanh = get2.hinhanh;
                    var get = listmh.Where(x => x.masv == id).ToList();




                    SelectList cateList = new SelectList(get, "namhoc", "namhoc");
                    SelectList cateList2 = new SelectList(get, "hocki", "hocki");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;



                    return View(listmh.ToList().Where(x => x.masv == id && x.namhoc == namhoc && x.hocki == hocki));

                }


            }




        }
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

            var lst = TempData["lstlmh"];

            return PartialView("DSMonDK2", lst);
        }
        public PartialViewResult DSMonDK(string id)
        {
            System.Threading.Thread.Sleep(3000);
            MonHoc monHoc = db.MonHocs.Find(id);
            string mamonh = monHoc.maMonHoc;

            var lst = db.LopMonHocs.Where(x => x.maMonHoc == mamonh).ToList();
            return PartialView("DSMonDK2", lst);
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
                                return RedirectToAction("DangKiHPSV2");
                            }
                        }
                        else if (lmonhoc.soLuongDangKy > 20)
                        {
                            lmonhoc.trangThai = "Khóa Lớp";
                            if (ModelState.IsValid)
                            {


                                db.SaveChanges();
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
                return View();



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

            var s = listdssv.Where(x => x.malop == id).FirstOrDefault();
            ViewBag.malop = s.malop;

            if (s == null)
            {
                return HttpNotFound();
            }
            return View(s);
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
                                  ngaythi = (DateTime)lmh.ngayThi




                              };
                if (Session["UserName"] != null)
                {
                    string ten = Session["UserName"].ToString();
                    var get2 = listtkb.Where(x => x.tendangnhap == ten).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.ToList().Where(x => x.tendangnhap == ten).ToList();




                    SelectList cateList = new SelectList(get, "namhoc", "namhoc");
                    SelectList cateList2 = new SelectList(get, "hocki", "hocki");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;



                    return View(listtkb.ToList().Where(x => x.tendangnhap == ten && x.namhoc == nam && x.hocki == hocki));
                }
                else
                {
                    string id = Session["MaSV"].ToString();
                    var get2 = listtkb.ToList().Where(x => x.masv == id).FirstOrDefault();
                    ViewBag.tensv = get2.tensv;
                    ViewBag.masv = get2.masv;
                    ViewBag.hinhanh = get2.hinhanh;
                    var get = listtkb.Where(x => x.masv == id).ToList();




                    SelectList cateList = new SelectList(get, "namhoc", "namhoc");
                    SelectList cateList2 = new SelectList(get, "hocki", "hocki");



                    ViewBag.namhoc = cateList;
                    ViewBag.hocki = cateList2;



                    return View(listtkb.ToList().Where(x => x.masv == id && x.namhoc == nam && x.hocki == hocki));

                }


            }



        }
        public ActionResult DSLopHP()
        {
            return View(db.LopMonHocs.ToList());
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

    }

}
