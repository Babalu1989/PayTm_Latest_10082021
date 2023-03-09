using System;
using System.Security.Cryptography;
using System.Text;

namespace PrepaidService
{
    public class SHASample
    {
        public SHASample() { }


        public string GetHMACSHA256(string text, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(text);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public string GetCheckSumLogic(string strMsg) //  CANUMBER, string AMOUNT, string strTranID,string strCompany, string strSerialNo)
        {
            //MessageKey = "BSESRPREP|" + CANUMBER + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http:// www.domain.com/response.jsp|TOEB50N43blx";            
            //String data = "BSESRPREP|" + CANUMBER + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|NA|NA|NA|NA|NA|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx";
            //String data = "BSESRPREP|" + strTranID + "|NA|" + AMOUNT + "|NA|NA|NA|INR|NA|R|bsesrprep|NA|NA|F|" + strCompany + "|R|" + CANUMBER + "|" + strSerialNo.TrimStart('0') + "|APP|NA|NA|http://115.249.67.72:9880/PaymentResponse.aspx";

            String commonkey = "TOEB50N43blx";
            SHASample dataprg = new SHASample();
            String hash = String.Empty;
            hash = dataprg.GetHMACSHA256(strMsg, commonkey);
            return hash;
        }

        public string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }
    }
}