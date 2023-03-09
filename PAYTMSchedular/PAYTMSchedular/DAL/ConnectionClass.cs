using System;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Web;



    public class ConnectionClass
    {
        static string str = "";
        public static string con()
        {

            string database = "";
            string user_id = "";
            string pass = "";
            
        //    Cryptograph crp = new Cryptograph();
        //    //HttpServerUtility myServer = HttpContext.Current.Server;
        //    //string vs = myServer.MapPath("IT-OPR.ini");
        //    string vs = AppDomain.CurrentDomain.BaseDirectory + "Prepaid.ini";
        //    //string vs = Application.StartupPath + "\\" + "bses.ini";

        //    string PW_KEY = "o8??^am(*)";  // Enter Encryption Key Here live server
        //                                   //string PW_KEY = "@!*fdfsfd+}|@";  //test server
        //                                   //Enter Encryption Key Here 

        //user_id = crp.Decrypt(MOBIniOperations.GetINI(vs, "Prepaid", crp.Encrypt("dbuserid", PW_KEY), "?"), PW_KEY);
        //pass = crp.Decrypt(MOBIniOperations.GetINI(vs, "Prepaid", crp.Encrypt("dbuserpwd", PW_KEY), "?"), PW_KEY);
        //database = crp.Decrypt(MOBIniOperations.GetINI(vs, "Prepaid", crp.Encrypt("dbconn", PW_KEY), "?"), PW_KEY);

            str = "Provider=MSDAORA.1; User ID=brplkyc; Password=brplkyc; Data Source=EBSTESTOLD;";
           // str = "Provider=MSDAORA.1; User ID=" + user_id + "; Password=" + pass + "; Data Source=" + database + ";";
            return str;
        }
      


    }

