using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using System.Data.Entity.Infrastructure;

namespace QLHD.Controllers
{
    public class EditController : Controller
    {
        //
        // GET: /Edit/
        QLHD2Entities db = new QLHD2Entities();
        [HttpGet]
        public PartialViewResult EditChuKyTT(int? machuky)
        {
            CHUKY_TT chuky = db.CHUKY_TT.SingleOrDefault(n=> n.CHUKY_ID == machuky);
            ViewBag.edit = machuky + "edit";
            return PartialView(chuky);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditChuKyTTPost(int? machuky)
        {
            if (machuky == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chuky = db.CHUKY_TT.Find(machuky);
            if (TryUpdateModel(chuky, "",
               new string[] { "CHUKY", "THANG" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("ChuKyThanhToan","HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(chuky);
        }

        //Bên thuê
        [HttpGet]
        public PartialViewResult EditBenThue(int? benthueid)
        {
            ViewBag.edit = benthueid + "edit";
            BENTHUE_TAM benthue = db.BENTHUE_TAM.SingleOrDefault(n=>n.BENTHUE_TAM_ID == benthueid);
            //if (benthue == null)
            //{
            //    return RedirectToAction("baoloi", "Home");
            //}
            return PartialView(benthue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBenThuePost(int? benthueid)
        {
            if (benthueid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var benthue = db.BENTHUE_TAM.Find(benthueid);
            if (TryUpdateModel(benthue, "",
               new string[] { "TEN"
                          ,"DAIDIEN"
                          ,"CHUCVU"
                          ,"DIACHI"
                          ,"CMND"
                          ,"NGAYCAP_CMND"
                          ,"NOICAP_CMND"
                          ,"MS_THUE"
                          ,"DIENTHOAI"
                          ,"TAIKHOAN"
                          ,"NGANHANG"
                          ,"FAX"
                          ,"NGAYSINH"
                          ,"TRINHDO"
                          ,"CONGVIEC" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("BenThue", "HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(benthue);
        }
        [HttpGet]
        public PartialViewResult editHTTT(int idHTTT)
        {
            return PartialView(db.HTTTs.SingleOrDefault(n => n.HTTT_ID == idHTTT));
        }
        [HttpPost, ActionName("editHTTT")]
        [ValidateAntiForgeryToken]
        public ActionResult editHTTTPost(int idHTTT)
        {
            var ht = db.HTTTs.SingleOrDefault(n => n.HTTT_ID == idHTTT);
            if (ht == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(ht, "",
               new string[] { "TEN_HTTT", "GHICHU" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("HTTT", "HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(ht);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editLoaiHDPost(int IDHD)
        {
            var hd = db.LOAI_HD_SUB.SingleOrDefault(n => n.ID_LOAIHD_SUB == IDHD);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(hd, "",
               new string[] { "TEN_HD_SUB", "GHICHU" }))
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
            return View(hd);
        }
    }
}
