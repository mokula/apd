using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Apd.Model.Entity;
using Apd.Model.Repository;
using Apd.Model.Value;
using Apd.WebApi.Factory;

namespace Apd.WebApi.Repository {
    public class DictionaryContactRepository : IContactRepository {
        private ConcurrentDictionary<int, Contact> dictionary = new ConcurrentDictionary<int, Contact>();
        private IContactFactory contactFactory;

        public DictionaryContactRepository(IContactFactory contactFactory, bool addFakeContacts) {
            this.contactFactory = contactFactory;
            
            if (addFakeContacts)
                this.InitializeFakeContacts();
        }
        
        public DictionaryContactRepository(IContactFactory contactFactory): this(contactFactory, true) {
        }

        private void InitializeFakeContacts() {
            this.dictionary.TryAdd(1, new Contact(1, new Name("Jhon"),new Name("Smith"), new BirthDate(new DateTime(1980, 3,24)),
                null,
                null
                ));
            
            this.dictionary.TryAdd(2, new Contact(2, new Name("Kathryn"),new Name("Rogers"), new BirthDate(new DateTime(1967, 4,1)),
                new[] {new Email("krogers@gmail.com"), new Email("krogers@hotmail.com")},
                new[] {new PhoneNumber("44 344 543"), new PhoneNumber("+1 800 345 123") }
            ));
        }
        
        public void AddContactEmail(int contactId, Email email) {
            //
        }

        public void AddContactPhoneNumber(int contactId, PhoneNumber phoneNumber) {
            //
        }

        public void DeleteContactEmail(int contactId, Email email) {
            //
        }

        public void DeleteContactPhoneNumber(int contactId, PhoneNumber phoneNumber) {
            // 
        }

        public Contact AddContact(Contact contact) {
            var contactWithNewId = this.contactFactory.CreateFromOther(this.dictionary.Count + 1, contact);
            this.dictionary.TryAdd(contactWithNewId.Id, contactWithNewId);
            return contactWithNewId;
        }

        public void DeleteContact(int id) {
            if (!this.dictionary.ContainsKey(id))
                throw new InvalidOperationException();

            this.dictionary.TryRemove(id, out _);
        }

        public Contact UpdateContact(Contact contact) {
            if (!this.dictionary.ContainsKey(contact.Id))
                throw new InvalidOperationException();

            this.dictionary[contact.Id] = contact;
            return contact;
        }

        public Contact GetContact(int id) {
            if (!this.dictionary.ContainsKey(id))
                throw new InvalidOperationException();

            return this.dictionary[id];
        }

        public IEnumerable<Contact> SearchContactsByName(string name) {
            return this.dictionary.Values.Where(x => (x.FirstName.Value + " " + x.LastName.Value).ToLower().Contains(name.ToLower())).ToList();
        }
    }
}