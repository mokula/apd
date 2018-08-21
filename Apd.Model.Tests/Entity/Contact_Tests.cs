
using System;
using Apd.Model.Entity;
using Apd.Model.Repository;
using Apd.Model.Value;
using Moq;
using NUnit.Framework;

namespace Apd.Model.Tests.Entity {
    [TestFixture]
    public class Contact_Tests {
        private Name firstName;
        private Name lastName;
        private BirthDate birthDate;
        private Mock<IContactRepository> mockRepository;
        
        [SetUp]
        public void Setup() {
            this.firstName = new Name("Jhon");
            this.lastName = new Name("Smith");
            this.birthDate = new BirthDate(new DateTime(1980, 1, 1));
            this.mockRepository = new Mock<IContactRepository>();
        }
        
        [Test]
        public void creating_instance_with_invalid_FirstName_should_throw_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, null, lastName, this.birthDate, new Email[0], new PhoneNumber[0]));
        }
        
        [Test]
        public void creating_instance_with_invalid_LasttName_should_throw_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, firstName, null, this.birthDate, new Email[0], new PhoneNumber[0]));
        }
        
        [Test]
        public void creating_instance_with_invalid_BirthDate_should_throw_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, firstName, lastName, null, new Email[0], new PhoneNumber[0]));
        }

        [Test]
        public void creating_instance_with_invalid_duplicate_Email_should_throw_ArgumentNullException() {
            Assert.Throws<InvalidOperationException>(() =>new Contact(1, firstName, lastName, birthDate,  new []{new Email("a@a.com"), new Email("a@a.com") }, new PhoneNumber[0]));
        }
        
        [Test]
        public void creating_instance_with_invalid_duplicate_PhoneNumber_should_throw_ArgumentNullException() {
            Assert.Throws<InvalidOperationException>(() =>new Contact(1, firstName, lastName, birthDate,  new Email[0], new []{new PhoneNumber("12312344"), new PhoneNumber("12312344")}));
        }
        
        [Test]
        public void AddEmail_should_throw_InvalidOperationException_for_duplicate_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new[] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("a@a.com");
            Assert.Throws<InvalidOperationException>(() => contact.AddEmail(email, this.mockRepository.Object));
        }
        
        [Test]
        public void AddPhoneNumber_should_throw_InvalidOperationException_for_duplicate_phone_number() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new []{new PhoneNumber("12312344")});
            var phoneNumber = new PhoneNumber("12312344");
            Assert.Throws<InvalidOperationException>(() => contact.AddPhoneNumber(phoneNumber, this.mockRepository.Object));
        }

        [Test]
        public void AddEmail_should_add_email_to_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var email = new Email("a@a.com");
            contact.AddEmail(email, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.AddContactEmail(contact.Id, email), Times.Once());
        }
        
        [Test]
        public void AddPhoneNumber_should_add_phone_number_to_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var phoneNumber = new PhoneNumber("1122334455");
            contact.AddPhoneNumber(phoneNumber, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.AddContactPhoneNumber(contact.Id, phoneNumber), Times.Once());
        }

        [Test]
        public void DeleteEmail_should_throw_InvalidOperationException_for_not_exisiting_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var email = new Email("a@a.com");
            Assert.Throws<InvalidOperationException>(() => contact.DeleteEmail(email, this.mockRepository.Object));
        }
        
        [Test]
        public void DeletePhoneNumber_should_throw_InvalidOperationException_for_not_exisiting_phone_number() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var phoneNumber = new PhoneNumber("211223432314");
            Assert.Throws<InvalidOperationException>(() => contact.DeletePhoneNumber(phoneNumber, this.mockRepository.Object));
        }
        
        [Test]
        public void DeleteEmail_should_delete_email_from_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new [] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("a@a.com");
            contact.DeleteEmail(email, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.DeleteContactEmail(contact.Id, email), Times.Once());
        }
        
        [Test]
        public void DeleteContactPhoneNumber_should_delete_phone_number_from_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new [] {new PhoneNumber("1122334455")});
            var phoneNumber = new PhoneNumber("1122334455");
            contact.DeletePhoneNumber(phoneNumber, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.DeleteContactPhoneNumber(contact.Id, phoneNumber), Times.Once());
        }
    }
}