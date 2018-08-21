using System;
using Apd.Common.Communication;
using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using Castle.Components.DictionaryAdapter.Xml;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace Apd.Desktop.Tests.Service {
    [TestFixture]
    public class RestApi_Tests {
        private RestApi restApi;
        private Mock<IRestClientFactory> mockClientFactory;
        private Mock<IRestClient> mockClient;
        private Mock<IMessenger> messenger;

        [SetUp]
        public void Setup() {
            this.messenger = new Mock<IMessenger>();
            this.mockClientFactory =new Mock<IRestClientFactory>();
            this.mockClient = new Mock<IRestClient>();
            this.mockClientFactory.Setup(x => x.CreateClient()).Returns(this.mockClient.Object);
            
            this.restApi = new RestApi(this.mockClientFactory.Object, this.messenger.Object, "http://test/");
        }

        private string BuildResourceAddress(string actionName) {
            return $"/{ApiResources.Prefix}/{ApiResources.Contacts}/{actionName}/";
        }
        
        [Test]
        public void ExecuteAsyncGet_should_create_GET_request_with_valid_resource() {
            IRestRequest request = null;
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback((IRestRequest r) => request = r);
            this.restApi.ExecuteAsyncGet(ApiActionNames.AllContacts);
            var expoextedResource = this.BuildResourceAddress(ApiActionNames.AllContacts);
            Assert.AreEqual(expoextedResource, request.Resource);
            Assert.AreEqual(Method.GET, request.Method);
        }

        [Test]
        public void ExecuteAsyncGet_should_send_ApiRequestStarted_message_before_executing_request() {
            object lastMessage = null;
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback(() => {
                Assert.IsInstanceOf<ApiRequestStarted>(lastMessage);
            });
            this.restApi.ExecuteAsyncGet(ApiActionNames.AllContacts);
        }
        
        [Test]
        public void ExecuteAsyncGet_should_send_ApiRequestEnded_message_after_executing_request() {
            object lastMessage = null;
            var mockResponse = new Mock<IRestResponse>();
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(mockResponse.Object);
            this.restApi.ExecuteAsyncGet(ApiActionNames.AllContacts).Wait();
            Assert.IsInstanceOf<ApiRequestEnded>(lastMessage);
        }

        [Test]
        public void ExecuteAsyncGet_should_send_ApiRequestError_message_when_exception_is_thrown() {
            object lastMessage = null;
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Throws<Exception>();
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            this.restApi.ExecuteAsyncGet(ApiActionNames.AllContacts).Wait();
            Assert.IsInstanceOf<ApiRequestError>(lastMessage);
        }
        
        [Test]
        public void ExecuteAsyncPostWithJson_should_create_POST_request_with_valid_resource() {
            IRestRequest request = null;
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback((IRestRequest r) => request = r);
            this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.AddContacts, "data");
            var expoextedResource = this.BuildResourceAddress(ApiActionNames.AddContacts);
            Assert.AreEqual(expoextedResource, request.Resource);
            Assert.AreEqual(Method.POST, request.Method);
        }
        
        [Test]
        public void ExecuteAsyncPostWithJson_should_send_ApiRequestStarted_message_before_executing_request() {
            object lastMessage = null;
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Callback(() => {
                Assert.IsInstanceOf<ApiRequestStarted>(lastMessage);
            });
            this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.AddContacts, "");
        }
        
        [Test]
        public void ExecuteAsyncPostWithJson_should_send_ApiRequestEnded_message_after_executing_request() {
            object lastMessage = null;
            var mockResponse = new Mock<IRestResponse>();
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(mockResponse.Object);
            this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.AddContacts, "").Wait();
            Assert.IsInstanceOf<ApiRequestEnded>(lastMessage);
        }

        [Test]
        public void ExecuteAsyncPostWithJson_should_send_ApiRequestError_message_when_exception_is_thrown() {
            object lastMessage = null;
            this.mockClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Throws<Exception>();
            this.messenger.Setup(x => x.Send(It.IsAny<object>())).Callback((object o) => lastMessage = o);
            this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.AddContacts, "").Wait();
            Assert.IsInstanceOf<ApiRequestError>(lastMessage);
        }

    }
}