using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hints
{
    class QEncDec
    {
        void DoBase64(string str)
        {
            byte[] byteStr = Encoding.UTF8.GetBytes(str);
            string encodeStr = Convert.ToBase64String(byteStr);
            //
            byte[] decodeByte = Convert.FromBase64String(encodeStr);
            string decodeStr = Encoding.UTF8.GetString(decodeByte);
        }

        void DoSHA256(string str)
        {
            byte[] byteStr = Encoding.UTF8.GetBytes(str);
            SHA256 mySha256 = SHA256.Create();

            byte[] byteSha = mySha256.ComputeHash(byteStr);
            string encodeStr = "";
            for (int i = 0; i < byteSha.Length; i++)
            {
                encodeStr += string.Format("{0:X2}", byteSha[i]);
            }
        }
    }
}
