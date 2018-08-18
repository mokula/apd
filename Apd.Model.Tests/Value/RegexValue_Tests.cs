using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class RegexValue_Tests {
        [Test]
        [TestCase("44312", @"^\d+$")]
        [TestCase("aabaac", @"^[abc]+$")]
        public void can_be_created_with_matching_pattern_and_value(string value, string pattern) {
            var regexValue = new RegexValue(value, pattern);
            Assert.AreEqual(value, regexValue.Value);
        }

        [Test]
        [TestCase("44312a", @"^\d+$")]
        [TestCase("aabaac4", @"^[abc]+$")]
        public void cannot_be_created_with_not_matching_pattern_and_value(string value, string pattern) {
            Assert.Throws<ArgumentException>(() => new RegexValue(value, pattern));
        }

        [Test]
        public void cannot_be_created_with_null_value() {
            Assert.Throws<ArgumentException>(() => new RegexValue(null, "a*"));
        }
        
        [Test]
        public void cannot_be_created_with_null_pattern() {
            Assert.Throws<ArgumentException>(() => new RegexValue("asd", null));
        }
    }
}