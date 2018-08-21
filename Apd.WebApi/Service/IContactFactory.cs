using Apd.Common.Communication.DataTransferObject;
using Apd.Model.Entity;

namespace Apd.WebApi.Service {
    public interface IContactFactory {
        Contact CreateFromOther(int id, Contact other);
        ContactDto ConvertToDto(Contact contact);
        Contact CreateFromDto(ContactDto dto);
    }
}