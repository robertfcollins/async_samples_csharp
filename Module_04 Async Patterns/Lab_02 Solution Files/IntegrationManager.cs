using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace LegacyAsync
{
    public class IntegrationManager
    {   

        public async Task ExecuteAsync()
        {
            var result = await RentTimeOnEapMiningServerAsync("Password", 4);
            Console.WriteLine($"mining result: {result.MiningText}");
            Console.WriteLine($"Elapsed seconds: {result.ElapsedSeconds:N}");  
        }

        // APM style
        public Task<MiningResultDto> RentTimeOnApmMiningServerAsync(string authToken, int requestedAmount)
        {
            if (!AuthorizeTheToken(authToken))
            {
                throw new Exception("Failed Authorization");
            }

            TaskCompletionSource<MiningResultDto> tcs = new TaskCompletionSource<MiningResultDto>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/{requestedAmount}");
            var startTime = DateTime.UtcNow;
            request.BeginGetResponse(asyncResult =>
                {
                    try
                    {
                        var resultDto = new MiningResultDto();
                        HttpWebResponse response = (asyncResult.AsyncState as HttpWebRequest).EndGetResponse(asyncResult) as HttpWebResponse;
                        using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                        {
                            resultDto.MiningText = httpWebStreamReader.ReadToEnd();
                        }
                        resultDto.ElapsedSeconds = (DateTime.UtcNow - startTime).TotalSeconds;           
                        tcs.SetResult(resultDto);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }, request);

            return tcs.Task;
        }


        public Task<MiningResultDto> RentTimeOnEapMiningServerAsync(string authToken, int requestedAmount)
        {
            if (!AuthorizeTheToken(authToken))
            {
                throw new Exception("Failed Authorization");
            }

            TaskCompletionSource<MiningResultDto> tcs = new TaskCompletionSource<MiningResultDto>();
            var wc = new WebClient();
            var startTime = DateTime.UtcNow;
            wc.DownloadStringCompleted += (s,e) =>
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                else
                {
                    var resultDto = new MiningResultDto();
                    resultDto.MiningText = e.Result.ToString();
                    resultDto.ElapsedSeconds = (DateTime.UtcNow - startTime).TotalSeconds;           
                    tcs.SetResult(resultDto);
                }
            };
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/{requestedAmount}");
            wc.DownloadStringAsync(uri);
            return tcs.Task;
        }

        private Boolean AuthorizeTheToken(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return false;
            }
            return true;
        }


    }

}