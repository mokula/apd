using System.Linq;
using Apd.Common.Communication;
using Apd.Common.Communication.DataTransferObject;
using Apd.Common.Container;
using Apd.Desktop.Messaging;
using Apd.Desktop.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;

namespace Apd.Desktop.Service {
    public class Contacts : IContacts {
        private IRestApi restApi;
        private IContainer container;
        private IMessenger messenger;

        public Contacts(IRestApi restApi, IMessenger messenger, IContainer container) {
            this.restApi = restApi;
            this.messenger = messenger;
            this.container = container;
        }
        
        public async void GetContactsByNameAsync(string name) {
            var actionName = string.IsNullOrEmpty(name) ? ApiActionNames.AllContacts : ApiActionNames.ContactsByName;
            var contactsJson = await this.restApi.ExecuteAsyncGet(actionName, name);
            if (string.IsNullOrEmpty(contactsJson))
                return;
            
            var vms = JsonConvert.DeserializeObject<ContactDto[]>(contactsJson).Select(this.ContactDtoToViewModel).ToArray();
            this.messenger.Send(new ContactsReceived(vms));
        }

        public async void AddContactAsync(ContactViewModel contactVm) {
            var contactJson = this.ContactViewModelToDto(contactVm).SerializeToJson();
            var result = await this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.AddContacts, contactJson);
            if (!string.IsNullOrEmpty(result))
                this.messenger.Send(new ContactAdded(this.ContactDtoToViewModel(JsonConvert.DeserializeObject<ContactDto>(result))));
        }
        
        public async void UpdateContactAsync(ContactViewModel contactVm) {
            var contactJson = this.ContactViewModelToDto(contactVm).SerializeToJson();
            var result = await this.restApi.ExecuteAsyncPostWithJson(ApiActionNames.UpdateContact, contactJson);
            if (!string.IsNullOrEmpty(result))
                this.messenger.Send(new ContactUpdated(this.ContactDtoToViewModel(JsonConvert.DeserializeObject<ContactDto>(result))));
        }
        
        public async void DeleteContactAsync(ContactViewModel contactVm) {
            var result = await this.restApi.ExecuteAsyncDelete(ApiActionNames.DeleteContact, contactVm.Id.ToString());
            if (!string.IsNullOrEmpty(result))
                this.messenger.Send(new ContactDeleted(contactVm));
        }

        private ContactViewModel ContactDtoToViewModel(ContactDto dto) {
            var vm = this.container.Resolve<ContactViewModel>();
            vm.Id = dto.Id;
            vm.FirstName = dto.FirstName;
            vm.LastName = dto.LastName;
            vm.BirthDate = dto.BirthDate;
            
            foreach (var email in dto.Emails) 
                vm.Emails.Add(email);
            
            foreach (var phone in dto.PhoneNumbers) 
                vm.PhoneNumbers.Add(phone);
            
            return vm;
        }

        private ContactDto ContactViewModelToDto(ContactViewModel vm) {
            return new ContactDto {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Emails = vm.Emails.ToArray(),
                PhoneNumbers = vm.PhoneNumbers.ToArray()
            };
        }
    }
}