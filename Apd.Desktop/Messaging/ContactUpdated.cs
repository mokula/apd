using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class ContactUpdated: ContactMessage {
        public ContactUpdated(ContactViewModel contact) : base(contact) {
        }
    }
}