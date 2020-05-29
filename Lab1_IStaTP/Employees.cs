using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Employees
    {
        public int EmployeeId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Required(ErrorMessage = "Field mustn`t be empty")]
        [Display(Name = "Name of employee")]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Required(ErrorMessage = "Field mustn`t be empty")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Departments Department { get; set; }
    }
}
