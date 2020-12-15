using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ItemStockChecker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ThreadBuilder.Start();

            ConsoleOutputHelper.Write("No more items are being monitored.\nPress any key to close application");
            Console.ReadKey();

        }
    }
}
