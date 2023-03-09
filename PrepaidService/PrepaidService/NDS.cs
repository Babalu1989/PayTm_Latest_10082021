using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Text;


namespace PrepaidService
{
    public class NDS
    {
        public NDS()
        {

        }

        public static string con()
        {
            string str = "";
            string database = "";
            string user_id = "";
            string pass = "";

            try
            {
                //Cryptograph crp = new Cryptograph();

                ////string vs = myServer.MapPath("IT-OPR.ini");

                //string vs = AppDomain.CurrentDomain.BaseDirectory + "Prepaid.ini";
                ////string vs = AppDomain.CurrentDomain.BaseDirectory + "KYC.ini"; //LIVE

                //// ASSING KEY TO CONNECT DATABASE.


                //string PW_KEY = "o8??^am(*)";  // Insert Key Here

                //user_id = crp.Decrypt(NDSINI.GetINI(vs, "Prepaid", crp.Encrypt("dbuserid", PW_KEY), "?"), PW_KEY);
                //pass = crp.Decrypt(NDSINI.GetINI(vs, "Prepaid", crp.Encrypt("dbuserpwd", PW_KEY), "?"), PW_KEY);
                //database = crp.Decrypt(NDSINI.GetINI(vs, "Prepaid", crp.Encrypt("dbconn", PW_KEY), "?"), PW_KEY);// Put user code to initialize the page here
                //str = "Provider=MSDAORA.1; User ID=" + user_id + "; Password=" + pass + "; Data Source=" + database + ";";
                  str = "Provider=MSDAORA.1; User ID=brplkyc; Password=brplkyc; Data Source=EBSTESTOLD;";	///for test    
            }
            catch (Exception ex)
            {

            }

            return str;
        }
    }

   
}