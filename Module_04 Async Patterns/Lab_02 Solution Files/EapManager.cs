using System;
using System.IO;
using System.Net;
using System.Threading;

namespace LegacyAsync
{
    public class EapManager
    {

        private DateTime _startMiningDateTimeUtc;

        public void EapStyleMiningAsync(int requestedAmount)
        {
            _startMiningDateTimeUtc = DateTime.UtcNow;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(EapMining_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/{requestedAmount}"));
        }

        private void EapMining_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception(e.Error.Message);

            var miningResult = e.Result.ToString();
            double elapsedSeconds = (DateTime.UtcNow - _startMiningDateTimeUtc).TotalSeconds;
            Console.WriteLine(miningResult);
            Console.WriteLine($"Elapsed seconds: {elapsedSeconds}");            
        }

    }
}