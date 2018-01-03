using System;
using System.IO;
using System.Threading;
using System.Net;

namespace LegacyAsync
{
    public delegate string AsyncMiningCaller(int coinCount);

    public class ApmManager
    {
 
        private DateTime _startMiningDateTimeUtc;
        
        #region async request/response

        public void BeginApmStyleMining(string url)
        {
            _startMiningDateTimeUtc = DateTime.UtcNow;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(EndApmStyleMining), request);
        }

        public void EndApmStyleMining(IAsyncResult result)
        {
            HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            string miningResult;
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                miningResult = httpWebStreamReader.ReadToEnd();
            }
            double elapsedSeconds = (DateTime.UtcNow - _startMiningDateTimeUtc).TotalSeconds;
            Console.WriteLine(miningResult);
            Console.WriteLine($"Elapsed seconds: {elapsedSeconds}");
        }

        #endregion


    }
}