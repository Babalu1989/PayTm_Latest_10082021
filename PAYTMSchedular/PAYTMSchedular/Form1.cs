using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using SAP.Middleware.Connector;
using System.IO;

namespace PAYTMSchedular
{
    public partial class Form1 : Form
    {
        string Doc_id = string.Empty;
        string Status_Code = string.Empty;
        string CA_Number = string.Empty;
        string MeterNo = string.Empty;
        string Trans_Amount = string.Empty;
        string Tarrif_Category = string.Empty;
        string Tarriff_Id = string.Empty;
        string Transaction_Ack = string.Empty;
        string strResponse = string.Empty;
        string Error = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }
        public void HES_DataUpdate()
        {
            string _sPayMethod = string.Empty, _sDocID = string.Empty, _sCANo = string.Empty, _sNetAmt = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = Trans.Paytmdatadetail();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strResponse = string.Empty;
                        if (Checked_TransUpdateData_DateTime(dt.Rows[i]["ENTRY_DATE"].ToString()) == true)
                        {
                            _sPayMethod = dt.Rows[i]["PAYMENTMETHOD"].ToString();
                            _sDocID = dt.Rows[i]["DOC_ID"].ToString();
                            _sCANo = dt.Rows[i]["CA_NUMBER"].ToString();
                            _sNetAmt = dt.Rows[i]["NETAMOUNT"].ToString();


                            if ((_sPayMethod == "ONLINE1") || (_sPayMethod == "ONLINE3"))
                                _sPayMethod = "ONLINE1";

                            try
                            {
                                HES_TEST.Service1 objhes = new HES_TEST.Service1();
                               // HES.Service1 objhes = new HES.Service1();
                                int insertneeded = 0;
                                try
                                {
                                    strResponse = objhes.UpdateBalance(dt.Rows[i]["DOC_ID"].ToString(), dt.Rows[i]["NETAMOUNT"].ToString(), dt.Rows[i]["RECHARGETYPE"].ToString(),
                                            _sPayMethod, dt.Rows[i][0].ToString(), dt.Rows[i]["TRAN_DATE"].ToString(),
                                            dt.Rows[i]["AGENTNO"].ToString(), dt.Rows[i]["CA_NUMBER"].ToString(), dt.Rows[i]["DUPLICATEFLAG"].ToString());
                                    insertneeded = 1;
                                }
                                catch (Exception exx)
                                {
                                    insertneeded = 0;
                                    Trans.InsertService_DataLogs("UpdateBalance", _sPayMethod, _sDocID, _sCANo, _sNetAmt, exx.Message.ToString(), "BRPL", "FAILED");
                                    LogMessageToFile_New(exx.Message, exx.StackTrace, _sPayMethod, _sDocID, _sCANo, _sNetAmt,"HES Server Not Responded");
                                    strResponse = string.Empty;
                                }
                                if (insertneeded > 0)
                                {
                                    if (strResponse.Contains("@"))
                                    {
                                        string[] ParsResponse = strResponse.Split('@');
                                        Doc_id = ParsResponse[0];
                                        Status_Code = ParsResponse[1];
                                        CA_Number = ParsResponse[2];
                                        MeterNo = ParsResponse[3];
                                        Trans_Amount = ParsResponse[4];
                                        Tarrif_Category = ParsResponse[5];
                                        Tarriff_Id = ParsResponse[6];
                                        Transaction_Ack = ParsResponse[7];

                                        Trans.InsertService_DataLogs("UpdateBalance", _sPayMethod, _sDocID, _sCANo, _sNetAmt, strResponse.ToString(), "BRPL", "SUCCESS");
                                    }
                                    else
                                    {
                                        Doc_id = string.Empty;
                                        Status_Code = string.Empty;
                                        CA_Number = string.Empty;
                                        MeterNo = string.Empty;
                                        Trans_Amount = string.Empty;
                                        Tarrif_Category = string.Empty;
                                        Tarriff_Id = string.Empty;
                                        Transaction_Ack = string.Empty;

                                        Trans.InsertService_DataLogs("UpdateBalance", _sPayMethod, _sDocID, _sCANo, _sNetAmt, strResponse.ToString(), "BRPL", "FAILED");

                                    }

                                    if (Doc_id != null && CA_Number != null && Transaction_Ack != null)
                                    {
                                        if (Doc_id.ToString() != "" && CA_Number.ToString() != "" && Transaction_Ack.ToString() != "")
                                        {
                                            Trans.UpdateHES_Details(Doc_id, Status_Code, CA_Number, MeterNo, Trans_Amount, Tarrif_Category, Tarriff_Id, Transaction_Ack);
                                            Trans.Update_Recharge(Doc_id, CA_Number);
                                        }
                                    }
                                }                               

                                
                            }
                            catch (Exception ex)
                            {
                                Trans.InsertService_DataLogs("UpdateBalance", _sPayMethod, _sDocID, _sCANo, _sNetAmt, ex.Message.ToString(), "BRPL", "FAILED");
                                LogMessageToFile_New(ex.Message, ex.StackTrace, _sPayMethod, _sDocID, _sCANo, _sNetAmt,"After or Before HES Hit");
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Trans.InsertService_DataLogs("UpdateBalance", _sPayMethod, _sDocID, _sCANo, _sNetAmt, ex.Message.ToString(), "BRPL", "FAILED");
                Error = ex.Message;
               // LogMessageToFile(ex.Message, ex.StackTrace);
                LogMessageToFile_New(ex.Message, ex.StackTrace, _sPayMethod, _sDocID, _sCANo, _sNetAmt, "First Try Catch");
            }
        }

        public void SAP_DataUpdate()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Trans.Get_SAP_UpdateData();
                DataSet dsOutput = new DataSet();
                string _sDocID = string.Empty;

                string _sCANumber = string.Empty;
                string _sAmount = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //SAP.WebService objsap = new SAP.WebService();
                        //dsOutput = objsap.ZBAPI_FICA_PREPAID_MTR("10200616935", "10200616935", "000150031537", "BRPL", "PREPAID", "",
                        //                             "700228918", "X", "20190101", "", "", "", "", "00", "41430005", "GCC", "1000", "1000",
                        //                             " 123 12 NHP  NEW DELHI 110028", "DH", "18", "PAYTM");

                        _sCANumber = dt.Rows[i]["CANUMBER"].ToString();
                        _sAmount = dt.Rows[i]["TRANS_AMOUNT"].ToString();


                        try
                        {
                            if (dt.Rows[i]["DOCID"].ToString().Length > 12)
                                _sDocID = dt.Rows[i]["DOCID"].ToString().Substring(dt.Rows[i]["DOCID"].ToString().Length - 12, 12);
                            else
                                _sDocID = dt.Rows[i]["DOCID"].ToString();

                            //dsOutput=get_ZBAPI_FICA_PREPAID_MTR(_sDocID, _sDocID, dt.Rows[i]["CANUMBER"].ToString(), dt.Rows[i]["COMPANY"].ToString(), "PREPAID", "GENUS",
                            //                            dt.Rows[i]["CONTRACT"].ToString(),"Y", "","","","","","00",
                            //                            dt.Rows[i]["METERNO"].ToString(), dt.Rows[i]["ACCT_CLASS"].ToString(), dt.Rows[i]["TRANS_AMOUNT"].ToString(),
                            //                            dt.Rows[i]["TRANS_AMOUNT"].ToString(), dt.Rows[i]["ADDRESS"].ToString(),
                            //                            dt.Rows[i]["TARRIF_CATEGORY"].ToString(), dt.Rows[i]["TARRIF_ID"].ToString(), "PAYTM");

                            string _sPaymentMethod = string.Empty;
                            if ((dt.Rows[i]["PAYMENTMETHOD"].ToString() == "ONLINE1") || (dt.Rows[i]["PAYMENTMETHOD"].ToString() == "ONLINE3"))
                                _sPaymentMethod = "ONLINE1";
                            else
                                _sPaymentMethod = dt.Rows[i]["PAYMENTMETHOD"].ToString();

                            dsOutput = get_ZBAPI_FICA_PREPAID_MTR(_sDocID, dt.Rows[i]["TRANSACTIONID"].ToString(), dt.Rows[i]["CANUMBER"].ToString(), dt.Rows[i]["COMPANY"].ToString(), "PREPAID", "GENUS",
                                                      dt.Rows[i]["CONTRACT"].ToString(), "Y", "", "", "", "", "", "00",
                                                      dt.Rows[i]["METERNO"].ToString(), dt.Rows[i]["ACCT_CLASS"].ToString(), dt.Rows[i]["TRANS_AMOUNT"].ToString(),
                                                      dt.Rows[i]["TRANS_AMOUNT"].ToString(), dt.Rows[i]["ADDRESS"].ToString(),
                                                      dt.Rows[i]["TARRIF_CATEGORY"].ToString(), dt.Rows[i]["TARRIF_ID"].ToString()
                                                      , _sPaymentMethod);

                            if (dsOutput.Tables[3].Rows.Count > 0)
                            {
                                if (dsOutput.Tables[3].Rows[0][2] != null)
                                {
                                    // UPDATE SAP FLAG IN PAYTM_HESUPDATEDETAILS TABLE
                                    //Trans.Update_SAP_Data(dt.Rows[i]["DOCID"].ToString(), dt.Rows[i]["CANUMBER"].ToString());

                                    if (Trans.Update_SAP_Data(dt.Rows[i]["DOCID"].ToString(), dt.Rows[i]["CANUMBER"].ToString()) == true)
                                        Trans.Update_SAP_Recharge_Data(dt.Rows[i]["DOCID"].ToString(), dt.Rows[i]["CANUMBER"].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Trans.InsertService_DataLogs("UpdateBalance", "ISU - SAP", _sDocID, _sCANumber, _sAmount, ex.Message.ToString(), "BRPL", "FAILED");
                            LogMessageToFile(ex.Message, ex.StackTrace);

                        }
                        // break;

                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                LogMessageToFile(ex.Message, ex.StackTrace);
            }
        }

        public DataSet get_ZBAPI_FICA_PREPAID_MTR(string strDOC_ID, string strTRANS_ID, string strCA, string strCOMPANY, string strCONSUMER_TYPE,
                     string strMETER_MANFR, string strCONTRACT, string strCA_VALID_ISU, string strENTRY_DATE, string strS_ENC_TKN_1, string strS_ENC_TKN_2,
                                            string strS_ENC_TKN_3, string strS_ENC_TKN_4, string strGENUS_RESP_CODE, string strMETER_NO, string strACC_CLASS,
                            string strAMNT_BANK, string strAMNT_ISU, string strADDRESS, string strTARIFTYP, string strTARIFID, string strPAY_METHOD)
        {
            bool destinationIsInialised = false;

            DataSet dsBAPIOutput = new DataSet("BAPI_RESULT");
            DAL.ZBAPI_FICA_PREPAID_MTR _objOutPut = new DAL.ZBAPI_FICA_PREPAID_MTR();

            string messageText = "";
            string messageCode = "0";
            DataTable[] bapiResult = new DataTable[6];

            DataTable dtIT_Input = new DataTable();
            DataTable dtIT_Output_DUPL = new DataTable();
            DataTable dtIT_Output_NU = new DataTable();
            DataTable dtIT_Output_FIN = new DataTable();
            DataTable dtIT_Return = _objOutPut.formDataTableError();
            DataTable dtMessageText = _objOutPut.makeMessageTextTable();

            try
            {
                DAL.clsConnect cfg = new DAL.clsConnect();

                if (destinationIsInialised == false)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(cfg);
                    destinationIsInialised = true;
                }

                if (destinationIsInialised == true)
                {
                    RfcDestination dest = RfcDestinationManager.GetDestination("mySAPdestination");
                    RfcRepository repo = dest.Repository;
                    IRfcFunction testfn = repo.CreateFunction("ZBAPI_FICA_PREPAID_MTR");

                    RfcStructureMetadata am = repo.GetStructureMetadata("ZFICA_PREPAID_MTR_STR");
                    IRfcStructure articol = am.CreateStructure();
                    IRfcTable artable = am.CreateTable();
                    articol.SetValue("DOC_ID", strDOC_ID);
                    articol.SetValue("TRANS_ID", strTRANS_ID);
                    articol.SetValue("CA", strCA);
                    articol.SetValue("COMPANY", strCOMPANY);
                    articol.SetValue("CONSUMER_TYPE", strCONSUMER_TYPE);
                    articol.SetValue("METER_MANFR", strMETER_MANFR);
                    articol.SetValue("CONTRACT", strCONTRACT);
                    articol.SetValue("CA_VALID_ISU", strCA_VALID_ISU);
                    // articol.SetValue("ENTRY_DATE", DateTime.ParseExact(strENTRY_DATE, "yyyymmdd", CultureInfo.InvariantCulture)); 20190115                   
                    articol.SetValue("ENTRY_DATE", System.DateTime.Now.ToString("yyyyMMdd"));
                    articol.SetValue("S_ENC_TKN_1", strS_ENC_TKN_1);
                    articol.SetValue("S_ENC_TKN_2", strS_ENC_TKN_2);
                    articol.SetValue("S_ENC_TKN_3", strS_ENC_TKN_3);
                    articol.SetValue("S_ENC_TKN_4", strS_ENC_TKN_4);
                    articol.SetValue("GENUS_RESP_CODE", strGENUS_RESP_CODE);
                    articol.SetValue("METER_NO", strMETER_NO);
                    articol.SetValue("ACC_CLASS", strACC_CLASS);
                    articol.SetValue("AMNT_BANK", strAMNT_BANK);
                    articol.SetValue("AMNT_ISU", strAMNT_ISU);
                    articol.SetValue("ADDRESS", strADDRESS);
                    articol.SetValue("TARIFTYP", strTARIFTYP);
                    articol.SetValue("TARIFID", strTARIFID);
                    articol.SetValue("PAY_METHOD", strPAY_METHOD);
                    artable.Insert(articol);

                    testfn.SetValue("IT_INPUT", artable);
                    testfn.Invoke(dest);

                    IRfcTable _IT_INPUT = testfn.GetTable("IT_INPUT");
                    IRfcTable _IT_Output_DUPL = testfn.GetTable("IT_OUTPUT_DUPL");
                    IRfcTable _IT_Output_NU = testfn.GetTable("IT_OUTPUT_NU");
                    IRfcTable _IT_Output_FIN = testfn.GetTable("IT_OUTPUT_FIN");
                    IRfcTable irfcReturn = testfn.GetTable("IT_RETURN");

                    dtIT_Input = _objOutPut.converttodotnetatble(_IT_INPUT);
                    dtIT_Output_DUPL = _objOutPut.converttodotnetatble(_IT_Output_DUPL);
                    dtIT_Output_NU = _objOutPut.converttodotnetatble(_IT_Output_NU);
                    dtIT_Output_FIN = _objOutPut.converttodotnetatble(_IT_Output_FIN);
                    dtIT_Return = _objOutPut.converttodotnetatble(irfcReturn);
                }

            }
            catch (RfcCommunicationException ex)
            {
                messageText = "RfcCommunicationException :" + ex.Message.ToString();
                messageCode = "91";
            }
            catch (RfcLogonException ex)
            {
                messageText = "RfcLogonException :" + ex.Message.ToString();
                messageCode = "92";
            }
            catch (RfcAbapException ex)
            {
                messageText = "RfcAbapException :" + ex.Message.ToString();
                messageCode = "93";
            }
            catch (Exception ex)
            {
                messageText = ex.Message.ToString();
                messageCode = "94";
            }

            if (messageText.Trim() != "")
            {
                _objOutPut.pushMessageTextInDataTable(dtMessageText, messageCode, messageText.Trim());
            }

            bapiResult[0] = dtIT_Input;
            bapiResult[0].TableName = "IT_Input";
            bapiResult[1] = dtIT_Output_DUPL;
            bapiResult[1].TableName = "IT_Output_DUPL";
            bapiResult[2] = dtIT_Output_NU;
            bapiResult[2].TableName = "IT_Output_NU";
            bapiResult[3] = dtIT_Output_FIN;
            bapiResult[3].TableName = "IT_Output_FIN";
            bapiResult[4] = dtIT_Return;
            bapiResult[4].TableName = "IT_Return";
            bapiResult[5] = dtMessageText;
            bapiResult[5].TableName = "MessageText";

            dsBAPIOutput.Tables.Add(bapiResult[0]);
            dsBAPIOutput.Tables.Add(bapiResult[1]);
            dsBAPIOutput.Tables.Add(bapiResult[2]);
            dsBAPIOutput.Tables.Add(bapiResult[3]);
            dsBAPIOutput.Tables.Add(bapiResult[4]);
            dsBAPIOutput.Tables.Add(bapiResult[5]);


            return dsBAPIOutput;
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
                    _iHour = _SpanTime1.Minutes;
                else
                    _iHour = _SpanTime1.Hours + (_SpanTime1.Days * 24);

                if (_iHour > 14)
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

        private bool Checked_TransUpdateData_DateTime(string _sDateTime)
        {
            bool _Result = false;

            TimeSpan _SpanTime1;
            int _iMinutes = 0;

            if (Convert.ToDateTime(System.DateTime.Now) > Convert.ToDateTime(_sDateTime))
            {
                _SpanTime1 = Convert.ToDateTime(System.DateTime.Now) - Convert.ToDateTime(_sDateTime);

                if (_SpanTime1.Days == 0)
                    _iMinutes = _SpanTime1.Minutes;
                else
                    _iMinutes = _SpanTime1.Minutes + (_SpanTime1.Days * 24);

                if (_iMinutes > 1)
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

        private void FAIL_DataUpdate()
        {
            DataTable _dtFailData = new DataTable();
            _dtFailData = Trans.GetFail_TransData();

            if (_dtFailData.Rows.Count > 0)
            {
                for (int i = 0; i < _dtFailData.Rows.Count; i++)
                {
                    if (Checked_TransData_DateTime(_dtFailData.Rows[i]["ENTRY_DATE"].ToString()) == true)
                    {
                        Trans.Update_FAIL_Data(_dtFailData.Rows[i]["TRANSACTIONID"].ToString());
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                HES_DataUpdate();
               
            }
            catch (Exception exx)
            {

            }
            try
            {
                SAP_DataUpdate();

               
            }
            catch (Exception exx)
            {

            }
            try
            {
                FAIL_DataUpdate();
            }
            catch (Exception exx)
            {

            }
           
            Application.Exit();
        }

        public static void LogMessageToFile(string errorMessage, string errorDetails)
        {
            //SendmailDAL sendMail = new SendmailDAL();
            //sendMail.sendmail("gouravgoutam6@gmail.com", "info@bookarity.com", "Bookarity Log", errorMessage + "<br/><br/>" + errorDetails);
            string directory = AppDomain.CurrentDomain.BaseDirectory + "/Logs/";
            if ((!Directory.Exists(Path.GetDirectoryName(directory))))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(directory));
            }

            string file = AppDomain.CurrentDomain.BaseDirectory + "/Logs/" + "Exception_Log" + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".txt";
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            using (StreamWriter w = File.AppendText(file))
            {
                w.WriteLine("Log Entry : ");
                w.WriteLine("Team Name: " + "Babalu");
                w.WriteLine("Error Time: " + DateTime.Now);
                w.WriteLine("Error Message: " + errorMessage);
                w.WriteLine("Error Details: " + errorDetails);
                w.WriteLine("_____________________________________________________________________");
                w.Flush();
                w.Close();
            }

        }

        public static void LogMessageToFile_New(string errorMessage, string errorDetails, string paymode, string docid,string cano,string amount,string reason)
        {
            //SendmailDAL sendMail = new SendmailDAL();
            //sendMail.sendmail("gouravgoutam6@gmail.com", "info@bookarity.com", "Bookarity Log", errorMessage + "<br/><br/>" + errorDetails);
            string directory = AppDomain.CurrentDomain.BaseDirectory + "/Logs/";
            if ((!Directory.Exists(Path.GetDirectoryName(directory))))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(directory));
            }

            string file = AppDomain.CurrentDomain.BaseDirectory + "/Logs/" + "Exception_Log" + "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ".txt";
            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            using (StreamWriter w = File.AppendText(file))
            {
                w.WriteLine("Log Entry : ");
                w.WriteLine("Team Name: " + "Sajid");
                w.WriteLine("Error Place: " + reason);
                w.WriteLine("Pay Method: " + paymode);
                w.WriteLine("Doc ID: " +docid);
                w.WriteLine("CA No: " + cano);
                w.WriteLine("Amount: " + amount);
                w.WriteLine("Error Time: " + DateTime.Now);
                w.WriteLine("Error Message: " + errorMessage);
                w.WriteLine("Error Details: " + errorDetails);
                w.WriteLine("_____________________________________________________________________");
                w.Flush();
                w.Close();
            }

        }
    }
}
