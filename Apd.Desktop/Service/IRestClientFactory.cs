using RestSharp;

namespace Apd.Desktop.Service {
    public interface IRestClientFactory {
        IRestClient CreateClient();
    }
}