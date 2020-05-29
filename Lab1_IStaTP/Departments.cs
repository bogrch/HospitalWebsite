using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Departments
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
            PatientRegistration = new HashSet<PatientRegistration>();
        }

        public int DepartmentId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Required(ErrorMessage = "Field mustn`t be empty")]
        [Display(Name= "Department")]
        public string DepartmentNaming { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name = "Chief")]
        public string ChiefOfDepartment { get; set; }
        [Range(1,100,ErrorMessage ="Value must be between 1 and 100")]

        [Display(Name = "Quantity of places for patients")]
        public int? QuantityOfPlaces { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<PatientRegistration> PatientRegistration { get; set; }
    }
}
