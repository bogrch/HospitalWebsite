using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_IStaTP
{
    public partial class Categories
    {
        public Categories()
        {
            Diseases = new HashSet<Diseases>();
        }

        public int CategoryId { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "String length must be between 3 and 20 symbols")]

        [Required(ErrorMessage = "Field mustn`t be empty")]
        [Display(Name = "Name of category of diseases")]
        public string CategoryNaming { get; set; }

        public virtual ICollection<Diseases> Diseases { get; set; }
    }
}
