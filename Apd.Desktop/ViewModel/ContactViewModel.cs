using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Apd.Desktop.Messaging;
using Apd.Desktop.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Apd.Desktop.ViewModel {
    public class ContactViewModel : ViewModelBase {
        private int id;
        private string newEmail;
        private string newPhoneNumber;
        private string firstName;
        private string lastName;
        private DateTime? birthDate;
        private ContactViewModelValidator validator;
        private IContacts contacts; 
            
        public string ValidationMessage => this.validator.ValidationMsg;
        public bool IsValid => this.validator.IsValid;

        public ContactViewModel(IContacts contacts) {
            this.contacts = contacts;
            this.validator = new ContactViewModelValidator(this);
            this.MessengerInstance.Register<SelectedContactChanged>(this, m => this.ClearTempValues());
        }
        
        private string selectedEmail;
        public string SelectedEmail {
            get => selectedEmail;
            set { 
                if (this.selectedEmail == value)
                    return;

                this.selectedEmail = value;
                this.RaisePropertyChanged(nameof(this.SelectedEmail));
                this.DeleteSelectedEmail.RaiseCanExecuteChanged();
            }
        }
        
        private string selectedPhoneNumber;
        public string SelectedPhoneNumber {
            get => selectedPhoneNumber;
            set { 
                if (this.selectedPhoneNumber == value)
                    return;

                this.selectedPhoneNumber = value;
                this.RaisePropertyChanged(nameof(this.SelectedPhoneNumber));
                this.DeleteSelectedPhoneNumber.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand deleteSelectedEmail;
        public RelayCommand DeleteSelectedEmail {
            get {
                if (this.deleteSelectedEmail == null)
                    this.deleteSelectedEmail = new RelayCommand(() => {
                        this.Emails.Remove(this.SelectedEmail);
                        this.AddNewEmail.RaiseCanExecuteChanged();
                        this.Validate();
                    }, () => this.SelectedEmail != null);

                return this.deleteSelectedEmail;
            } 
        }
        
        private RelayCommand deleteSelectedPhoneNumber;
        public RelayCommand DeleteSelectedPhoneNumber {
            get {
                if (this.deleteSelectedPhoneNumber == null)
                    this.deleteSelectedPhoneNumber = new RelayCommand(() => {
                        this.PhoneNumbers.Remove(this.SelectedPhoneNumber);
                        this.AddNewPhoneNumber.RaiseCanExecuteChanged();
                        this.Validate();
                    }, () => this.SelectedPhoneNumber != null);

                return this.deleteSelectedPhoneNumber;
            } 
        }

        public ObservableCollection<string> Emails { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> PhoneNumbers { get; } = new ObservableCollection<string>();

        public string DisplayName => $"{this.firstName} {this.lastName}";

        public int Id {
            get => this.id;
            set {
                if (this.id == value)
                    return;

                this.id = value;
                this.RaisePropertyChanged(nameof(this.Id));
                this.RaisePropertyChanged(nameof(this.IsNew));
            }
        }

        public bool IsNew => this.Id < 0;
        
        public string FirstName {
            get => this.firstName;
            set {
                if (this.firstName == value)
                    return;

                this.firstName = value;
                this.RaisePropertyChanged(nameof(this.FirstName));
                this.RaisePropertyChanged(nameof(this.DisplayName));
                this.Validate();
            }
        }
        
        public string LastName {
            get => this.lastName;
            set {
                if (this.lastName == value)
                    return;

                this.lastName = value;
                this.RaisePropertyChanged(nameof(this.LastName));
                this.RaisePropertyChanged(nameof(this.DisplayName));
                this.Validate();
            }
        }

        public DateTime? BirthDate {
            get => this.birthDate; 
            set {
                if (this.birthDate == value)
                    return;

                this.birthDate = value;
                this.RaisePropertyChanged(nameof(this.BirthDate));
                this.Validate();
            }
        }

        public string NewEmail {
            get => newEmail;
            set { 
                if (this.newEmail == value)
                    return;

                this.newEmail = value;
                this.RaisePropertyChanged(nameof(this.newEmail));
                this.AddNewEmail.RaiseCanExecuteChanged();
            }
        }
        
        public string NewPhoneNumber {
            get => newPhoneNumber;
            set { 
                if (this.newPhoneNumber == value)
                    return;

                this.newPhoneNumber = value;
                this.RaisePropertyChanged(nameof(this.NewPhoneNumber));
                this.AddNewPhoneNumber.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand addNewEmail;
        public RelayCommand AddNewEmail {
            get {
                if (this.addNewEmail == null)
                    this.addNewEmail = new RelayCommand(
                        () => {
                            this.Emails.Add(this.NewEmail);
                            this.AddNewEmail.RaiseCanExecuteChanged();
                        }, 
                        () => this.validator.IsEmailValid(this.NewEmail) && !this.Emails.Contains(newEmail));

                return this.addNewEmail;
            }
        }
        
        private RelayCommand addNewPhoneNumber;
        public RelayCommand AddNewPhoneNumber {
            get {
                if (this.addNewPhoneNumber == null)
                    this.addNewPhoneNumber = new RelayCommand(
                        () => {
                            this.PhoneNumbers.Add(this.NewPhoneNumber);
                            this.AddNewPhoneNumber.RaiseCanExecuteChanged();
                        }, 
                        () => this.validator.IsPhoneValid(this.NewPhoneNumber) && !this.PhoneNumbers.Contains(this.NewPhoneNumber));

                return this.addNewPhoneNumber;
            }
        }

        public void Validate() {
            this.validator.Validate();
            this.RaisePropertyChanged(nameof(this.IsValid));
            this.RaisePropertyChanged(nameof(this.ValidationMessage));
            this.SaveContact.RaiseCanExecuteChanged();
        }

        private RelayCommand saveContact;

        public RelayCommand SaveContact {
            get {
                if (this.saveContact == null)
                    this.saveContact = new RelayCommand(() => {
                        if (this.IsNew)
                            this.contacts.AddContactAsync(this);
                        else
                            this.contacts.UpdateContactAsync(this);
                    }, () => this.IsValid);

                return this.saveContact;
            }
        }

        private void ClearTempValues() {
            this.NewEmail = null;
            this.NewPhoneNumber = null;
            this.SelectedEmail = null;
            this.selectedPhoneNumber = null;
        }

        private RelayCommand delete;

        public RelayCommand Delete {
            get {
                if (this.delete == null)
                    this.delete = new RelayCommand(() => this.contacts.DeleteContactAsync(this), () => !this.IsNew);

                return this.delete;
            }
        }
    }
}