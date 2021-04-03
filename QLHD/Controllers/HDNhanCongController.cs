using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace QLHD.Controllers
{
    public class HDNhanCongController : Controller
    {
        //
        // GET: /HDNhanCong/
        QLHD2Entities db = new QLHD2Entities();

        public ActionResult Index(int? page)
        {
            //var hd = db.HOPDONG_NHANCONG.ToList().OrderByDescending(n=>n.HOPDONG_NHANCONG_ID);
            // hd1 -> danh sách hợp đồng chưa thanh lý
            var hd1 = db.HOPDONG_NHANCONG
                .Where(n => !db.THANHLY_HDNHANCONG.Where(m=>m.HOPDONG_NHANCONG_ID == n.HOPDONG_NHANCONG_ID).Any())
                .ToList().OrderByDescending(n => n.HOPDONG_NHANCONG_ID);
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd1.ToPagedList(pagenumber, pagesize));
        }
        public PartialViewResult DSHDThanhLy()
        {
            var hd = db.HOPDONG_NHANCONG
                .Where(n=>db.THANHLY_HDNHANCONG.Where(m=>m.HOPDONG_NHANCONG_ID == n.HOPDONG_NHANCONG_ID).Any())
                .ToList().OrderByDescending(n => n.HOPDONG_NHANCONG_ID);
           
            return PartialView(hd);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HOPDONG_NHANCONG hopdong, HttpPostedFileBase upload)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/HD_NHANCONG"),
                                                   Path.GetFileName(upload.FileName));
                        upload.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                        hopdong.FIle = upload.FileName;
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
                hopdong.ID_TRANGTHAI = 0;
                db.HOPDONG_NHANCONG.Add(hopdong);
                db.SaveChanges();
                return RedirectToAction("Index", "HDNhanCong");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }
        public PartialViewResult partialSearch()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "--Chọn thông tin cần tìm--", Value = "0", Selected = true });

            items.Add(new SelectListItem { Text = "Số HĐ", Value = "1" });

            items.Add(new SelectListItem { Text = "Ngày HĐ", Value = "2" });

            items.Add(new SelectListItem { Text = "Loại HĐ", Value = "3" });
            ViewBag.loaiTK = items;
            return PartialView();
        }
        //Hiển thị thông tin hợp đồng
        public ActionResult ShowHDNhanCong(int? HDNhanCong_ID)
        {
            HOPDONG_NHANCONG hd = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == HDNhanCong_ID);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            return View(hd);
        }
        [HttpGet]
        public ActionResult EditHDNhanCong(int? HDNhanCong_ID)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            
            HOPDONG_NHANCONG HDnhancong = db.HOPDONG_NHANCONG.Find(HDNhanCong_ID);
            if (HDnhancong == null)
            {
                return HttpNotFound();
            }
            return View(HDnhancong);
        }
        [HttpPost]//, ActionName("EditHDNhanCong")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditHDNhanCongPost(int? HDNhanCong_ID, HttpPostedFileBase FILE)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            //if (HDNhanCong_ID == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var HDnhancong = db.HOPDONG_NHANCONG.Find(HDNhanCong_ID);
            if (FILE != null && FILE.ContentLength > 0)
                try
                {
                    var fileName = Path.GetFileName(FILE.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HD_NHANCONG"), fileName);
                    FILE.SaveAs(path);
                    HDnhancong.FIle = FILE.FileName;
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
            if (TryUpdateModel(HDnhancong, "",
               new string[] { "HTTT_ID"
      ,"CHUKY_ID"
      ,"NAM_HD_ID"
      ,"ID_LOAIHD_SUB"
      ,"THOIHAN_TT_ID"
      ,"SO_HD"
      ,"NGAY_HD"
      ,"TEN_BT"
      ,"DIACHI_BT"
      ,"DIENTHOAI_BT"
      ,"FAX_BT"
      ,"TAIKHOAN_BT"
      ,"NGANHANG_BT"
      ,"MSTHUE_BT"
      ,"DAIDIEN_BT","CMND_BT","NGAYCAP_CMND_BT"
      ,"NOICAP_BT","CHUCVU_BT","TEN_CT","DIACHI_CT","CMND_CT","NGAYCAP_CT","NOICAP_CT","NGAYSINH_CT", "NOIO_CT", "NOILAMVIEC_CT"
      ,"DIENTHOAI_CT","TRINHDO_CT", "MSTHUE_CT","CONGVIEC_CT"
      ,"SLTRAM_KYTHUAT", "TENTRAM_KYTHUAT", "LUONG_KYTHUAT", "SLTRAM_TAISAN", "TENTRAM_TAISAN", "LUONG_TAISAN", "SLTRAM_MPD", "TENTRAM_MPD", "LUONG_MPD", "SLTRAM_BAOVE", "TENTRAM_BAOVE", "LUONG_BAOVE"
      ,"THANG_GT","TONG_GT","NGAY_BD","NGAY_KT","GHICHU", "DONVI_ID", "THANG", "VAT", "SO_CHUKY"
      ,"THULAO", "SOTIEN_BAOHIEM", "SO_SOBAOHIEM", "VTT_KHONGHOTRO"}))
            {
                try
                {
                    db.SaveChanges();
                    luulichsuchinhsua(3, HDNhanCong_ID, id_taikhoan);
                    return RedirectToAction("Index", "HDNhanCong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(HDnhancong);
        }
        [HttpGet]
        public ActionResult ThanhToanHDNC(int HDNhanCong_ID)
        {
            if (db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == HDNhanCong_ID) == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (db.THANHTOAN_NHANCONG.Where(n => n.HOPDONG_NHANCONG_ID == HDNhanCong_ID) == null)
            {
                ViewBag.sohd = "HĐ chưa được thanh toán !!";
            }
            ViewBag.hopdong = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == HDNhanCong_ID);
            ViewBag.hdid = HDNhanCong_ID;
            return View(db.THANHTOAN_NHANCONG.Where(n => n.HOPDONG_NHANCONG_ID == HDNhanCong_ID));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThanhToanHDNC(THANHTOAN_NHANCONG thanhtoan)
        {
            //ViewBag.hdid = HDid;
            if (ModelState.IsValid)
            {
                db.THANHTOAN_NHANCONG.Add(thanhtoan);
                db.SaveChanges();
                return RedirectToAction("ThanhToanHDNC", "HDNhanCong", new { @HDNhanCong_ID = thanhtoan.HOPDONG_NHANCONG_ID });
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            String loaiTK = f["txtloaiTimKiem"].ToString();
            String tukhoa = f["txtTimKiem"].ToString();
            //String date = f["txtngayHD"].ToString();
            //DateTime ngayHD = DateTime.ParseExact(date, "MMM dd yyyy", CultureInfo.InvariantCulture);


            List<HOPDONG_NHANCONG> listKQTK = db.HOPDONG_NHANCONG.ToList();
            ViewBag.tukhoa = tukhoa;
            if (loaiTK == "1")
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.SO_HD.Contains(tukhoa)).ToList();
            }
            if (loaiTK == "2")
            {
                int namHD = Int32.Parse(f["NAM_HD_ID"].ToString());
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiTK == "3")
            {
                int loaiHD = Int32.Parse(f["ID_LOAIHD_SUB"].ToString());
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
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
        [HttpGet]
        public PartialViewResult editTT(int IDTT)
        {
            ViewBag.edit = IDTT + "edit";
            return PartialView(db.THANHTOAN_NHANCONG.SingleOrDefault(n => n.ID_TT == IDTT));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editTTPost(int IDTT)
        {
            var thanhtoan = db.THANHTOAN_NHANCONG.Find(IDTT);
            var hopdong = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == thanhtoan.HOPDONG_NHANCONG_ID);
            if (TryUpdateModel(thanhtoan, "",
               new string[] { "TUNGAY_TT", "DENNGAY_TT", "NGAY_TT", "SOTIEN_TT", "SOUYNHIEMCHI_TT",
                               "CHUTK_TT", "SOTK_TT", "NGANHANG_TT", "GHICHU_TT"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("ThanhToanHDNC", "HDNhanCong", new { @HDNhanCong_ID = hopdong.HOPDONG_NHANCONG_ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(thanhtoan);
        }
        [HttpGet]
        public PartialViewResult detailTT(int IDTT)
        {
            ViewBag.detail = IDTT + "detail";
            return PartialView(db.THANHTOAN_NHANCONG.SingleOrDefault(n => n.ID_TT == IDTT));
        }
        [HttpGet]
        public PartialViewResult xoaTT(int IDTT)
        {
            ViewBag.delete = IDTT + "delete";
            return PartialView(db.THANHTOAN_NHANCONG.SingleOrDefault(n => n.ID_TT == IDTT));
        }
        [HttpPost]
        public ActionResult xoaTTPost(int IDTT)
        {
            THANHTOAN_NHANCONG thanhtoan = db.THANHTOAN_NHANCONG.SingleOrDefault(n => n.ID_TT == IDTT);
            var hopdong = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == thanhtoan.HOPDONG_NHANCONG_ID);
            if (thanhtoan == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.THANHTOAN_NHANCONG.Remove(thanhtoan);
            db.SaveChanges();
            return RedirectToAction("ThanhToanHDNC", "HDNhanCong", new { @HDid = hopdong.HOPDONG_NHANCONG_ID });
        }
        public ActionResult DSThanhToan_HDNC(int? page)
        {
            var hd = this.db.HOPDONG_NHANCONG.ToList();
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }

        public PartialViewResult deleteHDNC(int? idHDNC)
        {
            ViewBag.hdnc = idHDNC + "delete";
            return PartialView(db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == idHDNC));
        }
        [HttpPost]
        public ActionResult xacnhanxoaHDNC(int? idHDNC)
        {
            //THANHTOAN_NHANCONG tt = db.THANHTOAN_NHANCONG.Find(idHDNC);
            //db.THANHTOAN_NHANCONG.Remove(tt);
            HOPDONG_NHANCONG hd = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == idHDNC);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.HOPDONG_NHANCONG.Remove(hd);
            db.SaveChanges();
            ViewBag.thongbaoxoa = "Xóa HĐ &ldquo;" + hd.SO_HD + "&rdquo; thành công!!!";
            return RedirectToAction("Index", "HDNhanCong");
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
        public ActionResult ExportData(int HDid)
        {
            HOPDONG_NHANCONG hd = db.HOPDONG_NHANCONG.Find(HDid);
            String filename = hd.FIle;
            if (filename == null)
            {
                ViewBag.baoloi = "Hợp đồng này chưa lưu file!!";
                return RedirectToAction("Index");
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Content/HD_NHANCONG/" + filename;


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

        public JsonResult getBT(int id)
        {
            BENTHUE_TAM bt = db.BENTHUE_TAM.FirstOrDefault(n => n.BENTHUE_TAM_ID == id);
            var ten = bt.TEN;
            var diachi = bt.DIACHI;
            var dienthoai = bt.DIENTHOAI;
            var fax = bt.FAX;
            var msthue = bt.MS_THUE;
            var taikhoan = bt.TAIKHOAN;
            var nganhang = bt.NGANHANG;
            var daidien = bt.DAIDIEN;
            var chucvu = bt.CHUCVU;
            //var fax = bt.FAX;
            return Json(new { ten, diachi, dienthoai, fax, msthue, taikhoan, nganhang, daidien, chucvu }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ThanhLyHD(int HDid)
        {
            var hd = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == HDid);
            if ( hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            ViewBag.id = HDid;
            ViewBag.loaihd = hd.LOAI_HD_SUB.TEN_HD_SUB;
            ViewBag.ctv = hd.TEN_CT;
            ViewBag.cmnd = hd.CMND_CT;
            ViewBag.ngaysinh = hd.NGAYSINH_CT;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThanhLyHD(THANHLY_HDNHANCONG thanhly)
        {
            
            //ViewBag.hdid = HDid;
            if (ModelState.IsValid)
            {
                var hd = db.HOPDONG_NHANCONG.Find(thanhly.HOPDONG_NHANCONG_ID);
                if (hd == null)
                {
                    return RedirectToAction("baoloi", "Home");
                }
                if (TryUpdateModel(hd))
                {
                    hd.ID_TRANGTHAI = 2;
                    db.SaveChanges();
                }

                db.THANHLY_HDNHANCONG.Add(thanhly);
                db.SaveChanges();
                return RedirectToAction("Index", "HDNhanCong");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return RedirectToAction("Index", "HDNhanCong");
        }

        [HttpPost]
        public ActionResult HuyThanhLy(int? HDid)
        {
            var hd = db.HOPDONG_NHANCONG.Find(HDid);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(hd))
            {
                hd.ID_TRANGTHAI = 0;
                db.SaveChanges();
            }
            
            THANHLY_HDNHANCONG thanhly = db.THANHLY_HDNHANCONG.SingleOrDefault(n => n.HOPDONG_NHANCONG_ID == HDid);
            db.THANHLY_HDNHANCONG.Remove(thanhly);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult export_thanhly()
        {
            List<THANHLY_HDNHANCONG> thanhly = db.THANHLY_HDNHANCONG.ToList();
            GridView gv = new GridView();
            int i = 1;
            gv.DataSource = from P in thanhly
                            select new
                            {
                                SO_TT = i++,
                                //LOAI_HD = P.HOPDONG_NHANCONG.LOAI_HD_SUB.TEN_HD_SUB,
                                LOAI_HD = P.HOPDONG_NHANCONG_ID,
                                SO_BIENBAN_THANHLY = P.SOBIENBAN_TL,
                                NGAY_LAP = P.NGAYLAP_BBTL,
                                TEN_CTV = P.TEN_CTV,
                                NGAY_SINH = P.NGAYSINH,
                                CMND = P.CMND,
                                NGAY_CO_HIEU_LUC_THANHLY = P.NGAY_HIEULUC_TL
                                //SO_HOP_DONG = P.HOPDONG_DOANHTHU_ID.
                                
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_THANHLY_NHANCONG_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "HDNhanCong");
        }

        public JsonResult getHD_HetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);            
            var hd1 = db.HOPDONG_NHANCONG.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            var hd2 = db.HOPDONG_NHANCONG.Where(n => n.NGAY_KT < date).ToList();
            int hd = hd1.Count();
            int hd_hethan = hd2.Count();
            return Json(new { hd, hd_hethan }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult HDHetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd1 = db.HOPDONG_NHANCONG.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            return PartialView(hd1);
        }

        public PartialViewResult HDHetHan2()
        {
            DateTime date = System.DateTime.Now;
            var hd2 = db.HOPDONG_NHANCONG.Where(n => n.NGAY_KT < date).ToList();            
            return PartialView(hd2);
        }
        public JsonResult getChuKyTT(int chuky)
        {
            CHUKY_TT ck = db.CHUKY_TT.FirstOrDefault(n => n.CHUKY_ID == chuky);
            var thang = ck.THANG;
            return Json(new { thang }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getHD_thanhtoan()
        {
            DateTime date = System.DateTime.Now;
            //DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd = db.HOPDONG_NHANCONG.ToList();
            List<HOPDONG_NHANCONG> danhsach = new List<HOPDONG_NHANCONG>();
            DateTime sysdate = System.DateTime.Now;
            for (int i = 0; i < hd.Count(); i++)
            {
                var hdid = hd[i].HOPDONG_NHANCONG_ID;
                var thanhtoan = db.THANHTOAN_NHANCONG.Where(n => n.HOPDONG_NHANCONG_ID == hdid).OrderByDescending(n => n.ID_TT);
                DateTime ngaytt = new DateTime();
                DateTime hantt = new DateTime();
                if (thanhtoan.Count() >= 1)
                {
                    var thanhtoan1 = thanhtoan.First();
                    ngaytt = thanhtoan1.DENNGAY_TT.Value;
                    hantt = ngaytt.AddMonths(hd[i].CHUKY_TT.THANG.Value);

                }
                else
                    if (thanhtoan.Count() == 0)
                    {
                        ngaytt = hd[i].NGAY_BD.Value;
                        hantt = ngaytt.AddMonths(hd[i].CHUKY_TT.THANG.Value);
                    }
                if (sysdate > hantt)
                {
                    danhsach.Add(hd[i]);
                }
            }
            int sohd = danhsach.Count();
            return Json(new { sohd }, JsonRequestBehavior.AllowGet);
        }
    }
}
