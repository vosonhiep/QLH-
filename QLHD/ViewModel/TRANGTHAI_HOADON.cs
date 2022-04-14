using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TRANGTHAI_HOADON
    {
        public int TRANGTHAI_XUAT_HOADON { get; set; }
        public string TRANGTHAI_HOADON_VALUE { get; set; }

        public TRANGTHAI_HOADON(int key, string value)
        {
            TRANGTHAI_XUAT_HOADON = key;
            TRANGTHAI_HOADON_VALUE = value;
        }
    }
}