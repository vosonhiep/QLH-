using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.ViewModel
{
    public class TIENDO_Model
    {
        public int TIENDO_ID { get; set; }
        public string TENTIENDO { get; set; }

        public TIENDO_Model(int id, string tenTT)
        {
            TIENDO_ID = id;
            TENTIENDO = tenTT;
        }
    }
}