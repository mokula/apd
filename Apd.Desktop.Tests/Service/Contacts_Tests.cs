using System;
using System.Linq;
using Apd.Common.Communication.DataTransferObject;
using Apd.Common.Container;
using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using Apd.Desktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Apd.Desktop.Tests.Service {
    [TestFixture]
    public class Contacts_Tests {
        private Mock<IContainer> mockContainer;
        private Mock<IMessenger> mockMessanger;
        private Mock<IRestApi> mockRestApi;
        private Mock<IContacts> mockContacts;
        private Contacts contacts;

        private ContactDto dto_1;
        private ContactDto dto_2;

        [SetUp]
        public void Setup() {
            this.mockContacts = new Mock<IContacts>();
            
            this.mockContainer = new Mock<IContainer>();
            this.mockContainer.Setup(x => x.Resolve<ContactViewModel>()).Returns(() => new ContactViewModel(mockContacts.Object));
            
            this.mockMessanger = new Mock<IMessenger>();
            this.mockRestApi = new Mock<IRestApi>();
            
            this.contacts = new Contacts(this.mockRestApi.Object, this.mockMessanger.Object, this.mockContainer.Object);
            this.dto_1 = new ContactDto {
                Id = 1,
                FirstName = "Kathryn",
                LastName =  "Rogers",
                BirthDate = new DateTime(1967, 4,1),
                Emails = new[] {"krogers@gmail.com", "krogers@hotmail.com"},
                PhoneNumbers = new[] {"44 344 543", "+1 800 345 123"}
            };
            
            this.dto_2 = new ContactDto {
                Id = 2,
                FirstName = "Jhon",
                LastName =  "Smith",
                BirthDate = null,
                Emails = new[] {"smith@gmail.com"},
                PhoneNumbers = new[] {"66 344 543"}
            };
        }

        [Test]
        public void GetContactsByNameAsync_should_send_ContactsRecived_message_with_parsed_contacts() {
            object lastMessage = null;
            var json = JsonConvert.SerializeObject(new[] {this.dto_1, this.dto_2});
            this.mockRestApi.Setup(x => x.ExecuteAsyncGet(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(json);
            this.mockMessanger.Setup(x => x.Send(It.IsAny<object>()))
                .Callback((object m) => lastMessage = m);
            
            contacts.GetContactsByNameAsync("asd");
            Assert.IsInstanceOf<ContactsReceived>(lastMessage);
            var vms = ((ContactsReceived) lastMessage).Contacts;
            var vm = vms.First();
            Assert.AreEqual(2, vms.Count());
            Assert.AreEqual(this.dto_1.Id, vm.Id);
            Assert.AreEqual(this.dto_1.FirstName, vm.FirstName);
            Assert.AreEqual(this.dto_1.LastName, vm.LastName);
            Assert.AreEqual(this.dto_1.BirthDate, vm.BirthDate);
            Assert.AreEqual(this.dto_1.Emails.Length, vm.Emails.Count);
            Assert.AreEqual(this.dto_1.PhoneNumbers.Length, vm.PhoneNumbers.Count);
        }

        [Test]
        public void AddContactAsync_should_send_ContactAdded_message_with_parsed_contact() {
            object lastMessage = null;
            var json = this.dto_1.SerializeToJson();
            this.mockRestApi.Setup(x => x.ExecuteAsyncPostWithJson(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(json);
            this.mockMessanger.Setup(x => x.Send(It.IsAny<object>()))
                .Callback((object m) => lastMessage = m);
            
            contacts.AddContactAsync(new ContactViewModel(this.mockContacts.Object));
            Assert.IsInstanceOf<ContactAdded>(lastMessage);
            var vm = ((ContactAdded) lastMessage).Contact;
            Assert.AreEqual(this.dto_1.Id, vm.Id);
            Assert.AreEqual(this.dto_1.FirstName, vm.FirstName);
            Assert.AreEqual(this.dto_1.LastName, vm.LastName);
            Assert.AreEqual(this.dto_1.BirthDate, vm.BirthDate);
            Assert.AreEqual(this.dto_1.Emails.Length, vm.Emails.Count);
            Assert.AreEqual(this.dto_1.PhoneNumbers.Length, vm.PhoneNumbers.Count);
        }
        
        [Test]
        public void UpdateContactAsync_should_send_ContactUpdated_message_with_parsed_contact() {
            object lastMessage = null;
            var json = this.dto_1.SerializeToJson();
            this.mockRestApi.Setup(x => x.ExecuteAsyncPostWithJson(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(json);
            this.mockMessanger.Setup(x => x.Send(It.IsAny<object>()))
                .Callback((object m) => lastMessage = m);
            
            contacts.UpdateContactAsync(new ContactViewModel(this.mockContacts.Object));
            Assert.IsInstanceOf<ContactUpdated>(lastMessage);
            var vm = ((ContactUpdated) lastMessage).Contact;
            Assert.AreEqual(this.dto_1.Id, vm.Id);
            Assert.AreEqual(this.dto_1.FirstName, vm.FirstName);
            Assert.AreEqual(this.dto_1.LastName, vm.LastName);
            Assert.AreEqual(this.dto_1.BirthDate, vm.BirthDate);
            Assert.AreEqual(this.dto_1.Emails.Length, vm.Emails.Count);
            Assert.AreEqual(this.dto_1.PhoneNumbers.Length, vm.PhoneNumbers.Count);
        }
        
        [Test]
        public void DeleteContactAsync_should_send_ContactDeleted_message_with_deleted_contact() {
            object lastMessage = null;
            this.mockRestApi.Setup(x => x.ExecuteAsyncDelete(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("1");
            this.mockMessanger.Setup(x => x.Send(It.IsAny<object>()))
                .Callback((object m) => lastMessage = m);

            var contact = new ContactViewModel(this.mockContacts.Object);
            contacts.DeleteContactAsync(contact);
            Assert.IsInstanceOf<ContactDeleted>(lastMessage);
            var vm = ((ContactDeleted) lastMessage).Contact;
            Assert.AreSame(contact, vm);
        }
    }
}