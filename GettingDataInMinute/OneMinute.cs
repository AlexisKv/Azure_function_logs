using System;
using System.Linq;
using System.Threading.Tasks;
using Azure_function_logs.Models;
using Azure_function_logs.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace GettingDataInMinute
{
    public class OneMinute
    {
        private IPublicApi _api;
        private readonly StorageProvider _blobStorage;
        
        public OneMinute(IPublicApi api, StorageProvider storageProvider)
        {
            _api = api;
            _blobStorage = storageProvider;
        }

        [FunctionName("PullData")]
        public async Task Run([TimerTrigger("*/5 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var result = await _api.GetApi();
            var output = result.entries.First();
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}\n API : {output.API} " +
                               $"\n Description : {output.Description}");
            var id = _blobStorage.SaveBlob(result); 
            
            await _blobStorage.LogRequest(result, id);
        }
    }
}