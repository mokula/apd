using System;
using Apd.Model.Value;
using NUnit.Framework;

namespace Apd.Model.Tests.Value {
    [TestFixture]
    public class BirthDate_Tests {

        private static DateTime valid_date;
        
        [Test]
        public void is_empty_when_crewated_without_value() {
            var birthDate = new BirthDate();        
            Assert.IsTrue(birthDate.IsEmpty);
        }

        [Test]
        public void cannot_be_before_1900_1_1() {
            Assert.Throws<ArgumentException>(() => new BirthDate(new DateTime(1899, 12, 31)));
        }
        
        [Test]
        public void cannot_be_after_today() {
            Assert.Throws<ArgumentException>(() => new BirthDate(DateTime.Today.AddDays(1)));
        }
    }
}