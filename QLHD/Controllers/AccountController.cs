using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLHD.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Net;
namespace QLHD.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        QLHD2Entities db = new QLHD2Entities();
        int maTK;
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            db.Database.Connection.Open();
            //String sTK = f["txtTaiKhoan"].ToString();
            //String sMK = f["txtMatKhau"].ToString();
            //String mkmd5 = changeMD5(sMK);
            String sTK = "tranglh.agg";
            String mkmd5 = "b346e4fff0b0403daaa83aee71f84390";
            THANHVIEN tv = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == sTK && n.MATKHAU == mkmd5);
            LICHSU_DANGNHAP lichsu = new LICHSU_DANGNHAP();
            if (tv != null)
            {
                FormsAuthentication.SetAuthCookie(tv.TENTRUYCAP, false);
                Session["TaiKhoan"] = tv;
                maTK = tv.ID_THANHVIEN;

                lichsu.TENTRUYCAP = tv.TENTRUYCAP;
                lichsu.TRANGTHAI = "Đăng nhập";
                lichsu.DATE = System.DateTime.Now;

                db.LICHSU_DANGNHAP.Add(lichsu);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu sai!";
            return View();
        }
        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }
        public static String changeMD5(String mk)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(mk));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }
            return sbHash.ToString();  
        }
        public ActionResult resetMK(int maTK)
        {
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == maTK);
            if (user == null) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user.MATKHAU = changeMD5("123456");
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            String xacnhan = "Reset mật khẩu thành công !!";
            return RedirectToAction("ThanhVien", "HeThong", new {thongbao = xacnhan });
        }
        [HttpGet]
        public PartialViewResult getResetMK(int maTK)
        {
            ViewBag.idTV = maTK + "reset";
            return PartialView(db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == maTK));
        }
        [HttpGet]
        public PartialViewResult deleteTV(int? maTK)
        {
            ViewBag.xoaTV = maTK + "delete";
            return PartialView(db.THANHVIENs.SingleOrDefault(n => n.ID_THANHVIEN == maTK));
        }
    }
}
