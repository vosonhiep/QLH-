using QLHD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace QLHD.Controllers
{
    public class ThongKeThanhToanController : Controller
    {
        //
        // GET: /ThongKeThanhToan/
        QLHD2Entities db = new QLHD2Entities();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //Checl role user
        public int checkRole(THANHVIEN nv)
        {
            //Get current User       
            if (nv.HETHONG.TENHETHONG == "LD-TTVT")
            {
                return 1;
            }
            return 0;
        }
        public ActionResult ThongKeTT_HDCP(int? page)
        {
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            var hd = new List<HOPDONG_CHIPHI>();
            if (this.checkRole(user) == 1)
            {
                var hd01 = db.HOPDONG_CHIPHI.Where(n=>n.DONVI_ID == user.DONVI_ID).ToList();
                hd.AddRange(hd01);
            }
            else
            {
                var hd02 = db.HOPDONG_CHIPHI.ToList();
                hd.AddRange(hd02);
            }                
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(hd.OrderBy(n=>n.HOPDONG_CHIPHI_ID).ToPagedList(pageNumber, pageSize));
        }
        public JsonResult getHD()
        {            
            HOPDONG_CHIPHI hd = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == 1).First();            
            //var fax = bt.FAX;
            return Json(new { hd }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult thongke_HDCP_ChuaTT()
        {
            DateTime ngay_tt = System.DateTime.Now;
            //ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.Where(n=>n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.loai_hd = 0;
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            danhsach = HD_ToiHanTT(ngay_tt);
            return View(danhsach);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult thongke_HDCP_ChuaTT(FormCollection f)
        {
            int loai_hd = Int32.Parse(f["loaiHD"].ToString());
            DateTime ngay_tt = DateTime.Parse(f["ngay_tt"].ToString());
            
            //ViewBag.loaihd = db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB);
            if (ngay_tt == null)
            {
                ngay_tt = System.DateTime.Now;
            }
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            danhsach = HD_ToiHanTT(ngay_tt);
            if (loai_hd == 0)
            {
                danhsach = HD_ToiHanTT(ngay_tt);
            }
            else
            {
                danhsach = HD_ToiHanTT(ngay_tt).Where(n => n.LOAI_HD_SUB.ID_LOAIHD_SUB == loai_hd).ToList();
            }
            ViewBag.loai_hd = loai_hd;
            ViewBag.ngay = ngay_tt;
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            return View(danhsach);
        }
        
        public List<HOPDONG_CHIPHI> HD_ToiHanTT(DateTime ngay_tt)
        {
            DateTime date = System.DateTime.Now;
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            var hd = new List<HOPDONG_CHIPHI>();
            if (this.checkRole(user) == 1)
            {
                var hd01 = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == user.DONVI_ID).ToList();
                hd.AddRange(hd01);
            }
            else
            {
                var hd02 = db.HOPDONG_CHIPHI.ToList();
                hd.AddRange(hd02);
            }
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            DateTime sysdate = ngay_tt ;
            if (ngay_tt == null)
            {
                sysdate = System.DateTime.Now;
            }
            for (int i = 0; i < hd.Count(); i++)
            {
                var hdid = hd[i].HOPDONG_CHIPHI_ID;
                var thanhtoan = db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == hdid).OrderByDescending(n => n.ID_TT).ToList();
                //DateTime tungay = new DateTime();
                //DateTime denngay = new DateTime();
                DateTime ngaytt = new DateTime();
                DateTime hantt = new DateTime();
                if (thanhtoan.Count() >= 1)
                {
                    var thanhtoan1 = thanhtoan.First();
                    ngaytt = thanhtoan1.DENNGAY_TT.Value;
                    hantt = ngaytt.AddMonths(hd[i].CHUKY_TT1.THANG.Value);

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

                //for (int n = 0; n < thanhtoan.Count(); n++)
                //{
                //    tungay = thanhtoan[n].TUNGAY_TT.Value;
                //    denngay = thanhtoan[n].DENNGAY_TT.Value;
                //    while (sysdate < tungay || sysdate > denngay)
                //    {
                //        danhsach.Add(hd[i]);
                //        break;
                //    }
                //}
            }
            return danhsach;
        }


        [HttpGet]
        public ActionResult thongke_HDNC_ChuaTT()
        {
            DateTime ngay_tt = System.DateTime.Now;
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.loai_hd = 0;
            List<HOPDONG_NHANCONG> danhsach = new List<HOPDONG_NHANCONG>();
            danhsach = HDNC_ToiHanTT(ngay_tt);
            return View(danhsach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult thongke_HDNC_ChuaTT(FormCollection f)
        {
            int loai_hd = Int32.Parse(f["loaiHD"].ToString());
            DateTime ngay_tt = DateTime.Parse(f["ngay_tt"].ToString());
            if (ngay_tt == null)
            {
                ngay_tt = System.DateTime.Now;
            }
            List<HOPDONG_NHANCONG> danhsach = new List<HOPDONG_NHANCONG>();
            danhsach = HDNC_ToiHanTT(ngay_tt);
            if (loai_hd == 0)
            {
                danhsach = HDNC_ToiHanTT(ngay_tt);
            }
            else
            {
                danhsach = HDNC_ToiHanTT(ngay_tt).Where(n => n.LOAI_HD_SUB.ID_LOAIHD_SUB == loai_hd).ToList();
            }
            ViewBag.loai_hd = loai_hd;
            ViewBag.ngay = ngay_tt;
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 3).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            return View(danhsach);
        }
        public List<HOPDONG_NHANCONG> HDNC_ToiHanTT(DateTime ngay_tt)
        {
            DateTime date = System.DateTime.Now;
            var hd = db.HOPDONG_NHANCONG.ToList();
            List<HOPDONG_NHANCONG> danhsach = new List<HOPDONG_NHANCONG>();
            DateTime sysdate = ngay_tt;
            if (ngay_tt == null)
            {
                sysdate = System.DateTime.Now;
            }
            for (int i = 0; i < hd.Count(); i++)
            {
                var hdid = hd[i].HOPDONG_NHANCONG_ID;
                var thanhtoan = db.THANHTOAN_NHANCONG.Where(n => n.HOPDONG_NHANCONG_ID == hdid).OrderByDescending(n => n.ID_TT).ToList();
                //DateTime tungay = new DateTime();
                //DateTime denngay = new DateTime();
                DateTime ngaytt = new DateTime();
                DateTime hantt = new DateTime();
                ngaytt = hd[i].NGAY_BD.Value;
                hantt = ngaytt.AddMonths(hd[i].CHUKY_TT.THANG.Value);
                if (sysdate > hantt)
                {
                    danhsach.Add(hd[i]);
                }

                //for (int n = 0; n < thanhtoan.Count(); n++)
                //{
                //    tungay = thanhtoan[n].TUNGAY_TT.Value;
                //    denngay = thanhtoan[n].DENNGAY_TT.Value;
                //    while (sysdate < tungay || sysdate > denngay)
                //    {
                //        danhsach.Add(hd[i]);
                //        break;
                //    }
                //}
            }
            return danhsach;
        }
    }
}
