using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class PhoneNumber_Tests {
        [Test]
        [TestCase("asd")]
        [TestCase("4455 // 43")]
        [TestCase("")]
        [TestCase(null)]
        public void creating_instance_with_invalid_phone_number_format_should_throw_ArgumentException(string val) {
            Assert.Throws<ArgumentException>(() => new PhoneNumber(val));
        }
    }
}