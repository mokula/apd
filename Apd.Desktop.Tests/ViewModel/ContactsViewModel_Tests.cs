using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using Apd.Desktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;

namespace Apd.Desktop.Tests.ViewModel {
    [TestFixture]
    public class ContactsViewModel_Tests {
        private ContactsViewModel vm;
        private Mock<IContacts> contacts;
        
        [SetUp]
        public void Setup() {
            this.contacts = new Mock<IContacts>();
            this.vm = new ContactsViewModel(this.contacts.Object);
        }

        [Test]
        public void SearchContacts_should_use_IContacts_GetContactsByNameAsync_to_search_for_contacts() {
            this.vm.SearchContacts.Execute("name");
            this.contacts.Verify(x => x.GetContactsByNameAsync("name"));
        }

        [Test]
        public void should_replace_ContactsList_with_contacts_received_in_ContactsReceived_message() {
            this.vm.ContactsList.Add(new ContactViewModel(this.contacts.Object));
            var contact_1 = new ContactViewModel(this.contacts.Object);
            var contact_2 = new ContactViewModel(this.contacts.Object);
            Messenger.Default.Send(new ContactsReceived(new[] {contact_1, contact_2}));
            Assert.AreEqual(2, this.vm.ContactsList.Count);
            Assert.IsTrue(this.vm.ContactsList.Contains(contact_1));
            Assert.IsTrue(this.vm.ContactsList.Contains(contact_2));
        }

        [Test]
        public void SelectedContact_change_should_send_SelectedContactChanged_messege() {
            SelectedContactChanged msg = null;
            Messenger.Default.Register<SelectedContactChanged>(this, m => msg = m);
            this.vm.SelectedContact = new ContactViewModel(this.contacts.Object);
            Assert.IsNotNull(msg);
            Assert.AreSame(this.vm.SelectedContact, msg.Contact);
        }

        [Test]
        public void ContactsReceived_messege_clears_selected_contact() {
            this.vm.SelectedContact = new ContactViewModel(this.contacts.Object);
            Messenger.Default.Send(new ContactsReceived(new ContactViewModel[]{}));
            Assert.IsNull(this.vm.SelectedContact);
        }

        [Test]
        public void should_add_contact_to_ContactList_from_ContactAdded_message() {
            Messenger.Default.Send(new ContactAdded(new ContactViewModel(this.contacts.Object)));
            Assert.AreEqual(1, this.vm.ContactsList.Count);
        }

        [Test]
        public void should_replace_contact_in_ContactList_with_contact_from_ContactUpdated_message() {
            var exisitngcontact = new ContactViewModel(this.contacts.Object);
            exisitngcontact.Id = 1;
            exisitngcontact.FirstName = "Jhon";
            
            var updatedCotnact = new ContactViewModel(this.contacts.Object);
            updatedCotnact.Id = 1;
            updatedCotnact.FirstName = "Mark";
            
            this.vm.ContactsList.Add(exisitngcontact);
            Messenger.Default.Send(new ContactUpdated(updatedCotnact));
            Assert.AreEqual(1, this.vm.ContactsList.Count);
            Assert.AreEqual(updatedCotnact.FirstName, this.vm.ContactsList[0].FirstName);
        }
        
        [Test]
        public void should_remove_contact_from_ContactList_with_same_id_as_contact_from_ContactDeleted_message() {
            var exisitngcontact = new ContactViewModel(this.contacts.Object);
            exisitngcontact.Id = 1;
            exisitngcontact.FirstName = "Jhon";
            
            this.vm.ContactsList.Add(exisitngcontact);
            Messenger.Default.Send(new ContactDeleted(exisitngcontact));
            Assert.AreEqual(0, this.vm.ContactsList.Count);
        }
    }
}