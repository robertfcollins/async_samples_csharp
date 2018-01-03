using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.IO;

namespace SelfAssessmentLibrary
{
    public class AssessmentManager
    {
        public async Task<ServerTime> CallWebServiceThatReturnsCustomClass()
        {
            var uri = new Uri($"http://date.jsontest.com");            
            var client = new HttpClient();
            
            Task<HttpResponseMessage> webTask = client.GetAsync(uri);
            var response = await webTask;

            Task<Stream> contentTask = response.Content.ReadAsStreamAsync();
            var stream = await contentTask;
            var serializer = new DataContractJsonSerializer(typeof(ServerTime));
            var serverTime = (ServerTime)serializer.ReadObject(stream);
            return serverTime;
        }

        public async Task<int> DontMineHereAsync()
        {
            throw new Exception("No, this is forbidden");
        }

        public async Task<long> GetWebServiceResultsThenAddInParallel()
        {
            var serverTime = await CallWebServiceThatReturnsCustomClass();
            long combinedHours = 0;
            long epochHours = (serverTime.milliseconds_since_epoch / 1000 / 60 / 60);

            Parallel.For(0, epochHours, i => {
                Interlocked.Add(ref combinedHours, i);
            });
            return combinedHours;
        }

    }

    public class ServerTime
    {
        public string time {get;set;}

        public long milliseconds_since_epoch {get;set;}

        public string date {get;set;}

    }

}

