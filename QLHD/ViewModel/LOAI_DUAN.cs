using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class LOAI_DUAN
    {
        public int LOAI_DUAN_ID { get; set; }
        public string TEN_LOAI_DUAN { get; set; }

        public LOAI_DUAN(int id, string tenLTT)
        {
            LOAI_DUAN_ID = id;
            TEN_LOAI_DUAN = tenLTT;
        }
    }
}