using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hints
{
    class QFileReader
    {
        public void Run()
        {
            List<string> strs = new List<string>();
            using (StreamReader sr = new StreamReader(new FileStream("fileName", FileMode.Open, FileAccess.Read)))
            {
                while(sr.Peek() >= 0)
                {
                    strs.Add(sr.ReadLine());
                }
            }
        }
    }
}
