using System;

namespace Apd.Model.Value {
    public class BirthDate {
        public DateTime? Value { get; }
        public bool IsEmpty => !Value.HasValue;

        public BirthDate(DateTime? value = null) {
            if (!value.HasValue)
                return;

            if (value.Value.Date < new DateTime(1900, 1, 1) || value.Value.Date > DateTime.Today)
                throw new ArgumentException();
                
            this.Value = value;
        }
    }
}