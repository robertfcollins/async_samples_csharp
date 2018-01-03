using System;
using System.Threading.Tasks;

namespace Module4SelfAssessment
{
    class Program
    {
        // static void Main(string[] args)
        // {
        //     Console.WriteLine("start");
        //     var solution = new Module4Lab3Solution();
        //     solution.Launch();
        //     Console.ReadLine();
        // }

        // static void Main(string[] args)
        // {
        //     Console.WriteLine("start");
        //     var solution = new ApmExample();
        //     solution.BeginApmCoinSales(13);
        //     Console.ReadLine();
        // }

        static async Task Main(string[] args)
        {
            Console.WriteLine("start");
            var solution = new Module4Lab3Solution();
            await solution.LaunchBatchAsync();
            Console.ReadLine();            
        }
    }
}
