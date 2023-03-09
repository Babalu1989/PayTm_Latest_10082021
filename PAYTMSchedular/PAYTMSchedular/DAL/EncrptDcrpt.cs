using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

/// <summary>
/// Summary description for EncrptDcrpt
/// </summary>
public class EncrptDcrpt
{
	public EncrptDcrpt()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string EncryptText(string strText)
    {
        return Encrypt(strText, "&%#@?,:*");
    }

    //Decrypt the text
    public static string DecryptText(string strText)
    {
        return Decrypt(strText, "&%#@?,:*");
    }
    private static string Encrypt(string strText, string strEncrKey)
    {
        byte[] byKey = null;
        byte[] IV = { 0X12, 0X34, 0X56, 0X78, 0X90, 0XAB, 0XCD, 0XEF };

        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());

        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    //The function used to decrypt the text
    private static string Decrypt(string strText, string sDecrKey)
    {
        byte[] byKey = null;
        byte[] IV = { 0X12, 0X34, 0X56, 0X78, 0X90, 0XAB, 0XCD, 0XEF };
        byte[] inputByteArray = new byte[strText.Length + 1];

        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            return encoding.GetString(ms.ToArray());

        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }
}
