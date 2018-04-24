using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CSharpStudy
{
    class CServer
    {
        static Socket listener;

        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
                Thread serverThread = new Thread(server.Run);

                serverThread.Start();

                string inputStr;
                while (true)
                {
                    inputStr = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inputStr) && inputStr.Equals("QUIT"))
                    {
                        break;
                    }
                }
                listener.Close();
                listener.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        class Server
        {
            public void Run()
            {
                const int DEFAULT_BUFLEN = 4096;
                const int DEFAULT_PORT = 9009;
                const int FILENAME_LEN = 28;
                
                byte[] bytesFileName = new byte[FILENAME_LEN];
                byte[] bytes = new byte[DEFAULT_BUFLEN];
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint localEP = new IPEndPoint(ipAddress, DEFAULT_PORT);

                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int recvLen;

                try
                {
                    listener.Bind(localEP);
                    listener.Listen(10);
                    while (true)
                    {
                        using (Socket handler = listener.Accept())
                        {
                            handler.Receive(bytesFileName);
                            string fileName = Encoding.UTF8.GetString(bytesFileName);
                            bool isLastFile = fileName.EndsWith("1");
                            //fileName.Remove(fileName.Length - 1);
                            fileName = fileName.Substring(0, fileName.Length - 1);

                            Directory.CreateDirectory(".\\SERVER");
                            using (FileStream fWrite = new FileStream(".\\SERVER\\" + fileName, FileMode.Create, FileAccess.Write))
                            {
                                while((recvLen = handler.Receive(bytes)) > 0)
                                {
                                    fWrite.Write(bytes, 0, recvLen);
                                }
                            }

                            if (isLastFile) break;
                            handler.Close();
                        }
                    }
                    listener.Close();
                    listener.Shutdown(SocketShutdown.Both);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}