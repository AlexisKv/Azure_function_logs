using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure_function_logs.Models;
using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using GettingDataInMinute;
using Microsoft.Azure.Cosmos.Table;
using CloudTable = Microsoft.WindowsAzure.Storage.Table.CloudTable;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TableClientConfiguration = Microsoft.Azure.Cosmos.Table.TableClientConfiguration;

namespace Azure_function_logs.Services
{
    public class StorageProvider
    {
        private readonly BlobContainerClient _blobClient;
        private readonly Microsoft.Azure.Cosmos.Table.CloudTable _cloudTable;
        private readonly IConfiguration _config;

        public StorageProvider(IConfiguration config)
        {
            _config = config;
            var client = new BlobServiceClient(config.ConnectionString);
            
            try
            {
                _blobClient = client.CreateBlobContainer(config.BlobContainerName);
            }
            catch (Exception e)
            {
                _blobClient = client.GetBlobContainerClient(config.BlobContainerName);
            }

            var account = CloudStorageAccount.Parse(config.ConnectionString);
            var tableClient = account.CreateCloudTableClient(new TableClientConfiguration());
            
            _cloudTable = tableClient.GetTableReference(config.AzureTableName);
            _cloudTable.CreateIfNotExists();
        }
        
        public string SaveBlob(Root response)
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);

            var content = JsonSerializer.Serialize(response);
            
            streamWriter.Write(content);
            
            streamWriter.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            var name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var fileName = $"{_config.FilePrefix}-{name}.json";

            _blobClient.UploadBlob(fileName, stream);
            return name;
        }

        public async Task LogRequest(Root response, string id)
        {
            var serialized = JsonSerializer.Serialize(response);
            var entry = new ApiResponseEntity(id, serialized, DateTime.Now);
            var operation = TableOperation.Insert(entry);
            await _cloudTable.ExecuteAsync(operation);
        }

        public IEnumerable<ApiResponseEntity> GetLogs(DateTime from, DateTime to)
        {
            var items = _cloudTable.ExecuteQuery(new TableQuery<ApiResponseEntity>())
                .Where(x => x.Timestamp >= from && x.Timestamp <= to);

            return items;
        }

        public async Task<string> GetBloobAsync(string id)
        {
            // var dateTime = DateTime.Parse(id);
            // var name = dateTime.ToString("yyyy-MM-dd-HH-mm-ss");
            var fileName = $"{_config.FilePrefix}-{id}.json";

            var blobclient = _blobClient.GetBlobClient(fileName);
            
            using var stream = new MemoryStream();
            await blobclient.DownloadToAsync(stream);

            stream.Position = 0;

            using var streamReader = new StreamReader(stream);
            
            var response = await streamReader.ReadToEndAsync();

            return response;
        }
    }
   
   
}