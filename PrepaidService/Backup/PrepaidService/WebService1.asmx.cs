using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Configuration;
using System.Xml;
namespace PrepaidService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        //public string keyvalue = "@BRPL%Pre$Mtr#";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        OleDbCommand cmd;
        OleDbConnection con;
        OleDbDataAdapter ada;
        string KEYVALUE = string.Empty;
        string AGENCY = string.Empty;
        public string strConnection = ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
        [WebMethod]
        public DataSet PAYTM_CA_DETAILS(string KEY, string AGENCYNAME, string CANUMBER, string AMOUNT, string TRANS_ID, string FLAG)
        {
            string Companycode = "";
            string Contract = "";
            string CANumber = "";
            string Name = "";
            string Address = "";
            string Mobile = "";
            string Email = "";
            string Serialno = "";
            string Acc_det_Id = "";
            string ACCTCLASS = "";
            string Docid = "";
            string Flag = "";
            string Manufacture = "";
            string Message = "";

            DataTable dt = new DataTable();
            dt.Columns.Add("COMP_CODE", typeof(string));
            dt.Columns.Add("CONTRACT", typeof(string));
            dt.Columns.Add("CONTRACT_ACCOUNT", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ADDRESS", typeof(string));
            dt.Columns.Add("TEL1_NUMBR", typeof(string));
            dt.Columns.Add("E_MAIL", typeof(string));
            dt.Columns.Add("SERIALNO", typeof(string));
            dt.Columns.Add("ACCT_DET_ID", typeof(string));
            dt.Columns.Add("ACCT_CLASS", typeof(string));
            dt.Columns.Add("DOC_ID", typeof(string));
            dt.Columns.Add("MANUFACTURER", typeof(string));
            dt.Columns.Add("FLAG", typeof(string));
            dt.Columns.Add("MESSAGE", typeof(string));
            
            ISU.WebService Pre = new ISU.WebService();
            try
            {
                if (KEY != "" && AGENCYNAME != "")
                {
                    con = new OleDbConnection(strConnection);
                    con.Open();
                    string Querycheck = "SELECT KEYID,AGENCYNAME FROM PREPAID_AGENCYKEY WHERE KEYID='" + KEY + "' AND UPPER(AGENCYNAME)='" + AGENCYNAME.ToUpper() + "'";
                    ada = new OleDbDataAdapter(Querycheck, con);
                    DataTable dt2 = new DataTable();
                    ada.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        KEYVALUE = dt2.Rows[0]["KEYID"].ToString();
                        AGENCY = dt2.Rows[0]["AGENCYNAME"].ToString();
                    }
                }
                if (KEY == KEYVALUE && AGENCYNAME.ToUpper() == AGENCY)
                {
                    if (!string.IsNullOrEmpty(CANUMBER) && !string.IsNullOrEmpty(AMOUNT) && FLAG == "C")
                    {
                        ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, FLAG);
                        if (ds.Tables[0].Rows[0]["COMP_CODE"].ToString() != "")
                        {
                            Companycode = ds.Tables[0].Rows[0]["COMP_CODE"].ToString();
                        }
                        else
                        {
                            Companycode = "";
                        }
                        if (ds.Tables[0].Rows[0]["CONTRACT"].ToString() != "")
                        {
                            Contract = ds.Tables[0].Rows[0]["CONTRACT"].ToString();
                        }
                        else
                        {
                            Contract = "";
                        }
                        if (ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString() != "")
                        {
                            CANumber = ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString();
                        }
                        else
                        {
                            CANumber = "";
                        }
                        if (ds.Tables[0].Rows[0]["NAME"].ToString() != "")
                        {
                            Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                        }
                        else
                        {
                            Name = "";
                        }
                        if (ds.Tables[0].Rows[0]["ADDRESS"].ToString() != "")
                        {
                            Address = ds.Tables[0].Rows[0]["ADDRESS"].ToString();
                        }
                        else
                        {
                            Address = "";
                        }
                        if (ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() != "")
                        {
                            Mobile = ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString();
                        }
                        else
                        {
                            Mobile = "";
                        }
                        if (ds.Tables[0].Rows[0]["E_MAIL"].ToString() != "")
                        {
                            Email = ds.Tables[0].Rows[0]["E_MAIL"].ToString();
                        }
                        else
                        {
                            Email = "";
                        }
                        if (ds.Tables[0].Rows[0]["SERIALNO"].ToString() != "")
                        {
                            Serialno = ds.Tables[0].Rows[0]["SERIALNO"].ToString();
                        }
                        else
                        {
                            Serialno = "";
                        }
                        if (ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString() != "")
                        {
                            Acc_det_Id = ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString();
                        }
                        else
                        {
                            Acc_det_Id = "";
                        }
                        if (ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString() != "")
                        {
                            ACCTCLASS = ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString();
                        }
                        else
                        {
                            ACCTCLASS = "";
                        }
                        if (ds.Tables[0].Rows[0]["DOC_ID"].ToString() != "")
                        {
                            Docid = ds.Tables[0].Rows[0]["DOC_ID"].ToString();
                        }
                        else
                        {
                            Docid = "";
                        }
                        if (ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() != "")
                        {
                            Manufacture = ds.Tables[0].Rows[0]["MANUFACTURER"].ToString();
                        }
                        else
                        {
                            Manufacture = "";
                        }
                        if (ds.Tables[0].Rows[0]["FLAG"].ToString() != "")
                        {
                            Flag = ds.Tables[0].Rows[0]["FLAG"].ToString();
                        }
                        else
                        {
                            Flag = "";
                        }
                        if (ds.Tables[0].Rows[0]["MESSAGE"].ToString() != "")
                        {
                            Message = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
                        }
                        else
                        {
                            Message = "";
                        }
                        if (Message.Contains("Check successful") && Flag.Contains("Y"))
                        {
                            con = new OleDbConnection(strConnection);
                            con.Open();
                            string strInsert = "INSERT INTO PAYTM_CHECKBAPIDATA (COMPANY,CONTRACT,CA_NUMBER,NETAMOUNT,NAME,ADDRESS,MOBILE,E_MAIL,SERIALNO,ACCTDET_ID,ACCT_CLASS,DOC_ID,MANUFACTURE,FLAG,MESSAGE)";
                            strInsert += "VALUES ('" + Companycode + "','" + Contract + "','" + CANumber + "','" + AMOUNT + "','" + Name + "','" + Address + "','" + Mobile + "','" + Email + "','" + Serialno + "','" + Acc_det_Id + "','" + ACCTCLASS + "','" + Docid + "','" + Manufacture + "','" + Flag + "','" + Message + "')";
                            cmd = new OleDbCommand(strInsert, con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (!string.IsNullOrEmpty(CANUMBER) && !string.IsNullOrEmpty(AMOUNT) && !string.IsNullOrEmpty(TRANS_ID) && FLAG == "R")
                    {
                        ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, FLAG);

                        if (ds.Tables[0].Rows[0]["COMP_CODE"].ToString() != "")
                        {
                            Companycode = ds.Tables[0].Rows[0]["COMP_CODE"].ToString();
                        }
                        else
                        {
                            Companycode = "";
                        }
                        if (ds.Tables[0].Rows[0]["CONTRACT"].ToString() != "")
                        {
                            Contract = ds.Tables[0].Rows[0]["CONTRACT"].ToString();
                        }
                        else
                        {
                            Contract = "";
                        }
                        if (ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString() != "")
                        {
                            CANumber = ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString();
                        }
                        else
                        {
                            CANumber = "";
                        }
                        if (ds.Tables[0].Rows[0]["NAME"].ToString() != "")
                        {
                            Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                        }
                        else
                        {
                            Name = "";
                        }
                        if (ds.Tables[0].Rows[0]["ADDRESS"].ToString() != "")
                        {
                            Address = ds.Tables[0].Rows[0]["ADDRESS"].ToString();
                        }
                        else
                        {
                            Address = "";
                        }
                        if (ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() != "")
                        {
                            Mobile = ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString();
                        }
                        else
                        {
                            Mobile = "";
                        }
                        if (ds.Tables[0].Rows[0]["E_MAIL"].ToString() != "")
                        {
                            Email = ds.Tables[0].Rows[0]["E_MAIL"].ToString();
                        }
                        else
                        {
                            Email = "";
                        }
                        if (ds.Tables[0].Rows[0]["SERIALNO"].ToString() != "")
                        {
                            Serialno = ds.Tables[0].Rows[0]["SERIALNO"].ToString();
                        }
                        else
                        {
                            Serialno = "";
                        }
                        if (ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString() != "")
                        {
                            Acc_det_Id = ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString();
                        }
                        else
                        {
                            Acc_det_Id = "";
                        }
                        if (ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString() != "")
                        {
                            ACCTCLASS = ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString();
                        }
                        else
                        {
                            ACCTCLASS = "";
                        }
                        if (ds.Tables[0].Rows[0]["DOC_ID"].ToString() != "")
                        {
                            Docid = ds.Tables[0].Rows[0]["DOC_ID"].ToString();
                        }
                        else
                        {
                            Docid = "";
                        }
                        if (ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() != "")
                        {
                            Manufacture = ds.Tables[0].Rows[0]["MANUFACTURER"].ToString();
                        }
                        else
                        {
                            Manufacture = "";
                        }
                        if (ds.Tables[0].Rows[0]["FLAG"].ToString() != "")
                        {
                            Flag = ds.Tables[0].Rows[0]["FLAG"].ToString();
                        }
                        else
                        {
                            Flag = "";
                        }
                        if (ds.Tables[0].Rows[0]["MESSAGE"].ToString() != "")
                        {
                            Message = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
                        }
                        else
                        {
                            Message = "";
                        }
                        if (Message.Contains("Successful") && Flag.Contains("Y"))
                        {
                            con = new OleDbConnection(strConnection);
                            con.Open();
                            string Querycheck = "SELECT TRANSACTIONID FROM PAYTM_RECHARGEDATA WHERE PAYMENTMETHOD='" + AGENCY + "' AND TRANSACTIONID='" + TRANS_ID + "'";
                            ada = new OleDbDataAdapter(Querycheck, con);
                            DataTable dt1 = new DataTable();
                            ada.Fill(dt1);
                            if (dt1.Rows.Count == 0)
                            {
                                string strInsert = "INSERT INTO PAYTM_RECHARGEDATA(COMPANY,CONTRACT,CA_NUMBER,NAME,ADDRESS,MOBILE,E_MAIL,SERIALNO,ACCTDET_ID,ACCT_CLASS,DOC_ID,NETAMOUNT,TRANSACTIONID,MANUFACTURE,FLAG,MESSAGE,AGENTNO,PAYMENTMETHOD,RECHARGETYPE,DUPLICATEFLAG)";
                                strInsert += "VALUES ('" + Companycode + "','" + Contract + "','" + CANumber + "','" + Name + "','" + Address + "','" + Mobile + "','" + Email + "','" + Serialno + "','" + Acc_det_Id + "','" + ACCTCLASS + "','" + Docid + "','" + AMOUNT + "','" + TRANS_ID + "','" + Manufacture + "','" + Flag + "','" + Message + "','AUTO','" + AGENCY + "','1','N')";
                                try
                                {
                                    cmd = new OleDbCommand(strInsert, con);
                                    int i = cmd.ExecuteNonQuery();
                                    if (i > 0)
                                    {
                                        Message = TRANS_ID;
                                        ds.Tables[0].Rows[0][13] = Message;
                                        ds.AcceptChanges();
                                    }
                                    else
                                    {
                                        Message = TRANS_ID;
                                        Flag = "N";
                                        ds.Tables[0].Rows[0][12] = Flag;
                                        ds.Tables[0].Rows[0][13] = Message;
                                        ds.AcceptChanges();
                                    }
                                }
                                catch (Exception exx)
                                {
                                    Message = TRANS_ID;
                                    Flag = "N";
                                    ds.Tables[0].Rows[0][12] = Flag;
                                    ds.Tables[0].Rows[0][13] = Message;
                                    ds.AcceptChanges();
                                }
                            }
                            else
                            {
                                Message = TRANS_ID;
                                Flag = "D";
                                ds.Tables[0].Rows[0][12] = Flag;
                                ds.Tables[0].Rows[0][13] = Message;
                                ds.AcceptChanges();
                            }
                        }
                    }
                    else if (ds.Tables.Count <= 0)
                    {
                        dt.Rows.Add("Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available");
                        dt.AcceptChanges();
                        ds.Tables.Add(dt);
                    }
                }
                else
                {
                    dt.Rows.Add("Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!");
                    dt.AcceptChanges();
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return ds;
        }
        [WebMethod]
        public DataSet CLUSTER_CA_DISPLAY(string KEY, string AGENCYNAME, string CANUMBER)
        {
            string CA_NUMBER = string.Empty;
            string BP_NUMBER = string.Empty;
            string BP_NAME = string.Empty;
            string BP_TYPE = string.Empty;
            string SEARCH_TERM1 = string.Empty;
            string SEARCH_TERM2 = string.Empty;
            string HOUSE_NUMBER = string.Empty;
            string HOUSE_NUMBER_SUP = string.Empty;
            string FLOOR = string.Empty;
            string PREMISE_TYPE = string.Empty;
            string STREET = string.Empty;
            string STREET2 = string.Empty;
            string STREET3 = string.Empty;
            string STREET4 = string.Empty;
            string CITY = string.Empty;
            string POST_CODE = string.Empty;
            string REGION = string.Empty;
            string COUNTRY = string.Empty;
            string DESC_CON_OBJECT = string.Empty;
            string REG_STR_GROUP = string.Empty;
            string DEVICE_SR_NUMBER = string.Empty;
            string TELEPHONE_NO = string.Empty;
            string MRU = string.Empty;
            string FUNC_DESCR = string.Empty;
            string OUTAGE_FROMTIME = string.Empty;
            string OUTAGE_TOTIME = string.Empty;
            string LEGACY_ACCT = string.Empty;
            string BILL_CLASS = string.Empty;
            string RATE_CAT = string.Empty;
            string ACTIVITY = string.Empty;
            string ADR_NOTES = string.Empty;
            string TEL1_NUMBER = string.Empty;
            string VERTRAG = string.Empty;
            string E_MAIL = string.Empty;
            string MOVE_OUT_DATE = string.Empty;
            string CON_OBJ_NO = string.Empty;
            string CLERK_ID = string.Empty;
            string TEXT = string.Empty;
            string STATUS = string.Empty;
            string DISCREASON = string.Empty;
            string POLE_NO = string.Empty;
            try
            {
                if (KEY != "" && AGENCYNAME != "")
                {
                    con = new OleDbConnection(strConnection);
                    con.Open();
                    string Querycheck = "SELECT KEYID,AGENCYNAME FROM PREPAID_AGENCYKEY WHERE KEYID='" + KEY + "' AND UPPER(AGENCYNAME)='" + AGENCYNAME.ToUpper() + "'";
                    ada = new OleDbDataAdapter(Querycheck, con);
                    DataTable dt2 = new DataTable();
                    ada.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                    {
                        KEYVALUE = dt2.Rows[0]["KEYID"].ToString();
                        AGENCY = dt2.Rows[0]["AGENCYNAME"].ToString();
                    }
                }
                if (KEY == KEYVALUE && AGENCYNAME.ToUpper() == AGENCY)
                {
                    ISU.WebService objisu = new ISU.WebService();
                    ds = objisu.Z_BAPI_CMS_ISU_CA_DISPLAY(CANUMBER, "", "", "", "", "");

                    if (ds.Tables[0].Rows[0]["CA_NUMBER"].ToString() != "")
                    {
                        CA_NUMBER = ds.Tables[0].Rows[0]["CA_NUMBER"].ToString();
                    }
                    else
                    {
                        CA_NUMBER = "";
                    }
                    if (ds.Tables[0].Rows[0]["BP_NUMBER"].ToString() != "")
                    {
                        BP_NUMBER = ds.Tables[0].Rows[0]["BP_NUMBER"].ToString();
                    }
                    else
                    {
                        BP_NUMBER = "";
                    }
                    if (ds.Tables[0].Rows[0]["BP_NAME"].ToString() != "")
                    {
                        BP_NAME = ds.Tables[0].Rows[0]["BP_NAME"].ToString();
                    }
                    else
                    {
                        BP_NAME = "";
                    }
                    if (ds.Tables[0].Rows[0]["BP_TYPE"].ToString() != "")
                    {
                        BP_TYPE = ds.Tables[0].Rows[0]["BP_TYPE"].ToString();
                    }
                    else
                    {
                        BP_TYPE = "";
                    }
                    if (ds.Tables[0].Rows[0]["SEARCH_TERM1"].ToString() != "")
                    {
                        SEARCH_TERM1 = ds.Tables[0].Rows[0]["SEARCH_TERM1"].ToString();
                    }
                    else
                    {
                        SEARCH_TERM1 = "";
                    }
                    if (ds.Tables[0].Rows[0]["SEARCH_TERM2"].ToString() != "")
                    {
                        SEARCH_TERM2 = ds.Tables[0].Rows[0]["SEARCH_TERM2"].ToString();
                    }
                    else
                    {
                        SEARCH_TERM2 = "";
                    }
                    if (ds.Tables[0].Rows[0]["HOUSE_NUMBER"].ToString() != "")
                    {
                        HOUSE_NUMBER = ds.Tables[0].Rows[0]["HOUSE_NUMBER"].ToString();
                    }
                    else
                    {
                        HOUSE_NUMBER = "";
                    }
                    if (ds.Tables[0].Rows[0]["HOUSE_NUMBER_SUP"].ToString() != "")
                    {
                        HOUSE_NUMBER_SUP = ds.Tables[0].Rows[0]["HOUSE_NUMBER_SUP"].ToString();
                    }
                    else
                    {
                        HOUSE_NUMBER_SUP = "";
                    }
                    if (ds.Tables[0].Rows[0]["FLOOR"].ToString() != "")
                    {
                        FLOOR = ds.Tables[0].Rows[0]["FLOOR"].ToString();
                    }
                    else
                    {
                        FLOOR = "";
                    }
                    if (ds.Tables[0].Rows[0]["PREMISE_TYPE"].ToString() != "")
                    {
                        PREMISE_TYPE = ds.Tables[0].Rows[0]["PREMISE_TYPE"].ToString();
                    }
                    else
                    {
                        PREMISE_TYPE = "";
                    }
                    if (ds.Tables[0].Rows[0]["STREET"].ToString() != "")
                    {
                        STREET = ds.Tables[0].Rows[0]["STREET"].ToString();
                    }
                    else
                    {
                        STREET = "";
                    }
                    if (ds.Tables[0].Rows[0]["STREET2"].ToString() != "")
                    {
                        STREET2 = ds.Tables[0].Rows[0]["STREET2"].ToString();
                    }
                    else
                    {
                        STREET2 = "";
                    }
                    if (ds.Tables[0].Rows[0]["STREET3"].ToString() != "")
                    {
                        STREET3 = ds.Tables[0].Rows[0]["STREET3"].ToString();
                    }
                    else
                    {
                        STREET3 = "";
                    }
                    if (ds.Tables[0].Rows[0]["STREET4"].ToString() != "")
                    {
                        STREET4 = ds.Tables[0].Rows[0]["STREET4"].ToString();
                    }
                    else
                    {
                        STREET4 = "";
                    }
                    if (ds.Tables[0].Rows[0]["CITY"].ToString() != "")
                    {
                        CITY = ds.Tables[0].Rows[0]["CITY"].ToString();
                    }
                    else
                    {
                        CITY = "";
                    }
                    if (ds.Tables[0].Rows[0]["POST_CODE"].ToString() != "")
                    {
                        POST_CODE = ds.Tables[0].Rows[0]["POST_CODE"].ToString();
                    }
                    else
                    {
                        POST_CODE = "";
                    }
                    if (ds.Tables[0].Rows[0]["REGION"].ToString() != "")
                    {
                        REGION = ds.Tables[0].Rows[0]["REGION"].ToString();
                    }
                    else
                    {
                        REGION = "";
                    }
                    if (ds.Tables[0].Rows[0]["COUNTRY"].ToString() != "")
                    {
                        COUNTRY = ds.Tables[0].Rows[0]["COUNTRY"].ToString();
                    }
                    else
                    {
                        COUNTRY = "";
                    }
                    if (ds.Tables[0].Rows[0]["DESC_CON_OBJECT"].ToString() != "")
                    {
                        DESC_CON_OBJECT = ds.Tables[0].Rows[0]["DESC_CON_OBJECT"].ToString();
                    }
                    else
                    {
                        DESC_CON_OBJECT = "";
                    }
                    if (ds.Tables[0].Rows[0]["REG_STR_GROUP"].ToString() != "")
                    {
                        REG_STR_GROUP = ds.Tables[0].Rows[0]["REG_STR_GROUP"].ToString();
                    }
                    else
                    {
                        REG_STR_GROUP = "";
                    }
                    if (ds.Tables[0].Rows[0]["DEVICE_SR_NUMBER"].ToString() != "")
                    {
                        DEVICE_SR_NUMBER = ds.Tables[0].Rows[0]["DEVICE_SR_NUMBER"].ToString();
                    }
                    else
                    {
                        DEVICE_SR_NUMBER = "";
                    }
                    if (ds.Tables[0].Rows[0]["TELEPHONE_NO"].ToString() != "")
                    {
                        TELEPHONE_NO = ds.Tables[0].Rows[0]["TELEPHONE_NO"].ToString();
                    }
                    else
                    {
                        TELEPHONE_NO = "";
                    }
                    if (ds.Tables[0].Rows[0]["MRU"].ToString() != "")
                    {
                        MRU = ds.Tables[0].Rows[0]["MRU"].ToString();
                    }
                    else
                    {
                        MRU = "";
                    }
                    if (ds.Tables[0].Rows[0]["FUNC_DESCR"].ToString() != "")
                    {
                        FUNC_DESCR = ds.Tables[0].Rows[0]["FUNC_DESCR"].ToString();
                    }
                    else
                    {
                        FUNC_DESCR = "";
                    }
                    if (ds.Tables[0].Rows[0]["OUTAGE_FROMTIME"].ToString() != "")
                    {
                        OUTAGE_FROMTIME = ds.Tables[0].Rows[0]["OUTAGE_FROMTIME"].ToString();
                    }
                    else
                    {
                        OUTAGE_FROMTIME = "";
                    }
                    if (ds.Tables[0].Rows[0]["OUTAGE_TOTIME"].ToString() != "")
                    {
                        OUTAGE_TOTIME = ds.Tables[0].Rows[0]["OUTAGE_TOTIME"].ToString();
                    }
                    else
                    {
                        OUTAGE_TOTIME = "";
                    }
                    if (ds.Tables[0].Rows[0]["LEGACY_ACCT"].ToString() != "")
                    {
                        LEGACY_ACCT = ds.Tables[0].Rows[0]["LEGACY_ACCT"].ToString();
                    }
                    else
                    {
                        LEGACY_ACCT = "";
                    }
                    if (ds.Tables[0].Rows[0]["BILL_CLASS"].ToString() != "")
                    {
                        BILL_CLASS = ds.Tables[0].Rows[0]["BILL_CLASS"].ToString();
                    }
                    else
                    {
                        BILL_CLASS = "";
                    }
                    if (ds.Tables[0].Rows[0]["RATE_CAT"].ToString() != "")
                    {
                        RATE_CAT = ds.Tables[0].Rows[0]["RATE_CAT"].ToString();
                    }
                    else
                    {
                        RATE_CAT = "";
                    }
                    if (ds.Tables[0].Rows[0]["ACTIVITY"].ToString() != "")
                    {
                        ACTIVITY = ds.Tables[0].Rows[0]["ACTIVITY"].ToString();
                    }
                    else
                    {
                        ACTIVITY = "";
                    }
                    if (ds.Tables[0].Rows[0]["ADR_NOTES"].ToString() != "")
                    {
                        ADR_NOTES = ds.Tables[0].Rows[0]["ADR_NOTES"].ToString();
                    }
                    else
                    {
                        ADR_NOTES = "";
                    }
                    if (ds.Tables[0].Rows[0]["TEL1_NUMBER"].ToString() != "")
                    {
                        TEL1_NUMBER = ds.Tables[0].Rows[0]["TEL1_NUMBER"].ToString();
                    }
                    else
                    {
                        TEL1_NUMBER = "";
                    }
                    if (ds.Tables[0].Rows[0]["VERTRAG"].ToString() != "")
                    {
                        VERTRAG = ds.Tables[0].Rows[0]["VERTRAG"].ToString();
                    }
                    else
                    {
                        VERTRAG = "";
                    }
                    if (ds.Tables[0].Rows[0]["E_MAIL"].ToString() != "")
                    {
                        E_MAIL = ds.Tables[0].Rows[0]["E_MAIL"].ToString();
                    }
                    else
                    {
                        E_MAIL = "";
                    }
                    if (ds.Tables[0].Rows[0]["MOVE_OUT_DATE"].ToString() != "")
                    {
                        MOVE_OUT_DATE = ds.Tables[0].Rows[0]["MOVE_OUT_DATE"].ToString();
                    }
                    else
                    {
                        MOVE_OUT_DATE = "";
                    }
                    if (ds.Tables[0].Rows[0]["CON_OBJ_NO"].ToString() != "")
                    {
                        CON_OBJ_NO = ds.Tables[0].Rows[0]["CON_OBJ_NO"].ToString();
                    }
                    else
                    {
                        CON_OBJ_NO = "";
                    }
                    if (ds.Tables[0].Rows[0]["CLERK_ID"].ToString() != "")
                    {
                        CLERK_ID = ds.Tables[0].Rows[0]["CLERK_ID"].ToString();
                    }
                    else
                    {
                        CLERK_ID = "";
                    }
                    if (ds.Tables[0].Rows[0]["TEXT"].ToString() != "")
                    {
                        TEXT = ds.Tables[0].Rows[0]["TEXT"].ToString();
                    }
                    else
                    {
                        TEXT = "";
                    }
                    if (ds.Tables[0].Rows[0]["STATUS"].ToString() != "")
                    {
                        STATUS = ds.Tables[0].Rows[0]["STATUS"].ToString();
                    }
                    else
                    {
                        STATUS = "";
                    }
                    if (ds.Tables[0].Rows[0]["DISCREASON"].ToString() != "")
                    {
                        DISCREASON = ds.Tables[0].Rows[0]["DISCREASON"].ToString();
                    }
                    else
                    {
                        DISCREASON = "";
                    }
                    if (ds.Tables[0].Rows[0]["POLE_NO"].ToString() != "")
                    {
                        POLE_NO = ds.Tables[0].Rows[0]["POLE_NO"].ToString();
                    }
                    else
                    {
                        POLE_NO = "";
                    }
                    if (CA_NUMBER != null || CA_NUMBER != "")
                    {
                        con = new OleDbConnection(strConnection);
                        con.Open();
                        string strInsert = "INSERT INTO PAYTM_CLUSTERCADATA(CA_NUMBER,BP_NUMBER,BP_NAME,BP_TYPE,SEARCH_TERM1,SEARCH_TERM2,HOUSE_NUMBER,HOUSE_NUMBER_SUP,FLOOR,";
                        strInsert += "PREMISE_TYPE,STREET,STREET2,STREET3,STREET4,CITY,POST_CODE,REGION,COUNTRY,DESC_CON_OBJECT,REG_STR_GROUP,DEVICE_SR_NUMBER,TELEPHONE_NO,MRU,";
                        strInsert += "FUNC_DESCR,OUTAGE_FROMTIME,OUTAGE_TOTIME, LEGACY_ACCT,BILL_CLASS,RATE_CAT,ACTIVITY,ADR_NOTES,TEL1_NUMBER,VERTRAG,E_MAIL,MOVE_OUT_DATE,CON_OBJ_NO,CLERK_ID,TEXT,STATUS,DISCREASON,POLE_NO)";
                        strInsert += "VALUES ('" + CA_NUMBER + "','" + BP_NUMBER + "','" + BP_NAME + "','" + BP_TYPE + "','" + SEARCH_TERM1 + "','" + SEARCH_TERM2 + "','" + HOUSE_NUMBER + "','" + HOUSE_NUMBER_SUP + "','" + FLOOR + "',";
                        strInsert += "'" + PREMISE_TYPE + "','" + STREET + "','" + STREET2 + "','" + STREET3 + "','" + STREET4 + "','" + CITY + "','" + POST_CODE + "','" + REGION + "','" + COUNTRY + "','" + DESC_CON_OBJECT + "'";
                        strInsert += ",'" + REG_STR_GROUP + "','" + DEVICE_SR_NUMBER + "','" + TELEPHONE_NO + "','" + MRU + "','" + FUNC_DESCR + "','" + OUTAGE_FROMTIME + "','" + OUTAGE_TOTIME + "','" + LEGACY_ACCT + "','" + BILL_CLASS + "','" + RATE_CAT + "','" + ACTIVITY + "'";
                        strInsert += ",'" + ADR_NOTES + "','" + TEL1_NUMBER + "','" + VERTRAG + "','" + E_MAIL + "','" + MOVE_OUT_DATE + "','" + CON_OBJ_NO + "','" + CLERK_ID + "','" + TEXT + "','" + STATUS + "','" + DISCREASON + "','" + POLE_NO + "')";
                        cmd = new OleDbCommand(strInsert, con);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                string strError = e.Message;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    con.Close();
                }
            }
            return ds;
        }
    }
}