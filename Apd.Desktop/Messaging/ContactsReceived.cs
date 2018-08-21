using System.Collections.Generic;
using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class ContactsReceived {
        public IEnumerable<ContactViewModel> Contacts { get; }
        
        public ContactsReceived(IEnumerable<ContactViewModel> contacts) {
            this.Contacts = contacts;
        }
    }
}