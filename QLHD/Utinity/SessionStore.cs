using QLHD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLHD.Utinity
{
    public class SessionStore
    {

        public static THANHVIEN users
        {
            get
            {
                return GetSession("user") as THANHVIEN;
            }
            set
            {
                SetSession("user", value);
            }
        }

        public static object GetSession(string key)
        {
            try
            {
                return System.Web.HttpContext.Current.Session[key];
            }
            catch (System.NullReferenceException)
            {
                return null;
            }
        }
        public static void SetSession(string key, object value)
        {
            System.Web.HttpContext.Current.Session[key] = value;
        }

    }
}