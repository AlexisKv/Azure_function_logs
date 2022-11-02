using System.Threading.Tasks;
using Azure_function_logs.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;

namespace AzureTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var publicApis = RestService.For<IPublicApi>("https://api.publicapis.org");
            var octocat = await publicApis.GetApi();
            octocat.Should().NotBeNull();
        }
    }
}