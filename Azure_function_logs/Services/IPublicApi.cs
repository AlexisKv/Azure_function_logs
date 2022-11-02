using System.Threading.Tasks;
using Azure_function_logs.Models;
using Refit;

namespace Azure_function_logs.Services
{
    public interface IPublicApi
    {
        [Get("/random?auth=null")]
        Task<Root> GetApi();
    }
}