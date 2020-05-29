using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Diseases
    {
        public Diseases()
        {
            PatientRegistration = new HashSet<PatientRegistration>();
        }

        public int DiseaseId { get; set; }
        public int? CategoryId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Required(ErrorMessage = "Field mustn`t be empty")]
        [Display(Name = "Name of Disease")]
        public string DiseaseNaming { get; set; }

        public virtual Categories Category { get; set; }
        public virtual ICollection<PatientRegistration> PatientRegistration { get; set; }
    }
}
