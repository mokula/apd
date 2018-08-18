using System.Collections.Generic;
using Apd.Model.Entity;
using Apd.Model.Value;

namespace Apd.Model.Repository {
    public interface IContactRepository {
        void AddContactEmail(int contactId, Email email);
        void AddContactPhoneNumber(int contactId, PhoneNumber phoneNumber);
        void DeleteContactEmail(int contactId, Email email);
        void DeleteContactPhoneNumber(int contactId, PhoneNumber phoneNumber);

        void AddContact(Contact contact);
        void DeleteContact(Contact contact);
        void UpdateContact(Contact contact);

        void GetContact(int id);
        IEnumerable<Contact> SearchContactsByName(string name);
    }
}