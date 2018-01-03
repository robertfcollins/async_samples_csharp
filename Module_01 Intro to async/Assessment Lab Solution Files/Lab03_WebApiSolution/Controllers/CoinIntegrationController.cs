using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http;

namespace Lab03_WebApiSolution.Controllers
{
    [Route("api/[controller]")]
    public class CoinIntegrationController : Controller
    {
        // GET api/coinintegration/5
        [HttpGet("{howMany}")]
        public async Task<string> Get(int howMany)
        {
            var uri = new Uri($"http://asynccoinfunction.azurewebsites.net/api/asynccoin/{howMany}");
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(uri);
            return result;
        }

    }
}
