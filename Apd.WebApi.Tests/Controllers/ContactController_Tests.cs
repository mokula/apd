using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Results;
using Apd.Common.DataTransferObject;
using Apd.Model.Entity;
using Apd.Model.Repository;
using Apd.Model.Value;
using Apd.WebApi.Controllers;
using Apd.WebApi.Factory;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Apd.WebApi.Tests.Controllers {
    [TestFixture]
    public class ContactController_Tests {
        private Mock<IContactFactory> mockFactory;
        private Mock<IContactRepository> mockRepository;
        private ContactController controller;
        private Contact contact;
        private ContactDto dto;

        [SetUp]
        public void Setup() {
            this.mockFactory = new Mock<IContactFactory>();
            this.mockRepository = new Mock<IContactRepository>();
            this.controller = new ContactController(this.mockRepository.Object, this.mockFactory.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            
            this.contact = new Contact(1, new Name("Kathryn"),new Name("Rogers"), new BirthDate(new DateTime(1967, 4,1)),
                new[] {new Email("krogers@gmail.com"), new Email("krogers@hotmail.com")},
                new[] {new PhoneNumber("44 344 543"), new PhoneNumber("+1 800 345 123")}) ;

            this.dto = new ContactDto {
                Id = 1,
                FirstName = "Kathryn",
                LastName =  "Rogers",
                BirthDate = new DateTime(1967, 4,1),
                Emails = new[] {"krogers@gmail.com", "krogers@hotmail.com"},
                PhoneNumbers = new[] {"44 344 543", "+1 800 345 123"}
            };
        }

        [Test]
        public void get_contact_returns_dto_parsed_to_json() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.GetContact(1) as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void get_contact_returns_not_found_result_for_invalid_id() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.GetContact(1);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void search_contact_by_name_returns_array_of_contacts_parrsed_to_json() {
            this.mockRepository.Setup(x => x.SearchContactsByName(It.IsAny<string>())).Returns(new[] { this.contact });
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.SearchContactsbyName("asd") as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, JsonConvert.SerializeObject(new[] {this.dto}));
        }
        
        [Test]
        public void get_all_contacts_returns_array_of_contacts_parrsed_to_json() {
            this.mockRepository.Setup(x => x.SearchContactsByName(It.IsAny<string>())).Returns(new[] { this.contact });
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.GetAllContacts() as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, JsonConvert.SerializeObject(new[] {this.dto}));
        }
        
        [Test]
        public void delete_contact_returns_ok_result_for_valid_id() {
            var actionResult = this.controller.DeleteContact(1);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }
        
        [Test]
        public void delete_contact_returns_not_found_result_for_invalid_id() {
            this.mockRepository.Setup(x => x.DeleteContact(It.IsAny<int>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.DeleteContact(1);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        
        [Test]
        public void update_contact_returns_updated_contact_parsed_to_json() {
            this.mockRepository.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Returns(this.contact);
            
            var actionResult = this.controller.UpdateContact("") as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void update_contact_returns_bad_request_for_invalid_contact() {
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.UpdateContact(this.dto.SerializeToJson());
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
        
        [Test]
        public void add_contact_returns_updated_contact_parsed_to_json() {
            this.mockRepository.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Returns(this.contact);
            
            var actionResult = this.controller.AddContact("") as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void add_contact_returns_bad_request_for_invalid_contact() {
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.AddContact(this.dto.SerializeToJson());
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }

        [Test]
        public void delete_contact_email_returns_ok_result_for_valid_email() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.DeleteContactEmail(1, this.contact.Emails.First().Value);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public void delete_contact_email_returns_bad_request_for_invalid_email() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.DeleteContactEmail(1, "a@b.c");
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
        
        [Test]
        public void add_contact_email_returns_ok_result_for_valid_email() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.AddContactEmail(1, "a@b.c");
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public void add_contact_email_returns_bad_request_for_invalid_email() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.AddContactEmail(1, this.contact.Emails.First().Value);
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
        
        // -------------------
        
        [Test]
        public void delete_contact_phone_returns_ok_result_for_valid_phone() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.DeleteContactPhone(1, this.contact.PhoneNumbers.First().Value);
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public void delete_contact_phone_returns_bad_request_for_invalid_phonel() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.DeleteContactPhone(1, "11222333666");
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
        
        [Test]
        public void add_contact_phone_returns_ok_result_for_valid_phone() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.AddContactPhone(1, "11222333666");
            Assert.IsInstanceOf<OkResult>(actionResult);
        }

        [Test]
        public void add_contact_phone_returns_bad_request_for_invalid_phone() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            var actionResult = this.controller.AddContactPhone(1, this.contact.PhoneNumbers.First().Value);
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
    }
}