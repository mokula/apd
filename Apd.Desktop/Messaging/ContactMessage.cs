using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class ContactMessage {
        public ContactViewModel Contact { get; set; }

        public ContactMessage(ContactViewModel contact) {
            this.Contact = contact;
        }
    }
}