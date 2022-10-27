using System;
using System.Linq;
using Azure_function_logs.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GettingDataInMinute
{
    public class OneMinute
    {
        public IPublicApi _api;
        
        public OneMinute(IPublicApi api )
        {
            _api = api;
        }
        
        [Function("PullData")]
        public async void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var result = await _api.GetApi();
            var output = result.entries.First();
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} Api: {output.API}" +
                               $" Description {output.Description}");
            
        }
    }
}