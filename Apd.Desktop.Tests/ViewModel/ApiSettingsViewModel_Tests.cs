using Apd.Desktop.Service;
using Apd.Desktop.ViewModel;
using Moq;
using NUnit.Framework;

namespace Apd.Desktop.Tests.ViewModel {
    [TestFixture]
    public class ApiSettingsViewModel_Tests {
        [Test]
        public void ApiAddress_should_be_propagated_to_IRestApi() {
            var address = "http://api/";
            var mockRestApi = new Mock<IRestApi>();
            var vm = new ApiSettingsViewModel(mockRestApi.Object);
            vm.ApiAddress = address;
            mockRestApi.VerifySet(x => x.ApiAddress = address);

        }
    }
}