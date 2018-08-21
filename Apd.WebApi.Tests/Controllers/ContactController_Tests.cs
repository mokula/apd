using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Apd.Common.Communication.DataTransferObject;
using Apd.Model.Entity;
using Apd.Model.Repository;
using Apd.Model.Value;
using Apd.WebApi.Controllers;
using Apd.WebApi.Service;
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
        public void GetContact_should_return_result_with_dto_parsed_to_json_and_status_code_OK() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.GetContact(1) as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void GetContact_for_invalid_id_should_return_NotFoundResult() {
            this.mockRepository.Setup(x => x.GetContact(It.IsAny<int>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.GetContact(1);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void SearchContactsbyName_should_return_result_with_array_of_dtos_parsed_to_json_and_status_code_OK() {
            this.mockRepository.Setup(x => x.SearchContactsByName(It.IsAny<string>())).Returns(new[] { this.contact });
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.SearchContactsbyName("asd") as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, JsonConvert.SerializeObject(new[] {this.dto}));
        }
        
        [Test]
        public void GetAllContacts_should_return_result_with_array_of_dtos_parsed_to_json_and_status_code_OK() {
            this.mockRepository.Setup(x => x.SearchContactsByName(It.IsAny<string>())).Returns(new[] { this.contact });
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            
            var actionResult = this.controller.GetAllContacts() as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, JsonConvert.SerializeObject(new[] {this.dto}));
        }
        
        [Test]
        public void Delete_for_valid_id_should_return_OkNegotiatedContentResult_with_value_1() {
            var actionResult = this.controller.DeleteContact(1);
            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(actionResult);
            Assert.AreEqual("1", ((OkNegotiatedContentResult<string>)actionResult).Content);
            
        }
        
        [Test]
        public void Delete_for_invalid_id_should_return_NotFoundResult() {
            this.mockRepository.Setup(x => x.DeleteContact(It.IsAny<int>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.DeleteContact(1);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        
        [Test]
        public void UpdateContact_should_return_result_with_dto_parsed_to_json_and_status_code_OK() {
            this.mockRepository.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Returns(this.contact);
            
            var actionResult = this.controller.UpdateContact(new ContactDto()) as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void UpdateContact_for_invalid_contact_should_return_BadRequestResult() {
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.UpdateContact(this.dto);
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
        
        [Test]
        public void AddContact_should_return_result_with_dto_parsed_to_json_and_status_code_OK() {
            this.mockRepository.Setup(x => x.UpdateContact(It.IsAny<Contact>())).Returns(this.contact);
            this.mockFactory.Setup(x => x.ConvertToDto(It.IsAny<Contact>())).Returns(this.dto);
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Returns(this.contact);
            
            var actionResult = this.controller.AddContact(new ContactDto()) as ResponseMessageResult;
            var task = actionResult.Response.Content.ReadAsStringAsync();
            task.Wait();
            Assert.AreEqual(actionResult.Response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(task.Result, this.dto.SerializeToJson());
        }

        [Test]
        public void AddContact_for_invalid_contact_should_return_BadRequestResult() {
            this.mockFactory.Setup(x => x.CreateFromDto(It.IsAny<ContactDto>())).Throws<InvalidOperationException>();
            var actionResult = this.controller.AddContact(this.dto);
            Assert.IsInstanceOf<BadRequestResult>(actionResult);
        }
    }
}