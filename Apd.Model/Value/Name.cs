using System;

namespace Apd.Model.Value {
    public class Name {
        public string Value { get; }
        
        public Name(string value) {
            value = (value ?? "").Trim();
            
            if (string.IsNullOrEmpty(value) || value.Length < 2)
                throw new ArgumentException();
            
            this.Value = value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
        }
    }
}