using System;
using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using Apd.Desktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;

namespace Apd.Desktop.Tests.ViewModel {
    
    [TestFixture]
    public class ContactViewModel_Tests {
        private Mock<IContacts> contacts;
        public ContactViewModel vm;

        [SetUp]
        public void Setup() {
            this.contacts = new Mock<IContacts>();
            this.vm = new ContactViewModel(this.contacts.Object);
        }

        private void PopulateContactWithValidData() {
            this.vm.Id = 1;
            this.vm.FirstName = "Jhon";
            this.vm.LastName = "Smith";
            this.vm.BirthDate = new DateTime(1980, 1, 1);
        }

        [Test]
        public void DeleteSelectedEmail_command_shoud_remove_SelectedEmail_from_Emails() {
            var email = "a@.a";
            this.vm.Emails.Add(email);
            this.vm.SelectedEmail = email;
            this.vm.DeleteSelectedEmail.Execute(null);
            Assert.AreEqual(0, this.vm.Emails.Count);
        }
        
        [Test]
        public void DeleteSelectedPhoneNumber_command_should_remove_SelectedPhoneNumber_from_PhoneNumbers() {
            var phoneNumber = "11 22 3334 555";
            this.vm.PhoneNumbers.Add(phoneNumber);
            this.vm.SelectedPhoneNumber = phoneNumber;
            this.vm.DeleteSelectedPhoneNumber.Execute(null);
            Assert.AreEqual(0, this.vm.PhoneNumbers.Count);
        }

        [Test]
        public void SaveContact_command_should_be_active_when_ContactViewModel_is_valid() {
            this.PopulateContactWithValidData();
            Assert.IsTrue(this.vm.SaveContact.CanExecute(null));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("a")]
        public void SaveContact_command_should_be_inactive_when_FirstName_is_invalid(string name) {
            this.PopulateContactWithValidData();
            this.vm.FirstName = name;
            Assert.IsFalse(this.vm.SaveContact.CanExecute(null));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("a")]
        public void SaveContact_command_should_be_inactive_when_LastName_is_invalid(string name) {
            this.PopulateContactWithValidData();
            this.vm.LastName = name;
            Assert.IsFalse(this.vm.SaveContact.CanExecute(null));
        }
        
        [Test]
        [TestCase("1789-1-1")]
        [TestCase("2030-1-1")]
        public void SaveContact_command_should_be_inactive_when_BirthDate_is_invalid(DateTime birthDate) {
            this.PopulateContactWithValidData();
            this.vm.BirthDate = birthDate;
            Assert.IsFalse(this.vm.SaveContact.CanExecute(null));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("asd@asd")]
        public void AddNewEmail_should_be_inactive_when_NewEmail_is_invalid(string email) {
            this.PopulateContactWithValidData();
            this.vm.NewEmail = email;
            Assert.IsFalse(this.vm.AddNewEmail.CanExecute(null));
        }
        
        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("1f4")]
        public void AddNewPhoneNumber_should_be_inactive_when_NewPhoneNumber_is_invalid(string phoneNumber) {
            this.PopulateContactWithValidData();
            this.vm.NewPhoneNumber = phoneNumber;
            Assert.IsFalse(this.vm.AddNewPhoneNumber.CanExecute(null));
        }

        [Test]
        public void SelectedContactChanged_message_clears_NewEmail_NewPhoneNumber_SelectedEmail_SelectedPhoneNumber() {
            var email = "a@a.pl";
            var phoneNumber = "11 3323 4455";
            this.vm.Emails.Add(email);
            this.vm.PhoneNumbers.Add(phoneNumber);
            this.vm.NewEmail = email;
            this.vm.NewPhoneNumber = phoneNumber;
            this.vm.SelectedEmail = email;
            this.vm.SelectedPhoneNumber = phoneNumber;
            
            Messenger.Default.Send(new SelectedContactChanged(null));
            Assert.IsTrue(string.IsNullOrEmpty(this.vm.NewEmail));
            Assert.IsTrue(string.IsNullOrEmpty(this.vm.NewPhoneNumber));
            Assert.IsTrue(string.IsNullOrEmpty(this.vm.SelectedEmail));
            Assert.IsTrue(string.IsNullOrEmpty(this.vm.SelectedPhoneNumber));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-20)]
        [TestCase(-100)]
        public void IsNew_is_true_when_for_negative_Id(int id) {
            this.vm.Id = id;
            Assert.IsTrue(this.vm.IsNew);
        }

        [Test]
        public void SaveContact_command_should_use_IContacts_AddContactAsync_when_IsNew_is_true() {
            this.PopulateContactWithValidData();
            this.vm.Id = -1;
            this.vm.SaveContact.Execute(null);
            this.contacts.Verify(x => x.AddContactAsync(this.vm), Times.Once);
        }
        
        [Test]
        public void SaveContact_command_should_use_IContacts_UpdateContactAsync_when_IsNew_is_false() {
            this.PopulateContactWithValidData();
            this.vm.SaveContact.Execute(null);
            this.contacts.Verify(x => x.UpdateContactAsync(this.vm), Times.Once);
        }
    }
}