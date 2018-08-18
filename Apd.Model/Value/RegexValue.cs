using System;
using System.Text.RegularExpressions;

namespace Apd.Model.Value {
    public class RegexValue {
        public string Value { get; }
        
        public RegexValue(string value, string pattern) {
            if (value == null || pattern == null || !Regex.IsMatch(value, pattern))
                throw new ArgumentException();

            this.Value = value;
        }
    }
}