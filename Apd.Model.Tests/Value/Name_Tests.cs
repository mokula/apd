using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class Name_Tests {
        [Test]
        public void creating_instance_should_change_first_letter_to_upper_case_and_rest_to_lower_case() {
            var name = new Name("jHoN");
            Assert.AreEqual(name.Value, "Jhon");
        }

        [Test]
        public void creating_instance_should_trim_white_spaces() {
           var name = new Name(" Abc     ");
            Assert.AreEqual(name.Value, "Abc");
        }

        [Test]
        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void creating_instance_with_null_or_empty_value_should_throw_ArgumentException(string val) {
            Assert.Throws<ArgumentException>(() => new Name(val));
        }

        [Test]
        [TestCase("a")]
        [TestCase("  b")]
        public void creating_instance_with_less_than_two_characters_should_throw_ArgumentException(string val) {
            Assert.Throws<ArgumentException>(() => new Name(val));
        }
    }
}