using System.Threading.Tasks;
using Azure_function_logs.Services;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GettingDataInMinute
{
    public class BlobApi
    {
        private readonly StorageProvider _blobStorage;


        public BlobApi(StorageProvider storage)
        {
            _blobStorage = storage;
        }

        [FunctionName("BlobApi")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger("get", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["id"];

            var result = await _blobStorage.GetBloobAsync(id);
            
            return new ObjectResult(result);
        }
    }
}