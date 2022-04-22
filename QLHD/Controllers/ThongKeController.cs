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
    public class ThongKeController : Controller
    {
        //
        public int checkRole(THANHVIEN nv)
        {
            //Get current User       
            if (nv.HETHONG.TENHETHONG == "LD-TTVT")
            {
                return 1;
            }
            return 0;
        }
        // GET: /ThongKe/
        QLHD2Entities db = new QLHD2Entities();
        public ActionResult ThongKe_HDChiPhi(int? page)
        {
            var hd = db.HOPDONG_CHIPHI.ToList();
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            //List<NAM_HD> namhd = new List<NAM_HD>();
            //namhd = db.NAM_HD.ToList();
            //namhd.Add(new NAM_HD { NAM_HD_ID = 0, NAM = "Tất cả" });
            //ViewBag.namhd = new SelectList(namhd, "NAM_HD_ID", "NAM",0);
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            ViewBag.namHD1 = 0;
            ViewBag.loaiHD1 = 0;
            ViewBag.donvi1 = 0;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult thongke_hopdongtoihan(FormCollection f, int? page)
        {
            int loaihd = Int32.Parse(f["loaiHD"].ToString());
            DateTime thang = System.DateTime.Now;
            List<HOPDONG_CHIPHI> listKQTK = db.HOPDONG_CHIPHI.Where(n => n.NGAY_BD < thang && n.ID_LOAIHD_SUB == loaihd).ToList();
            return View(listKQTK);
        }
        public ActionResult ketquathongke(FormCollection f, int? page)
        {
            int namHD = Int32.Parse(f["namHD"].ToString());
            int loaiHD = Int32.Parse(f["loaiHD"].ToString());
            int donvi = Int32.Parse(f["donvi"].ToString());
            List<HOPDONG_CHIPHI> listKQTK = db.HOPDONG_CHIPHI.ToList();
            if (namHD != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            ViewBag.namHD1 = namHD;
            ViewBag.loaiHD1 = loaiHD;
            ViewBag.donvi1 = donvi;
            //var tongtien = listKQTK.Sum(n => n.TONG_GT);
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ThongKe_HDDoanhThu(int? page)
        {
            var hd = db.HOPDONG_DOANHTHU.ToList();
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            ViewBag.namHD1 = 0;
            ViewBag.loaiHD1 = 0;
            ViewBag.donvi1 = 0;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }

        List<int> lstNam = new List<int>() { };
        public void KhoiTaoNam()
        {
            for (int i = 2015; i < 2025; i++)
            {
                lstNam.Add(i);
            }
        }
        
        public ActionResult ThongKe_HDCNTT(int? page)
        {
            KhoiTaoNam();
            int nam = 2022;
            int loai = 1;
            ViewBag.namHD = new SelectList(lstNam, "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            //ViewBag.DM_HH = new SelectList(db.DM_NHOM_HANGHOA.ToList().OrderBy(n => n.NHOM_HANGHOA_ID), "NHOM_HANGHOA_ID", "TEN_NHOM_HANGHOA");
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            ViewBag.namHD1 = 0;
            ViewBag.loaiHD1 = 0;
            var lstTK = db.proc_thongke_dt_thang(2022,1).ToList();
           
            if (lstTK == null)
            {
                ViewBag.thongbao = "Hiện chưa có doanh thu nào trong CSDL!!";
            }
            
            return View(lstTK.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult ketquathongke_HDDT(FormCollection f, int? page)
        {
            int namHD = Int32.Parse(f["namHD"].ToString());
            int loaiHD = Int32.Parse(f["loaiHD"].ToString());
            int donvi = Int32.Parse(f["donvi"].ToString());
            List<HOPDONG_DOANHTHU> listKQTK = db.HOPDONG_DOANHTHU.ToList();
            if (namHD != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            ViewBag.namHD1 = namHD;
            ViewBag.loaiHD1 = loaiHD;
            ViewBag.donvi1 = donvi;
            //var tongtien = listKQTK.Sum(n => n.TONG_GT);
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThongKe_HDNhanCong(int? page)
        {
            var hd = db.HOPDONG_NHANCONG.ToList();
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pagenumber = (page ?? 1);
            int pagesize = 10;
            ViewBag.namHD1 = 0;
            ViewBag.loaiHD1 = 0;
            ViewBag.donvi1 = 0;
            return View(hd.ToPagedList(pagenumber, pagesize));
        }
        public ActionResult ketquathongke_HDNC(FormCollection f, int? page)
        {
            int namHD = Int32.Parse(f["namHD"].ToString());
            int loaiHD = Int32.Parse(f["loaiHD"].ToString());
            int donvi = Int32.Parse(f["donvi"].ToString());
            List<HOPDONG_NHANCONG> listKQTK = db.HOPDONG_NHANCONG.ToList();
            if (namHD != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            
            if (loaiHD != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                listKQTK = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            ViewBag.namHD1 = namHD;
            ViewBag.loaiHD1 = loaiHD;
            ViewBag.donvi1 = donvi;
            //var tongtien = listKQTK.Sum(n => n.TONG_GT);
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(listKQTK.OrderBy(n => n.SO_HD).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult export_hdcp(int? namHD, int? loaiHD, int? donvi)
        {
            List<HOPDONG_CHIPHI> hopdong = db.HOPDONG_CHIPHI.ToList();
            if (namHD != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_CHIPHI.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            if (namHD == 0 && loaiHD == 0 && donvi == 0)
            {
                hopdong = db.HOPDONG_CHIPHI.ToList();
            }
            
            var tenhd = "";
            if (loaiHD != 0)
            {
                var a = db.LOAI_HD_SUB.Where(n => n.ID_LOAIHD_SUB == loaiHD).First();
                tenhd = a.TEN_HD_SUB;
            }
            else tenhd = "Tất cả";
            var i = 1;
            GridView gv = new GridView();
            gv.DataSource = from P in hopdong
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
                                //DA_TT_DEN = P.THANHTOAN_CHIPHI.Select(n=>n.DENNGAY_TT).First(),
                                TONG_TIEN_TT = P.THANHTOAN_CHIPHI.Sum(n=>n.SOTIEN_TT),
                                SO_TIEN_CON_LAI = P.TONG_GT - P.THANHTOAN_CHIPHI.Sum(n=>n.SOTIEN_TT),
                                THANG_GT = P.THANG,
                                TONG_GT = P.TONG_GT,
                                CHU_KY = P.CHUKY_TT.CHUKY,
                                HTTT = P.HTTT.TEN_HTTT,                                
                                GHI_CHU = P.GHICHU,
                                LAN_TT = P.THANHTOAN_CHIPHI.Count
                            };
            
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_HD_CHI_PHI_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gv.HeaderRow.Cells[0].Style.Add("background-color", "green");
            Response.Write("<table style='font-size:20px'><tr><td colspan='12' align='center'><b>Thống kê danh sách hợp đồng</b></td></tr>"
                + "<tr><td colspan='12' style='color:red' align='center'><b>Loại hợp đồng: " + tenhd + "</b></td></tr>"
                + "</table>");
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Write("<table><tr></tr>"
                + "<tr><td></td><td colspan='4'><b>(Nguồn dữ liệu: chương trình quản lý hợp đồng. Thời gian lấy dữ liệu: " + System.DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy") + ")</b></td></tr>"
                +"</table>");
            Response.Flush();
            Response.End();
            return RedirectToAction("ThongKe", "ketquathongke", new { @HD = hopdong });
        }


        public ActionResult export_hdnc(int? namHD, int? loaiHD, int? donvi)
        {
            List<HOPDONG_NHANCONG> hopdong = db.HOPDONG_NHANCONG.ToList();
            if (namHD != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_NHANCONG.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            if (namHD == 0 && loaiHD == 0 && donvi == 0)
            {
                hopdong = db.HOPDONG_NHANCONG.ToList();
            }
            
            GridView gv = new GridView();
            int i = 1;
            gv.DataSource = from P in hopdong
                            select new
                            {                 
                               SO_TT = i++,
                               P.LOAI_HD_SUB.TEN_HD_SUB,
                               P.NAM_HD,
                               P.SO_HD,
                               P.NGAY_HD,
                               P.TEN_CT,
                               P.NGAYSINH_CT,
                               P.DIACHI_CT,
                               P.NOIO_CT,
                               P.DIENTHOAI_CT,
                               P.CMND_CT,
                               P.NGAYCAP_CT,
                               P.NOICAP_CT,
                               P.NOILAMVIEC_CT,
                               P.MSTHUE_CT,
                               P.SLTRAM_KYTHUAT,
                               P.TENTRAM_KYTHUAT,
                               P.SLTRAM_TAISAN,
                               P.TENTRAM_TAISAN,
                               P.SLTRAM_MPD,
                               P.TENTRAM_MPD,
                               P.SLTRAM_BAOVE,
                               P.TENTRAM_BAOVE,
                               P.THULAO,
                               P.SOTIEN_BAOHIEM,
                               P.SO_SOBAOHIEM,
                               P.VTT_KHONGHOTRO,
                               P.TONG_GT,
                               P.HTTT.TEN_HTTT,
                               P.NGAY_BD,
                               P.NGAY_KT                                
                            };
            //gv.Columns[1].FooterText = hopdong.AsEnumerable().Select(x => x.TONG_GT).Sum().ToString(); 
           
            gv.DataBind();
            var tongtien = hopdong.Sum(n => n.TONG_GT).ToString();
            gv.FooterRow.Cells[1].Text = "Tổng tiền:";
            gv.FooterRow.Cells[6].Text = tongtien;

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_HD_NHAN_CONG_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ThongKe", "ketquathongke_HDNC", new { @HD = hopdong });
        }
        public ActionResult export_hddt(int? namHD, int? loaiHD, int? donvi)
        {
            List<HOPDONG_DOANHTHU> hopdong = db.HOPDONG_DOANHTHU.ToList();
            if (namHD != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.DONVI_ID == donvi && n.ID_LOAIHD_SUB == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.Where(n => n.NAM_HD_ID == namHD && n.ID_LOAIHD_SUB == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            if (namHD == 0 && loaiHD == 0 && donvi == 0)
            {
                hopdong = db.HOPDONG_DOANHTHU.ToList();
            }
            GridView gv = new GridView();
            gv.DataSource = from P in hopdong
                            select new
                            {
                                //SO_HOP_DONG = P.HOPDONG_DOANHTHU_ID.
                                SO_HD = P.SO_HD,
                                LOAI_HD = P.LOAI_HD_SUB.TEN_HD_SUB,
                                NGAY_HD = P.NGAY_HD,
                                BEN_THUE = P.TEN_BT,
                                BEN_CHO_THUE = P.TEN_CT,
                                TONG_GT = P.TONG_GT,
                                CHU_KY = P.CHUKY_TT.CHUKY,
                                HTTT = P.HTTT.TEN_HTTT
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_HD_DOANH_THUS_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ThongKe", "ketquathongke_HDDT", new { @HD = hopdong });
        }

        public ActionResult thongke_chiphi_HDNC(int? page)
        {
            var hd = db.HOPDONG_NHANCONG.ToList().OrderByDescending(n=>n.HOPDONG_NHANCONG_ID);
            if (hd == null)
            {
                ViewBag.thongbao = "Hiện chưa có hợp đồng nào trong CSDL!!";
            }
            ViewBag.namHD = new SelectList(db.NAM_HD.ToList().OrderByDescending(n => n.NAM_HD_ID), "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.LOAI_HD_SUB.ToList().OrderBy(n => n.ID_LOAIHD_SUB), "ID_LOAIHD_SUB", "TEN_HD_SUB");
            ViewBag.donvi = new SelectList(db.DONVIs.ToList().OrderBy(n => n.DONVI_ID), "DONVI_ID", "TEN_DV");
            int pagenumber = (page ?? 1);
            int pagesize = 30;
            ViewBag.tonggiatri = db.HOPDONG_NHANCONG.Sum(n=>n.TONG_GT);
            return View(hd.ToPagedList(pagenumber, pagesize));
        }

        public ActionResult tk_hdcp()
        {
            GridView gv = new GridView();
            THANHVIEN user = db.THANHVIENs.SingleOrDefault(n => n.TENTRUYCAP == User.Identity.Name);

            var plhd = db.PHULUC_HDCP.Select(m => new
            {
                SO_HD = m.HOPDONG_CHIPHI.SO_HD + "[" + m.SO_PHULUC + "]",
                //LOAI_HD = m.HOPDONG_CHIPHI.LOAI_HD_SUB.TEN_HD_SUB,
                LOAI_HD = "Phụ Lục HĐ",
                TEN_TRAM = m.HOPDONG_CHIPHI.TRAM_DT,
                TEN_BT = m.HOPDONG_CHIPHI.TEN_BT,
                TEN_CT = m.TEN_CT,
                DIACHI = m.HOPDONG_CHIPHI.DIACHI_CT,
                NGAY_KY = m.NGAY_KY_PL,
                NGAY_BD = m.NGAY_BD_PL,
                NGAY_KT = m.NGAY_KT_PL,
                CHI_PHI = m.THANG,
                THUE_VAT = m.VAT,
                TONG_THANG = m.THANG_GT,
                CHUKY_DAU = m.CHUKY_TT.CHUKY,
                CHUKY_SAU = m.CHUKY_TT1.CHUKY,
                TONG_GT = m.TONG_GT,
                GHICHU = m.GHICHU
            });

            //If TTVT export excel
            if (this.checkRole(user) == 1)
            {
                var hdcp = db.HOPDONG_CHIPHI.Where(n=>n.DONVI_ID == user.DONVI_ID).Select(n => new
                {
                    SO_HD = n.SO_HD,
                    LOAI_HD = n.LOAI_HD_SUB.TEN_HD_SUB,
                    TEN_TRAM = n.TRAM_DT,
                    TEN_BT = n.TEN_BT,
                    TEN_CT = n.TEN_CT,
                    DIACHI = n.DIACHI_CT,
                    NGAY_KY = n.NGAY_HD,
                    NGAY_BD = n.NGAY_BD,
                    NGAY_KT = n.NGAY_KT,
                    CHI_PHI = n.THANG,
                    THUE_VAT = n.VAT,
                    TONG_THANG = n.THANG_GT,
                    CHUKY_DAU = n.CHUKY_TT.CHUKY,
                    CHUKY_SAU = n.CHUKY_TT1.CHUKY,
                    TONG_GT = n.TONG_GT,
                    GHICHU = n.GHICHU
                });
                gv.DataSource = hdcp.Union(plhd).OrderBy(n => n.SO_HD).ToList();
            } else {
                var hdcp = db.HOPDONG_CHIPHI.Select(n => new
                {
                    SO_HD = n.SO_HD,
                    LOAI_HD = n.LOAI_HD_SUB.TEN_HD_SUB,
                    TEN_TRAM = n.TRAM_DT,
                    TEN_BT = n.TEN_BT,
                    TEN_CT = n.TEN_CT,
                    DIACHI = n.DIACHI_CT,
                    NGAY_KY = n.NGAY_HD,
                    NGAY_BD = n.NGAY_BD,
                    NGAY_KT = n.NGAY_KT,
                    CHI_PHI = n.THANG,
                    THUE_VAT = n.VAT,
                    TONG_THANG = n.THANG_GT,
                    CHUKY_DAU = n.CHUKY_TT.CHUKY,
                    CHUKY_SAU = n.CHUKY_TT1.CHUKY,
                    TONG_GT = n.TONG_GT,
                    GHICHU = n.GHICHU
                });
                gv.DataSource = hdcp.Union(plhd).OrderBy(n => n.SO_HD).ToList();
            }           
            
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=dshd_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("HDChiPhi", "Index");
        }

       

        public ActionResult ketquathongke_HDCNTT(FormCollection f, int? page)
        {
            ViewBag.namHD = new SelectList(lstNam, "NAM_HD_ID", "NAM");
            ViewBag.loaiHD = new SelectList(db.DM_LOAI_HOPDONG.ToList().OrderBy(n => n.LOAI_HOPDONG_ID), "LOAI_HOPDONG_ID", "TEN_LOAI_HOPDONG");
            int namHD = Int32.Parse(f["namHD"].ToString());
            int loaiHD = Int32.Parse(f["loaiHD"].ToString());

            List<proc_thongke_dt_thang_Result> listKQTK = db.proc_thongke_dt_thang(namHD, loaiHD).ToList();
            

            if (listKQTK == null)
            {
                ViewBag.thongbao = "Hiện chưa có doanh thu nào trong CSDL!!";
            }
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(listKQTK.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult export_hdcntt(int? namHD, int? loaiHD, int? donvi)
        {
            List<HOPDONG_DT_CNTT> hopdong = db.HOPDONG_DT_CNTT.ToList();
            if (namHD != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.NAM_HD_ID == namHD).ToList();
            }
            if (loaiHD != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.LOAI_HOPDONG_ID == loaiHD).ToList();
            }
            if (donvi != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.DONVI_ID == donvi).ToList();
            }
            if (namHD != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.NAM_HD_ID == namHD && n.LOAI_HOPDONG_ID == loaiHD).ToList();
            }
            if (namHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.NAM_HD_ID == namHD && n.DONVI_ID == donvi).ToList();
            }
            if (donvi != 0 && loaiHD != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.DONVI_ID == donvi && n.LOAI_HOPDONG_ID == loaiHD).ToList();
            }
            if (namHD != 0 && loaiHD != 0 && donvi != 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.Where(n => n.NAM_HD_ID == namHD && n.LOAI_HOPDONG_ID == loaiHD && n.DONVI_ID == donvi).ToList();
            }
            if (namHD == 0 && loaiHD == 0 && donvi == 0)
            {
                hopdong = db.HOPDONG_DT_CNTT.ToList();
            }
            GridView gv = new GridView();
            gv.DataSource = from P in hopdong
                            select new
                            {
                                //SO_HOP_DONG = P.HOPDONG_DT_CNTT_ID.
                                SO_HD = P.SO_HD,
                                LOAI_HD = P.DM_LOAI_HOPDONG.TEN_LOAI_HOPDONG,
                                NGAY_HD = P.NGAY_HD,
                                BEN_THUE = P.DM_CHUTHE_HOPDONG.TEN_CHUTHE,
                                BEN_CHO_THUE = P.DM_CHUTHE_HOPDONG1.TEN_CHUTHE,
                                TONG_GT = P.GIATRI_PHANCUNG_HD + P.GIATRI_DICHVU_HD,
                                CHU_KY = P.CHUKY_TT.CHUKY,
                                HTTT = P.HTTT.TEN_HTTT
                            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DS_HD_CNTT_" + System.DateTime.Now + ".xls");
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ThongKe", "ketquathongke_HDCNTT", new { @HD = hopdong });
        }

       
    }
}
