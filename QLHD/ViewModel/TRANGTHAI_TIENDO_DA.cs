using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TRANGTHAI_TIENDO_DA
    {
        public int TRANGTHAI_TIENDO_DA_ID { get; set; }
        public string TEN_TRANGTHAI_TIENDO_DA { get; set; }

        public TRANGTHAI_TIENDO_DA(int id, string tenLTT)
        {
            TRANGTHAI_TIENDO_DA_ID = id;
            TEN_TRANGTHAI_TIENDO_DA = tenLTT;
        }
    }
}