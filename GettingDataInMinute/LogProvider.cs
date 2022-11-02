using System;
using System.Threading.Tasks;
using Azure_function_logs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GettingDataInMinute
{
    public class LogProvider
    {
        private readonly StorageProvider _storageProvider ;

        public LogProvider(StorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        [FunctionName("LogProvider")]
        public Task<IActionResult> Run(
            [HttpTrigger("get", Route = null)] HttpRequest req, ILogger log)
        {   
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string from = req.Query["from"];
            string to = req.Query["to"];

            var dateTimeFrom = DateTime.Parse(from);
            var dateTimeTo = DateTime.Parse(to);

            var getLogs = _storageProvider.GetLogs(dateTimeFrom, dateTimeTo);
            
            return Task.FromResult<IActionResult>(new OkObjectResult(getLogs));
            
        }
    }
}