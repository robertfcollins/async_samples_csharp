using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace AsyncReturnTypeLibrary
{
    public class AsyncReturnTypeManager
    {

        public async Task PauseFiveSecondsAsync()
        {
            await Task.Delay(5000);
            return;
        }

        public async void PauseSixSecondsAsync()
        {
            await Task.Delay(6000);
        }

        public async Task CallWebServiceThatDoesNotReturnResults()
        {
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/3");
            var client = new HttpClient();
            await client.GetAsync(uri);
            return;
        }

        public async Task<string> CallWebServiceThatReturnsString()
        {
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/3");
            var client = new HttpClient();
            Task<string> webTask = client.GetStringAsync(uri);
            string result = await webTask;
            return result;
        }

#region parallel

        public int MineAndGetAggregateSeconds(int iterations)
        {
            int totalSeconds = 0;

            var result = Parallel.For<int>(0, iterations,
                () => 0, 
                (i, loop, subtotal) => {
                    var startDate = DateTime.UtcNow;
                    MineAsyncCoinsWithPrimes(i);
                    var subtotalSeconds = (DateTime.UtcNow - startDate).Seconds;
                    Console.WriteLine($"Processed for {subtotalSeconds} seconds");
                    subtotal += subtotalSeconds;
                    return subtotal;
                }, 
                (x) => Interlocked.Add(ref totalSeconds, x)
                );

            return totalSeconds;
        }


        private void MineAsyncCoinsWithPrimes(int howMany)
        {
            double allCoins = 0;
            for (int x = 2; x < howMany * 50000; x++)
            {
                int primeCounter = 0;
                for (int y = 1; y < x; y++)
                {
                    if (x % y == 0)
                    {
                        primeCounter++;
                    }

                    if(primeCounter == 2) break;
                }
                if(primeCounter != 2)
                {
                    allCoins += .01;
                }

                primeCounter = 0;
            }
            Console.WriteLine($"Found {allCoins} Async Coin with primes");
            Console.WriteLine($" Worker Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }

#endregion

    }
}
