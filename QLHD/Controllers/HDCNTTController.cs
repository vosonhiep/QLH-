﻿using System;
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
using Newtonsoft.Json;
using QLHD.Utinity;
using System.Web.Routing;

namespace QLHD.Controllers
{
    public class HDCNTTController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionStore.users == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: /HDCNTT/
        QLHD2Entities db = new QLHD2Entities();

        public ActionResult Index(int? page)
        {
            //if (SessionStore.users == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            var hdCNTT = db.HOPDONG_DT_CNTT.ToList().OrderByDescending(n => n.HOPDONG_DT_CNTT_ID);
            ViewBag.BENTHUEID = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");

            int pageNumber = (page ?? 1);
            int pageSize = 10;

            return View(hdCNTT.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            String loaiTK = f["txtloaiTimKiem"].ToString();
            String tukhoa = f["txtTimKiem"].ToString();

            List<HOPDONG_DT_CNTT> listKQTK = db.HOPDONG_DT_CNTT.ToList();
            ViewBag.tukhoa = tukhoa;
            if (loaiTK == "1")
            {
                listKQTK = db.HOPDONG_DT_CNTT.Where(n => n.SO_HD.Contains(tukhoa)).ToList();
            }
            if (loaiTK == "2")
            {
                int namHD = Int32.Parse(f["NAM_HD_ID"].ToString());
                listKQTK = db.HOPDONG_DT_CNTT.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiTK == "3")
            {
                int loaiHD = Int32.Parse(f["ID_LOAIHD_SUB"].ToString());
                listKQTK = db.HOPDONG_DT_CNTT.Where(n => n.LOAI_HOPDONG_ID == loaiHD).ToList();
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


        public ActionResult IndexTimKiem(int? idda)
        {
            int? page = 1;
            List<HOPDONG_DT_CNTT> listKQTK = db.HOPDONG_DT_CNTT.ToList();
            listKQTK = db.HOPDONG_DT_CNTT.Where(n => n.DUAN_ID == idda).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 15;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy hợp đồng nào!!";
                return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + listKQTK.Count + " kết quả!";
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult partialSearch()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "--Chọn thông tin cần tìm--", Value = "0" });

            items.Add(new SelectListItem { Text = "Số HĐ", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Năm HĐ", Value = "2" });
            items.Add(new SelectListItem { Text = "Loại HĐ", Value = "3" });
            items.Add(new SelectListItem { Text = "Khác", Value = "4" });
            ViewBag.loaiTK = items;
            ViewBag.loaiHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            return PartialView();
        }

        /// <summary>
        /// Tạo mới HĐ CNTT
        /// </summary>

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.BENCHOTHUE = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            ViewBag.BENTHUEID = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            //ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.BENTHUE_TAM_ID), "BENTHUE_TAM_ID", "TEN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            ViewBag.DUAN = new SelectList(db.QLDA_CNTT.ToList().OrderByDescending(n => n.NGAY_START_DA), "DUAN_ID", "TEN_DA");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HOPDONG_DT_CNTT hopdong, HttpPostedFileBase upload)
        {
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.BENCHOTHUE = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            ViewBag.BENTHUEID = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            //ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.BENTHUE_TAM_ID), "BENTHUE_TAM_ID", "TEN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            ViewBag.DUAN = new SelectList(db.QLDA_CNTT.ToList().OrderByDescending(n => n.NGAY_START_DA), "DUAN_ID", "TEN_DA");
            //if (ModelState.IsValid)
            //{
            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (upload != null && upload.ContentLength > 0)
            {
                //start test luu file to DB
                // Converting to bytes.  
                byte[] uploadedFile = new byte[upload.InputStream.Length];
                upload.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                // Initialization.  
                fileContent = Convert.ToBase64String(uploadedFile);
                fileContentType = upload.ContentType;

                // Saving info.
                TBL_FILE newFile = new TBL_FILE();
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                int flag = db.SaveChanges();
                //end test luu file to DB
                if (flag == 1)
                {
                    hopdong.FILE_ID = newFile.file_id;
                    hopdong.FILE = newFile.file_name;
                }
            }

            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);

            hopdong.USER = user.TENTRUYCAP;
            db.HOPDONG_DT_CNTT.Add(hopdong);
            var rs = db.SaveChanges();
            if (rs == 1)
            {
                return RedirectToAction("Index", "HDCNTT");
            }
            else
            {
                return View();
            }

            //}
            //else ViewBag.baoloi = "Lưu không thành công!";
            //return View();
        }

        /// <summary>
        /// Chỉnh sửa HĐ CNTT
        /// </summary>
        /// 
        [HttpGet]
        public ActionResult EditHDCNTT(int? HDCNTT_ID)
        {
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.BENCHOTHUE = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            ViewBag.BENTHUEID = new SelectList(db.DM_CHUTHE_HOPDONG.ToList().OrderBy(n => n.CHUTHE_HOPDONG_ID), "CHUTHE_HOPDONG_ID", "TEN_CHUTHE");
            //ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.BENTHUE_TAM_ID), "BENTHUE_TAM_ID", "TEN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            ViewBag.DUAN = new SelectList(db.QLDA_CNTT.ToList().OrderByDescending(n => n.NGAY_START_DA), "DUAN_ID", "TEN_DA");
            HOPDONG_DT_CNTT HDcntt = db.HOPDONG_DT_CNTT.Find(HDCNTT_ID);

            if (HDcntt == null)
            {
                return HttpNotFound();
            }
            ViewBag.listTDTT = db.DT_CNTT_TIENDO_TT.Where(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID).OrderBy(x => x.TIENDO_TT_ID).ToList();
            return View(HDcntt);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditHDCNTTPost(int? HDCNTT_ID, HttpPostedFileBase upload)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.MALOAIHD_SUB = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 2).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");

            var HDCNTT = db.HOPDONG_DT_CNTT.Find(HDCNTT_ID);

            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (upload != null && upload.ContentLength > 0)
            {
                //start test luu file to DB
                // Converting to bytes.  
                byte[] uploadedFile = new byte[upload.InputStream.Length];
                upload.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                // Initialization.  
                fileContent = Convert.ToBase64String(uploadedFile);
                fileContentType = upload.ContentType;

                // Saving info.
                TBL_FILE newFile = new TBL_FILE();
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                int flag = db.SaveChanges();
                //end test luu file to DB
                if (flag == 1)
                {
                    HDCNTT.FILE_ID = newFile.file_id;
                    HDCNTT.FILE = newFile.file_name;
                }
            }
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            int id_taikhoan = user.ID_THANHVIEN;
            var rs = TryUpdateModel(HDCNTT, "",
                new string[] {"TEN_DUAN", "TEN_GOITHAU", "LOAI_HOPDONG_ID", "TEN_HOPDONG", "CHUKY_ID", "HTTT_ID", "SO_HD", "NGAY_HD", "BEN_THUE_ID",
                            "BEN_CHOTHUE_ID", "GIATRI_PHANCUNG_HD", "GIATRI_DICHVU_HD", "GIATRI_TAMUNG",
                            "NGAY_HIEULUC_HD", "NGAY_BBNT", "GIATRI_BLTHHD", "NGAY_BLTHHD", "THOIGIAN_BLTHHD", "NGAY_HETHAN_BLTHHD",
                            "GHICHU", "DONVI_ID", "THANG", "VAT", "SO_CHUKY", "DUAN_ID"});
            {
                try
                {
                    db.SaveChanges();
                    luulichsuchinhsua(2, HDCNTT_ID, id_taikhoan);
                    return RedirectToAction("Index", "HDCNTT");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("EditHDCNTT", new { HDCNTT_ID = HDCNTT.HOPDONG_DT_CNTT_ID });
                }
            }
        }

        /// <summary>
        /// Xoá HĐ CNTT
        /// </summary>

        [HttpGet]
        public ActionResult DeleteHDCNTT(int HDCNTT_ID)
        {
            HOPDONG_DT_CNTT HDCNTT = db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID);
            if (HDCNTT == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(HDCNTT);
        }

        public PartialViewResult PartiaDeleteHDCNTT(int HDCNTT_ID)
        {
            ViewBag.delete = HDCNTT_ID + "delete";
            HOPDONG_DT_CNTT HDCNTT = db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID);
            if (HDCNTT == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(HDCNTT);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XacNhanXoaHDCNTT(int HDCNTT_ID)
        {
            HOPDONG_DT_CNTT HDCNTT = db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID);
            if (HDCNTT == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DT_CNTT_TIENDO_TT.RemoveRange(db.DT_CNTT_TIENDO_TT.Where(x => x.HOPDONG_DT_CNTT_ID == HDCNTT_ID));
            db.HOPDONG_DT_CNTT.Remove(HDCNTT);
            db.SaveChanges();
            return RedirectToAction("Index", "HDCNTT");
        }

        /// <summary>
        /// Show HĐ CNTT
        /// </summary>

        public ActionResult ShowHDCNTT(int maHD)
        {
            HOPDONG_DT_CNTT hd = db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == maHD);

            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            ViewBag.DUAN = new SelectList(db.QLDA_CNTT.ToList().OrderByDescending(n => n.NGAY_START_DA), "DUAN_ID", "TEN_DA");
            ViewBag.listTDTT = db.DT_CNTT_TIENDO_TT.Where(n => n.HOPDONG_DT_CNTT_ID == hd.HOPDONG_DT_CNTT_ID).OrderBy(x => x.TIENDO_TT_ID).ToList();
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

        public JsonResult getBT(int id)
        {
            if (id != 0)
            {
                DM_CHUTHE_HOPDONG bt = db.DM_CHUTHE_HOPDONG.FirstOrDefault(n => n.CHUTHE_HOPDONG_ID == id);
                var ten = bt.TEN_CHUTHE;
                var diachi = bt.DIACHI;
                var dienthoai = bt.DIENTHOAI;
                var msthue = bt.MSTHUE;
                var taikhoan = bt.STK_NGANHANG;
                var nganhang = bt.TEN_NGANHANG;
                var daidien = bt.DAIDIEN;
                var chucvu = bt.CHUCVU;
                //var fax = bt.FAX;
                return Json(new { ten, diachi, dienthoai, msthue, taikhoan, nganhang, daidien, chucvu }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult getChuKyTT(int chuky)
        {
            CHUKY_TT chuky_tt = db.CHUKY_TT.FirstOrDefault(n => n.CHUKY_ID == chuky);

            var ten = chuky_tt.CHUKY;
            var thang = chuky_tt.THANG;

            return Json(new { ten, thang }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportData(int HDCNTT_ID)
        {
            HOPDONG_DT_CNTT hd = db.HOPDONG_DT_CNTT.Find(HDCNTT_ID);
            TBL_FILE fileInfo = new TBL_FILE();
            // Model binding.  
            try
            {
                // Loading dile info.  
                fileInfo = this.db.TBL_FILE.Where(n => n.file_id == hd.FILE_ID).FirstOrDefault();

                // Info.  
                return this.GetFile(fileInfo.file_base6, fileInfo.file_ext, fileInfo.file_name);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return File(fileInfo.file_base6, fileInfo.file_ext);
            //return RedirectToAction("Index");
        }

        private FileResult GetFile(string fileContent, string fileContentType, string filename)
        {
            // Initialization.  
            FileResult file = null;

            try
            {
                // Get file.  
                byte[] byteContent = Convert.FromBase64String(fileContent);
                file = this.File(byteContent, fileContentType);
                file.FileDownloadName = filename;
            }
            catch (Exception ex)
            {
                // Info.  
                throw ex;
            }

            // info.  
            return file;
        }

        List<TRANGTHAI_HOADON> trangthai_hoadon = new List<TRANGTHAI_HOADON>()
            {
                new TRANGTHAI_HOADON(1, "Đã xuất"),
                new TRANGTHAI_HOADON(0, "Chưa xuất")
            };

        List<TRANGTHAI_THANHTOAN> trangthai_thanhtoan = new List<TRANGTHAI_THANHTOAN>()
            {
                new TRANGTHAI_THANHTOAN(1, "Đã thanh toán"),
                new TRANGTHAI_THANHTOAN(0, "Chưa thanh toán")
            };

        [HttpGet]
        public JsonResult TaoTienDoTT(int? idHD)
        {
            var new_hd_obj = db.HOPDONG_DT_CNTT.Find(idHD);
            if (new_hd_obj.NGAY_BBNT.HasValue)
            {
                //Hardcode Số tháng thuê
                //new_hd_obj.THANG = 60;
                int sl_dot_tt = (int)Math.Round((double)(new_hd_obj.THANG / new_hd_obj.CHUKY_TT.THANG));
                List<DT_CNTT_TIENDO_TT> lst_TDTT = new List<DT_CNTT_TIENDO_TT>();
                //Case Chu kỳ thanh toan là Cuối kỳ
                DateTime date_toihan = new_hd_obj.NGAY_BBNT.Value.AddMonths(new_hd_obj.CHUKY_TT.THANG.Value);
                //Case Chu kỳ thanh toan là Đầu kỳ
                if (new_hd_obj.THOIHAN_TT_ID == 2)
                {
                    date_toihan = new_hd_obj.NGAY_BBNT.Value;
                }
                for (int i = 0; i < sl_dot_tt; i++)
                {
                    DT_CNTT_TIENDO_TT new_obj = new DT_CNTT_TIENDO_TT();
                    new_obj.HOPDONG_DT_CNTT_ID = new_hd_obj.HOPDONG_DT_CNTT_ID;
                    new_obj.DOT_TT = i + 1;
                    new_obj.GIATRI_TT = (new_hd_obj.GIATRI_PHANCUNG_HD + new_hd_obj.GIATRI_DICHVU_HD) / sl_dot_tt;
                    new_obj.THOIGIAN_TT = 30;
                    //Thời hạn đến kỳ thanh toán
                    new_obj.THOIHAN_TT = date_toihan;
                    //Chưa xuất hóa đơn
                    new_obj.TRANGTHAI_XUAT_HOADON = 0;
                    new_obj.TRANGTHAI_TT = 0;
                    lst_TDTT.Add(new_obj);
                    //Tính ngày gia hạn tiếp theo
                    date_toihan = date_toihan.AddMonths(new_hd_obj.CHUKY_TT.THANG.Value);
                }
                db.DT_CNTT_TIENDO_TT.AddRange(lst_TDTT);
                db.SaveChanges();
                List<DT_CNTT_TIENDO_TT> lst_TDTT_view = db.DT_CNTT_TIENDO_TT.Where(x => x.HOPDONG_DT_CNTT_ID == idHD).ToList();
                var list = (from obj in lst_TDTT_view
                            select
                                new
                                {
                                    DOT_TT = obj.DOT_TT,
                                    THOIGIAN_TT = obj.THOIGIAN_TT,
                                    THOIHAN_TT = obj.THOIHAN_TT,
                                    GIATRI_TT = obj.GIATRI_TT,
                                    CHUKY = obj.HOPDONG_DT_CNTT.CHUKY_TT.CHUKY,
                                    TRANGTHAI_XUAT_HOADON = obj.TRANGTHAI_XUAT_HOADON,
                                    SO_HOADON = obj.SO_HOADON,
                                    NGAY_HOADON = obj.NGAY_HOADON,
                                    TRANGTHAI_TT = obj.TRANGTHAI_TT
                                });
                return Json(new { error = "", data = list }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "", data = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult EditTienDoTT(int? HDCNTT_ID, int? TIENDO_ID)
        {
            ViewBag.edit = TIENDO_ID + "edit";
            ViewBag.TT_HOADON = new SelectList(trangthai_hoadon, "TRANGTHAI_XUAT_HOADON", "TRANGTHAI_HOADON_VALUE");
            ViewBag.TT_THANHTOAN = new SelectList(trangthai_thanhtoan, "TRANGTHAI_THANHTOAN_ID", "TRANGTHAI_THANHTOAN_VALUE");
            return PartialView(db.DT_CNTT_TIENDO_TT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID && n.TIENDO_TT_ID == TIENDO_ID));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditTienDoTTPost(int idHDCNTT, int idTD, HttpPostedFileBase upload)
        {
            DT_CNTT_TIENDO_TT td = db.DT_CNTT_TIENDO_TT.Where(x => x.HOPDONG_DT_CNTT_ID == idHDCNTT && x.TIENDO_TT_ID == idTD).FirstOrDefault();

            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (upload != null && upload.ContentLength > 0)
            {
                //start test luu file to DB
                // Converting to bytes.  
                byte[] uploadedFile = new byte[upload.InputStream.Length];
                upload.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                // Initialization.  
                fileContent = Convert.ToBase64String(uploadedFile);
                fileContentType = upload.ContentType;

                // Saving info.
                TBL_FILE newFile = new TBL_FILE();
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                int flag = db.SaveChanges();
                //end test luu file to DB
                if (flag == 1)
                {
                    td.FILE_ID = newFile.file_id;
                    td.FILE = newFile.file_name;
                }
            }

            var rs = TryUpdateModel(td, "",
                   new string[] { "DOT_TT", "GIATRI_TT", "THOIGIAN_TT", "THOIHAN_TT", "SO_HOADON",
                                   "NGAY_HOADON", "TRANGTHAI_XUAT_HOADON", "TRANGTHAI_TT", "FILE", "GHICHU"});
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("EditHDCNTT", "HDCNTT", new { @HDCNTT_ID = td.HOPDONG_DT_CNTT_ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("EditHDCNTT", new { idHDCNTT = td.HOPDONG_DT_CNTT_ID, TIENDO_ID = td.TIENDO_TT_ID });
                }
            }
        }

        [HttpGet]
        public PartialViewResult DeleteTienDoTT(int? HDCNTT_ID, int? TIENDO_ID)
        {
            ViewBag.delete = TIENDO_ID + "delete";
            return PartialView(db.DT_CNTT_TIENDO_TT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID && n.TIENDO_TT_ID == TIENDO_ID));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteTienDoTTPost(int HDCNTT_ID, int TIENDO_ID)
        {
            DT_CNTT_TIENDO_TT thanhtoan = db.DT_CNTT_TIENDO_TT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDCNTT_ID && n.TIENDO_TT_ID == TIENDO_ID);
            int idhdcntt = thanhtoan.HOPDONG_DT_CNTT_ID;

            db.DT_CNTT_TIENDO_TT.Remove(thanhtoan);
            db.SaveChanges();
            return RedirectToAction("EditHDCNTT", "HDCNTT", new { @HDCNTT_ID = idhdcntt });
        }


        public ActionResult ExportDataTienDo(int HDCNTT_ID, int TIENDO_ID)
        {
            DT_CNTT_TIENDO_TT td = db.DT_CNTT_TIENDO_TT.Where(x => x.HOPDONG_DT_CNTT_ID == HDCNTT_ID && x.TIENDO_TT_ID == TIENDO_ID).FirstOrDefault();
            TBL_FILE fileInfo = new TBL_FILE();
            // Model binding.  
            try
            {
                // Loading dile info.  
                fileInfo = this.db.TBL_FILE.Where(n => n.file_id == td.FILE_ID).FirstOrDefault();

                // Info.  
                return this.GetFile(fileInfo.file_base6, fileInfo.file_ext, fileInfo.file_name);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return File(fileInfo.file_base6, fileInfo.file_ext);
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

        public ActionResult DSThanhToan_HDCNTT(int? page)
        {
            var hd = this.db.HOPDONG_DT_CNTT.ToList();
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
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
            if (db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDid) == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            // hiệp chưa chỉnh sửa
            if (db.DT_CNTT_TIENDO_TT.Where(n => n.HOPDONG_DT_CNTT_ID == HDid) == null)
            {
                ViewBag.sohd = "HĐ chưa được thanh toán !!";
            }
            HOPDONG_DT_CNTT hd = db.HOPDONG_DT_CNTT.SingleOrDefault(n => n.HOPDONG_DT_CNTT_ID == HDid);
            ViewBag.hopdong = hd;
            ViewBag.hdid = HDid;

            return View(db.DT_CNTT_TIENDO_TT.OrderByDescending(n => n.HOPDONG_DT_CNTT_ID == HDid));
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

        public static List<LOAITIENTRINH> listLoaiTienTrinh = new List<LOAITIENTRINH>()
            {
                new LOAITIENTRINH(true, "Viễn thông"),
                new LOAITIENTRINH(false, "Khách hàng")
            };

        public static List<TRANGTHAI_TIENDO_DA> listTrangThai = new List<TRANGTHAI_TIENDO_DA>()
            {
                new TRANGTHAI_TIENDO_DA(1, "Chưa thực hiện"),
                new TRANGTHAI_TIENDO_DA(2, "Đang thực hiện"),
                new TRANGTHAI_TIENDO_DA(3, "Hoàn thành"),
                new TRANGTHAI_TIENDO_DA(4, "Trễ hạn")
            };

        public static List<LOAI_DUAN> listLoaiDA = new List<LOAI_DUAN>()
            {
                new LOAI_DUAN(1, "Phần mềm"),
                new LOAI_DUAN(2, "Đường truyền"),
                new LOAI_DUAN(3, "Thiết bị"),
                new LOAI_DUAN(4, "Hỗn hợp")
            };

        public static List<TRANGTHAI_DUAN> listTTDA = new List<TRANGTHAI_DUAN>()
            {
                new TRANGTHAI_DUAN(1, "Chưa thực hiện"),
                new TRANGTHAI_DUAN(2, "Đang thực hiện"),
                new TRANGTHAI_DUAN(3, "Hoàn thành"),
                new TRANGTHAI_DUAN(4, "Trễ hạn")
            };

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult DSDuAn(int? page)
        {
            //if (SessionStore.users == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            var duan = db.QLDA_CNTT.ToList().OrderByDescending(n => n.DUAN_ID);

            // Cập nhật tiến (trễ hạn) độ DA
            foreach (var item in duan)
            {
                if (item.QLDA_CNTT_TIENDO.Count > 0)
                {
                    // Kiểm tra Số lượng tiến độ hoàn thành = Số lượng tiến độ dự án
                    if (item.QLDA_CNTT_TIENDO.Count(x => x.TRANGTHAI_THUCHIEN == 3) == item.QLDA_CNTT_TIENDO.Count)
                    {
                        item.TRANGTHAI_DA = 3;      // Dự án đã hoàn thành
                    }
                    else if (CheckTreHan(item, item.QLDA_CNTT_TIENDO.LastOrDefault()))      // tiến độ cuối cùng bị trễ => Dự án trễ hạn
                    {
                        item.TRANGTHAI_DA = 4;      // Dự án trễ hạn
                    }
                    else
                    {
                        item.TRANGTHAI_DA = 2;      // Dự án đang thực hiện
                    }
                }
            }
            db.SaveChanges();
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");

            int pageNumber = (page ?? 1);
            int pageSize = 10;

            return View(duan.ToPagedList(pageNumber, pageSize));
        }

        public bool CheckTreHan(QLDA_CNTT da, QLDA_CNTT_TIENDO last)
        {
            foreach (var item in da.QLDA_CNTT_TIENDO)
            {
                if (item.NGAY_GIAO > last.NGAY_HETHAN)
                {
                    return true;
                }
            }
            return false;
        }
        public PartialViewResult partialSearchDA()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "--Chọn thông tin cần tìm--", Value = "0" });

            items.Add(new SelectListItem { Text = "Tên dự án", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "Chủ đầu tư", Value = "2" });
            items.Add(new SelectListItem { Text = "Loại dự án", Value = "3" });
            items.Add(new SelectListItem { Text = "Loại hợp đồng", Value = "4" });
            items.Add(new SelectListItem { Text = "Trạng thái", Value = "5" });
            items.Add(new SelectListItem { Text = "Khác", Value = "5" });
            ViewBag.loaiTK = items;
            ViewBag.loaiHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.loaiDA = new SelectList(listLoaiDA, "LOAI_DUAN_ID", "TEN_LOAI_DUAN");
            ViewBag.TTDA = new SelectList(listTTDA, "LOAITRANGTHAI_ID", "TENLOAITRANGTHAI");

            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KetQuaTimKiemDA(FormCollection f, int? page)
        {
            string tenDA = f["TEN_DUAN"].ToString();
            string chuDT = f["CHU_DT"].ToString();
            int loaiDA = Int32.Parse(f["LOAI_DUAN_ID"].ToString());
            int TTDA = Int32.Parse(f["LOAITRANGTHAI_ID"].ToString());
            DateTime tungay = DateTime.Parse(f["TU_NGAY"].ToString());
            DateTime denngay = DateTime.Parse(f["DEN_NGAY"].ToString());
            List<QLDA_CNTT> listKQTK = db.QLDA_CNTT.ToList();
            if (tenDA != "")
            {
                listKQTK = listKQTK.Where(n => n.TEN_DA.ToUpper().Contains(tenDA.ToUpper())).ToList();
            }
            if (chuDT != "")
            {
                listKQTK = listKQTK.Where(n => n.CHUDAUTU.ToUpper().Contains(chuDT.ToUpper())).ToList();
            }
            if (loaiDA > 0)
            {
                listKQTK = listKQTK.Where(n => n.LOAI_DA == loaiDA).ToList();
            }
            if (TTDA > 0)
            {
                listKQTK = listKQTK.Where(n => n.TRANGTHAI_DA == TTDA).ToList();
            }
            listKQTK = listKQTK.Where(x => x.NGAY_START_DA >= tungay && x.NGAY_START_DA <= denngay).ToList();

            ViewBag.loaiHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            ViewBag.loaiDA = new SelectList(listLoaiDA, "LOAI_DUAN_ID", "TEN_LOAI_DUAN");

            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(listKQTK.OrderBy(n => n.DUAN_ID).ToPagedList(pageNumber, pageSize));
        }


        static List<DM_KHOITAO_TIENDO_DA> _lst = null;
        [HttpGet]
        public ActionResult CreateDuAn(int? page)
        {
            ViewBag.LOAIDA = new SelectList(listLoaiDA, "LOAI_DUAN_ID", "TEN_LOAI_DUAN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            _lst = db.DM_KHOITAO_TIENDO_DA.OrderBy(x => x.TIENDO_DA_ID).ToList();
            var date = DateTime.Now;
            foreach (var item in _lst)
            {
                item.NGAY_GIAO = date;
                item.NGAY_HETHAN = date.AddDays(item.SONGAY_THUCHIEN.Value);
                date = item.NGAY_HETHAN.AddDays(1);
            }

            List<Object> ngay_td_lst = new List<Object>();
            foreach (var item in _lst)
            {
                var ngay_td = new
                {
                    TIENDO_DA_ID = item.TIENDO_DA_ID,
                    NGAY_GIAO = item.NGAY_GIAO,
                    NGAY_HETHAN = item.NGAY_HETHAN,
                    SONGAY_THUCHIEN = item.SONGAY_THUCHIEN
                };

                ngay_td_lst.Add(ngay_td);
            }
            ViewBag.DS_NGAY_TD = ngay_td_lst;
            return View(_lst);
        }

        public QLDA_CNTT_TIENDO ConvertToTDDA(DM_KHOITAO_TIENDO_DA item, int idDA)
        {
            QLDA_CNTT_TIENDO td = new QLDA_CNTT_TIENDO();
            td.DUAN_ID = idDA;
            td.TEN_TIENDO_DA = item.TEN_TIENDO_DA;
            td.DONVI_CHUTRI = item.DONVI_CHUTRI;
            td.NGUOI_THUCHIEN = item.NGUOI_THUCHIEN;
            td.TRANGTHAI_THUCHIEN = item.TRANGTHAI_THUCHIEN;
            td.NGAY_GIAO = item.NGAY_GIAO;
            td.NGAY_HETHAN = item.NGAY_HETHAN;
            td.GHICHU_HIENTRANG = item.GHICHU_HIENTRANG;
            td.GHICHU_TONDONG = item.GHICHU_TONDONG;
            td.STT = item.STT;
            td.VTT = item.VTT;

            return td;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateDuAnPost(List<DM_KHOITAO_TIENDO_DA> lst, QLDA_CNTT da)
        {
            //if (ModelState.IsValid)
            //{
            da.TRANGTHAI_DA = 2;        // đang thực hiện
            db.QLDA_CNTT.Add(da);
            var result = db.SaveChanges();
            if (result == 1)
            {
                // Thêm tiến độ
                foreach (var item in lst)
                {
                    db.QLDA_CNTT_TIENDO.Add(ConvertToTDDA(item, da.DUAN_ID));
                }
                db.SaveChanges();
                //Gọi proceduce generate ds tiến độ DA
                //db.FUNC_GEN_TIENDO_DA(da.DUAN_ID);
                return RedirectToAction("DSDuAn", "HDCNTT");
            }
            else
            {
                return View();
            }
            //}
            //else
            //{
            //    ViewBag.baoloi = "Lưu không thành công!";
            //    return View();
            //}

        }

        [HttpGet]
        public ActionResult EditDA(int? maDA)
        {
            ViewBag.LOAIDA = new SelectList(listLoaiDA, "LOAI_DUAN_ID", "TEN_LOAI_DUAN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");

            QLDA_CNTT da = db.QLDA_CNTT.Find(maDA);

            if (da == null)
            {
                return HttpNotFound();
            }
            return View(da);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDAPost(int? maDA)
        {
            ViewBag.LOAIDA = new SelectList(listLoaiDA, "LOAI_DUAN_ID", "TEN_LOAI_DUAN");
            ViewBag.LOAIHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");

            var da = db.QLDA_CNTT.Find(maDA);
            //var da = db.QLDA_CNTT.Find(duan.DUAN_ID);

            var rs = TryUpdateModel(da, "",
                new string[] { "TEN_DA", "CHUDAUTU", "LOAI_DA", "LOAI_HOPDONG_ID", "NGAY_START_DA", "TRANGTHAI_DA", "DOANHTHU_DUKIEN" });
            {
                try
                {
                    db.SaveChanges();
                    //luulichsuchinhsua(2, HDCNTT_ID, id_taikhoan);

                    return RedirectToAction("DSDuAn", "HDCNTT");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("EditDA", new { maDA = da.DUAN_ID });
                }
            }
        }

        [HttpGet]
        public PartialViewResult addTDDA(int? idDA)
        {
            ViewBag.IDDuAn = idDA;
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.DVTH = new SelectList(listLoaiTienTrinh, "LOAITIENTRINH_ID", "TENLOAITIENTRINH");
            ViewBag.NGUOITHUCHIEN = new SelectList(db.THANHVIENs.ToList().OrderBy(n => n.DONVI_ID), "ID_THANHVIEN", "TEN_THANHVIEN");
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateTienDoDAPost(QLDA_CNTT_TIENDO tdda, HttpPostedFileBase upload)
        {
            //if (ModelState.IsValid)
            //{
            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (upload != null && upload.ContentLength > 0)
            {
                //start test luu file to DB
                // Converting to bytes.  
                byte[] uploadedFile = new byte[upload.InputStream.Length];
                upload.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                // Initialization.  
                fileContent = Convert.ToBase64String(uploadedFile);
                fileContentType = upload.ContentType;

                // Saving info.
                TBL_FILE newFile = new TBL_FILE();
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                int flag = db.SaveChanges();
                //end test luu file to DB
                if (flag == 1)
                {
                    tdda.FILE_ID = newFile.file_id;
                }
            }

            tdda.TRANGTHAI_THUCHIEN = 2;
            db.QLDA_CNTT_TIENDO.Add(tdda);

            // Tăng STT lên 1 nếu bị trùng
            if (db.QLDA_CNTT_TIENDO.Where(x => x.STT == tdda.STT).FirstOrDefault() != null)
            {
                var lst = db.QLDA_CNTT_TIENDO.Where(x => x.STT >= tdda.STT && x.DUAN_ID == tdda.DUAN_ID).ToList();
                foreach (var item in lst)
                {
                    item.STT += 1;
                }
            }
            var rs = db.SaveChanges();
            if (rs == 1)
            {
                return RedirectToAction("ShowQuyTrinhDA", "HDCNTT", new { maDA = tdda.DUAN_ID });
            }
            else
            {
                return View();

            }
        }

        [HttpGet]
        public PartialViewResult editTDDA(int? idDA, int? idTD)
        {
            ViewBag.edit = idTD;
            ViewBag.IDDuAn = idDA;
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.DVTH = new SelectList(listLoaiTienTrinh, "LOAITIENTRINH_ID", "TENLOAITIENTRINH");
            ViewBag.NGUOITHUCHIEN = new SelectList(db.THANHVIENs.ToList().OrderBy(n => n.DONVI_ID), "ID_THANHVIEN", "TEN_THANHVIEN");
            ViewBag.TRANGTHAI_TD = new SelectList(listTrangThai, "TRANGTHAI_TIENDO_DA_ID", "TEN_TRANGTHAI_TIENDO_DA");
            return PartialView(db.QLDA_CNTT_TIENDO.SingleOrDefault(x => x.DUAN_ID == idDA && x.TIENDO_DA_ID == idTD));
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult editTDDAPost(int idDA, int idTD, HttpPostedFileBase upload)
        {
            QLDA_CNTT_TIENDO td = db.QLDA_CNTT_TIENDO.Where(x => x.DUAN_ID == idDA && x.TIENDO_DA_ID == idTD).FirstOrDefault();
            //QLDA_CNTT_TIENDO td = db.QLDA_CNTT_TIENDO.Where(x => x.DUAN_ID == tdda.DUAN_ID && x.TIENDO_DA_ID == tdda.TIENDO_DA_ID).FirstOrDefault();

            var STT = td.STT;
            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (upload != null && upload.ContentLength > 0)
            {
                //start test luu file to DB
                // Converting to bytes.  
                byte[] uploadedFile = new byte[upload.InputStream.Length];
                upload.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                // Initialization.  
                fileContent = Convert.ToBase64String(uploadedFile);
                fileContentType = upload.ContentType;

                // Saving info.
                TBL_FILE newFile = new TBL_FILE();
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                int flag = db.SaveChanges();
                //end test luu file to DB
                if (flag == 1)
                {
                    td.FILE_ID = newFile.file_id;
                }
            }

            var rs = TryUpdateModel(td, "",
                   new string[] { "TEN_TIENDO_DA", "DONVI_CHUTRI", "NGUOI_THUCHIEN", "TRANGTHAI_THUCHIEN", "NGAY_GIAO",
                                   "NGAY_HETHAN", "FILE_ID", "GHICHU_HIENTRANG", "GHICHU_TONDONG", "STT", "VTT"});
            {
                try
                {
                    // Tăng STT lên 1 nếu bị trùng
                    //if (db.QLDA_CNTT_TIENDO.Where(x => x.STT == td.STT && td.STT != STT && x.DUAN_ID == td.DUAN_ID).FirstOrDefault() != null)
                    //{
                    //    var lst = db.QLDA_CNTT_TIENDO.Where(x => x.STT >= td.STT && x.TIENDO_DA_ID != td.TIENDO_DA_ID && x.DUAN_ID == td.DUAN_ID).ToList();

                    //    foreach (var item in lst)
                    //    {
                    //        if (STT >= td.STT)
                    //        {
                    //            var temp = lst.SkipWhile(n => n.STT != item.STT).Skip(1).FirstOrDefault();
                    //            if ((temp != null) && (item.STT + 1) != temp.STT)
                    //            {
                    //                item.STT += 1;
                    //            }
                    //            else
                    //            {
                    //                break;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            item.STT += 1;
                    //        }

                    //    }
                    //}

                    db.SaveChanges();

                    return RedirectToAction("ShowQuyTrinhDA", "HDCNTT", new { @maDA = td.DUAN_ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("editTDDA", new { idDA = td.DUAN_ID, idTD = td.TIENDO_DA_ID });
                }
            }
        }

        [HttpGet]
        public PartialViewResult deleteTDDA(int? idDA, int? idTD)
        {
            ViewBag.delete = idTD + "delete";
            QLDA_CNTT_TIENDO td = db.QLDA_CNTT_TIENDO.SingleOrDefault(n => n.DUAN_ID == idDA && n.TIENDO_DA_ID == idTD);
            if (td == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(td);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XacNhanXoaTDDA(int idDA, int idTD)
        {
            QLDA_CNTT_TIENDO td = db.QLDA_CNTT_TIENDO.SingleOrDefault(n => n.DUAN_ID == idDA && n.TIENDO_DA_ID == idTD);
            if (td == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.QLDA_CNTT_TIENDO.Remove(td);

            db.SaveChanges();
            return RedirectToAction("ShowQuyTrinhDA", "HDCNTT", new { @maDA = idDA });
        }


        public PartialViewResult PartiaDeleteDA(int maDA)
        {
            ViewBag.delete = maDA + "delete";
            QLDA_CNTT da = db.QLDA_CNTT.SingleOrDefault(n => n.DUAN_ID == maDA);
            if (da == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(da);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult XacNhanXoaDA(int maDA)
        {
            QLDA_CNTT da = db.QLDA_CNTT.SingleOrDefault(n => n.DUAN_ID == maDA);
            if (da == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            // Xóa tiến độ DA
            db.QLDA_CNTT_TIENDO.RemoveRange(db.QLDA_CNTT_TIENDO.Where(x => x.DUAN_ID == maDA));
            // Xóa tiến độ doanh thu HĐ CNTT
            List<HOPDONG_DT_CNTT> lstHDCNTT = db.HOPDONG_DT_CNTT.Where(x => x.DUAN_ID == da.DUAN_ID).ToList();
            foreach (var item in lstHDCNTT)
            {
                db.DT_CNTT_TIENDO_TT.RemoveRange(db.DT_CNTT_TIENDO_TT.Where(x => x.HOPDONG_DT_CNTT_ID == item.HOPDONG_DT_CNTT_ID));
            }
            //Xóa hợp đồng
            db.HOPDONG_DT_CNTT.RemoveRange(db.HOPDONG_DT_CNTT.Where(x => x.DUAN_ID == da.DUAN_ID));
            // Xóa dự án
            db.QLDA_CNTT.Remove(da);

            db.SaveChanges();
            return RedirectToAction("DSDuAn", "HDCNTT");
        }

        [HttpGet]
        public ActionResult ShowQuyTrinhDA(int? maDA)
        {
            ViewBag.idDA = maDA;
            var dsTienTrinh = db.QLDA_CNTT_TIENDO.Where(x => x.DUAN_ID == maDA).OrderBy(x => x.STT).ToList();
            if (dsTienTrinh.Count() > 0)
            {
                ViewBag.TEN_DUAN = dsTienTrinh.Select(x => x.QLDA_CNTT.TEN_DA).FirstOrDefault();
            }

            // Cập nhật trễ hạn
            foreach (var item in dsTienTrinh)
            {
                if (item.TRANGTHAI_THUCHIEN != 3 && item.NGAY_HETHAN < DateTime.Now)
                {
                    item.TRANGTHAI_THUCHIEN = 4;
                }
            }
            db.SaveChanges();
            return View(dsTienTrinh);
        }

        public ActionResult ExportData_TienTrinh(int TTid)
        {
            QLDA_CNTT_TIENDO td = db.QLDA_CNTT_TIENDO.Find(TTid);
            TBL_FILE fileInfo = new TBL_FILE();
            // Model binding.  
            try
            {
                // Loading dile info.  
                fileInfo = this.db.TBL_FILE.Where(n => n.file_id == td.FILE_ID).FirstOrDefault();

                // Info.  
                return this.GetFile(fileInfo.file_base6, fileInfo.file_ext, fileInfo.file_name);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return File(fileInfo.file_base6, fileInfo.file_ext);
            //return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult index2()
        {
            return View();
        }

        public static string ToMillisecondDate(DateTime dt)
        {
            DateTime StartTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).Date;
            return "/Date(" + ((dt.Date - StartTime.Date).TotalSeconds * 1000).ToString() + ")/";
        }

        [HttpGet]
        public PartialViewResult show_gantt_TDDA(int? idDA)
        {
            ViewBag.TEN_DUAN = "";
            ViewBag.IDDuAn = idDA;
            var dsTienTrinh = db.QLDA_CNTT_TIENDO.Where(x => x.DUAN_ID == idDA).OrderBy(x => x.STT).ToList();
            if (dsTienTrinh.Count() > 0)
            {
                ViewBag.TEN_DUAN = dsTienTrinh.Select(x => x.QLDA_CNTT.TEN_DA).FirstOrDefault();
            }
            List<Object> chart_obj_lst = new List<Object>();
            int i = 0;
            foreach (QLDA_CNTT_TIENDO tiendo_da in dsTienTrinh)
            {
                GANTT_SUB_MODEL gantt_model = new GANTT_SUB_MODEL();
                gantt_model.from = ToMillisecondDate(tiendo_da.NGAY_GIAO).ToString();
                gantt_model.to = ToMillisecondDate(tiendo_da.NGAY_HETHAN).ToString();
                gantt_model.label = tiendo_da.THANHVIEN.TEN_THANHVIEN;
                if (tiendo_da.TRANGTHAI_THUCHIEN == 1)
                {
                    //Chua thuc hien
                    gantt_model.customClass = "ganttBlue";
                }
                else if (tiendo_da.TRANGTHAI_THUCHIEN == 2)
                {
                    //Dang thuc hienct
                    gantt_model.customClass = "ganttOrange";
                }
                else if (tiendo_da.TRANGTHAI_THUCHIEN == 3)
                {
                    //Hoan thanh
                    gantt_model.customClass = "ganttGreen";
                }
                else
                {
                    //Hoan thanh
                    gantt_model.customClass = "ganttRed";
                }
                GANTT_SUB_MODEL[] arr_sub = { gantt_model };
                var loai_tdda = "VNPT";
                if (tiendo_da.VTT == true)
                {
                    loai_tdda = "VNPT";
                }
                else
                {
                    loai_tdda = "Khách hàng";
                }
                var gantt_arr = new
                {
                    name = loai_tdda,
                    desc = tiendo_da.TEN_TIENDO_DA,
                    values = arr_sub,
                };

                chart_obj_lst.Add(gantt_arr);
                i++;
            }
            ViewBag.LST_TDDA = chart_obj_lst;
            return PartialView();
        }
    }
}