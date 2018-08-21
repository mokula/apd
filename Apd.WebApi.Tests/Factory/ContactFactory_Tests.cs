using System;
using Apd.Model.Entity;
using Apd.Model.Value;
using Apd.WebApi.Service;
using NUnit.Framework;

namespace Apd.WebApi.Tests.Factory {
    [TestFixture]
    public class ContactFactory_Tests {
        private ContactFactory factory;
        
        [SetUp]
        public void Setup() {
            this.factory = new ContactFactory();
        }
        
        [Test]
        public void CreateFromOther_should_create_new_ionstance_of_Contact_with_provided_Id_and_other_values_coppied_from_other_contact() {
            var contact = new Contact(1, new Name("Jhon"), new Name("Smith"), new BirthDate(new DateTime(1980, 3, 24)), null, null);
            var other = factory.CreateFromOther(2, contact);
            Assert.AreEqual(2, other.Id);
            Assert.AreEqual(contact.FirstName, other.FirstName);
            Assert.AreEqual(contact.LastName, other.LastName);
            Assert.AreEqual(contact.BirthDate, other.BirthDate);
            Assert.AreEqual(0, other.Emails.Count);
            Assert.AreEqual(0, other.PhoneNumbers.Count);
        }

        [Test]
        public void ConvertToDto_should_create_new_insance_of_ContactDto_with_values_coppied_from_Contact() {
            var contact = new Contact(1, new Name("Jhon"), new Name("Smith"), new BirthDate(new DateTime(1980, 3, 24)), new [] {new Email("a@a.com") }, null);
            var dto = this.factory.ConvertToDto(contact);
            Assert.AreEqual(dto.Id, contact.Id);
            Assert.AreEqual(dto.FirstName, contact.FirstName.Value);
            Assert.AreEqual(dto.LastName, contact.LastName.Value);
            Assert.AreEqual(dto.BirthDate, contact.BirthDate.Value);
            Assert.AreEqual(1, dto.Emails.Length);
            Assert.AreEqual(0, dto.PhoneNumbers.Length);
        }

        [Test]
        public void CreateFromDto_should_create_new_insance_of_Contact_with_values_coppied_from_ContactDto() {
            var contact = new Contact(1, new Name("Jhon"), new Name("Smith"), new BirthDate(new DateTime(1980, 3, 24)), new [] {new Email("a@a.com") }, null);
            var dto = this.factory.ConvertToDto(contact);
            var frtomDto = this.factory.CreateFromDto(dto);
            Assert.AreEqual(contact.Id, frtomDto.Id);
            Assert.AreEqual(contact.FirstName.Value, frtomDto.FirstName.Value);
            Assert.AreEqual(contact.LastName.Value, frtomDto.LastName.Value);
            Assert.AreEqual(contact.BirthDate.Value, frtomDto.BirthDate.Value);
            Assert.AreEqual(contact.Emails.Count, frtomDto.Emails.Count);
            Assert.AreEqual(contact.PhoneNumbers.Count, frtomDto.PhoneNumbers.Count);
        }
    }
}