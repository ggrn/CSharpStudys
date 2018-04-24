using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Hints
{
    class QSync
    {
        public class Worker
        {
            private static Mutex mut = new Mutex();
            public static int count = 1;
            public string Name { get; set; }
            public void DoWork()
            {
                //mut.WaitOne();
                Console.WriteLine("[{0}]", Name);
                for (int i = 0; i < 30; i++)
                {
                    Console.Write("{0} ", i);
                }
                Console.WriteLine();
                //mut.ReleaseMutex();
            }
        }

        public void Run()
        {
            Worker mainWorker = new Worker { Name = "Main" };
            Worker t1Worker = new Worker { Name = "Thread1" };
            Worker t2Worker = new Worker { Name = "Thread2" };

            Thread thread1 = new Thread(t1Worker.DoWork);
            Thread thread2 = new Thread(t2Worker.DoWork);

            thread1.Start();
            thread2.Start();

            mainWorker.DoWork();

            thread1.Join();
            thread2.Join();
        }
    }
}
