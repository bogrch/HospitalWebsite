using Lab1_IStaTP.Utilities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Lab1_IStaTP
{
    [Authorize(Roles="admin")]
    public partial class PatientRegistration
    {
        public int AccountId { get; set; }
        [Display(Name = "Patient")]
        public int? PatientId { get; set; }
        [Display(Name = "Disease")]
        public int? DiseaseId { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Complexity")]

        public int? ComplexityId { get; set; }
        [Display(Name ="Date of starting treatment")]
        [DataType(DataType.Date)]
        [DateValidation(1999, 2020, ErrorMessage = "Incorrect year")]


        public DateTime? PrdateOfStart { get; set; }
        [Display(Name = "Date of finishing treatment")]
        [DataType(DataType.Date)]
        [DateValidation(1999,2020,ErrorMessage ="Incorrect year")]
        


        public DateTime? PrdateOfDischarge { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Complexity of disease")]
        public virtual Compexities Complexity { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Department")]
        public virtual Departments Department { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Disease")]
        public virtual Diseases Disease { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name ="Patient Name")]
        public virtual Patients Patient { get; set; }
        
    }
}
