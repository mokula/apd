using Apd.Desktop.ViewModel;

namespace Apd.Desktop.Messaging {
    public class SelectedContactChanged {
        public ContactViewModel Contact { get; }

        public SelectedContactChanged(ContactViewModel contact) {
            this.Contact = contact;
        }
    }
}