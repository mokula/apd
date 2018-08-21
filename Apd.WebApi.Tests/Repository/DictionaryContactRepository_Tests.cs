using System;
using System.Linq;
using Apd.Model.Entity;
using Apd.Model.Value;
using Apd.WebApi.Service;
using Moq;
using NUnit.Framework;

namespace Apd.WebApi.Tests.Repository {
    [TestFixture]
    public class DictionaryContactRepository_Tests {
        private DictionaryContactRepository repo;
        private Contact contact_1;
        private Contact contact_2;
        private Contact contact_3;
        
        private Mock<IContactFactory> mockFactory;

        [SetUp]
        public void Setup() {
            
            this.contact_1 = new Contact(-1, new Name("Kathryn"),new Name("Rogers"), new BirthDate(new DateTime(1967, 4,1)),
                new[] {new Email("krogers@gmail.com"), new Email("krogers@hotmail.com")},
                new[] {new PhoneNumber("44 344 543"), new PhoneNumber("+1 800 345 123")}) ;
            
            this.contact_2 = new Contact(1, new Name("Kathryn"),new Name("Rogers"), new BirthDate(new DateTime(1967, 4,1)),
                new[] {new Email("krogers@gmail.com"), new Email("krogers@hotmail.com")},
                new[] {new PhoneNumber("44 344 543"), new PhoneNumber("+1 800 345 123")}) ;
            
            this.contact_3 = new Contact(2, new Name("Jhon"),new Name("Smith"), new BirthDate(new DateTime(1980, 3,24)),
                null,
                null
            );
            
            this.mockFactory = new Mock<IContactFactory>();
            this.repo = new DictionaryContactRepository(this.mockFactory.Object, false);
        }

        private void DefaultFactorySetup() {
            this.mockFactory.Setup(x => x.CreateFromOther(It.IsAny<int>(), It.IsNotNull<Contact>())).Returns((int x, Contact y) => y);
        }

        [Test]
        public void AddContact_to_empty_reposiotry_should_overwrite_Contact_Id_with_1() {
            this.mockFactory.Setup(x => x.CreateFromOther(It.IsAny<int>(), this.contact_1)).Returns(this.contact_2);
            var addedContact = this.repo.AddContact(this.contact_1);
            Assert.AreEqual(1, addedContact.Id);
        }

        [Test]
        public void UpdateContact_should_replace_exisitng_contact_with_same_Id() {
            this.DefaultFactorySetup();
            var contactToUpdate = new Contact(1, new Name("Abc"),new Name("Abc"), new BirthDate(new DateTime(1980, 1,1)), null, null);
            this.repo.AddContact(this.contact_2);
            this.repo.UpdateContact(contactToUpdate);
            
            Assert.AreSame(contactToUpdate, this.repo.GetContact(this.contact_2.Id));
        }

        [Test]
        [TestCase("s", 2)]
        [TestCase("JHON", 1)]
        [TestCase("gErs", 1)]
        public void SearchContactsByName_should_retun_contacts_with_matching_FirstName_or_LastName_case_invariant(string val, int numberOfMatchingNames) {
            this.DefaultFactorySetup();
            this.repo.AddContact(this.contact_2);
            this.repo.AddContact(this.contact_3);
            var contacts = this.repo.SearchContactsByName(val);
            Assert.AreEqual(numberOfMatchingNames, contacts.Count());
        }
    }
}