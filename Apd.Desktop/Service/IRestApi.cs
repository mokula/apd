using System.Threading.Tasks;
using RestSharp;

namespace Apd.Desktop.Service {
    public interface IRestApi {
        string ApiAddress { get; set; }
        Task<string> ExecuteAsyncGet(string actionName, string param = "");
        Task<string> ExecuteAsyncPost(string actionName, string param);
        Task<string> ExecuteAsyncPostWithJson(string actionName, string jsonData);
        Task<string> ExecuteAsyncDelete(string actionName, string param);
    }
}