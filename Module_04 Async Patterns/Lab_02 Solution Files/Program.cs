using System;
using System.Threading.Tasks;

namespace LegacyAsync
{
    class Program
    {
        // static async Task Main(string[] args)
        // {
        //     Console.WriteLine("start");
        //     var manager = new IntegrationManager();
        //     await manager.ExecuteAsync();
        //     Console.ReadLine();
        // }

        static void Main(string[] args)
        {
            Console.WriteLine("start");
            var manager = new EapManager();
            manager.EapStyleMiningAsync(4);

            // var apmManager = new ApmManager();
            // apmManager.BeginApmStyleMining("https://asynccoinfunction.azurewebsites.net/api/asynccoin/4");

            Console.ReadLine();
        }
    }
}
