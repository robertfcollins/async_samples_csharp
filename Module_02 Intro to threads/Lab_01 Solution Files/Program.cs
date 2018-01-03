using System;
using AsyncCoinMiner;

namespace Module02_Lab01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            var miningManager = new AsyncCoinMiningManager();
            miningManager.Execute();
            Console.ReadKey();
        }
    }
}
