using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;

namespace QLHD.Controllers
{
    public class HeThongController : Controller
    {
        //
        // GET: /HeThong/
        QLHD2Entities db = new QLHD2Entities();

        public ActionResult BenThue()
        {
            var benthue = db.BENTHUE_TAM.ToList();
            return View(benthue);
        }
        public ActionResult HeThong()
        {
            return View(db.HETHONGs.ToList());
        }
        public ActionResult ChuKyThanhToan()
        {
            var chuky = db.CHUKY_TT.ToList();
            return View(chuky);
        }
        public ActionResult ThanhVien(string thongbao)
        {
            var thanhvien = db.THANHVIENs.ToList();
            ViewBag.thongbao = thongbao;
            return View(thanhvien);
        }
        public ActionResult LoaiHD(int? page)
        {
            var loaihd = db.LOAI_HD_SUB.ToList();
            //var hd = this.db.HOPDONG_CHIPHI.ToList();
            
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            //ViewBag.page = page;
            return View(loaihd.ToPagedList(pagenumber, pagesize));         
        }
        //public PartialViewResult menuLoaiHD()
        //{
        //    var loai = db.LOAIHDs.ToList();
        //    return PartialView(loai);
        //}
        //public PartialViewResult HDPartial(int loaiHD = 100)
        //{
        //    if (loaiHD == 100)
        //    {
        //        return PartialView(db.LOAI_HD_SUB.OrderBy(n=>n.IDLOAI_HD).ToList());
        //    }
        //    LOAIHD loai = db.LOAIHDs.SingleOrDefault(n=>n.IDLOAI_HD == loaiHD);
        //    if (loai == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    List<LOAI_HD_SUB> hd = db.LOAI_HD_SUB.Where(n=>n.IDLOAI_HD == loaiHD).OrderBy(n=>n.ID_LOAIHD_SUB).ToList();
        //    if (hd.Count == 0)
        //    {
        //        ViewBag.thongbao = "Không có hợp đồng nào thuộc loại này!!!";
        //    }
        //    return PartialView(hd);
        //}
        public ActionResult HTTT()
        {
            var ht = db.HTTTs.ToList();
            return View(ht);
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost, ActionName("DoiMatKhau")]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhauPost(FormCollection f)
        {
            String id = User.Identity.Name;
            String mkcu = f["mkcu"].ToString();
            String mkmoi = f["mkmoi"].ToString();
            String mkxacthuc = f["MATKHAU"].ToString();
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == id);
            String mkcu1 = QLHD.Controllers.AccountController.changeMD5(mkcu);
            String mkmoi1 = QLHD.Controllers.AccountController.changeMD5(mkmoi);
            String mkxacthuc1 = QLHD.Controllers.AccountController.changeMD5(mkxacthuc);
            if (mkcu1 != user.MATKHAU)
            {
                ViewBag.baoloi1 = "Mật khẩu cũ không chính xác! Vui lòng nhập lại!";
                return View();
            }
            if (mkmoi1 != mkxacthuc1)
            {
                ViewBag.baoloi1 = "Mật khẩu mới và xác thực mật khẩu không trùng khớp! Vui lòng nhập lại!";
                return View();
            }
            user.MATKHAU = mkmoi1;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public ActionResult ThongTinCaNhan()
        {
            String id = User.Identity.Name;
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == id);
            return View(user);
        }
        [HttpPost]//, ActionName("ThongTinCaNhan")]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditUserPost(HttpPostedFileBase IMG)
        {
            String id = User.Identity.Name;
            var id1 = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == id);
            var user = db.THANHVIENs.Find(id1.ID_THANHVIEN);

            if (IMG != null && IMG.ContentLength > 0)
                try
                {
                    var fileName = Path.GetFileName(IMG.FileName);                
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    IMG.SaveAs(path);
                    user.IMG = IMG.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (TryUpdateModel(user, "",
               new string[] {"TEN_THANHVIEN", "CHUCVU", "NGAYSINH", "DIACHI", "TRINHDO", "TONGIAO", "DANTOC", "DIENTHOAI",
                               "EMAIL","CMND", "NOICAP", "NGAYCAP"}))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("ThongTinCaNhan", "HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult deleteChuKyTT(int chuky)
        {
            CHUKY_TT CK = db.CHUKY_TT.SingleOrDefault(n => n.CHUKY_ID == chuky);
            if (CK == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            HOPDONG_CHIPHI hdcp = db.HOPDONG_CHIPHI.SingleOrDefault(n => n.CHUKY_ID == chuky);
            HOPDONG_NHANCONG hdnc = db.HOPDONG_NHANCONG.SingleOrDefault(n => n.CHUKY_ID == chuky);
            HOPDONG_DOANHTHU hddt = db.HOPDONG_DOANHTHU.SingleOrDefault(n => n.CHUKY_ID == chuky);
            if (hdcp != null || hdnc != null || hddt != null)
            {
                ViewBag.canhbao = "Chu kỳ này hiện đang áp dụng cho 1 số hợp đồng, không thể xóa!!!";
            }
            return View(CK);
        }
        [HttpPost, ActionName("deleteChuKyTT")]
        public ActionResult xacnhanxoaChuKy(int chuky)
        {
            CHUKY_TT CK = db.CHUKY_TT.SingleOrDefault(n => n.CHUKY_ID == chuky);
            if (CK == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.CHUKY_TT.Remove(CK);
            db.SaveChanges();
            return RedirectToAction("ChuKy", "HeThong");
        }
        [HttpPost]
        public ActionResult xacnhanxoaThanhVien(int idTV)
        {
            THANHVIEN thanhvien = db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == idTV);
            if (thanhvien == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.THANHVIENs.Remove(thanhvien);
            db.SaveChanges();
            return RedirectToAction("ThanhVien", "HeThong");
        }
        [HttpGet]
        public PartialViewResult editUser(int thanhvien)
        {
            THANHVIEN tv = db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == thanhvien);
            return PartialView(tv);
        }
        [HttpPost, ActionName("editUser")]
        [ValidateAntiForgeryToken]
        public ActionResult editUserPost2(int thanhvien)
        {
            return RedirectToAction("ThanhVien", "HeThong");
        }

        [HttpGet]
        public PartialViewResult PartialHTTT()
        {
            //ViewBag.httt = db.HTTTs.SingleOrDefault(n => n.HTTT_ID == htttID);
            //ViewBag.htID = htttID;
            return PartialView(db.HTTTs.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editHTTT(int htttID)
        {
            var httt = db.HTTTs.SingleOrDefault(n => n.HTTT_ID == htttID);
            if (httt == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            if (TryUpdateModel(httt, "",
               new string[] {"TEN_HTTT", "GHICHU"}))
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
            return View(httt);
        }
        [HttpPost]
        public ActionResult xacnhanxoaHTTT(int? htttID)
        {
            HTTT ht = db.HTTTs.SingleOrDefault(n => n.HTTT_ID == htttID);
            if (ht == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            //var chiphi = db.HOPDONG_CHIPHI.Where(n => n.HTTT_ID == htttID);
            //var doanhthu = db.HOPDONG_DOANHTHU.Where(n => n.HTTT_ID == htttID);
            //var nhancong = db.HOPDONG_NHANCONG.Where(n => n.HTTT_ID == htttID);
            //if (chiphi != null || doanhthu != null || nhancong != null)
            //{
            //    ViewBag.thongbaoxoa = "Xóa không thành công!! vì HTTT có sử dụng cho HĐ!";
            //    return RedirectToAction("HTTT", "HeThong");
            //}
            db.HTTTs.Remove(ht);
            db.SaveChanges();
            ViewBag.thongbaoxoa = "Xóa HTTT &ldquo;" + ht.TEN_HTTT + "&rdquo; thành công!!!";
            return RedirectToAction("HTTT", "HeThong");
        }
        [HttpGet]
        public PartialViewResult xoaHTTT(int? htttID)
        {
            var httt = db.HTTTs.SingleOrDefault(n => n.HTTT_ID == htttID);
            ViewBag.thongbaoxoa1 = "Bạn có chắc muốn xóa lịch sử thanh toán này không? (" + httt.TEN_HTTT+")";
            ViewBag.id = htttID + "a";
            return PartialView(db.HTTTs.SingleOrDefault(n => n.HTTT_ID == htttID));
        }
        [HttpGet]
        public PartialViewResult xoaBenThue(int? ID)
        {
            ViewBag.id = ID + "delete";
            return PartialView(db.BENTHUE_TAM.SingleOrDefault(n => n.BENTHUE_TAM_ID == ID));
        }
        [HttpPost]
        public ActionResult xacnhanxoaBenThue(int? ID)
        {
            BENTHUE_TAM bt = db.BENTHUE_TAM.SingleOrDefault(n => n.BENTHUE_TAM_ID == ID);
            if (bt == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.BENTHUE_TAM.Remove(bt);
            db.SaveChanges();
            ViewBag.thongbaoxoa = "Xóa HTTT &ldquo;" + bt.TEN + "&rdquo; thành công!!!";
            return RedirectToAction("BenThue", "HeThong");
        }

        //In danh mục hợp đồng
        public ActionResult Expor_LoaiHD()
        {
            var hd = this.db.LOAI_HD_SUB.ToList();
            GridView gv = new GridView();
            gv.DataSource = from p in hd
                            select new
                            {
                                ID = p.ID_LOAIHD_SUB,
                                TEN_HD = p.TEN_HD_SUB,
                                GHI_CHU = p.GHICHU
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DanhMucHD.xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");
        }

        public void luulichsuchinhsua(int loaihd, int idhd, int taikhoan)
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
        public ActionResult lichsudangnhap(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            var tentruycap = User.Identity.Name;
            var nguoidung = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == tentruycap);
            ViewBag.tennd = nguoidung.TEN_THANHVIEN;
            var ls = db.LICHSU_DANGNHAP.Where(n => n.TENTRUYCAP == tentruycap).OrderByDescending(n => n.LICHSU_ID).ToList();
            return View(ls.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult PhanQuyen()
        {
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);
            if (user.TENTRUYCAP != "admin")
            {
                return RedirectToAction("baoloi", "Index");
            }
            var thanhvien = db.THANHVIENs.ToList();
            //ViewBag.idthanhvien = thanhvien.ID_THANHVIEN + "ID";
            ViewBag.user = new SelectList(db.THANHVIENs.ToList().OrderBy(n => n.ID_THANHVIEN), "ID_THANHVIEN", "TEN_THANHVIEN");
            return View(thanhvien);
        }
        public JsonResult DanhSachUsers()
        {
            var users = db.THANHVIENs.ToList();
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getquyen_chuagan(int idtv)
        {
            var quyendagan = db.DS_QUYEN_MENU.Select(n => n.USER_ID == idtv).ToList();
            
            //var quyenchuagan = db.MENU_CON.Select(n=>n.ID_MENU_CON != quyendagan.Contains(n.ID_MENU_CON));
            return Json(quyendagan, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult menu_cha()
        {
            //String id = User.Identity.Name;
            //int user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == id).ID_THANHVIEN;
            //List<DS_QUYEN_MENU> ds_quyen = db.DS_QUYEN_MENU.Where(n => n.USER_ID == user).ToList();
            //var menu_cha = (from p in db.MENU_CHA
            //                join f in ds_quyen
            //                on p.ID_MENU_CHA equals f.ID_MENU_CHA
            //                select new {
            //                    ID_MENU_CHA = p.ID_MENU_CHA,
            //                    TEN_MENU_CHA = p.TEN_MENU_CHA,
            //                    LINK = p.LINK,
            //                    ICON = p.ICON
            //                });
            var menu_cha = db.MENU_CHA.ToList();
            return PartialView(menu_cha);
        }
        public PartialViewResult menu_con(int id_menucha)
        {
            var menucon = db.MENU_CON.Where(n => n.ID_MENU_CHA == id_menucha).ToList();
            return PartialView(menucon);
        }
        public ActionResult AddMenu(int? id_menu)
        {
            if (id_menu == null)
            {
                id_menu = 0;
                dsmenucon(id_menu);
            }            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addMenuCha(MENU_CHA menucha)
        {
            if (ModelState.IsValid)
            {
                db.MENU_CHA.Add(menucha);
                db.SaveChanges();
                return RedirectToAction("AddMenu", "HeThong");
            }
            return View();
        }
        public PartialViewResult addmenucon()
        {
            ViewBag.nhommenu = new SelectList(db.MENU_CHA.ToList().OrderBy(n => n.ID_MENU_CHA), "ID_MENU_CHA", "TEN_MENU_CHA");
            return PartialView();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addMenuCon(MENU_CON menucon)
        {
            if (ModelState.IsValid)
            {
                db.MENU_CON.Add(menucon);
                db.SaveChanges();
                return RedirectToAction("AddMenu", "HeThong");
            }
            return View();
        }

        public PartialViewResult dsmenucon(int? id_menu)
        {
            var menucon = db.MENU_CON.ToList();
            if (id_menu == 0 || id_menu == null)
            {
                menucon = db.MENU_CON.ToList();
            }
            else
            {
                menucon = db.MENU_CON.Where(n => n.ID_MENU_CHA == id_menu).ToList();
            }
            return PartialView(menucon);
        }
        [HttpGet]
        public PartialViewResult EditMenuCon(int? IDMenu)
        {
            MENU_CON menu = db.MENU_CON.SingleOrDefault(n => n.ID_MENU_CON == IDMenu);
            ViewBag.nhommenu = new SelectList(db.MENU_CHA.ToList().OrderBy(n => n.ID_MENU_CHA), "ID_MENU_CHA", "TEN_MENU_CHA");
            ViewBag.idmenu = "edit" + IDMenu;
            return PartialView(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenuConPost(int? idmenu)
        {
            if (idmenu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var chuky = db.MENU_CON.Find(idmenu);
            if (TryUpdateModel(chuky, "",
               new string[] { "ID_MENU_CHA", "TEN_MENU", "LINK" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("AddMenu", "HeThong");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(chuky);
        }
        [HttpGet]
        public PartialViewResult XoaMenuCon(int? IDMenu)
        {
            MENU_CON menu = db.MENU_CON.SingleOrDefault(n => n.ID_MENU_CON == IDMenu);
            ViewBag.idmenu = "xoa" + IDMenu;
            return PartialView(menu);
        }
        [HttpPost]
        public ActionResult XoaMenuConPost(int? idmenu)
        {
            MENU_CON menu = db.MENU_CON.SingleOrDefault(n => n.ID_MENU_CON == idmenu);
            if (menu == null)
            {
                return RedirectToAction("baoloi", "Home");
            }
            db.MENU_CON.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("AddMenu", "HeThong");
        }

        public PartialViewResult dsmenu_cha()
        {
            var menucha = db.MENU_CHA.ToList();
            return PartialView(menucha);
        }

        //Manage DM tram
        public ActionResult DMTram(int? page)
        {
            var dsTram = db.TRAM_BTS.Where(s=>s.TRANG_THAI==1).ToList();
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(dsTram.ToPagedList(pagenumber, pagesize));
        }

        //Manage DM van ban
        public ActionResult DMVanBan(int? page)
        {
            var dsTram = db.DM_VANBAN.ToList();
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            return View(dsTram.ToPagedList(pagenumber, pagesize));
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

        //Print or download file Van Ban
        public ActionResult ExportDataVB(int VBid)
        {
            DM_VANBAN hd = db.DM_VANBAN.Find(VBid);
            TBL_FILE fileInfo = new TBL_FILE();
            // Model binding.  
            try
            {
                // Loading dile info.  
                fileInfo = this.db.TBL_FILE.Where(n=>n.file_id == hd.FILE_ID).FirstOrDefault();

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
    }
}
