using Lab1_IStaTP.Utilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Patients
    {
        public Patients()
        {
            PatientRegistration = new HashSet<PatientRegistration>();
        }
        public string DisplayName2
        {
            get
            {
                return this.Psurname + " " + this.Pname + " " + this.PdateOfBirth.ToString().Substring(0,10);
            }
        }

        public int PatientId { get; set; }
        [Required(ErrorMessage ="This field mustn`t be empty")]
        [Display(Name ="Patient Surname")]
                [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

       
        public string Psurname { get; set; }
        [Required(ErrorMessage = "This field mustn`t be empty")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Name")]
        public string Pname { get; set; }
        [Required(ErrorMessage = "This field mustn`t be empty")]

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]       
        [DateValidation(lowerBound:1900,upperBound: 2020, ErrorMessage = "Year must be between 1900 and 2020")]
        public DateTime? PdateOfBirth { get; set; }



        [Display(Name = "Neighbourhood")]
       
        public int? Pneighbourhood { get; set; }
        [Display(Name = "Address")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        public string Paddress { get; set; }
       
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Neighbourhood")]
        public virtual Neighbourhoods PneighbourhoodNavigation { get; set; }
        public virtual ICollection<PatientRegistration> PatientRegistration { get; set; }

        
    }
}
