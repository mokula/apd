using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Service {
    public interface IContacts {
        void GetContactsByNameAsync(string name);
        void AddContactAsync(ContactViewModel contactVm);
        void UpdateContactAsync(ContactViewModel contactVm);
        void DeleteContactAsync(ContactViewModel contactVm);
    }
}