using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TRANGTHAI_THANHTOAN
    {
        public int TRANGTHAI_THANHTOAN_ID { get; set; }
        public string TRANGTHAI_THANHTOAN_VALUE { get; set; }

        public TRANGTHAI_THANHTOAN(int key, string value)
        {
            TRANGTHAI_THANHTOAN_ID = key;
            TRANGTHAI_THANHTOAN_VALUE = value;
        }
    }
}