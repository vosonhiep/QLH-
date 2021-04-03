using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;

namespace QLHD.Controllers
{
    public class DAO
    {
        public DAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string str_connect = WebConfigurationManager.ConnectionStrings["connect_gobal"].ConnectionString;

        protected SqlConnection connection;
        protected SqlDataAdapter adapter;
        protected SqlCommand command;

        public void Connect()
        {
            try
            {
                connection = new SqlConnection(str_connect);
                connection.Open();
            }
            catch (Exception ex)
            {
            }
        }

        protected void Disconnect()
        {
            try
            {

                connection.Close();
            }
            catch (Exception ex)
            {
            }

        }

        public int excuteNonQuery(string strsql)
        {
            int ret = 0;
            try
            {
                Connect();
                command = new SqlCommand(strsql, connection);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                ret = 1;
            }
            catch (Exception ex)
            {

            }

            Disconnect();
            return ret;
        }
        //public void excuteNonQuery(string strsql, int store)
        //{
        //    try
        //    {
        //        Connect();
        //        command = new SqlCommand(strsql, connection);
        //        command.CommandTimeout = 0;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    Disconnect();
        //}

        public DataTable executeDataTable(string strsql)
        {

            DataTable dt = new DataTable();
            try
            {
                Connect();
                command = new SqlCommand(strsql, connection);
                command.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ex)
            {

            }

            Disconnect();
            return dt;
        }
    }
}
