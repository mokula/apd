using System;
using Newtonsoft.Json;

namespace Apd.Common.Communication.DataTransferObject {
    public class ContactDto {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string[] Emails { get; set; }
        public string[] PhoneNumbers { get; set; }

        public string SerializeToJson() {
            return JsonConvert.SerializeObject(this);
        }
        
        public static ContactDto FromJson(string json) {
            return JsonConvert.DeserializeObject<ContactDto>(json);
        }
    }
}