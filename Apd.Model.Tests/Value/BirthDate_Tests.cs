using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class BirthDate_Tests {
        
        [Test]
        public void creating_instance_with_value_before_1900_1_1_should_throw_ArgumentException() {
            Assert.Throws<ArgumentException>(() => new BirthDate(new DateTime(1899, 12, 31)));
        }
        
        [Test]
        public void creating_instance_with_date_after_today_should_throw_ArgumentException() {
            Assert.Throws<ArgumentException>(() => new BirthDate(DateTime.Today.AddDays(1)));
        }
    }
}