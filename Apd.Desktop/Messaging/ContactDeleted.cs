using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class ContactDeleted: ContactMessage {
        public ContactDeleted(ContactViewModel contact) : base(contact) {
        }
    }
}