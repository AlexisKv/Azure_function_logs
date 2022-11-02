using System;

namespace GettingDataInMinute
{
    public class Configuration : IConfiguration
    {
        public string ConnectionString => Environment.GetEnvironmentVariable("ConnectionString");
        public string BlobContainerName => Environment.GetEnvironmentVariable("BlobContainerName");
        public string AzureTableName => Environment.GetEnvironmentVariable("AzureTableName");
        public string FilePrefix => Environment.GetEnvironmentVariable("FilePrefix");
    }
}