using System;
using System.Net;
using System.Threading.Tasks;

namespace Lab03_ConsoleSolution
{
    public class AsyncCoinManager
    {
        public async void AcquireAsyncCoinAsync(int howMany)
        {
            Console.WriteLine("Started acquiring AsyncCoin.");
            var result = await CallCoinServiceAsync(howMany);
            Console.WriteLine($"result: {result}");
            Console.WriteLine("Finshed.");
        }

        public async Task<string> CallCoinServiceAsync(int howMany)
        {
            var uri = new Uri($"http://asynccoinfunction.azurewebsites.net/api/asynccoin/{howMany}");
            var webClient = new WebClient();
            var result = await webClient.DownloadStringTaskAsync(uri);
            return result;
        }
    }
}