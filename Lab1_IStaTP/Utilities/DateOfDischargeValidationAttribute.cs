using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_IStaTP.Utilities
{
    public class DateOfDischargeValidationAttribute : ValidationAttribute
    {
        private readonly DateTime d;
        public DateOfDischargeValidationAttribute(DateTime d)
        {
            this.d = d;
        }


    }
}
