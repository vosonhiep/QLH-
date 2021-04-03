using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
namespace QLHD.Controllers
{
    public class HomeController : Controller
    {
        QLHD2Entities db = new QLHD2Entities();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.HDChiPhi = db.HOPDONG_CHIPHI.Where(s=>s.TRANGTHAI != 2).Select(x => x.HOPDONG_CHIPHI_ID).Distinct().Count();
                ViewBag.HDDoanhThu = db.HOPDONG_DOANHTHU.Select(x => x.HOPDONG_DOANHTHU_ID).Distinct().Count();
                ViewBag.HDNhanCong = db.HOPDONG_NHANCONG.Select(x => x.HOPDONG_NHANCONG_ID).Distinct().Count();
                ViewBag.ThanhVien = db.THANHVIENs.Select(x => x.ID_THANHVIEN).Distinct().Count();
                ViewBag.TTHDChiPhi = db.THANHTOAN_CHIPHI.Sum(n => n.SOTIEN_TT)??0;
                ViewBag.TTHDDoanhThu = db.THANHTOAN_DOANHTHU.Sum(n => n.SOTIEN_TT)??0;
                ViewBag.TTNhanCong = db.THANHTOAN_NHANCONG.Sum(n => n.SOTIEN_TT) ?? 0;
                ViewBag.HDCNTT = db.HOPDONG_DOANHTHU.Select(x => x.HOPDONG_DOANHTHU_ID).Distinct().Count();
                ViewBag.TTHDCNTT = db.THANHTOAN_DOANHTHU.Sum(n => n.SOTIEN_TT) ?? 0;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult baoloi()
        {
            return View();
        }
        public ActionResult hotro()
        {         
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/HDSD_QLHD.docx";
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "HDSD_QLHD_" + System.DateTime.Now.Day + ".docx",
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
       
    }
}
