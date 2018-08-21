using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class Email_Tests {
        [Test]
        [TestCase("smithom@")]
        [TestCase("markfr.hotmail.com")]
        [TestCase("")]
        [TestCase(null)]
        public void creating_instance_with_invalid_email_format_should_throw_ArgumentException(string val) {
            Assert.Throws<ArgumentException>(() => new Email(val));
        }
    }
}