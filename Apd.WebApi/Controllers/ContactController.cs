
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Apd.Common.Communication;
using Apd.Common.Communication.DataTransferObject;
using Apd.Model.Repository;
using Apd.Model.Value;
using Apd.WebApi.Extension;
using Apd.WebApi.Service;
using Newtonsoft.Json;

namespace Apd.WebApi.Controllers {
    public class ContactController : ApiController {
        private IContactRepository contactRepository;
        private IContactFactory contactFactory;
        
        public ContactController(IContactRepository contactRepository, IContactFactory contactFactory) {
            this.contactRepository = contactRepository;
            this.contactFactory = contactFactory;
        }
        
        [HttpGet]
        [ActionName(ApiActionNames.ContactById)]
        public IHttpActionResult GetContact(int id) {
            try {
                var response = this.CreateResponsWithJsonContent(HttpStatusCode.OK, this.contactFactory.ConvertToDto(this.contactRepository.GetContact(id)).SerializeToJson());
                return this.ResponseMessage(response);
            }
            catch (InvalidOperationException) {
                return this.NotFound();
            }
        }
        
        [HttpGet]
        [ActionName(ApiActionNames.ContactsByName)]
        public IHttpActionResult SearchContactsbyName(string id) {
            var result = JsonConvert.SerializeObject(this.contactRepository.SearchContactsByName(id).Select(x => this.contactFactory.ConvertToDto(x)).ToArray());
            var response = this.CreateResponsWithJsonContent(HttpStatusCode.OK, result);
            return this.ResponseMessage(response);
        }
        
        [HttpGet]
        [ActionName(ApiActionNames.AllContacts)]
        public IHttpActionResult GetAllContacts() {
            return this.SearchContactsbyName(string.Empty);
        }
        
        [HttpDelete]
        [ActionName(ApiActionNames.DeleteContact)]
        public IHttpActionResult DeleteContact(int id) {
            try {
                this.contactRepository.DeleteContact(id);
                return this.Ok("1");
            }
            catch (InvalidOperationException) {
                return this.NotFound();
            }
        }

        [HttpPost]
        [ActionName(ApiActionNames.UpdateContact)]
        public IHttpActionResult UpdateContact([FromBody]ContactDto dto) {
            try {
                var contact = this.contactFactory.CreateFromDto(dto);
                contact = this.contactRepository.UpdateContact(contact);
                
                var response = this.CreateResponsWithJsonContent(HttpStatusCode.OK, this.contactFactory.ConvertToDto(contact).SerializeToJson());
                return this.ResponseMessage(response);
                
            }
            catch (InvalidOperationException) {
                return this.BadRequest();
            }
        }
        
        [HttpPost]
        [ActionName(ApiActionNames.AddContacts)]
        public IHttpActionResult AddContact([FromBody]ContactDto dto) {
            try {
                var contact = this.contactFactory.CreateFromDto(dto);
                contact = this.contactRepository.AddContact(contact);
                
                var response = this.CreateResponsWithJsonContent(HttpStatusCode.OK, this.contactFactory.ConvertToDto(contact).SerializeToJson());
                return this.ResponseMessage(response);
                
            }
            catch (InvalidOperationException) {
                return this.BadRequest();
            }
        }
        
//        [HttpPost]
//        [Route("api/contact/addemail/{id}/{email}")]
//        public IHttpActionResult AddContactEmail(int id, string email) {
//            try {
//                var contact = this.contactRepository.GetContact(id);
//                var newEmail = new Email(email);
//                contact.AddEmail(newEmail, this.contactRepository);
//                return this.Ok();
//            }
//            catch (InvalidOperationException) {
//                return this.BadRequest();
//            }
//        } 
//        
//        [HttpPost]
//        [Route("api/contact/deleteemail/{id}/{email}")]
//        public IHttpActionResult DeleteContactEmail(int id, string email) {
//            try {
//                var contact = this.contactRepository.GetContact(id);
//                var emailToDelete = new Email(email);
//                contact.DeleteEmail(emailToDelete, this.contactRepository);
//                return this.Ok();
//            }
//            catch (InvalidOperationException) {
//                return this.BadRequest();
//            }
//        } 
//        
//        [HttpPost]
//        [Route("api/contact/addphone/{id}/{phone}")]
//        public IHttpActionResult AddContactPhone(int id, string phone) {
//            try {
//                var contact = this.contactRepository.GetContact(id);
//                var newPhoneNumber = new PhoneNumber(phone);
//                contact.AddPhoneNumber(newPhoneNumber, this.contactRepository);
//                return this.Ok();
//            }
//            catch (InvalidOperationException) {
//                return this.BadRequest();
//            }
//        } 
//        
//        [HttpPost]
//        [Route("api/contact/deletephone/{id}/{phone}")]
//        public IHttpActionResult DeleteContactPhone(int id, string phone) {
//            try {
//                var contact = this.contactRepository.GetContact(id);
//                var phoneToDelete = new PhoneNumber(phone);
//                contact.DeletePhoneNumber(phoneToDelete, this.contactRepository);
//                return this.Ok();
//            }
//            catch (InvalidOperationException) {
//                return this.BadRequest();
//            }
//        } 
        
    }
}