using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using Azure_function_logs.Services;

[assembly: FunctionsStartup(typeof(GettingDataInMinute.Startup))]
namespace GettingDataInMinute
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddRefitClient<IPublicApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.publicapis.org"));
        }
    }
}   