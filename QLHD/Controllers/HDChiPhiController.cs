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
using System.Web.UI.WebControls;
using System.Web.UI;
using log4net;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace QLHD.Controllers
{
    public class HDChiPhiController : Controller
    {
        //
        // GET: /HDChiPhi/
        QLHD2Entities db = new QLHD2Entities();
        //check Quyen User bang field TENHETHONG of table HETHONG
        public int checkRole(THANHVIEN nv)
        {
            //Get current User       
            if (nv.HETHONG.TENHETHONG == "LD-TTVT" || nv.HETHONG.TENHETHONG == "TTVT")
                {
                //La LD or NhanVien cua TTVT=> chi xem duoc cac HD cua TTVT do' Start 17/03/2020
                return 1;
                }
            //Xem all HD CP End 17/03/2020
            return 0;
        }
       
        //private int HDCHIPHI_ID;

        public ActionResult Index(int? page)
        {
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            //bo sung phan quyen TTVT chi xem duoc cac HD cua TTVT
            var hd = new List<HOPDONG_CHIPHI>();
            if (this.checkRole(user) == 1) {
                var hd01 = db.HOPDONG_CHIPHI
                .Where(n => !db.THANHLY_HDCHIPHI.Where(m => m.HOPDONG_CHIPHI_ID == n.HOPDONG_CHIPHI_ID).Any() && n.DONVI_ID == user.DONVI_ID)
                .ToList().OrderByDescending(n => n.HOPDONG_CHIPHI_ID);
                hd.AddRange(hd01);
            }
            else
            {
                var hd02 = db.HOPDONG_CHIPHI
                .Where(n => !db.THANHLY_HDCHIPHI.Where(m => m.HOPDONG_CHIPHI_ID == n.HOPDONG_CHIPHI_ID).Any())
                .ToList().OrderByDescending(n => n.HOPDONG_CHIPHI_ID);
                hd.AddRange(hd02);
            }            
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult danhsachthanhtoan(int? page)
        {
            var hd = new List<HOPDONG_CHIPHI>();
            //Check quyen TTVT
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            if (this.checkRole(user) == 1)
            {
                var hd01 = this.db.HOPDONG_CHIPHI.Where(n=>n.DONVI_ID == user.DONVI_ID).ToList();
                hd.AddRange(hd01);
            }
            else {
                var hd02 = this.db.HOPDONG_CHIPHI.ToList();
                hd.AddRange(hd02);
            }            
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult dsHD_toihan(int? page)
        {
            var hd = new List<HOPDONG_CHIPHI>();
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            if (this.checkRole(user) == 1)
            {
                var hd01 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT <= System.DateTime.Now && n.DONVI_ID == user.DONVI_ID).ToList();
                hd.AddRange(hd01);
            }
            else
            {
                var hd02 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT <= System.DateTime.Now).ToList();
                hd.AddRange(hd02);
            }
               
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện không có hợp đồng nào tới hạn!!";
            }
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT", 1);
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n=>n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY", 7);
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT", 1);
            //lay list DM tram BTS
            ViewBag.lstBTS = new SelectList(db.TRAM_BTS.ToList().OrderBy(n => n.TRAM_ID), "TRAM_ID", "TEN_TRAM");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(HOPDONG_CHIPHI hopdong, HttpPostedFileBase upload)
        {
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            if (ModelState.IsValid)
            {
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
                        hopdong.File = newFile.file_name;
                    }
                }
                THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
                hopdong.USER = user.TENTRUYCAP;
                hopdong.TRANGTHAI = 0;
                db.HOPDONG_CHIPHI.Add(hopdong);
                db.SaveChanges();
                return RedirectToAction("Index", "HDChiPhi");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }

        [HttpGet]
        public ActionResult EditHDChiPhi(int? HDCHIPHI_ID)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            HOPDONG_CHIPHI HDchiphi = db.HOPDONG_CHIPHI.Find(HDCHIPHI_ID);
            if (HDchiphi == null)
            {
                return HttpNotFound();
            }
            //lay list tram BTS thuoc HD
            //List<int> lstBtsId = new List<int>();
            //if (!String.IsNullOrEmpty(HDchiphi.BTS_ID))
            //{
            //    lstBtsId = HDchiphi.BTS_ID.Split(',').Select(t => int.Parse(t)).ToList();
            //}
            ViewBag.lstBTS = new SelectList(db.TRAM_BTS.ToList().OrderBy(n => n.TRAM_ID), "TRAM_ID", "TEN_TRAM");

            return View(HDchiphi);
        }

        //[ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        //public ActionResult EditHDChiPhiPost(int? HDCHIPHI_ID, HOPDONG_CHIPHI newObj)
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditHDChiPhiPost(HOPDONG_CHIPHI newObj, HttpPostedFileBase upload)
        {
            ViewBag.DONVI = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            ViewBag.MALOAIHD = new SelectList(db.LOAI_HD_SUB.Where(n => n.LOAIHD == 1).ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.BENTHUEID = new SelectList(db.BENTHUE_TAM.ToList().OrderBy(n => n.TEN), "BENTHUE_TAM_ID", "TEN");
            ViewBag.HINHTHUCTT = new SelectList(db.HTTTs.ToList().OrderBy(n => n.HTTT_ID), "HTTT_ID", "TEN_HTTT");
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.NamHD = new SelectList(db.NAM_HD.ToList().OrderBy(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.THOIHAN_TT = new SelectList(db.THOIHAN_TT.ToList().OrderBy(n => n.THOIHAN_TT_ID), "THOIHAN_TT_ID", "TEN_THOIHAN_TT");
            var HDchiphi = db.HOPDONG_CHIPHI.Find(newObj.HOPDONG_CHIPHI_ID);


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
                    HDchiphi.FILE_ID = newFile.file_id;
                    HDchiphi.File = newFile.file_name;
                }
            }
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            int id_taikhoan = user.ID_THANHVIEN;

            var rsl = TryUpdateModel(HDchiphi, "",
               new string[] { "ID_LOAIHD_SUB", "HTTT_ID", "CHUKY_ID","CHUKY2_ID", "SO_HD", "NGAY_HD","NAM_HD_ID","DONVI_ID","THOIHAN_TT_ID",
                            "TEN_BT", "DIACHI_BT", "DIENTHOAI_BT", "FAX_BT", "TAIKHOAN_BT", "NGANHANG_BT", "MSTHUE_BT",
                            "DAIDIEN_BT", "CMND_BT", "NGAYCAP_CMND_BT", "NOICAP_BT", "CHUCVU_BT", "TEN_CT",
                            "DIACHI_CT", "CMND_CT", "NGAYCAP_CT", "NOICAP_CT", "NGAYSINH", "DIENTHOAI_CT", "DAIDIEN_CT",
                            "CHUCVU_CT", "TAIKHOAN_CT", "NGANHANG_CT", "MSTHUE_CT", "HOPDONG_DT", "SOLUONG_DT",
                            "DIENTICH_DT", "CONGSUAT_DT", "CHUNGLOAI_DT", "CONGVIEC_DT", "TRAM_DT","NOIDUNGKHAC_DT", "THANG_GT", "TONG_GT",
                            "NGAY_BD", "NGAY_KT", "GHICHU", "THANG", "VAT", "SO_CHUKY", "BTS_ID", "THANG_GT_NHA_ANTEN", "THANG_GT_NHA", "THANG_GT_ANTEN", "THANG_GT_MPD", "THANG_GT_KHAC"});

                try
                {
                    db.SaveChanges();
                    luulichsuchinhsua(1, newObj.HOPDONG_CHIPHI_ID, id_taikhoan);
                    return RedirectToAction("ShowHDChiPhi", new { maHD = HDchiphi.HOPDONG_CHIPHI_ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return RedirectToAction("ShowHDChiPhi", new { maHD = HDchiphi.HOPDONG_CHIPHI_ID });
                }
            }
            //return RedirectToAction("ShowHDChiPhi", new { maHD = HDchiphi.HOPDONG_CHIPHI_ID });

        [HttpGet]
        public ActionResult DeleteHDChiPhi(int HDCHIPHI_ID) 
        {
            HOPDONG_CHIPHI HDCP = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDCHIPHI_ID);
            if (HDCP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(HDCP);
        }
        [HttpPost, ActionName("DeleteHDChiPhi")]
        public ActionResult XacNhanXoaHDCP(int HDCHIPHI_ID)
        { 
            HOPDONG_CHIPHI HDCP = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDCHIPHI_ID);
            if (HDCP == null) 
            {
                Response.StatusCode = 404;
                return null;
            }
            db.HOPDONG_CHIPHI.Remove(HDCP);
            db.SaveChanges();
            return RedirectToAction("Index", "HDChiPhi");
        }

        public JsonResult getBT(int id)
        {
            BENTHUE_TAM bt = db.BENTHUE_TAM.FirstOrDefault(n => n.BENTHUE_TAM_ID == id);
            var ten = bt.TEN;
            var diachi = bt.DIACHI;
            var dienthoai = bt.DIENTHOAI;
            var msthue = bt.MS_THUE;
            var taikhoan = bt.TAIKHOAN;
            var nganhang = bt.NGANHANG;
            var daidien = bt.DAIDIEN;
            var chucvu = bt.CHUCVU;
            var fax = bt.FAX;
            return Json(new { ten, diachi, dienthoai, msthue, taikhoan, nganhang, daidien, chucvu, fax }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getChuKyTT(int chuky1, int chuky2)
        {
            CHUKY_TT chuky_dau = db.CHUKY_TT.FirstOrDefault(n => n.CHUKY_ID == chuky1);
            CHUKY_TT chuky_cuoi = db.CHUKY_TT.FirstOrDefault(n => n.CHUKY_ID == chuky2);
            var ten_dau = chuky_dau.CHUKY;
            var thang_dau = chuky_dau.THANG;
            var ten_cuoi = chuky_cuoi.CHUKY;
            var thang_cuoi = chuky_cuoi.THANG;
            return Json(new { ten_dau, thang_dau, ten_cuoi, thang_cuoi}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTT(int id)
        {
            var tt = db.THANHTOAN_CHIPHI.OrderByDescending(n => n.HOPDONG_CHIPHI_ID == id).First();

            var ngaykt = tt.DENNGAY_TT;
            var text = "";
            if (ngaykt >= System.DateTime.Now && ngaykt == null)
            {
                text = "<img src='~/Images/canhbao2.png' />";
            }
            return Json(new { text }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            String loaiTK = f["txtloaiTimKiem"].ToString();
            String tukhoa = f["txtTimKiem"].ToString();

            //Start 17/03/2020 Bo sung dieu kien khong lay HD da thanh ly
            List<HOPDONG_CHIPHI> listKQTK = db.HOPDONG_CHIPHI.Where(n => !db.THANHLY_HDCHIPHI.Where(m => m.HOPDONG_CHIPHI_ID == n.HOPDONG_CHIPHI_ID).Any()).ToList();
            //End 17/03/2020 Bo sung dieu kien khong lay HD da thanh ly
            ViewBag.tukhoa = tukhoa;
            if (loaiTK == "1")
            {
                 listKQTK = listKQTK.Where(n => n.SO_HD.Contains(tukhoa)).ToList();
            } else if (loaiTK == "2")
            {
                int namHD = Int32.Parse(f["NAM_HD_ID"].ToString());
                listKQTK = listKQTK.Where(n => n.NAM_HD_ID == namHD).ToList();
            }else if (loaiTK == "3") 
            {
                int loaiHD = Int32.Parse(f["ID_LOAIHD_SUB"].ToString());
                listKQTK = listKQTK.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            } else if (loaiTK == "4")
            {
                var lstBtsId = db.TRAM_BTS.Where(n => (n.TEN_TRAM.Contains(tukhoa)) || (n.MA_TRAM.Contains(tukhoa))).Select(s=>s.TRAM_ID.ToString()).ToList();
                listKQTK = listKQTK.Where(n => (lstBtsId.Contains(n.BTS_ID))||(n.TEN_CT.Contains(tukhoa))).ToList();
            }
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy hợp đồng nào!!";
                return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
            }
            //check role user
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            //If la LD_TTVT or NhanVien TTVT Start 17/03/2020
            if (this.checkRole(user) == 1)
            {
                listKQTK = listKQTK.Where(n => n.DONVI_ID == user.DONVI_ID).ToList();
            }
            //If la LD_TTVT or NhanVien TTVT End 17/03/2020
            ViewBag.ThongBao = "Đã tìm thấy " + listKQTK.Count + " kết quả!";
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem(int? page, string tukoa)
        {
            ViewBag.TuKhoa = tukoa;
            List<HOPDONG_CHIPHI> listKQTK = db.HOPDONG_CHIPHI.Where(n => n.SO_HD.Contains(tukoa)).ToList();
            //Phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            if (listKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
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
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            return PartialView();
        }
        //Hiển thị thông tin hợp đồng
        public ActionResult ShowHDChiPhi(int maHD) 
        {
            HOPDONG_CHIPHI hd = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == maHD);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            //lay list tram BTS thuoc HD
            List<int> lstBtsId = new List<int>();
            if (!String.IsNullOrEmpty(hd.BTS_ID))
            {
                lstBtsId = hd.BTS_ID.Split(',').Select(t => int.Parse(t)).ToList();
            }
            ViewBag.lstBTS = db.TRAM_BTS.Where(n=> lstBtsId.Contains(n.TRAM_ID)).ToList();
            return View(hd);
        }
        [HttpGet]
        public ActionResult ThanhToan(int HDid)
        {
            if (db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid) == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == HDid) == null)
            {
                ViewBag.sohd = "HĐ chưa được thanh toán !!";
            }
            HOPDONG_CHIPHI hd = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
            ViewBag.hopdong = hd;
            ViewBag.hdid = HDid;
            var tinhtrang = db.TINHTRANG_SD_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
            if (tinhtrang != null)
            {
                ViewBag.tinhtrang = tinhtrang.TINHTRANG_SD;
            }
            else
            {
                ViewBag.tinhtrang = "";
            }
            
            return View(db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == HDid));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThanhToan(THANHTOAN_CHIPHI thanhtoan)
        {
            if (ModelState.IsValid)
            {              
                thanhtoan.USER = User.Identity.Name;
                db.THANHTOAN_CHIPHI.Add(thanhtoan);
                db.SaveChanges();
                return RedirectToAction("ThanhToan", "HDChiPhi", new {@HDid = thanhtoan.HOPDONG_CHIPHI_ID });
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }
        public PartialViewResult lichsuthanhtoan(int HDid)
        {
            return PartialView(db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == HDid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult xacnhanEditThanhToan(int thanhtoanID)
        {
            
            var thanhtoan = db.THANHTOAN_CHIPHI.Find(thanhtoanID);
            if (TryUpdateModel(thanhtoan, "",
               new string[] { "TUNGAY_TT", "DENNGAY_TT", "NGAY_TT", "SOTIEN_TT", "SOUYNHIEMCHI_TT",
                               "CHUTK_TT", "SOTK_TT", "NGANHANG_TT", "GHICHU_TT"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("ThanhToan", "HDChiPhi", new {@HDid = thanhtoan.HOPDONG_CHIPHI_ID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(thanhtoan);
        }
        [HttpPost]
        public ActionResult xacnhanxoaThanhToan(int thanhtoanID)
        {
            THANHTOAN_CHIPHI thanhtoan = db.THANHTOAN_CHIPHI.SingleOrDefault(n => n.ID_TT == thanhtoanID);
            var hdid = thanhtoan.HOPDONG_CHIPHI_ID;
            if (thanhtoan == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.THANHTOAN_CHIPHI.Remove(thanhtoan);
            db.SaveChanges();
            return RedirectToAction("ThanhToan", "HDChiPhi", new {@HDid = hdid });
        }
        [HttpGet]
        public PartialViewResult lichsuTT1(int HDid)
        {
            if (db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == HDid) == null)
            {
                ViewBag.sohd = "HĐ chưa được thanh toán !!";
            }
            ViewBag.hopdong = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
            ViewBag.hdid = HDid;
            return PartialView(db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == HDid).ToList());
        }
        [HttpGet]
        public PartialViewResult xoaTT(int? IDTT)
        {
            ViewBag.idTT = IDTT + "delete";
            return PartialView(db.THANHTOAN_CHIPHI.SingleOrDefault(n => n.ID_TT == IDTT));
        }
        [HttpGet]
        public PartialViewResult editTT(int IDTT)
        {
            var tt = db.THANHTOAN_CHIPHI.SingleOrDefault(n => n.ID_TT == IDTT);
            ViewBag.hopdong = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == tt.HOPDONG_CHIPHI_ID);
            ViewBag.edit = IDTT + "edit";
            return PartialView(db.THANHTOAN_CHIPHI.SingleOrDefault(n => n.ID_TT == IDTT));
        }

        //Print or download file HD CP
        public ActionResult ExportData(int HDid)
        {
            HOPDONG_CHIPHI hd = db.HOPDONG_CHIPHI.Find(HDid);
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
        }

        //get data from DB to convert to file
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

        //Print or download file PLHD CP
        public ActionResult ExportDataPL(int PLid)
        {
            PHULUC_HDCP plhd = db.PHULUC_HDCP.SingleOrDefault(n => n.PHULUC_HDCP_ID == PLid);
            TBL_FILE fileInfo = new TBL_FILE();
            // Model binding.  
            try
            {
                // Loading dile info.  
                fileInfo = this.db.TBL_FILE.Where(n => n.file_id == plhd.FILE_ID).FirstOrDefault();

                // Info.  
                return this.GetFile(fileInfo.file_base6, fileInfo.file_ext, fileInfo.file_name);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return File(fileInfo.file_base6, fileInfo.file_ext);
        }

        public ActionResult Export_TT(int HDid)
        {
            var TT = this.db.THANHTOAN_CHIPHI.Where(n=>n.HOPDONG_CHIPHI_ID == HDid).ToList();
            GridView gv = new GridView();
            gv.DataSource = from P in TT
                            select new
                            {
                                //SO_HOP_DONG = P.HOPDONG_CHIPHI_ID.
                                SO_UY_NHIEM_CHI = P.SOUYNHIEMCHI_TT,
                                NGAY_THANH_TOAN = P.NGAY_TT,
                                SO_TIEN = P.SOTIEN_TT,
                                TU_NGAY = P.TUNGAY_TT,
                                DEN_NGAY = P.DENNGAY_TT,
                                CHU_TAI_KHOAN = P.CHUTK_TT,
                                SO_TAI_KHOAN = P.SOTK_TT,
                                NGAN_HANG = P.NGANHANG_TT,
                                GHI_CHU = P.GHICHU_TT
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DANH_SACH_THANHTOAN_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ThanhToan", "HDChiPhi", new { @HDid = HDid });
        }
        [HttpPost]
        public ActionResult uploadFile(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/HD_CHIPHI/"),
                                               Path.GetFileName(upload.FileName));
                    //string filepath = AppDomain.CurrentDomain.BaseDirectory + "~/Content/HD_CHIPHI" + upload;
                    upload.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    //hopdong.File = upload.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
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

        [HttpGet]
        public PartialViewResult updateFile(int? idhd)
        {            
            return PartialView(db.HOPDONG_CHIPHI.SingleOrDefault(n=>n.HOPDONG_CHIPHI_ID == idhd));
        }
        [HttpPost]//, ActionName("ThongTinCaNhan")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult updatefilepost(int? idhd, HttpPostedFileBase File)
        {
            var HDchiphi = db.HOPDONG_CHIPHI.Find(idhd);

            if (File != null && File.ContentLength > 0)
                try
                {
                    var FileName = Path.GetFileName(File.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/HD_CHIPHI/"), FileName);
                    File.SaveAs(path);
                    HDchiphi.File = File.FileName;
                    db.SaveChanges();
                    return RedirectToAction("EditHDChiPhi", "HDChiPhi", new { @HDCHIPHI_ID = idhd });
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("EditHDChiPhi", "HDChiPhi", new { @HDCHIPHI_ID = idhd });
        }

        public JsonResult getHD_HetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd1 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            var hd2 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT < date).ToList();
            int hd = hd1.Count();
            int hd_hethan = hd2.Count();
            return Json(new { hd, hd_hethan }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult HDHetHan()
        {
            DateTime date = System.DateTime.Now;
            DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd1 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT > date && n.NGAY_KT < date2).ToList();
            return PartialView(hd1);
        }

        public PartialViewResult HDHetHan2()
        {
            DateTime date = System.DateTime.Now;
            var hd2 = db.HOPDONG_CHIPHI.Where(n => n.NGAY_KT < date).ToList();
            return PartialView(hd2);
        }

        public ActionResult xacnhantinhtrangsd(int? page)
        {
            var hd = db.HOPDONG_CHIPHI.OrderByDescending(n => n.HOPDONG_CHIPHI_ID).ToList();
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }

        public PartialViewResult capnhatxacnhan(int? maHD)
        {
            var hd = db.TINHTRANG_SD_CHIPHI.Find(maHD);
            ViewBag.mahd = "update" + maHD;
            ViewBag.hd = maHD;
            return PartialView(hd);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addtinhtrang(TINHTRANG_SD_CHIPHI tinhtrang)
        {
            if (ModelState.IsValid)
            {
                db.TINHTRANG_SD_CHIPHI.Add(tinhtrang);
                db.SaveChanges();
                return RedirectToAction("xacnhantinhtrangsd", "HDChiPhi");
            }
            return RedirectToAction("xacnhantinhtrangsd");
        }

        public PartialViewResult editxacnhan(int? maHD)
        {
            TINHTRANG_SD_CHIPHI hd = db.TINHTRANG_SD_CHIPHI.SingleOrDefault(n=>n.HOPDONG_CHIPHI_ID == maHD);
            ViewBag.edit = "edit" + maHD;
            ViewBag.hd = maHD;
            return PartialView(hd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editxacnhanpost(int? mahd)
        {
            if (mahd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tinhtrang = db.TINHTRANG_SD_CHIPHI.SingleOrDefault(n=>n.HOPDONG_CHIPHI_ID == mahd);
            if (TryUpdateModel(tinhtrang, "",
               new string[] { "HOPDONG_CHIPHI_ID", "TEN_TB", "TINHTRANG_SD", "TINHTRANG", "GHICHU" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("xacnhantinhtrangsd", "HDChiPhi");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(tinhtrang);
        }

        public JsonResult getHD_thanhtoan()
        {
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            danhsach = HD_ToiHanTT();
            int sohd = danhsach.Count();
            return Json(new { sohd }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult hdthanhtoan()
        {
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            danhsach = HD_ToiHanTT();
            return PartialView(danhsach);
        }

        public ActionResult export_hdTT()
        {
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            danhsach = HD_ToiHanTT();
            GridView gv = new GridView();
            var i = 1;
            gv.DataSource = from P in danhsach
                            select new
                            {
                                STT = i++,
                                //SO_HOP_DONG = P.HOPDONG_CHIPHI_ID.
                                SỐ_HD = P.SO_HD,
                                LOẠI_HD = P.LOAI_HD_SUB.TEN_HD_SUB,
                                BEN_THUE = P.TEN_BT,
                                BEN_CHO_THUE = P.TEN_CT,
                                NGAY_HD = P.NGAY_HD.Value.ToString("dd/MM/yyyy"),
                                TU_NGAY = P.NGAY_BD.Value.ToString("dd/MM/yyyy"),
                                DEN_NGAY = P.NGAY_KT.Value.ToString("dd/MM/yyyy"),
                                TONG_TIEN_TT = P.THANHTOAN_CHIPHI.Sum(n => n.SOTIEN_TT),
                                SO_TIEN_CON_LAI = P.TONG_GT - P.THANHTOAN_CHIPHI.Sum(n => n.SOTIEN_TT),
                                SO_LAN_TT = P.THANHTOAN_CHIPHI.Count
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_HD_TOI_HAN_TT" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gv.HeaderRow.Cells[0].Style.Add("background-color", "green");
            //Response.Write("<table style='font-size:20px'><tr><td colspan='12' align='center'><b>Thống kê danh sách hợp đồng</b></td></tr>"
            //    + "<tr><td colspan='12' style='color:red' align='center'><b>Loại hợp đồng: " + tenhd + "</b></td></tr>"
            //    + "</table>");
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            //Response.Write("<table><tr></tr>"
            //    + "<tr><td></td><td colspan='4'><b>(Nguồn dữ liệu: chương trình quản lý hợp đồng. Thời gian lấy dữ liệu: " + System.DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + ")</b></td></tr>"
            //    + "</table>");
            Response.Flush();
            Response.End();
            return RedirectToAction("danhsachthanhtoan", "HDChiPhi");
        }

        public List<HOPDONG_CHIPHI> HD_ToiHanTT()
        {
            DateTime date = System.DateTime.Now;
            //DateTime date2 = System.DateTime.Now.AddMonths(2);
            var hd = db.HOPDONG_CHIPHI.ToList();
            List<HOPDONG_CHIPHI> danhsach = new List<HOPDONG_CHIPHI>();
            DateTime sysdate = System.DateTime.Now;
            for (int i = 0; i < hd.Count(); i++)
            {
                var hdid = hd[i].HOPDONG_CHIPHI_ID;
                var thanhtoan = db.THANHTOAN_CHIPHI.Where(n => n.HOPDONG_CHIPHI_ID == hdid).OrderByDescending(n => n.ID_TT);
                DateTime ngaytt = new DateTime();
                DateTime hantt = new DateTime();
                if (thanhtoan.Count() >= 1)
                {
                    var thanhtoan1 = thanhtoan.First();
                    ngaytt = thanhtoan1.DENNGAY_TT.Value;
                    hantt = ngaytt.AddMonths(hd[i].CHUKY_TT1.THANG.Value);

                }
                else
                {
                        ngaytt = hd[i].NGAY_BD.Value;
                        hantt = ngaytt.AddMonths(hd[i].CHUKY_TT.THANG.Value);
                    }
                if (sysdate > hantt)
                {
                    danhsach.Add(hd[i]);
                }
            }
            return danhsach;
        }

        [HttpGet]
        public ActionResult ThanhLyHD(int HDid)
        {
            var hd = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
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
        public ActionResult ThanhLyHD(THANHLY_HDCHIPHI thanhly)
        {

            //ViewBag.hdid = HDid;
            if (ModelState.IsValid)
            {
                var hd = db.HOPDONG_CHIPHI.Find(thanhly.HOPDONG_CHIPHI_ID);
                if (hd == null)
                {
                    return RedirectToAction("baoloi", "Home");
                }
                if (TryUpdateModel(hd))
                {
                    hd.TRANGTHAI = 2;
                    db.SaveChanges();
                }

                db.THANHLY_HDCHIPHI.Add(thanhly);
                db.SaveChanges();
                return RedirectToAction("Index", "HDChiPhi");
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return RedirectToAction("Index", "HDNhanCong");
        }

        public PartialViewResult DSHDThanhLy()
        {
            var hd = db.HOPDONG_CHIPHI
                .Where(n => db.THANHLY_HDCHIPHI.Where(m => m.HOPDONG_CHIPHI_ID == n.HOPDONG_CHIPHI_ID).Any())
                .ToList().OrderByDescending(n => n.HOPDONG_CHIPHI_ID);

            return PartialView(hd);
        }
        public ActionResult export_thanhly()
        {
            List<THANHLY_HDCHIPHI> thanhly = db.THANHLY_HDCHIPHI.ToList();
            GridView gv = new GridView();
            int i = 1;
            gv.DataSource = from P in thanhly
                            select new
                            {
                                SO_TT = i++,
                                SO_HD = P.HOPDONG_CHIPHI.SO_HD,
                                LOAI_HD = P.HOPDONG_CHIPHI.LOAI_HD_SUB.TEN_HD_SUB,
                                SO_BIENBAN_THANHLY = P.SOBIENBAN_TL,
                                NGAY_LAP = P.NGAYLAP_BBTL,
                                NGUOILAP = P.NGUOILAP_BBTL,
                                NGAY_HIEU_LUC = P.NGAY_HIEULUC_TL,
                                NOI_DUNG_TL = P.NOIDUNG_TL,
                                GHI_CHU = P.GHICHU
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_THANHLY_CHIPHI_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "HDChiPhi");
        }

        [HttpPost]
        public ActionResult HuyThanhLy(int? HDid)
        {
            var hd = db.HOPDONG_CHIPHI.Find(HDid);
            if (hd == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(hd))
            {
                hd.TRANGTHAI = 0;
                db.SaveChanges();
            }

            THANHLY_HDCHIPHI thanhly = db.THANHLY_HDCHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
            db.THANHLY_HDCHIPHI.Remove(thanhly);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult dsphuluc(int HDid)
        {
            var ds = db.PHULUC_HDCP.Where(n => n.HOPDONG_CHIPHI_ID == HDid).ToList();
            ViewBag.thongbao = "";
            if (ds.Count == 0)
            {
                ViewBag.thongbao = "HĐ chưa có phụ lục";
            }            
            ViewBag.hdid = HDid;
            return PartialView(ds);
        }

        public async Task<int> create_hd_original(HOPDONG_CHIPHI hd_original)
        {
            PHULUC_HDCP phuluc_original = new PHULUC_HDCP();
            phuluc_original.HOPDONG_CHIPHI_ID = hd_original.HOPDONG_CHIPHI_ID;
            phuluc_original.SO_PHULUC = "000-HĐ gốc";
            phuluc_original.TEN_CT = hd_original.TEN_CT;
            phuluc_original.NGAY_KY_PL = hd_original.NGAY_HD;
            phuluc_original.THANG = hd_original.THANG;
            phuluc_original.THANG_GT = hd_original.THANG_GT;
            phuluc_original.VAT = hd_original.VAT;
            phuluc_original.CHUKY_TT = hd_original.CHUKY_TT;
            phuluc_original.CHUKY_TT1 = hd_original.CHUKY_TT1;
            phuluc_original.NGAY_BD_PL = hd_original.NGAY_BD;
            phuluc_original.NGAY_KT_PL = hd_original.NGAY_KT;
            phuluc_original.SOCHUKY = hd_original.SO_CHUKY;
            phuluc_original.TONG_GT = hd_original.TONG_GT;
            phuluc_original.GHICHU = hd_original.GHICHU;
            db.PHULUC_HDCP.Add(phuluc_original);
            return await db.SaveChangesAsync();
        }

        public async Task<TBL_FILE> update_upload(HttpPostedFileBase upload)
        {
            //add new file
            string fileContent = string.Empty;
            string fileContentType = string.Empty;
            TBL_FILE newFile = new TBL_FILE();
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
                newFile.file_id = 0;
                newFile.file_name = upload.FileName;
                newFile.file_ext = fileContentType;
                newFile.file_base6 = fileContent;
                this.db.TBL_FILE.Add(newFile);
                //end test luu file to DB
                await db.SaveChangesAsync();
            }
            return newFile;
        }

        //update PL HD=> chua code
        [HttpGet]
        public ActionResult addPhuLucHD(int HDid)
        {
            HOPDONG_CHIPHI hd = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.HOPDONG_CHIPHI_ID == HDid);
            ViewBag.CHUKYTT = new SelectList(db.CHUKY_TT.ToList().OrderBy(n => n.CHUKY_ID), "CHUKY_ID", "CHUKY");
            ViewBag.hdid = HDid;
            ViewBag.hd = hd;
            return View(hd);
        }
        //Add new PL HĐ
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> addPhuLucHD(PHULUC_HDCP phuluc, HttpPostedFileBase upload)
        {
            
            if (ModelState.IsValid)
            {
                //Create new PLHD to save original HĐ
                var hd_original = db.HOPDONG_CHIPHI.Find(phuluc.HOPDONG_CHIPHI_ID);
                if (hd_original.PHULUC_HDCP.Count <= 0)
                {
                    await create_hd_original(hd_original);
                }

                //Update ngay het han cua HD theo PL HD
                var hdObject = db.HOPDONG_CHIPHI.Find(phuluc.HOPDONG_CHIPHI_ID);
                if (phuluc.NGAY_BD_PL != null)
                {
                    hdObject.NGAY_BD = phuluc.NGAY_BD_PL;
                    TryUpdateModel(hdObject, "", new string[] { "NGAY_BD" });
                }
                if (phuluc.NGAY_KT_PL != null)
                {
                    hdObject.NGAY_KT = phuluc.NGAY_KT_PL;
                    TryUpdateModel(hdObject, "", new string[] { "NGAY_KT" });
                }
                if (!String.IsNullOrEmpty(phuluc.TEN_CT))
                {
                    hdObject.TEN_CT = phuluc.TEN_CT;
                    TryUpdateModel(hdObject, "", new string[] { "TEN_CT" });
                }
                if (phuluc.THANG > 0)
                {
                    hdObject.THANG = phuluc.THANG;
                    TryUpdateModel(hdObject, "", new string[] { "THANG" });
                }

                if (upload != null && upload.ContentLength > 0)
                {
                    TBL_FILE result_upload = await this.update_upload(upload);
                    if (result_upload != null)
                    {
                        //If co file, them file_id vao PLHD
                        phuluc.FILE_ID = result_upload.file_id;
                        phuluc.FILE = result_upload.file_name;
                    }
                }
                //them moi PL HD
                db.PHULUC_HDCP.Add(phuluc);
                await db.SaveChangesAsync();
                return RedirectToAction("ShowHDChiPhi", "HDChiPhi", new { @maHD = phuluc.HOPDONG_CHIPHI_ID });
            }
            else ViewBag.baoloi = "Lưu không thành công!";
            return View();
        }

        [HttpGet]
        public PartialViewResult xoaPL(int PLid)
        {
            PHULUC_HDCP ph = db.PHULUC_HDCP.SingleOrDefault(n => n.PHULUC_HDCP_ID == PLid);
            ViewBag.delete = PLid + "delete";
            return PartialView(ph);
        }
        [HttpPost]
        public ActionResult xacnhanxoaPL(int PLid)
        {
            PHULUC_HDCP ph = db.PHULUC_HDCP.SingleOrDefault(n => n.PHULUC_HDCP_ID == PLid);
            if (ph == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            var mahd = ph.HOPDONG_CHIPHI_ID;
            db.PHULUC_HDCP.Remove(ph);
            db.SaveChanges();
            return RedirectToAction("ShowHDChiPhi", "HDChiPhi", new { @maHD = mahd });
        }

        [HttpGet]
        public ActionResult showPLHD(int PLid)
        {
            PHULUC_HDCP pl = db.PHULUC_HDCP.SingleOrDefault(n => n.PHULUC_HDCP_ID == PLid);
            if (pl == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            return PartialView(pl);
        }
    }
}
