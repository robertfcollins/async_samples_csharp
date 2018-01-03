using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Module4SelfAssessment
{
    public class Module4Lab3Solution
    {
        
        public void Launch()
        {
            var resultText = SellSomeCoin("SecretToken", 9, out int marketPrice);
            Console.WriteLine($"Current coin price: ${marketPrice}");
            Console.WriteLine(resultText);
        }

        public async Task LaunchAsync()
        {
            var result = await SellSomeCoinApmStyleAsync("SecretToken", 9);
            Console.WriteLine($"Current coin price: ${result.CurrentMarketPrice}");
            Console.WriteLine(result.ResultText);
        }

        #region 1. Take the following code and create a TAP-compliant async version

        public string SellSomeCoin(string authToken, int howMany, out int currentMarketPrice)
        {
            if(string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Failed Authorization");
            }
            currentMarketPrice = new Random().Next(50, 120);
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/sellcoin/{howMany}");
            var webClient = new WebClient();
            var result = webClient.DownloadString(uri);
            return result;
        }

        public async Task<SalesResult> SellSomeCoinAsync(string authToken, int howMany)
        {
            if(string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Failed Authorization");
            }
            var salesResult = new SalesResult();
            salesResult.CurrentMarketPrice = new Random().Next(50, 120);
            var httpClient = new HttpClient();
            salesResult.ResultText = await httpClient.GetStringAsync($"https://asynccoinfunction.azurewebsites.net/api/sellcoin/{howMany}");
            return salesResult;
        }

        #endregion

        #region 2. Write a TAP-compliant method that has equivalent functionality to following APM-style code:

        public Task<SalesResult> SellSomeCoinApmStyleAsync(string authToken, int requestedAmount)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new Exception("Failed Authorization");
            }

            TaskCompletionSource<SalesResult> tcs = new TaskCompletionSource<SalesResult>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://asynccoinfunction.azurewebsites.net/api/sellcoin/{requestedAmount}");

            request.BeginGetResponse(asyncResult =>
                {
                    try
                    {
                        var resultDto = new SalesResult();
                        HttpWebResponse response = (asyncResult.AsyncState as HttpWebRequest).EndGetResponse(asyncResult) as HttpWebResponse;
                        using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                        {
                            resultDto.ResultText = httpWebStreamReader.ReadToEnd();
                        }
                        resultDto.CurrentMarketPrice = new Random().Next(50, 120);
                        tcs.SetResult(resultDto);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }, request);

            return tcs.Task;
        }

        #endregion


        #region Create one TAP-compliant, I/O-bound method then add all optional overloads

        public async Task LaunchBatchAsync()
        {
            var progress = new Progress<int>(total => Console.WriteLine($"Progress: {total}%"));
            var authTokens = new string[] {"one", "two", "three", "four", "five", "six"};
            var result = await BatchSellSomeCoinAsync(authTokens, 9, progress);
            Console.WriteLine($"Current coin price: ${result.CurrentMarketPrice}");
            Console.WriteLine(result.ResultText);
        }

        public async Task<SalesResult> BatchSellSomeCoinAsync(string[] authTokens, int howMany)
        {
            return await BatchSellSomeCoinAsync(authTokens, howMany, CancellationToken.None, null);
        }

        public async Task<SalesResult> BatchSellSomeCoinAsync(string[] authTokens, int howMany, CancellationToken cancellationToken)
        {
            return await BatchSellSomeCoinAsync(authTokens, howMany, cancellationToken, null);
        }

        public async Task<SalesResult> BatchSellSomeCoinAsync(string[] authTokens, int howMany, IProgress<int> progress)
        {
            return await BatchSellSomeCoinAsync(authTokens, howMany, CancellationToken.None, progress);
        }

        public async Task<SalesResult> BatchSellSomeCoinAsync(string[] authTokens, int howMany,
            CancellationToken cancellationToken, IProgress<int> progress)
        {
            if (authTokens.Length < 1)
            {
                throw new Exception("No Authorization");
            }

            var salesResult = new SalesResult();
            salesResult.CurrentMarketPrice = new Random().Next(50, 120);
            double progressPercentage = 0;
            double progressChunks = 100 / authTokens.Length;

            foreach (var authToken in authTokens)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if(string.IsNullOrEmpty(authToken))
                {
                    throw new Exception("Failed Authorization");
                }
                var httpClient = new HttpClient();
                salesResult.ResultText += await httpClient.GetStringAsync($"https://asynccoinfunction.azurewebsites.net/api/sellcoin/{howMany}");
                salesResult.ResultText += Environment.NewLine;
                if (progress != null)
                {
                    progressPercentage += progressChunks;
                    progress.Report((int)progressPercentage);                    
                }
            }
                        
            return salesResult;
        }

        #endregion

    }

}
