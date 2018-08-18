using System;

namespace Apd.Model.Value {
    public class Email : RegexValue, IEquatable<Email> {
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        
        public Email(string value ): base(value, EmailPattern) {
        }

        public bool Equals(Email other) {
            return other != null &&
                   this.Value == other.Value;
        }
    }
}