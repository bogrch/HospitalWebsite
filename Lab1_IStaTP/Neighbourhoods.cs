using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Neighbourhoods
    {
        public Neighbourhoods()
        {
            Patients = new HashSet<Patients>();
        }

        public int NeighbourhoodId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Display(Name="Naming of neighbourhood")]
        [Required(ErrorMessage ="This field mustn`t be empty")]
        public string NeighbourhoodNaming { get; set; }

        public virtual ICollection<Patients> Patients { get; set; }
    }
}
