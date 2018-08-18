
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
        public void cannot_be_created_without_first_name() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, null, lastName, this.birthDate, new Email[0], new PhoneNumber[0]));
        }
        
        [Test]
        public void cannot_be_created_without_last_name() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, firstName, null, this.birthDate, new Email[0], new PhoneNumber[0]));
        }
        
        [Test]
        public void cannot_be_created_without_birth_date() {
            Assert.Throws<ArgumentNullException>(() =>new Contact(1, firstName, lastName, null, new Email[0], new PhoneNumber[0]));
        }

        [Test]
        public void cannot_be_created_with_duplicate_emails() {
            Assert.Throws<InvalidOperationException>(() =>new Contact(1, firstName, lastName, birthDate,  new []{new Email("a@a.com"), new Email("a@a.com") }, new PhoneNumber[0]));
        }
        
        [Test]
        public void cannot_be_created_with_duplicate_phone_numbers() {
            Assert.Throws<InvalidOperationException>(() =>new Contact(1, firstName, lastName, birthDate,  new Email[0], new []{new PhoneNumber("12312344"), new PhoneNumber("12312344")}));
        }

        [Test]
        public void can_add_unique_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new[] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("b@b.com");
            contact.AddEmail(email, this.mockRepository.Object);
            Assert.IsTrue(contact.Emails.Contains(email));
        }
        
        [Test]
        public void can_add_unique_phone_numbert() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new []{new PhoneNumber("12312344")});
            var phoneNumber = new PhoneNumber("1235553323");
            contact.AddPhoneNumber(phoneNumber, this.mockRepository.Object);
            Assert.IsTrue(contact.PhoneNumbers.Contains(phoneNumber));
        }
        
        [Test]
        public void cannot_add_duplicate_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new[] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("a@a.com");
            Assert.Throws<InvalidOperationException>(() => contact.AddEmail(email, this.mockRepository.Object));
        }
        
        [Test]
        public void cannot_add_duplicate_phone_numbert() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new []{new PhoneNumber("12312344")});
            var phoneNumber = new PhoneNumber("12312344");
            Assert.Throws<InvalidOperationException>(() => contact.AddPhoneNumber(phoneNumber, this.mockRepository.Object));
        }

        [Test]
        public void valid_email_is_added_to_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var email = new Email("a@a.com");
            contact.AddEmail(email, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.AddContactEmail(contact.Id, email), Times.Once());
        }
        
        [Test]
        public void valid_phone_number_is_added_to_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var phoneNumber = new PhoneNumber("1122334455");
            contact.AddPhoneNumber(phoneNumber, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.AddContactPhoneNumber(contact.Id, phoneNumber), Times.Once());
        }

        [Test]
        public void cannot_delete_not_existing_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var email = new Email("a@a.com");
            Assert.Throws<InvalidOperationException>(() => contact.DeleteEmail(email, this.mockRepository.Object));
        }
        
        [Test]
        public void can_delete_existing_email() {
            var contact = new Contact(1, firstName, lastName, birthDate, new[] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("a@a.com");
            contact.DeleteEmail(email, this.mockRepository.Object);
            Assert.AreEqual(contact.Emails.Count, 0);
        }
        
        [Test]
        public void cannot_delete_not_existing_phone_number() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new PhoneNumber[0]);
            var phoneNumber = new PhoneNumber("211223432314");
            Assert.Throws<InvalidOperationException>(() => contact.DeletePhoneNumber(phoneNumber, this.mockRepository.Object));
        }
        
        [Test]
        public void can_delete_existing_phone_number() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new [] {new PhoneNumber("211223432314") });
            var phoneNumber = new PhoneNumber("211223432314");
            contact.DeletePhoneNumber(phoneNumber, this.mockRepository.Object);
            Assert.AreEqual(contact.PhoneNumbers.Count, 0);
        }
        
        [Test]
        public void valid_email_is_deleted_from_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new [] {new Email("a@a.com")}, new PhoneNumber[0]);
            var email = new Email("a@a.com");
            contact.DeleteEmail(email, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.DeleteContactEmail(contact.Id, email), Times.Once());
        }
        
        [Test]
        public void valid_phone_number_is_deleted_from_repository() {
            var contact = new Contact(1, firstName, lastName, birthDate, new Email[0], new [] {new PhoneNumber("1122334455")});
            var phoneNumber = new PhoneNumber("1122334455");
            contact.DeletePhoneNumber(phoneNumber, this.mockRepository.Object);
            this.mockRepository.Verify(x => x.DeleteContactPhoneNumber(contact.Id, phoneNumber), Times.Once());
        }
    }
}