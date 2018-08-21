using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class RegexValue_Tests {
        [Test]
        [TestCase("44312a", @"^\d+$")]
        [TestCase("aabaac4", @"^[abc]+$")]
        public void creating_instance_with_non_matching_value_and_pattern_should_throw_ArgumentException(string value, string pattern) {
            Assert.Throws<ArgumentException>(() => new RegexValue(value, pattern));
        }

        [Test]
        public void creating_instance_with_null_value_should_throw_ArgumentException() {
            Assert.Throws<ArgumentException>(() => new RegexValue(null, "a*"));
        }
        
        [Test]
        public void creating_instance_with_null_pattern_should_throw_ArgumentException() {
            Assert.Throws<ArgumentException>(() => new RegexValue("asd", null));
        }
    }
}