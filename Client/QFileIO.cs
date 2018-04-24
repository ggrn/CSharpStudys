using System;
using System.IO;
using System.Linq;

namespace Client
{
    public class QFileIO
    {
        const int BUF_SIZE = 512;
        public void Run()
        {
            DirectoryInfo dir = new DirectoryInfo("..\\INPUT");
            FileInfo[] files = dir.GetFiles();
            byte[] buffer = new byte[BUF_SIZE];
            int nFReadLen;

            //if (dir.GetDirectories().Where(x=> x.Name.Equals("OUTPUT")).Count() == 0)
            //{
                Directory.CreateDirectory(".\\OUTPUT");
            //}

            foreach (var file in files)
            {
                if (file.Length> 2048)
                {
                    using (FileStream fRead = file.Open(FileMode.Open, FileAccess.Read))
                    using (FileStream fWrite = new FileStream(".\\OUTPUT\\" + file.Name, FileMode.Create, FileAccess.Write))
                    {
                        while ((nFReadLen = fRead.Read(buffer, 0, BUF_SIZE)) > 0)
                        {
                            fWrite.Write(buffer, 0, nFReadLen);                        }
                    }
                }
                Console.WriteLine("{0:\t{1}bytes", file.Name, file.Length);
            }
        }
    }
}
