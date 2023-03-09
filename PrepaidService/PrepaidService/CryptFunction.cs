using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

public class CryptFunction
{
    const string key = "BSESPREPAIDMTR19"; //16 bit Encrypt Key  
    public CryptFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string 
    public string Encrypt(String plainText)
    {
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
    }
    public string Decrypt(String encryptedText)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
    }
    public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
    {
        return rijndaelManaged.CreateEncryptor()
            .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
    }
    public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
    {
        return rijndaelManaged.CreateDecryptor()
            .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
    }
    public RijndaelManaged GetRijndaelManaged(String secretKey)
    {
        var keyBytes = new byte[16];
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
        return new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            KeySize = 128,
            BlockSize = 128,
            Key = keyBytes,
            IV = keyBytes
        };
    }

}