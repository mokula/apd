using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Apd.Desktop.ViewModel {
    public class ContactViewModelValidator {
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private const string PhoneNumberPattern = @"^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$";
        
        private ContactViewModel owner;

        public string ValidationMsg { get; private set; }
        public bool IsValid => string.IsNullOrEmpty(this.ValidationMsg);

        public ContactViewModelValidator(ContactViewModel owner) {
            this.owner = owner;
        }

        public void Validate() {
            this.ValidationMsg = "";
            
            if (this.IsValid) this.ValidateFirstName();
            if (this.IsValid) this.ValidateLastName();
            if (this.IsValid) this.ValidateBirthDate();
            if (this.IsValid) this.ValidateEmails();
            if (this.IsValid) this.ValidatePhoneNumbers();
        }

        private void ValidateFirstName() {
            this.ValidatedName("first name", owner.FirstName);
        }

        private void ValidateLastName() {
            this.ValidatedName("last name", owner.LastName);
        }

        private void ValidatedName(string label, string name) {
            name = (name?? "").Trim();

            if (string.IsNullOrEmpty(name) || name.Length < 2) 
                this.SetValidationMessegeWithLabel(label);
        }

        private void ValidateBirthDate() {
            if (!owner.BirthDate.HasValue)
                return;

            if (owner.BirthDate.Value.Date < new DateTime(1900, 1, 1) || owner.BirthDate.Value.Date > DateTime.Today)
                this.SetValidationMessegeWithLabel("birth date");
        }

        private void SetValidationMessegeWithLabel(string label) {
            this.ValidationMsg = $"Invalid {label}.";
        }

        private void ValidateEmails() {
            foreach (var email in this.owner.Emails) {
                if (!this.IsEmailValid(email)) 
                    this.SetValidationMessegeWithLabel("email");
            }
        }

        public bool IsEmailValid(string email) {
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, EmailPattern);
        }
        
        private void ValidatePhoneNumbers() {
            foreach (var phoneNumber in this.owner.PhoneNumbers) {
                if (!this.IsPhoneValid(phoneNumber)) 
                    this.SetValidationMessegeWithLabel("phone number");
            }
        }
        
        public bool IsPhoneValid(string phone) {
            return !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, PhoneNumberPattern);
        }
    }
}