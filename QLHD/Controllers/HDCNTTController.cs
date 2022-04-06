using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using System.Data;
using System.Web.Services;
using System.Collections;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity.Infrastructure;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.IO;
using QLHD.ViewModel;

namespace QLHD.Controllers
{
    public class HDCNTTController : Controller
    {
        // GET: /HDCNTT/
        QLHD2Entities db = new QLHD2Entities();
        public ActionResult Index(int? page)
        {
            var hdCNTT = db.HOPDONG_DOANHTHU.Where(n => !db.THANHLY_HDDOANHTHU.Where(m => m.HOPDONG_DOANHTHU_ID == n.HOPDONG_DOANHTHU_ID).Any())
                .ToList().OrderByDescending(n => n.HOPDONG_DOANHTHU_ID);
            int pageNumber = (page ?? 1);
            int pageSize = 10;

            return View(hdCNTT.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            String loaiTK = f["txtloaiTimKiem"].ToString();
            String tukhoa = f["txtTimKiem"].ToString();
            //String date = f["txtngayHD"].ToString();
            //DateTime ngayHD = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture);


            List<HOPDONG_DOANHTHU> listKQTK = db.HOPDONG_DOANHTHU.ToList();
            ViewBag.tukhoa = tukhoa;
            if (loaiTK == "1")
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.SO_HD.Contains(tukhoa)).ToList();
            }
            if (loaiTK == "2")
            {
                int namHD = Int32.Parse(f["NAM_HD_ID"].ToString());
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiTK == "3")
            {
                int loaiHD = Int32.Parse(f["ID_LOAIHD_SUB"].ToString());
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy hợp đồng nào!!";
                return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + listKQTK.Count + " kết quả!";
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Tạo mới HĐ CNTT
        /// </summary>
        
        [HttpGet]
        public ActionResult Create()
        {

            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.BENTHUE_TAM_ID), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HOPDONG_DOANHTHU hopdong, HttpPostedFileBase upload)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.BENTHUE_TAM_ID), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/HD_DOANHTHU"),
                                                   Path.GetFileName(upload.FileName));
                        upload.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                        hopdong.FILE = upload.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
                hopdong.USER = user.TENTRUYCAP;
                db.HOPDONG_DOANHTHU.Add(hopdong);
                db.SaveChanges();
                return RedirectToAction("Index", "HDDoanhThu");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }

        /// <summary>
        /// Chỉnh sửa HĐ CNTT
        /// </summary>
        /// 
        [HttpGet]
        public ActionResult EditHDCNTT(int? HDDOANHTHU_ID)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD_SUB = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");

            HOPDONG_DOANHTHU HDdoanhthu = db.HOPDONG_DOANHTHU.Find(HDDOANHTHU_ID);
            if (HDdoanhthu == null)
            {
                return HttpNotFound();
            }
            return View(HDdoanhthu);
        }

        [HttpPost]//, ActionName("EditHDDoanhThu")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditHDCNTTPost(int? HDDOANHTHU_ID, HttpPostedFileBase FILE)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD_SUB = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            //if (HDDOANHTHU_ID == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var HDdoanhthu = db.HOPDONG_DOANHTHU.Find(HDDOANHTHU_ID);

            if (FILE != null && FILE.ContentLength > 0)
                try
                {
                    var fileName = Path.GetFileName(FILE.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HD_DOANHTHU"), fileName);
                    FILE.SaveAs(path);
                    HDdoanhthu.FILE = FILE.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            int id_taikhoan = user.ID_THANHVIEN;
            if (TryUpdateModel(HDdoanhthu, "",
                new string[] {"CHUKY_ID", "HTTT_ID", "SO_HD", "NGAY_HD", "TEN_BT", "DIACHI_BT","LOAI_BENTHUE",
                            "CMND_BT", "NGAYCAP_BT", "NOICAP_BT", "NGAYSINH_BT", "DIENTHOAI_BT",
                             "DAIDIEN_BT", "CHUCVU_BT", "TAIKHOAN_BT", "NGANHANG_BT", "MSTHUE_BT",
                            "TEN_CT", "DIACHI_CT", "DIENTHOAI_CT", "FAX_CT", "TAIKHOAN_CT", "NGANHANG_CT",
                            "MSTHUE_CT", "DAIDIEN_CT", "CMND_CT", "NGAYCAP_CMND_CT", "NOICAP_CT",
                            "CHUCVU_CT", "HOPDONG_DT", "SOLUONG_DT", "DIENTICH_CT", "CONGSUAT_DT",
                            "CHUNGLOAI_DT", "CONGVIEC_DT", "TRAM_DT","NOIDUNGKHAC_DT", "THANG_GT", "TONG_GT", "NGAY_BD",
                            "NGAY_KT", "GHICHU", "DONVI_ID", "THANG", "VAT", "SO_CHUKY"}))
            {
                try
                {
                    db.SaveChanges();
                    luulichsuchinhsua(2, HDDOANHTHU_ID, id_taikhoan);
                    return RedirectToAction("Index", "HDDoanhThu");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(HDdoanhthu);
        }

        /// <summary>
        /// Xoá HĐ CNTT
        /// </summary>
        
        [HttpGet]
        public ActionResult DeleteHDCNTT(int HDDOANHTHU_ID)
        {
            HOPDONG_DOANHTHU HDDT = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == HDDOANHTHU_ID);
            if (HDDT == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(HDDT);
        }

        [HttpPost, ActionName("DeleteHDCNTT")]
        public ActionResult XacNhanXoaHDCNTT(int HDDOANHTHU_ID)
        {
            HOPDONG_DOANHTHU HDDT = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == HDDOANHTHU_ID);
            if (HDDT == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.HOPDONG_DOANHTHU.Remove(HDDT);
            db.SaveChanges();
            return RedirectToAction("Index", "HDCNTT");
        }

        /// <summary>
        /// Show HĐ CNTT
        /// </summary>

        public ActionResult ShowHDCNTT(int maHD)
        {
            HOPDONG_DOANHTHU hd = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == maHD);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }

            return View(hd);
        }

        

        [HttpGet]
        public ActionResult ThanhLyHDCNTT(int HDid)
        {
            var hd = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == HDid);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            ViewBag.id = HDid;
            ViewBag.loaihd = hd.LOAI_HD_SUB.TEN_HD_SUB;

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThanhLyHDCNTT(THANHLY_HDDOANHTHU thanhly)
        {

            //ViewBag.hdid = HDid;
            if (ModelState.IsValid)
            {
                var hd = db.HOPDONG_DOANHTHU.Find(thanhly.HOPDONG_DOANHTHU_ID);
                if (hd == null)
                {
                    return RedirectToAction("baoloi", "Home");
                }
                if (TryUpdateModel(hd))
                {
                    hd.TRANGTHAI = 2;
                    db.SaveChanges();
                }

                db.THANHLY_HDDOANHTHU.Add(thanhly);
                db.SaveChanges();
                return RedirectToAction("Index", "HDChiPhi");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return RedirectToAction("Index", "HDNhanCong");
        }

        public JsonResult getChuKyTT(int chuky)
        {
            CHUKY_TT chuky_tt = db.CHUKY_TT.FirstOrDefault(n => n.CHUKY_ID == chuky);

            var ten = chuky_tt.CHUKY;
            var thang = chuky_tt.THANG;

            return Json(new { ten, thang }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getHD_HetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd1 = db.HOPDONG_DOANHTHU.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            var hd2 = db.HOPDONG_DOANHTHU.Where(n => n.NGAY_KT < date).ToList();
            int hd = hd1.Count();
            int hd_hethan = hd2.Count();
            return Json(new { hd, hd_hethan }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportData(int HDid)
        {
            HOPDONG_DOANHTHU hd = db.HOPDONG_DOANHTHU.Find(HDid);
            String filename = hd.FILE;
            if (filename == null)
            {
                ViewBag.baoloi = "Hợp đồng này chưa lưu file!!";
                return RedirectToAction("Index");
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Content/HD_DOANHTHU/" + filename;


            byte[] filedata = System.IO.File.ReadAllBytes(filepath);

            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
            //return RedirectToAction("Index");
        }

        public void luulichsuchinhsua(int? loaihd, int? idhd, int? taikhoan)
        {
            HISTORY_CHANGE ls = new HISTORY_CHANGE();
            ls.LOAI_HD = loaihd;
            ls.ID_HD = idhd;
            ls.ID_TAIKHOAN = taikhoan;
            ls.NGAY_CHINHSUA = System.DateTime.Now.Date;
            //var n = "Lưu thành công";
            db.HISTORY_CHANGE.Add(ls);
            db.SaveChanges();
            //return n;
        }

        public PartialViewResult HDHetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd1 = db.HOPDONG_DOANHTHU.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            return PartialView(hd1);
        }

        public PartialViewResult HDHetHan2()
        {
            DateTime date = System.DateTime.Now;
            var hd2 = db.HOPDONG_DOANHTHU.Where(n => n.NGAY_KT < date).ToList();
            return PartialView(hd2);
        }

        [HttpGet]
        public ActionResult ThanhToanHDCNTT(int? HDid)
        {
            if (db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == HDid) == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (db.THANHTOAN_DOANHTHU.Where(n => n.HOPDONG_DOANHTHU_ID == HDid) == null)
            {
                ViewBag.sohd = "HĐ chưa được thanh toán !!";
            }
            ViewBag.hopdong = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.HOPDONG_DOANHTHU_ID == HDid);
            ViewBag.hdid = HDid;

            return View(db.THANHTOAN_DOANHTHU.OrderByDescending(n => n.HOPDONG_DOANHTHU_ID == HDid));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThanhToanHDCNTT(THANHTOAN_DOANHTHU thanhtoan)
        {
            //ViewBag.hdid = HDid;
            if (ModelState.IsValid)
            {
                db.THANHTOAN_DOANHTHU.Add(thanhtoan);
                db.SaveChanges();
                return RedirectToAction("ThanhToanHDDT", "HDDoanhThu", new { @HDid = thanhtoan.HOPDONG_DOANHTHU_ID });
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }

        [HttpGet]
        public ActionResult QuyTrinhHDCNTT()
        {
            var dsTienTrinh = new List<TienTrinh>()
            {
                new TienTrinh("Mua thiết bị lắp đặt IOC TCU", "Nguyễn Quốc Khang", "Chủ trì", DateTime.Now, DateTime.Now, true),
                new TienTrinh("Thi công lắp đặt1", "Phan Thanh Triều", "Chủ trì", DateTime.Now, DateTime.Now, true),
                new TienTrinh("Thi công lắp đặt2", "Phan Thanh Triều", "Chủ trì", DateTime.Now, DateTime.Now, true),
                new TienTrinh("Ký hợp đồng", "Khách hàng TCU", "Chủ trì", DateTime.Now, DateTime.Now, false)
            };
            return View(dsTienTrinh);
        }
    }
}