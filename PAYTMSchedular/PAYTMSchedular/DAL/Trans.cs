using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using System.Net;

public class Trans
    {
        private static Trans instance;
        public static OleDbConnection con;
        public static OleDbCommand cmd;
        public static OleDbTransaction ot;
        public static int result;
        private static DataTable dt;
        public static string sql = string.Empty;
        public static Trans Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Trans();
                }
                return instance;
            }
        }

    public static DataTable Paytmdatadetail()
    {
        string Selectquery = "SELECT COMPANY, CONTRACT, CA_NUMBER, NAME, ADDRESS, MOBILE, E_MAIL, SERIALNO, ACCTDET_ID, ACCT_CLASS, DOC_ID, NETAMOUNT, MANUFACTURE, FLAG, MESSAGE, AGENTNO, PAYMENTMETHOD, RECHARGETYPE, DUPLICATEFLAG, STATUS,  TO_CHAR(ENTRY_DATE,'dd/mm/yyyy') TRAN_DATE, ENTRY_DATE FROM PAYTM_RECHARGEDATA WHERE STATUS='R'  ";

        dt = new DataTable();
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Selectquery;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
        }
        catch (OleDbException ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return dt;
    }

    public static DataTable Get_SAP_UpdateData()
    {
        
        string Selectquery = " SELECT CONTRACT,ADDRESS,ACCT_CLASS,H.COMPANY, DOCID, CANUMBER, METERNO, TRANS_AMOUNT, STATUS_CODE, TARRIF_CATEGORY,TARRIF_ID, TRANSACTION_ACK, ";
        Selectquery += " SAP_STATUS, H.ENTRY_DATE,PAYMENTMETHOD,TRANSACTIONID FROM PAYTM_HESUPDATEDETAILS H, PAYTM_RECHARGEDATA R WHERE SAP_STATUS='N' AND STATUS ='H' AND H.DOCID=R.DOC_ID ";

        dt = new DataTable();
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Selectquery;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
        }
        catch (OleDbException ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return dt;
    }

    public static bool UpdateHES_Details(string strDocid,string strStatuscode,string strCAnumber,string strMeterno,string strAmmount,string strTarrifcate,
                                            string strTarrifid,string strTransAck)
    {
        string Insertquery = "INSERT INTO PAYTM_HESUPDATEDETAILS (COMPANY,DOCID,CANUMBER,METERNO,TRANS_AMOUNT,STATUS_CODE,TARRIF_CATEGORY,TARRIF_ID,TRANSACTION_ACK)";
        Insertquery += "VALUES ('BRPL','"+strDocid+"','"+strCAnumber+"','"+strMeterno+"','"+strAmmount+"','"+strStatuscode+"','"+strTarrifcate+"','"+strTarrifid+"','"+strTransAck+"')";
        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Insertquery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }

    public static bool Update_Recharge(string strDocid, string strCAnumber)
    {
        //string Updatequery = "UPDATE PAYTM_RECHARGEDATA SET STATUS='H' WHERE CA_NUMBER='"+strCAnumber+"' AND DOC_ID='"+strDocid+"'";
        string Updatequery = "UPDATE PAYTM_RECHARGEDATA SET STATUS='H',HES_UPDATE_DATE=SYSDATE WHERE CA_NUMBER='" + strCAnumber + "' AND DOC_ID='" + strDocid + "'";

        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Updatequery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }

    public static bool Update_SAP_Recharge_Data(string strDocid, string strCAnumber)
    {
        //string Updatequery = "UPDATE PAYTM_RECHARGEDATA SET STATUS='S' WHERE CA_NUMBER='" + strCAnumber + "' AND DOC_ID='" + strDocid + "'";
        string Updatequery = "UPDATE PAYTM_RECHARGEDATA SET STATUS='S',SAP_UPDATE_DATE=SYSDATE WHERE CA_NUMBER='" + strCAnumber + "' AND DOC_ID='" + strDocid + "'";

        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Updatequery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }

    public static bool Update_SAP_Data(string strDocid, string strCAnumber)
    {
        string Updatequery = "UPDATE PAYTM_HESUPDATEDETAILS SET SAP_STATUS='Y' WHERE CANUMBER='" + strCAnumber + "' AND DOCID='" + strDocid + "'";
        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Updatequery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }

    public static bool InsertService_DataLogs(string strServiceName, string strPayMethod, string strDocID, string strCA_No, string strAmmount, 
                                                string strMessage,string strCompany, string strStatus)
    {
        strMessage = strMessage.Replace("'", "''").Replace("&", " ");
        string Insertquery = "INSERT INTO RECHARGEDATA_LOG(SERVICE_NAME, PAYMENT_METHOD, DOC_ID, CA_NUMBER, AMOUNT, MESSAGE_TXT, COMPANY_NAME, STATUS) ";
        Insertquery += "VALUES ('" + strServiceName + "','" + strPayMethod + "','" + strDocID + "','" + strCA_No + "','" + strAmmount + "','" + strMessage + "','" + strCompany + "','" + strStatus + "')";
        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Insertquery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }


    public static DataTable GetFail_TransData()
    {
        string Selectquery = "SELECT TRANSACTIONID,ENTRY_DATE FROM PAYTM_RECHARGEDATA where STATUS='R' ";
       
        dt = new DataTable();
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Selectquery;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            da.Fill(dt);
        }
        catch (OleDbException ex)
        {
            throw ex;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return dt;
    }

    public static bool Update_FAIL_Data(string strTransactionID)
    {
        string Updatequery = "UPDATE PAYTM_RECHARGEDATA SET STATUS='F',FAIL_UPDATE_DATE=SYSDATE WHERE TRANSACTIONID='" + strTransactionID + "'";
        bool results = false;
        con = new OleDbConnection(ConnectionClass.con());
        cmd = new OleDbCommand();
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Updatequery;
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                results = true;
            }
            else
            {
                results = false;
            }
        }
        catch (OleDbException ex)
        {
            results = false;
        }
        finally
        {
            cmd.Dispose();
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
        return results;
    }

}




