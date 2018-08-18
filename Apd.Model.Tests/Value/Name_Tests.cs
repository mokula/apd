using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class Name_Tests {
        [Test]
        public void change_first_letter_to_upper_and_rest_to_lower() {
            var name = new Name("jHoN");
            Assert.AreEqual(name.Value, "Jhon");
        }

        [Test]
        public void trim_white_characters_on_both_sides() {
           var name = new Name(" Abc     ");
            Assert.AreEqual(name.Value, "Abc");
        }

        [Test]
        [TestCase("  ")]
        [TestCase("")]
        [TestCase(null)]
        public void cannot_create_with_null_or_empty_value(string val) {
            Assert.Throws<ArgumentException>(() => new Name(val));
        }

        [Test]
        [TestCase("a")]
        [TestCase("  b")]
        public void cannot_contain_less_than_two_characters(string val) {
            Assert.Throws<ArgumentException>(() => new Name(val));
        }
    }
}