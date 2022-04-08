using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TIENTRINH_Model
    {
        public int HOPDONG_ID { get; set; }
        public int TienTrinh_ID { get; set; }
        public string TenTienTrinh { get; set; }
        public string NguoiPhuTrach { get; set; }
        public int TienDo { get; set; }
        public DateTime NgayGiao { get; set; }
        public DateTime Deadline { get; set; }
        public string FILE { get; set; }
        public bool VTT { get; set; }

        public TIENTRINH_Model(int idTT, string tenTienTrinh, string nguoiThucHien, int tienDo, DateTime thoigiangiao, DateTime deadline, bool vtt)
        {
            HOPDONG_ID = 6;
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