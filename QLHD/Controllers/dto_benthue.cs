using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.Controllers
{
    public class dto_benthue
    {
        public dto_benthue() { }
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _tenBT;
        public string TEN_BT
        {
            get { return _tenBT; }
            set { _tenBT = value; }
        }
        private string _diachi;
        public string DIACHI
        {
            get { return _diachi; }
            set { _diachi = value; }
        }
        private string _dienthoaiBT;
        public string DIENTHOAI_BT
        {
            get { return _dienthoaiBT; }
            set { _dienthoaiBT = value; }
        }
        private string _msthue;
        public string MSTHUE
        {
            get { return _msthue; }
            set { _msthue = value; }
        }
        private string _taikhoan;
        public string TAIKHOAN
        {
            get { return _taikhoan; }
            set { _taikhoan = value; }
        }
        private string _nganhang;
        public string NGANHANG
        {
            get { return _nganhang; }
            set { _nganhang = value; }
        }
        private string _daidien;
        public string DAIDIEN
        {
            get { return _daidien; }
            set { _daidien = value; }
        }
        private string _chucvu;
        public string CHUCVU
        {
            get { return _chucvu; }
            set { _chucvu = value; }
        }
    }
}