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
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        OleDbCommand cmd;
        OleDbConnection con;
        OleDbDataAdapter ada;
        string KEYVALUE = string.Empty;
        string AGENCY = string.Empty;
        public string strConnection = NDS.con();


        [WebMethod]
        public DataSet PAYTM_CA_DETAILS(string _sInputString)
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

            string KEY = string.Empty;
            string AGENCYNAME = string.Empty;
            string CANUMBER = string.Empty;
            string AMOUNT = string.Empty;
            string TRANS_ID = string.Empty;
            string FLAG = string.Empty;
            string _sStringDecpData = string.Empty;
            string _sTransFlag = string.Empty;
            try
            {
                CryptFunction objCrypt = new CryptFunction();
                _sStringDecpData = objCrypt.Decrypt(_sInputString);
                string[] String_Decp_LST = _sStringDecpData.Split('|');
                if (String_Decp_LST.Length > 5)
                {
                    KEY = String_Decp_LST[0];
                    AGENCYNAME = String_Decp_LST[1];
                    CANUMBER = String_Decp_LST[2];
                    AMOUNT = String_Decp_LST[3];
                    TRANS_ID = String_Decp_LST[4];
                    FLAG = String_Decp_LST[5];
                    Tranaction_Service_LogData(KEY, AGENCYNAME, CANUMBER, AMOUNT, TRANS_ID, FLAG, _sInputString);
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

                                if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() == "GENUS"))
                                {
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
                                        //Address = "XXXXXXXXXX";
                                        ds.Tables[0].Rows[0]["ADDRESS"] = "XXXXXXXXXX";
                                    }
                                    else
                                    {
                                        Address = "";
                                    }
                                    if (ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() != "")
                                    {
                                        Mobile = ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString();
                                        // Mobile = "XXXXXXXXXX";
                                        ds.Tables[0].Rows[0]["TEL1_NUMBR"] = "XXXXXXXXXX";
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
                                        Docid = Docid.Substring(5, Docid.Length - 5);
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
                                        Manufacture = ""; //GENUS
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
                                else
                                {
                                    ds.Tables[0].Rows[0]["FLAG"] = "N";
                                    ds.Tables[0].Rows[0]["MESSAGE"] = "CA does not belong to GENUS Meter";

                                    //dt.Rows.Add("CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter",
                                    //            "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter",
                                    //            "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter",
                                    //            "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter");

                                    dt.AcceptChanges();
                                    ds.Tables.Add(dt);
                                }
                            }
                            else if (!string.IsNullOrEmpty(CANUMBER) && !string.IsNullOrEmpty(AMOUNT) && !string.IsNullOrEmpty(TRANS_ID) && FLAG == "R")
                            {
                                _sTransFlag = Check_Transaction_BSES(TRANS_ID);

                                if (_sTransFlag == "R")
                                {
                                    ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, FLAG);

                                    if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() == "GENUS"))
                                    {
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
                                            //Address = "XXXXXXXXXX";
                                            ds.Tables[0].Rows[0]["ADDRESS"] = "XXXXXXXXXX";
                                        }
                                        else
                                        {
                                            Address = "";
                                        }
                                        if (ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() != "")
                                        {
                                            Mobile = ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString();
                                            //Mobile = "XXXXXXXXXX";
                                            ds.Tables[0].Rows[0]["TEL1_NUMBR"] = "XXXXXXXXXX";
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
                                            Docid = Docid.Substring(5, Docid.Length - 5);
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
                                            if (AGENCY.Trim() == "PAYTM")
                                                AGENCY = "ONLINE2";
                                            else if (AGENCY.Trim() == "BILLDESK")
                                                AGENCY = "ONLINE1";
                                            else if (AGENCY.Trim() == "BILLDESKSDK")
                                                AGENCY = "ONLINE3";

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
                                                        Flag = "Y";
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
                                                catch (Exception ex)
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
                                                Flag = "N";
                                                ds.Tables[0].Rows[0][12] = Flag;
                                                ds.Tables[0].Rows[0][13] = Message;
                                                ds.AcceptChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ds.Tables[0].Rows[0]["FLAG"] = "N";
                                        ds.Tables[0].Rows[0]["MESSAGE"] = "CA does not belong to GENUS Meter";

                                        //dt.Rows.Add("CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter",
                                        //            "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", 
                                        //            "CA does not belong to GENUS Meter","CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter", 
                                        //            "CA does not belong to GENUS Meter", "CA does not belong to GENUS Meter");
                                        dt.AcceptChanges();
                                        ds.Tables.Add(dt);
                                    }
                                }
                                else if (_sTransFlag == "F")
                                {
                                    ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, "C");
                                    ds.Tables[0].Rows[0]["FLAG"] = "F";
                                    ds.Tables[0].Rows[0]["MESSAGE"] = "Transaction Failure";
                                    dt.AcceptChanges();
                                    ds.Tables.Add(dt);
                                }
                                else if (_sTransFlag == "P")
                                {
                                    ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, "C");
                                    ds.Tables[0].Rows[0]["FLAG"] = "P";
                                    ds.Tables[0].Rows[0]["MESSAGE"] = "Transaction Pending";
                                    dt.AcceptChanges();
                                    ds.Tables.Add(dt);
                                }
                                else if ((_sTransFlag == "H") || (_sTransFlag == "S"))
                                {
                                    ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", "000" + CANUMBER, "PRE", AMOUNT, "C");
                                    ds.Tables[0].Rows[0]["FLAG"] = "H";
                                    ds.Tables[0].Rows[0]["MESSAGE"] = "Meter Recharge Successful";
                                    dt.AcceptChanges();
                                    ds.Tables.Add(dt);
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

                        dt.Rows.Add("Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                      "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                      "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                      "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!");

                        dt.AcceptChanges();
                        ds.Tables.Add(dt);
                    }
                    finally
                    {
                        if (con != null)
                        {
                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }
                    }
                }
                else
                {
                    dt.Rows.Add("Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                        "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                        "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                        "Invalid Input Parameter", "Invalid Input Parameter");
                    dt.AcceptChanges();
                    ds.Tables.Add(dt);
                }
            }
            catch
            {
                dt.Rows.Add("Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter");
                dt.AcceptChanges();
                ds.Tables.Add(dt);
            }

            try
            {
                LogError(ds.Tables[0], KEY, AGENCYNAME, CANUMBER, AMOUNT, TRANS_ID);
            }
            catch (Exception exx)
            {

            }


            return ds;
        }

        private string Check_Transaction_BSES(string _sTransactionID)
        {
            string _sCheckFlag = string.Empty;
            string strInsert = string.Empty;

            con = new OleDbConnection(strConnection);
            con.Open();
            string Querycheck = " SELECT ENTRY_DATE,STATUS FROM PAYTM_RECHARGEDATA WHERE TRANSACTIONID='" + _sTransactionID + "'";
            ada = new OleDbDataAdapter(Querycheck, con);
            DataTable dt1 = new DataTable();
            ada.Fill(dt1);

            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["STATUS"].ToString() == "R")
                {
                    if (Checked_TransData_DateTime(dt1.Rows[0]["ENTRY_DATE"].ToString()) == true)
                    {
                        strInsert = "UPDATE PAYTM_RECHARGEDATA SET STATUS='F',FAIL_UPDATE_DATE=SYSDATE WHERE TRANSACTIONID='" + _sTransactionID + "'";

                        try
                        {
                            cmd = new OleDbCommand(strInsert, con);
                            int i = cmd.ExecuteNonQuery();

                            _sCheckFlag = "F";
                        }
                        catch (Exception ex)
                        {
                            _sCheckFlag = "P";
                        }
                    }
                    else
                    {
                        _sCheckFlag = "P";
                    }
                }
                if (dt1.Rows[0]["STATUS"].ToString() == "H")
                {
                    _sCheckFlag = "H";
                }
                else if (dt1.Rows[0]["STATUS"].ToString() == "S")
                {
                    _sCheckFlag = "S";
                }
                else if (dt1.Rows[0]["STATUS"].ToString() == "F")
                {
                    _sCheckFlag = "F";
                }

                return _sCheckFlag;
            }
            else
            {
                _sCheckFlag = "R";
                return _sCheckFlag;
            }
        }

        private bool Checked_TransData_DateTime(string _sDateTime)
        {
            bool _Result = false;

            TimeSpan _SpanTime1;
            int _iHour = 0;

            if (Convert.ToDateTime(System.DateTime.Now) > Convert.ToDateTime(_sDateTime))
            {
                _SpanTime1 = Convert.ToDateTime(System.DateTime.Now) - Convert.ToDateTime(_sDateTime);

                if (_SpanTime1.Days == 0)
                    _iHour = _SpanTime1.Hours;
                else
                    _iHour = _SpanTime1.Hours + (_SpanTime1.Days * 24);

                if (_iHour > 0)
                    _Result = true;
                else
                    _Result = false;
            }
            else
            {
                _Result = false;
            }
            return _Result;
        }

        [WebMethod]
        public string PAYTM_ENCRPT_DATA(string _sInputStringData)
        {
            string _sStringEncrptData = string.Empty;
            CryptFunction objCrypt = new CryptFunction();

            try
            {
                _sStringEncrptData = objCrypt.Encrypt(_sInputStringData);
                return _sStringEncrptData;
            }
            catch
            {
                return "Error";
            }

        }

        [WebMethod]
        public DataSet PAYTM_CA_DETAILS_BILLDESK_SDK(string KEY, string AGENCY, string CANUMBER, string AMOUNT, string COMPANY, string FLAG)
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
            string MessageKey = "";

            string AGENCYNAME = "";

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

            try
            {

                ISU.WebService Pre = new ISU.WebService();
                try
                {
                    if (KEY != "" && AGENCY != "")
                    {
                        con = new OleDbConnection(strConnection);
                        con.Open();
                        string Querycheck = "SELECT KEYID,AGENCYNAME FROM PREPAID_AGENCYKEY WHERE KEYID='" + KEY + "' AND UPPER(AGENCYNAME)='" + AGENCY.ToUpper() + "'";
                        ada = new OleDbDataAdapter(Querycheck, con);
                        DataTable dt2 = new DataTable();
                        ada.Fill(dt2);
                        if (dt2.Rows.Count > 0)
                        {
                            KEYVALUE = dt2.Rows[0]["KEYID"].ToString();
                            AGENCYNAME = dt2.Rows[0]["AGENCYNAME"].ToString();
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

                            SHASample _obj = new SHASample();
                            string StrTransID = "PRE" + CANUMBER + _obj.Get8Digits();

                            //string StrCheckSumLogic = _obj.GetCheckSumLogic(CANUMBER, AMOUNT, StrTransID, COMPANY, Serialno);

                            //StrCheckSumLogic = StrCheckSumLogic.ToUpper();



                            if (ds.Tables[0].Rows[0]["MESSAGE"].ToString() != "")
                            {
                                //Message = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
                                //MessageKey = "BSESRPREP|" + CANUMBER + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http:// www.domain.com/response.jsp|TOEB50N43blx";
                                //MessageKey = "BSESRPREP|" + CANUMBER + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx|" + StrCheckSumLogic + "";
                                MessageKey = //"BSESRPREP|" + StrTransID + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|" + COMPANY + "|R|" + CANUMBER + "|" + Serialno.TrimStart('0') + "|APP|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx|" + StrCheckSumLogic + "";
                                             "BSESRPREP|" + StrTransID + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|" + COMPANY + "|R|" + CANUMBER + "|" + Serialno.TrimStart('0') + "|APP|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx";

                                string StrCheckSumLogic = _obj.GetCheckSumLogic(MessageKey);

                                StrCheckSumLogic = StrCheckSumLogic.ToUpper();

                                MessageKey = MessageKey + "|" + StrCheckSumLogic + ""; //"BSESRPREP|" + StrTransID + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|" + COMPANY + "|R|" + CANUMBER + "|" + Serialno.TrimStart('0') + "|APP|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx|" + StrCheckSumLogic + "";

                                ds.Tables[0].Rows[0]["MESSAGE"] = MessageKey;
                            }
                            else
                            {
                                Message = "";
                            }


                            if (Flag.Contains("Y"))
                            {
                                con = new OleDbConnection(strConnection);
                                con.Open();
                                string strInsert = "INSERT INTO   PAYTM_CHECKBAPIDATA (COMPANY,CONTRACT,CA_NUMBER,NETAMOUNT,NAME,ADDRESS,MOBILE,E_MAIL,SERIALNO,ACCTDET_ID,ACCT_CLASS,DOC_ID,MANUFACTURE,FLAG,MESSAGE,MESSAGE_KEY)";
                                strInsert += "VALUES ('" + Companycode + "','" + Contract + "','" + CANumber + "','" + AMOUNT + "','" + Name + "','" + Address + "','" + Mobile + "','" + Email + "','" + Serialno + "','" + Acc_det_Id + "','" + ACCTCLASS + "','" + Docid + "','" + Manufacture + "','" + Flag + "','" + Message + "','" + MessageKey + "')";
                                cmd = new OleDbCommand(strInsert, con);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (ds.Tables.Count <= 0)
                        {
                            dt.Rows.Add("Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available",
                                "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available",
                                "Record Not Available", "Record Not Available", "Record Not Available", "Record Not Available",
                                "Record Not Available", "Record Not Available");
                            dt.AcceptChanges();
                            ds.Tables.Add(dt);
                        }
                    }
                    else
                    {
                        dt.Rows.Add("Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!",
                                    "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!",
                                    "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!",
                                    "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!",
                                    "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!", "Invalid key Or Agenecy Name!");

                        dt.AcceptChanges();
                        ds.Tables.Add(dt);
                    }
                }
                catch (Exception ex)
                {
                    string strError = ex.Message;

                    dt.Rows.Add("Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                  "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                  "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!",
                  "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!", "Technical Issue Occured! Please Try Again!");

                    dt.AcceptChanges();
                    ds.Tables.Add(dt);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }

            }
            catch
            {
                dt.Rows.Add("Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter",
                       "Invalid Input Parameter", "Invalid Input Parameter", "Invalid Input Parameter");
                dt.AcceptChanges();
                ds.Tables.Add(dt);
            }

            try
            {
                LogError(dt, KEY, AGENCYNAME, CANUMBER, AMOUNT, "");
            }
            catch (Exception exx)
            {

            }


            return ds;
        }

        private void Tranaction_Service_LogData(string _sKey, string _sAgencyName, string _sCANo, string _sAmount,
                                                                     string _sTransID, string _sFlag, string _sInputString)
        {
            using (con = new OleDbConnection(strConnection))
            {
                string strInsert = "INSERT INTO RECHARGESERVICE_LOG(INPUT_KEY,AGENCY_NAME,CA_NUMBER,AMOUNT,TRANS_ID,FLAG,INPUT_DATA)";
                strInsert += "VALUES ('" + _sKey + "','" + _sAgencyName + "','" + _sCANo + "','" + _sAmount + "','" + _sTransID + "','" + _sFlag + "','" + _sInputString + "')";

                cmd = new OleDbCommand(strInsert, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public string RECHARGE_CA_String(string _sString)
        {
            string Result = string.Empty;
            string _sKey = string.Empty, _sAgencyName = string.Empty;
            string Querycheck = string.Empty;

            if (_sString.ToString() != "")
            {
                try
                {
                    ISU.WebService Pre = new ISU.WebService();
                    string[] LineString = _sString.Split('|');

                    if (LineString[0].ToString() != "" && LineString[1].ToString() != "")
                    {
                        con = new OleDbConnection(strConnection);
                        con.Open();
                        Querycheck = "SELECT KEYID,AGENCYNAME FROM PREPAID_AGENCYKEY WHERE KEYID='" + LineString[0].ToString() + "' AND UPPER(AGENCYNAME)='" + LineString[1].ToString().ToUpper() + "'";
                        ada = new OleDbDataAdapter(Querycheck, con);
                        DataTable _dtCheck = new DataTable();
                        ada.Fill(_dtCheck);

                        if (_dtCheck.Rows.Count > 0)
                        {
                            _sKey = _dtCheck.Rows[0]["KEYID"].ToString();
                            _sAgencyName = _dtCheck.Rows[0]["AGENCYNAME"].ToString();
                        }
                        else

                            Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "P", "Agency Key/Name is incorrect.");

                        if (_sKey == LineString[0].ToString() && _sAgencyName.ToUpper() == LineString[1].ToString().ToUpper())
                        {
                            #region Validate Function
                            if (!string.IsNullOrEmpty(LineString[2].ToString()) && !string.IsNullOrEmpty(LineString[3].ToString()) && LineString[5].ToString() == "C")
                            {
                                DataSet _ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", LineString[2].ToString(), "PRE", LineString[3].ToString(), LineString[5].ToString());
                                if (_ds.Tables[0].Rows.Count > 0)
                                {
                                    if (_ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() == "GENUS")
                                    {
                                        if (_ds.Tables[0].Rows[0]["Message"].ToString().Contains("Check successful")
                                            && _ds.Tables[0].Rows[0]["Flag"].ToString().Contains("Y"))
                                        {
                                            con = new OleDbConnection(strConnection);
                                            con.Open();
                                            string strInsert = "INSERT INTO PAYTM_CHECKBAPIDATA (COMPANY,CONTRACT,CA_NUMBER,NETAMOUNT,NAME,ADDRESS,MOBILE,E_MAIL,SERIALNO,ACCTDET_ID,ACCT_CLASS,DOC_ID,MANUFACTURE,FLAG,MESSAGE)";
                                            strInsert += "VALUES ('" + _ds.Tables[0].Rows[0]["COMP_CODE"].ToString() + "','" + _ds.Tables[0].Rows[0]["CONTRACT"].ToString() + "','" + _ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString() + "'," +
                                                "'" + LineString[3].ToString() + "','" + _ds.Tables[0].Rows[0]["NAME"].ToString() + "','" + _ds.Tables[0].Rows[0]["ADDRESS"].ToString() + "','" + _ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() + "'," +
                                                "'" + _ds.Tables[0].Rows[0]["E_MAIL"].ToString() + "','" + _ds.Tables[0].Rows[0]["SERIALNO"].ToString() + "','" + _ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString() + "'," +
                                                "'" + _ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString() + "','" + _ds.Tables[0].Rows[0]["DOC_ID"].ToString() + "','" + _ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() + "','" + _ds.Tables[0].Rows[0]["FLAG"].ToString() + "','" + _ds.Tables[0].Rows[0]["Message"].ToString() + "')";
                                            cmd = new OleDbCommand(strInsert, con);
                                            cmd.ExecuteNonQuery();

                                            Result = bindResultString(_ds.Tables[0].Rows[0]["COMP_CODE"].ToString(), _ds.Tables[0].Rows[0]["CONTRACT"].ToString(), _ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString(), _ds.Tables[0].Rows[0]["NAME"].ToString(), _ds.Tables[0].Rows[0]["ADDRESS"].ToString(), _ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString(), _ds.Tables[0].Rows[0]["E_MAIL"].ToString(), _ds.Tables[0].Rows[0]["SERIALNO"].ToString(), _ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString(),
                                                _ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString(), _ds.Tables[0].Rows[0]["DOC_ID"].ToString(), _ds.Tables[0].Rows[0]["MANUFACTURER"].ToString(), _ds.Tables[0].Rows[0]["FLAG"].ToString(), _ds.Tables[0].Rows[0]["Message"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "N", "No Data Found against CA No: " + LineString[2].ToString());
                                    }
                                }
                                else
                                {
                                    Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "N", "No Data Found against CA No: " + LineString[2].ToString());
                                }
                            }
                            #endregion

                            #region Recharge Function

                            if (!string.IsNullOrEmpty(LineString[2].ToString()) && !string.IsNullOrEmpty(LineString[3].ToString())
                                && !string.IsNullOrEmpty(LineString[4].ToString()) && LineString[5].ToString() == "R")
                            {
                                con = new OleDbConnection(strConnection);
                                con.Open();
                                Querycheck = "SELECT * FROM PAYTM_RECHARGEDATA WHERE PAYMENTMETHOD='" + _sAgencyName + "' AND TRANSACTIONID='" + LineString[4].ToString() + "'";
                                ada = new OleDbDataAdapter(Querycheck, con);
                                DataTable _dtCheckTrans = new DataTable();
                                ada.Fill(_dtCheckTrans);
                                if (_dtCheckTrans.Rows.Count == 0)
                                {
                                    DataSet _ds = Pre.ZBAPI_PREPAID_RTGS("BRPL", LineString[2].ToString(), "PRE", LineString[3].ToString(), LineString[5].ToString());
                                    if (_ds.Tables[0].Rows.Count > 0)
                                    {
                                        if (_ds.Tables[0].Rows[0]["Message"].ToString().Contains("Successful") && _ds.Tables[0].Rows[0]["Flag"].ToString().Contains("Y"))
                                        {
                                            try
                                            {
                                                if (AGENCY.Trim() == "PAYTM")
                                                    AGENCY = "ONLINE2";
                                                else if (AGENCY.Trim() == "BILLDESK")
                                                    AGENCY = "ONLINE1";
                                                else if (AGENCY.Trim() == "SBI")
                                                    AGENCY = "RTGS1";

                                                string strInsert = "INSERT INTO PAYTM_RECHARGEDATA(COMPANY,CONTRACT,CA_NUMBER,NAME,ADDRESS,MOBILE,E_MAIL,SERIALNO,ACCTDET_ID,ACCT_CLASS,DOC_ID,NETAMOUNT,TRANSACTIONID,MANUFACTURE,FLAG,MESSAGE,AGENTNO,PAYMENTMETHOD,RECHARGETYPE,DUPLICATEFLAG)";
                                                strInsert += "VALUES ('" + _ds.Tables[0].Rows[0]["COMP_CODE"].ToString() + "','" + _ds.Tables[0].Rows[0]["CONTRACT"].ToString() + "','" + _ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString() + "','" + _ds.Tables[0].Rows[0]["NAME"].ToString() + "'," +
                                                    "'" + _ds.Tables[0].Rows[0]["ADDRESS"].ToString() + "','" + _ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString() + "','" + _ds.Tables[0].Rows[0]["E_MAIL"].ToString() + "','" + _ds.Tables[0].Rows[0]["SERIALNO"].ToString() + "','" + _ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString() + "'," +
                                                    "'" + _ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString() + "','" + _ds.Tables[0].Rows[0]["DOC_ID"].ToString() + "','" + LineString[3].ToString() + "','" + LineString[4].ToString() + "','" + _ds.Tables[0].Rows[0]["MANUFACTURER"].ToString() + "','" + _ds.Tables[0].Rows[0]["FLAG"].ToString() + "'," +
                                                    "'" + _ds.Tables[0].Rows[0]["Message"].ToString() + "','AUTO','" + _sAgencyName + "','1','N')";
                                                cmd = new OleDbCommand(strInsert, con);
                                                int i = cmd.ExecuteNonQuery();
                                                if (i > 0)
                                                {
                                                    Result = bindResultString(_ds.Tables[0].Rows[0]["COMP_CODE"].ToString(), _ds.Tables[0].Rows[0]["CONTRACT"].ToString(), _ds.Tables[0].Rows[0]["CONTRACT_ACCOUNT"].ToString(), _ds.Tables[0].Rows[0]["NAME"].ToString(),
                                                    _ds.Tables[0].Rows[0]["ADDRESS"].ToString(), _ds.Tables[0].Rows[0]["TEL1_NUMBR"].ToString(), _ds.Tables[0].Rows[0]["E_MAIL"].ToString(), _ds.Tables[0].Rows[0]["SERIALNO"].ToString(), _ds.Tables[0].Rows[0]["ACCT_DET_ID"].ToString(),
                                                    _ds.Tables[0].Rows[0]["ACCT_CLASS"].ToString(), _ds.Tables[0].Rows[0]["DOC_ID"].ToString(), _ds.Tables[0].Rows[0]["MANUFACTURER"].ToString(), _ds.Tables[0].Rows[0]["FLAG"].ToString(), LineString[4].ToString());
                                                }
                                                else
                                                {
                                                    Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "M", "Data Not Save.");
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "R", "WEB SERVICE EXCEPTION.");
                                            }
                                        }
                                    }
                                    else
                                        Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "N", "No Data Found against CA No: " + LineString[2].ToString());
                                }
                                else
                                {
                                    if (_dtCheckTrans.Rows[0]["STATUS"].ToString() == "R")
                                        Result = bindResultString(_dtCheckTrans.Rows[0]["COMPANY"].ToString(), _dtCheckTrans.Rows[0]["CONTRACT"].ToString(), _dtCheckTrans.Rows[0]["CA_NUMBER"].ToString(), _dtCheckTrans.Rows[0]["NAME"].ToString(),
                                       _dtCheckTrans.Rows[0]["ADDRESS"].ToString(), _dtCheckTrans.Rows[0]["MOBILE"].ToString(), _dtCheckTrans.Rows[0]["E_MAIL"].ToString(), _dtCheckTrans.Rows[0]["SERIALNO"].ToString(), _dtCheckTrans.Rows[0]["ACCTDET_ID"].ToString(),
                                       _dtCheckTrans.Rows[0]["ACCT_CLASS"].ToString(), _dtCheckTrans.Rows[0]["DOC_ID"].ToString(), _dtCheckTrans.Rows[0]["MANUFACTURE"].ToString(), "P", "Pending");
                                    else if (_dtCheckTrans.Rows[0]["STATUS"].ToString() == "F")
                                        Result = bindResultString(_dtCheckTrans.Rows[0]["COMPANY"].ToString(), _dtCheckTrans.Rows[0]["CONTRACT"].ToString(), _dtCheckTrans.Rows[0]["CA_NUMBER"].ToString(), _dtCheckTrans.Rows[0]["NAME"].ToString(),
                                        _dtCheckTrans.Rows[0]["ADDRESS"].ToString(), _dtCheckTrans.Rows[0]["MOBILE"].ToString(), _dtCheckTrans.Rows[0]["E_MAIL"].ToString(), _dtCheckTrans.Rows[0]["SERIALNO"].ToString(), _dtCheckTrans.Rows[0]["ACCTDET_ID"].ToString(),
                                        _dtCheckTrans.Rows[0]["ACCT_CLASS"].ToString(), _dtCheckTrans.Rows[0]["DOC_ID"].ToString(), _dtCheckTrans.Rows[0]["MANUFACTURE"].ToString(), "F", "Transaction Failure");
                                    else if (_dtCheckTrans.Rows[0]["STATUS"].ToString() == "H")
                                        Result = bindResultString(_dtCheckTrans.Rows[0]["COMPANY"].ToString(), _dtCheckTrans.Rows[0]["CONTRACT"].ToString(), _dtCheckTrans.Rows[0]["CA_NUMBER"].ToString(), _dtCheckTrans.Rows[0]["NAME"].ToString(),
                                        _dtCheckTrans.Rows[0]["ADDRESS"].ToString(), _dtCheckTrans.Rows[0]["MOBILE"].ToString(), _dtCheckTrans.Rows[0]["E_MAIL"].ToString(), _dtCheckTrans.Rows[0]["SERIALNO"].ToString(), _dtCheckTrans.Rows[0]["ACCTDET_ID"].ToString(),
                                        _dtCheckTrans.Rows[0]["ACCT_CLASS"].ToString(), _dtCheckTrans.Rows[0]["DOC_ID"].ToString(), _dtCheckTrans.Rows[0]["MANUFACTURE"].ToString(), "S", "Successful");
                                }
                            }
                            #endregion
                        }
                        else
                            Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "N", "No Data Found against CA No: " + LineString[2].ToString());
                    }
                    else
                        Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "Q", "Agency Key/Name could not be null.");

                }
                catch (Exception e)
                {
                    Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "R", "Web Server Exception.");
                }
                finally
                {

                }
            }
            else
            {
                Result = bindResultString("", "", "", "", "", "", "", "", "", "", "", "", "B", "You Could not Pass Blank Value.");
            }

            return Result;
        }

        public string bindResultString(string _sCOMP_CODE, string _sCONTRACT, string _sCONTRACT_ACCOUNT, string _sNAME,
                                       string _sADDRESS, string _sTEL1_NUMBR, string _sE_MAIL, string _sSERIALNO,
                                       string _sACCT_DET_ID, string _sACCT_CLASS, string _sDOC_ID, string _sMANUFACTURER,
                                       string _sFLAG, string _sMESSAGE)
        {
            string _result = string.Empty;
            _result = _sCOMP_CODE + "|" + _sCONTRACT + "|" + _sCONTRACT_ACCOUNT + "|" + _sNAME + "|" + _sADDRESS + "|" + _sTEL1_NUMBR + "|" + _sE_MAIL + "|" + _sSERIALNO + "|" + _sACCT_DET_ID + "|" + _sACCT_CLASS + "|" + _sDOC_ID + "|" + _sMANUFACTURER + "|" + _sFLAG + "|" + _sMESSAGE;
            return _result;
        }

        private void LogError(DataTable dt, string KEY, string AGENCYNAME, string CANUMBER, string AMOUNT, string TRANS_ID)
        {


            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += string.Format(" || KEY: {0}", KEY);
            message += string.Format(" || AGENCYNAME: {0}", AGENCYNAME);
            message += string.Format(" || CANUMBER: {0}", CANUMBER);
            message += string.Format(" || AMOUNT: {0}", AMOUNT);
            message += string.Format(" || TRANS_ID: {0}", TRANS_ID);
            message += string.Format(" || COMP_CODE: {0}", Convert.ToString(dt.Rows[0]["COMP_CODE"]));
            message += string.Format(" || CONTRACT: {0}", Convert.ToString(dt.Rows[0]["CONTRACT"]));
            message += string.Format(" || CONTRACT_ACCOUNT: {0}", Convert.ToString(dt.Rows[0]["CONTRACT_ACCOUNT"]));
            message += string.Format(" || NAME: {0}", Convert.ToString(dt.Rows[0]["NAME"]));
            message += string.Format(" || ADDRESS: {0}", Convert.ToString(dt.Rows[0]["ADDRESS"]));
            message += string.Format(" || TEL1_NUMBR: {0}", Convert.ToString(dt.Rows[0]["TEL1_NUMBR"]));
            message += string.Format(" || E_MAIL: {0}", Convert.ToString(dt.Rows[0]["E_MAIL"]));
            message += string.Format(" || SERIALNO: {0}", Convert.ToString(dt.Rows[0]["SERIALNO"]));
            message += string.Format(" || ACCT_DET_ID: {0}", Convert.ToString(dt.Rows[0]["ACCT_DET_ID"]));
            message += string.Format(" || ACCT_CLASS: {0}", Convert.ToString(dt.Rows[0]["ACCT_CLASS"]));
            message += string.Format(" || DOC_ID: {0}", Convert.ToString(dt.Rows[0]["DOC_ID"]));
            message += string.Format(" || MANUFACTURER: {0}", Convert.ToString(dt.Rows[0]["MANUFACTURER"]));
            message += string.Format(" || FLAG: {0}", Convert.ToString(dt.Rows[0]["FLAG"]));
            message += string.Format(" || MESSAGE: {0}", Convert.ToString(dt.Rows[0]["MESSAGE"]));
            message += Environment.NewLine;

            string filename = DateTime.Now.ToString("ddMMyyyy") + "Log.txt";
            string path = @"E:\e drive\PrepaidService\Log\" + filename;// Server.MapPath("~/Logs/"+filename);
            string path1 = @"E:\e drive\PrepaidService\Log\";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}