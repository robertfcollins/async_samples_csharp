using System;
using System.IO;
using System.Threading;
using System.Net;

namespace Module4SelfAssessment
{
    public class ApmExample
    {
        public void BeginApmCoinSales(int howMany)
        {
            string url = $"https://asynccoinfunction.azurewebsites.net/api/sellcoin/{howMany}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(EndApmCoinSales), request);
        }

        public void EndApmCoinSales(IAsyncResult result)
        {
            HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            string salesResult;
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                salesResult = httpWebStreamReader.ReadToEnd();
            }
            var marketPrice = new Random().Next(50, 120);
            Console.WriteLine($"Current coin price: {marketPrice}");
            Console.WriteLine(salesResult);
        }

    }
}