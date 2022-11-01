using System;
using System.IO;
using Azure_function_logs.Models;
using Azure.Storage.Blobs;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Azure_function_logs.Services
{
    public class StorageProvider
    {
        private readonly BlobContainerClient _blobClient;

        public StorageProvider()
        {
            var client = new BlobServiceClient("UseDevelopmentStorage=true");
            
            try
            {
                _blobClient = client.CreateBlobContainer("atea-api-blob");
            }
            catch (Exception e)
            {
                _blobClient = client.GetBlobContainerClient("atea-api-blob");
            } 
        }
        
        public void SaveBlob(Entry response)
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);

            var content = JsonSerializer.Serialize(response);
            
            streamWriter.Write(content);
            
            streamWriter.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            var fileName = $"ApiResponse{DateTime.Now.ToString("yyyyMMddHHmmss")}.json";

            _blobClient.UploadBlob(fileName, stream);
        }
    }
   
   
}