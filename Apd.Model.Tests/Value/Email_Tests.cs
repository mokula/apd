using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class Email_Tests {
        [Test]
        [TestCase("smith@abc.com")]
        [TestCase("mark@fr.hotmail.com")]
        public void can_be_created_with_valid_email_address(string val) {
            var email = new Email(val);
            Assert.AreEqual(val, email.Value);
        }
        
        [Test]
        [TestCase("smithom@")]
        [TestCase("markfr.hotmail.com")]
        [TestCase("")]
        [TestCase(null)]
        public void cannot_be_created_with_invalid_email_address(string val) {
            Assert.Throws<ArgumentException>(() => new Email(val));
        }
    }
}