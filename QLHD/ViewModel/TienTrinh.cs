using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TienTrinh
    {
        public string TenTienTrinh { get; set; }
        public string NguoiThucHien { get; set; }
        public string VaiTro { get; set; }
        public DateTime ThoiGianGiao { get; set; }
        public DateTime Deadline { get; set; }
        public bool VTT { get; set; }

        public TienTrinh(string tenTienTrinh, string nguoiThucHien, string vaiTro, DateTime thoigiangiao, DateTime deadline, bool vtt)
        {
            TenTienTrinh = tenTienTrinh;
            NguoiThucHien = nguoiThucHien;
            VaiTro = vaiTro;
            ThoiGianGiao = thoigiangiao;
            Deadline = deadline;
            VTT = vtt;
        }

    }
}