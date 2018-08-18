using System;

namespace Apd.Model.Value {

    public class PhoneNumber : RegexValue, IEquatable<PhoneNumber>{
        private const string PhoneNumberPattern = @"^(1[ \-\+]{0,3}|\+1[ -\+]{0,3}|\+1|\+)?((\(\+?1-[2-9][0-9]{1,2}\))|(\(\+?[2-8][0-9][0-9]\))|(\(\+?[1-9][0-9]\))|(\(\+?[17]\))|(\([2-9][2-9]\))|([ \-\.]{0,3}[0-9]{2,4}))?([ \-\.][0-9])?([ \-\.]{0,3}[0-9]{2,4}){2,3}$";

        public PhoneNumber(string value) : base(value, PhoneNumberPattern) {
        }

        public bool Equals(PhoneNumber other) {
            return other != null &&
                   this.Value == other.Value;
        }
    }
    
    
}