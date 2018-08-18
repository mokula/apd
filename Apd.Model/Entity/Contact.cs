using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Apd.Model.Repository;
using Apd.Model.Value;

namespace Apd.Model.Entity {
     public class Contact {
        private readonly List<Email> emails = new List<Email>();
        private readonly List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();
        
        public int Id { get; }
        public Name FirstName { get; }
        public Name LastName { get; }
        public BirthDate BirthDate { get; }
        public ReadOnlyCollection<Email> Emails { get; }
        public ReadOnlyCollection<PhoneNumber> PhoneNumbers { get; }

        public Contact(int id, Name firstName, Name lastName, BirthDate birthDate, IEnumerable<Email> email, IEnumerable<PhoneNumber> phoneNumbers) {
            if (firstName == null)
                throw new ArgumentNullException(nameof(firstName));
            if (lastName == null)
                throw new ArgumentNullException(nameof(lastName));
            if (birthDate == null)
                throw new ArgumentNullException(nameof(birthDate));
            
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.Emails = new ReadOnlyCollection<Email>(this.emails);
            this.PhoneNumbers = new ReadOnlyCollection<PhoneNumber>(this.phoneNumbers);
            
            this.InitializeUniqueList(this.emails, email);
            this.InitializeUniqueList(this.phoneNumbers, phoneNumbers);
        }

        private void InitializeUniqueList(IList list, IEnumerable elements) {
            if (list == null)
                return;

            foreach (var element in elements) {
                if (list.Contains(element))
                    throw new InvalidOperationException();
                
                list.Add(element);
            }
        }

        public void AddEmail(Email email, IContactRepository contactRepository) {
            this.AddToUniqueList(this.emails, email);
            contactRepository.AddContactEmail(this.Id, email);
        }
        
        public void AddPhoneNumber(PhoneNumber phoneNumber, IContactRepository contactRepository) {
            this.AddToUniqueList(this.phoneNumbers, phoneNumber);
            contactRepository.AddContactPhoneNumber(this.Id, phoneNumber);
        }

        private void AddToUniqueList(IList list, object element) {
            if (list.Contains(element))
                throw new InvalidOperationException();

            list.Add(element);
        }

         public void DeleteEmail(Email email, IContactRepository contactRepository) {
             this.DeleteFromList(this.emails, email);
             contactRepository.DeleteContactEmail(this.Id, email);
         }
         
         public void DeletePhoneNumber(PhoneNumber phoneNumber, IContactRepository contactRepository) {
             this.DeleteFromList(this.phoneNumbers, phoneNumber);
             contactRepository.DeleteContactPhoneNumber(this.Id, phoneNumber);
         }

         public void DeleteFromList(IList list, object element) {
             if (!list.Contains(element))
                 throw new InvalidOperationException();
             
             list.Remove(element);
         }
         
    }
}