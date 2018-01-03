using System;

namespace Lab03_ConsoleSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new AsyncCoinManager();
            manager.AcquireAsyncCoinAsync(3);
            Console.ReadLine();
        }
    }
}
