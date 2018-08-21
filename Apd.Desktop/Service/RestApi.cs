using System;
using System.Threading.Tasks;
using System.Windows.Markup;
using Apd.Common.Communication;
using Apd.Desktop.Messaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RestSharp;

namespace Apd.Desktop.Service {
    public class RestApi : IRestApi {
        private IRestClientFactory restClientFactory;
        private IMessenger messenger;
        
        public string ApiAddress { get; set; }
        
        public RestApi(IRestClientFactory restClientFactory, IMessenger messenger, string apiAddress) {
            this.restClientFactory = restClientFactory;
            this.ApiAddress = apiAddress;
            this.messenger = messenger;
        }
        
        private string BuildResourceAddress(string actionName) {
            return $"/{ApiResources.Prefix}/{ApiResources.Contacts}/{actionName}/";
        }

        public async Task<string> ExecuteAsyncGet(string actionName, string param = "") {
            return await this.ExecuteAsyncMethod(Method.GET, actionName, param);
        }
        
        public async Task<string> ExecuteAsyncPost(string actionName, string param) {
            return await this.ExecuteAsyncMethod(Method.POST, actionName, urlParam: param);
        }
        
        public async Task<string> ExecuteAsyncDelete(string actionName, string param) {
            return await this.ExecuteAsyncMethod(Method.DELETE, actionName, urlParam: param);
        }

        public async Task<string> ExecuteAsyncPostWithJson(string actionName, string jsonData) {
            return await this.ExecuteAsyncMethod(Method.POST, actionName, jsonParam: jsonData);
        }

        private async Task<string> ExecuteAsyncMethod(Method method, string actionName, string urlParam = null, string jsonParam = null) {
            try {
                this.messenger.Send(new ApiRequestStarted());
                var client = this.restClientFactory.CreateClient();
                client.BaseUrl = new Uri(this.ApiAddress.Trim('/'));
                var req = new RestRequest(this.BuildResourceAddress(actionName) + urlParam, method);
                if (!string.IsNullOrEmpty(jsonParam)) {
                    req.RequestFormat = DataFormat.Json;
                    req.AddParameter("application/json; charset=utf-8", jsonParam, ParameterType.RequestBody);
                }
                
                var response = await client.ExecuteTaskAsync(req);
                
                if (response.ErrorException == null)
                    this.messenger.Send(new ApiRequestEnded());
                else 
                    this.messenger.Send(new ApiRequestError());
                
                return response.Content;
            }
            catch {
                this.messenger.Send(new ApiRequestError());
                return null;
            }
        } 
    }
}