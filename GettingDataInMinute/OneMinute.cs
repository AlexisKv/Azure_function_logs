using System;
using System.Linq;
using System.Threading.Tasks;
using Azure_function_logs.Services;
using Microsoft.Azure.WebJobs;
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
        
        [FunctionName("PullData")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var result = await _api.GetApi();
            var output = result.entries.First();    
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
        }
    }
}