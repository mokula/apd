using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class ContactAdded : ContactMessage {
        public ContactAdded(ContactViewModel contact) : base(contact) {
        }
    }
}