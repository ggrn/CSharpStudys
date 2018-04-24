using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSharpStudy
{
    public class Client
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> accounts = new Dictionary<string, string>();
            using(StreamReader sr = new StreamReader("../../../TestData/INSPECTOR_ALL.TXT"))
            {
                string strLine = null;
                while(!string.IsNullOrEmpty(strLine = sr.ReadLine()))
                {
                    string[] accData = strLine.Split();
                    accounts.Add(accData[0], accData[1]);
                }
            }

            string inputStr = null;
            string id = null;
            while (true)
            {
                inputStr = Console.ReadLine();
                if (string.IsNullOrEmpty(inputStr)) continue;

                string[] loginData = inputStr.Split();
                id = accounts.Where(x => x.Key == loginData[0] && x.Value == "SHA256").Select(x => x.Key).FirstOrDefault();
                if (!string.IsNullOrEmpty(id))
                {
                    Console.WriteLine("Login Success");
                    break;
                }
                else
                {
                    Console.WriteLine("Fail");
                }
            }

            //string now = DateTime.Now.ToString("yyyyMMddHHmmss");
            //string today = DateTime.ParseExact(str, "yyyyMMdd", null);
        }

        public static void TransferFile(string id)
        {
            const int DEFAULT_BUFLEN = 4096;
            const int DEFAULT_PORT = 9009;
            const int FILENAME_LEN = 28;

            string[] dirs = Directory.GetFiles(".\\{0}", id);

            byte[] bytesFileName = new byte[FILENAME_LEN];
            byte[] bytes = new byte[DEFAULT_BUFLEN];
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, DEFAULT_PORT);

            try
            {
                foreach (string fileName in dirs)
                {
                    using (Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        sender.Connect(remoteEP);

                        bytesFileName = Encoding.UTF8.GetBytes(fileName.Substring(11) + Convert.ToInt32(fileName.Equals(dirs.Last())));
                        sender.Send(bytesFileName, 0, FILENAME_LEN, SocketFlags.None);

                        using (FileStream fRead = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                        {
                            int bytesRead;
                            while ((bytesRead = fRead.Read(bytes, 0, DEFAULT_BUFLEN))>0)
                            {
                                sender.Send(bytes, 0, bytesRead, SocketFlags.None);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
