using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class LOAITIENTRINH
    {
        public bool LOAITIENTRINH_ID { get; set; }
        public string TENLOAITIENTRINH { get; set; }

        public LOAITIENTRINH(bool id, string tenLTT)
        {
            LOAITIENTRINH_ID = id;
            TENLOAITIENTRINH = tenLTT;
        }
    }
}