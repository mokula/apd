using System.Linq;
using Apd.Common.Communication.DataTransferObject;
using Apd.Model.Entity;
using Apd.Model.Value;

namespace Apd.WebApi.Service {
    public class ContactFactory : IContactFactory{
        public Contact CreateFromOther(int id, Contact other) {
            return new Contact(id, other.FirstName, other.LastName, other.BirthDate, other.Emails, other.PhoneNumbers);
        }

        public ContactDto ConvertToDto(Contact contact) {
            return new ContactDto {
                Id =  contact.Id,
                FirstName = contact.FirstName.Value,
                LastName = contact.LastName.Value,
                BirthDate = contact.BirthDate.Value,
                Emails =  contact.Emails.Select(x => x.Value).ToArray(),
                PhoneNumbers =  contact.PhoneNumbers.Select(x => x.Value).ToArray(),
            };
        }

        public Contact CreateFromDto(ContactDto dto) {
            return new Contact(
                dto.Id,
                new Name(dto.FirstName),
                new Name(dto.LastName),
                new BirthDate(dto.BirthDate),
                dto.Emails.Select(x => new Email(x)),
                dto.PhoneNumbers.Select(x => new PhoneNumber(x))
                );
        }
    }
}