using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Apd.Desktop.ViewModel {
    public class ContactsViewModel : ViewModelBase {
        private IContacts contacts;
        private ContactViewModel selectedContact;

        public ContactViewModel SelectedContact {
            get => this.selectedContact;
            set {
                if (this.selectedContact == value)
                    return;

                this.selectedContact = value;
                this.RaisePropertyChanged(nameof(this.SelectedContact));
                this.RaisePropertyChanged(nameof(this.IsContactSelected));
                this.MessengerInstance.Send(new SelectedContactChanged(value));
            }
        }

        public bool IsContactSelected => this.SelectedContact != null;
        
        public ObservableCollection<ContactViewModel> ContactsList { get; } = new ObservableCollection<ContactViewModel>();
        public ICollectionView ContactsListView { get; } 

        private RelayCommand<string> searchContacts;
        public RelayCommand<string> SearchContacts {
            get {
                if (this.searchContacts == null)
                    this.searchContacts = new RelayCommand<string>(name => this.contacts.GetContactsByNameAsync(name));

                return this.searchContacts;
            }
        }

        public ContactsViewModel(IContacts contacts) {
            this.contacts = contacts;
            this.ContactsListView = CollectionViewSource.GetDefaultView(this.ContactsList);
            this.ContactsListView.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
            this.RegisterForMesseges();
        }

        private void RegisterForMesseges() {
            this.MessengerInstance.Register<ContactsReceived>(this, msg => {
                this.ContactsList.Clear();
                foreach (var contactViewModel in msg.Contacts) 
                    this.ContactsList.Add(contactViewModel);

                this.SelectedContact = null;
            });
            
            this.MessengerInstance.Register<ContactAdded>(this, msg => {
                this.ContactsList.Add(msg.Contact);
                this.SelectedContact = null;
            });
            
            this.MessengerInstance.Register<ContactUpdated>(this, msg => {
                var exisitingContact = this.ContactsList.FirstOrDefault(x => x.Id == msg.Contact.Id);
                if (exisitingContact == null)
                    return;

                this.ContactsList.Remove(exisitingContact);
                this.ContactsList.Add(msg.Contact);
                this.SelectedContact = null;
            });
            
            this.MessengerInstance.Register<ContactDeleted>(this, msg => {
                var exisitingContact = this.ContactsList.FirstOrDefault(x => x.Id == msg.Contact.Id);
                if (exisitingContact != null)
                    this.ContactsList.Remove(exisitingContact);

                this.SelectedContact = null;
            });
        }

        private RelayCommand addNew;

        public RelayCommand AddNew {
            get {
                if (this.addNew == null)
                    this.addNew = new RelayCommand(() => {
                        this.SelectedContact = new ContactViewModel(this.contacts) {
                            Id = -1,
                            FirstName = "New"
                        };
                    });

                return this.addNew;
            }
        }
    }
}
