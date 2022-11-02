namespace GettingDataInMinute
{
    public interface IConfiguration
    {
        string ConnectionString { get; }
        string BlobContainerName { get; }
        string AzureTableName { get; }
        string FilePrefix { get; }
    }
}