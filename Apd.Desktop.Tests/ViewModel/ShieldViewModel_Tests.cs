using Apd.Desktop.Messaging;
using Apd.Desktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace Apd.Desktop.Tests.ViewModel {
    [TestFixture]
    public class ShieldViewModel_Tests {
        private ShieldViewModel vm;

        [SetUp]
        public void  Setup() {
            this.vm = new ShieldViewModel();
        }

        [Test]
        public void ApiRequestStarted_message_should_set_Active_to_true() {
            Messenger.Default.Send(new ApiRequestStarted());
            Assert.IsTrue(this.vm.Active);
        }
        
        [Test]
        public void ApiRequestEnded_message_should_set_Active_to_false() {
            Messenger.Default.Send(new ApiRequestEnded());
            Assert.IsFalse(this.vm.Active);
        }
        
        [Test]
        public void ApiRequestError_message_should_set_Active_to_false() {
            Messenger.Default.Send(new ApiRequestError());
            Assert.IsFalse(this.vm.Active);
        }
    }
}