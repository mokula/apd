using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class PhoneNumber_Tests {
        [Test]
        [TestCase("12312344")]
        [TestCase("+1 800 564")]
        public void can_be_created_with_valid_phone_number(string val) {
            var email = new PhoneNumber(val);
            Assert.AreEqual(val, email.Value);
        }
        
        [Test]
        [TestCase("asd")]
        [TestCase("4455 // 43")]
        [TestCase("")]
        [TestCase(null)]
        public void cannot_be_created_with_invalid_phone_number(string val) {
            Assert.Throws<ArgumentException>(() => new PhoneNumber(val));
        }
    }
}