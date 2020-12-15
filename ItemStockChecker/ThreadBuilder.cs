using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ItemStockChecker
{
    public class ThreadBuilder
    {
        public static List<Thread> _threads = new List<Thread>();

        public static async Task Start()
        {
            await Task.Run(() =>
            {
                try
                {

                    foreach (UrlElement url in UrlRetriever.GetUrls())
                    {
                        Thread scraperThread = Loader.CreateThread(new Uri(url.Name));

                        if (scraperThread == null) continue;

                        _threads.Add(scraperThread);
                        scraperThread.Start();

                        // Stagger the threads
                        Random r = new Random();
                        int rInt = r.Next(1, 10);

                        Thread.Sleep(new TimeSpan(0, 0, rInt));
                    }

                    foreach (Thread thread in _threads)
                    {
                        thread.Join();
                    }
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write(e);
                }
            });
        }
    }
}
