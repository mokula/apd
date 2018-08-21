using Apd.Desktop.Service;
using NUnit.Framework;
using RestSharp;

namespace Apd.Desktop.Tests.Service {
    [TestFixture]
    public class RestClientFactory_Tests {
        [Test]
        public void CreateClient_should_create_instace_of_RestClient() {
            var restClientFactory = new RestClientFactory();
            Assert.IsInstanceOf<RestClient>(restClientFactory.CreateClient());
        }
    }
}