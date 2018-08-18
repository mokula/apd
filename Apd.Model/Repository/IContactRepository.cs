using System.Collections.Generic;
using Apd.Model.Entity;
using Apd.Model.Value;

namespace Apd.Model.Repository {
    public interface IContactRepository {
        void AddContactEmail(int contactId, Email email);
        void AddContactPhoneNumber(int contactId, PhoneNumber phoneNumber);
        void DeleteContactEmail(int contactId, Email email);
        void DeleteContactPhoneNumber(int contactId, PhoneNumber phoneNumber);

        Contact AddContact(Contact contact);
        void DeleteContact(int id);
        Contact UpdateContact(Contact contact);

        Contact GetContact(int id);
        IEnumerable<Contact> SearchContactsByName(string name);
    }
}