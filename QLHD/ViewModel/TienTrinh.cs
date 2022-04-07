using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TienTrinh
    {
        public int TienTrinh_ID { get; set; }
        public string TenTienTrinh { get; set; }
        public string NguoiPhuTrach { get; set; }
        public int TienDo { get; set; }
        public DateTime NgayGiao { get; set; }
        public DateTime Deadline { get; set; }
        public string FILE { get; set; }
        public bool VTT { get; set; }

        public TienTrinh(int idTT, string tenTienTrinh, string nguoiThucHien, int tienDo, DateTime thoigiangiao, DateTime deadline, bool vtt)
        {
            TienTrinh_ID = idTT;
            TenTienTrinh = tenTienTrinh;
            NguoiPhuTrach = nguoiThucHien;
            TienDo = tienDo;
            NgayGiao = thoigiangiao;
            Deadline = deadline;
            FILE = "BAO_CAO_Cong_viec_thang_8-2016_PCN_va_TT.xls";
            VTT = vtt;
        }

    }
}