using RestSharp;

namespace Apd.Desktop.Service {
    public class RestClientFactory : IRestClientFactory{
        public IRestClient CreateClient() {
            return new RestClient();
        }
    }
}