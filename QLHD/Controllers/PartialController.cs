using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using QLHD.Controllers;
using System.Data.Entity.Infrastructure;
using System.IO;
//using System.Data.Entity.Infrastructure;
namespace QLHD.Controllers
{
    public class PartialController : Controller
    {
        //
        // GET: /Partial/
        QLHD2Entities db = new QLHD2Entities();
        
        //Thêm bên thuế
        [HttpGet]
        public PartialViewResult addBenThue()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addBenThue(BENTHUE_TAM benthue)
        {
            if (ModelState.IsValid)
            {
                db.BENTHUE_TAM.Add(benthue);
                db.SaveChanges();
                return RedirectToAction("BenThue","HeThong");
            }
            
            return View();
        }
        //Thêm chu kỳ
        [HttpGet]
        public PartialViewResult addChuKy()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addChuKy(CHUKY_TT ck)
        {
            if (ModelState.IsValid)
            {
                db.CHUKY_TT.Add(ck);
                db.SaveChanges();
                return RedirectToAction("ChuKyThanhToan", "HeThong");
            }
            return View();
        }

        //Thêm Thành viên
        [HttpGet]
        public PartialViewResult addThanhVien()
        {
            ViewBag.HETHONG = new SelectList(db.HETHONGs.ToList().OrderBy(n => n.TENHETHONG), "HETHONG_ID", "TENHETHONG");
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addThanhVien(THANHVIEN tv)
        {
            ViewBag.HETHONG = new SelectList(db.HETHONGs.ToList().OrderBy(n => n.TENHETHONG), "HETHONG_ID", "TENHETHONG");
            if (ModelState.IsValid)
            {
                var mk = "123456";
                String mkmd5 = QLHD.Controllers.AccountController.changeMD5(mk);
                tv.MATKHAU = mkmd5;
                tv.IMG = "User.png";
                db.THANHVIENs.Add(tv);
                db.SaveChanges();
                return RedirectToAction("ThanhVien","HeThong");
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult addLoaiHD()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addLoaiHD(LOAI_HD_SUB loaiHD)
        {
            if (ModelState.IsValid)
            {
                db.LOAI_HD_SUB.Add(loaiHD);
                db.SaveChanges();
                return RedirectToAction("LoaiHD", "HeThong");
            }
            return View();
        }

        public PartialViewResult resetmkPartial()
        {
            return PartialView();
        }

        //Thêm HTTt
        [HttpGet]
        public PartialViewResult addHTTT()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addHTTT(HTTT httt)
        {
            if (ModelState.IsValid)
            {
                db.HTTTs.Add(httt);
                db.SaveChanges();
                return RedirectToAction("HTTT","HeThong");
            }
            return View();
        }
        public PartialViewResult avarta()
        {
           THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
           return PartialView(user);
        }
        public PartialViewResult avarta2()
        {
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            return PartialView(user);
        }
        //add Hệ thống
        [HttpGet]
        public PartialViewResult addHT()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addHT(HETHONG ht)
        {
            if (ModelState.IsValid)
            {
                db.HETHONGs.Add(ht);
                db.SaveChanges();
                return RedirectToAction("HeThong", "HeThong");
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult EditUser(int? idTV)
        {
            ViewBag.edit = idTV + "edit";
            return PartialView(db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == idTV));
        }
        [HttpGet]
        public PartialViewResult editLoaiHD(int? idHD)
        {
            ViewBag.edit = idHD + "edit";
            return PartialView(db.LOAI_HD_SUB.SingleOrDefault(n => n.ID_LOAIHD_SUB == idHD));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editHTTTPost(int? idHD)
        {
            var loaihd = db.LOAI_HD_SUB.SingleOrDefault(n => n.ID_LOAIHD_SUB == idHD);
            if (loaihd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(loaihd, "",
               new string[] { "TEN_HD_SUB", "GHICHU", "LOAIHD" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("LoaiHD", "HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(loaihd);
        }
        [HttpGet]
        public PartialViewResult xoaLoaiHD(int? idHD)
        {
            ViewBag.delete = idHD + "delete";
            return PartialView(db.LOAI_HD_SUB.SingleOrDefault(n => n.ID_LOAIHD_SUB == idHD));
        }
        [HttpPost]
        public ActionResult xacnhanxoaLoaiHD(int idHD)
        {
            LOAI_HD_SUB hd = db.LOAI_HD_SUB.SingleOrDefault(n => n.ID_LOAIHD_SUB == idHD);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.LOAI_HD_SUB.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("LoaiHD", "HeThong");
        }
        [HttpGet]
        public PartialViewResult xoaChuKy(int? idCK)
        {
            ViewBag.xoa = idCK + "delete";
            return PartialView(db.CHUKY_TT.SingleOrDefault(n => n.CHUKY_ID == idCK));
        }
        [HttpPost]
        public ActionResult xacnhanxoaChuKy(int idCK)
        {
            CHUKY_TT chuky = db.CHUKY_TT.SingleOrDefault(n => n.CHUKY_ID == idCK);
            if (chuky == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.CHUKY_TT.Remove(chuky);
            db.SaveChanges();
            return RedirectToAction("ChuKy", "HeThong");
        }

        //Show edit modal BTS
        [HttpGet]
        public PartialViewResult editDMTram(int? idTram)
        {
            ViewBag.edit = idTram + "edit";
            return PartialView(db.TRAM_BTS.SingleOrDefault(n => n.TRAM_ID == idTram));
        }

        //Post edit modal BTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editTramHTTTPost(int? idTram)
        {
            var tram_obj = db.TRAM_BTS.SingleOrDefault(n => n.TRAM_ID == idTram);
            if (tram_obj == null)
            {
                return RedirectToAction("baoloi", "Home");
            }            
            try
            {
                TryUpdateModel(tram_obj, "", new string[] {"MA_TRAM", "TEN_TRAM", "LONG_TRAM", "LAT_TRAM", "DIA_CHI_TRAM"});
                db.SaveChanges();

                return RedirectToAction("DMTram", "HeThong");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            
            return View(tram_obj);
        }
        //Show modal add new BTS
        [HttpGet]
        public PartialViewResult addTram()
        {
            return PartialView();
        }

        //Add new BTS
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addTram(TRAM_BTS tram_obj)
        {
            if (ModelState.IsValid)
            {
                var douple_obj = db.TRAM_BTS.Where(s => s.MA_TRAM == tram_obj.MA_TRAM).ToList().Count();
                if (douple_obj < 1) {
                    //active bts
                    tram_obj.TRANG_THAI = 1;
                    db.TRAM_BTS.Add(tram_obj);
                    db.SaveChanges();
                    return RedirectToAction("DMTram", "HeThong");
                }
                else
                {
                    ViewBag.baoloi = "Lưu không thành công!";
                }

            }
            return View();
        }

        //xoa tram (disable tram)
        [HttpGet]
        public PartialViewResult disableTram(int? idTram)
        {
            ViewBag.delete = idTram + "delete";
            return PartialView(db.TRAM_BTS.SingleOrDefault(n => n.TRAM_ID == idTram));
        }

        //xoa tram (disable tram)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xacnhandisableTram(int? idTram)
        {
            var tram_obj = db.TRAM_BTS.SingleOrDefault(n => n.TRAM_ID == idTram);
            if (tram_obj == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            try
            {
                //xoa tram (disable tram)
                tram_obj.TRANG_THAI = 0;
                TryUpdateModel(tram_obj, "", new string[] { "TRANG_THAI" });
                db.SaveChanges();

                return RedirectToAction("DMTram", "HeThong");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(tram_obj);
        }

        //Show modal add new BTS
        [HttpGet]
        public PartialViewResult addVB()
        {
            return PartialView();
        }

        //Add new BTS
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addVB(DM_VANBAN vb_obj, HttpPostedFileBase upload)
        {
            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            if (ModelState.IsValid)
            {
                //Upload file cho DM Van Ban
                //if (upload != null && upload.ContentLength > 0)
                //    try
                //    {
                //        string path = Path.Combine(Server.MapPath("~/Content/VAN_BAN_UPLOAD/"),
                //                                   Path.GetFileName(upload.FileName));
                //        upload.SaveAs(path);
                //        ViewBag.Message = "Văn bản được thêm thành công";
                //        vb_obj.FILE_URL = upload.FileName;
                //        vb_obj.NGAY_UPLOAD = DateTime.Today;
                //    }
                //    catch (Exception ex)
                //    {
                //        ViewBag.Message = "Lỗi:" + ex.Message.ToString();
                //    }
                //else
                //{
                //    ViewBag.Message = "You have not specified a file.";
                //}

                //vb_obj.FILE_URL = upload.FileName;


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
                if(flag == 1)
                {
                    vb_obj.NGAY_UPLOAD = DateTime.Today;
                    vb_obj.FILE_ID = newFile.file_id;
                    //save to DB
                    db.DM_VANBAN.Add(vb_obj);
                    db.SaveChanges();
                }
                return RedirectToAction("DMVanBan", "HeThong");
            }
            return View();
        }

        //Xoa VB
        [HttpGet]
        public PartialViewResult xoaVB(int? idVB)
        {
            ViewBag.delete = idVB + "delete";
            return PartialView(db.DM_VANBAN.SingleOrDefault(n => n.ID == idVB));
        }

        [HttpPost]
        public ActionResult xacnhanxoaVB(int IDVB)
        {
            DM_VANBAN hd = db.DM_VANBAN.SingleOrDefault(n => n.ID == IDVB);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.DM_VANBAN.Remove(hd);
            db.SaveChanges();
            return RedirectToAction("DMVanBan", "HeThong");
        }
    }
}

