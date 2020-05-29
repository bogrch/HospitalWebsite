using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_IStaTP.Utilities
{
    public class DateValidationAttribute : ValidationAttribute
    {
        private readonly int lowerBound; 
        private readonly int upperBound;
        public DateValidationAttribute(int lowerBound, int upperBound)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }
        public override bool IsValid(object value)
        {
            string s = value.ToString();
            DateTime.TryParse(s, out DateTime d);
            // DateTime d = value;
            return (d.Year <= upperBound && d.Year >= lowerBound);
            
        }
    }
}
