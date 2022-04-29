using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TRANGTHAI_DUAN
    {
        public int LOAITRANGTHAI_ID { get; set; }
        public string TENLOAITRANGTHAI { get; set; }

        public TRANGTHAI_DUAN(int id, string tenLoaiTT)
        {
            LOAITRANGTHAI_ID = id;
            TENLOAITRANGTHAI = tenLoaiTT;
        }
    }
}