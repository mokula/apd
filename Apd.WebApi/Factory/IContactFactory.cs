using Apd.Common.DataTransferObject;
using Apd.Model.Entity;

namespace Apd.WebApi.Factory {
    public interface IContactFactory {
        Contact CreateFromOther(int id, Contact other);
        ContactDto ConvertToDto(Contact contact);
        Contact CreateFromDto(ContactDto dto);
    }
}