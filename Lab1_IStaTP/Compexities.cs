using System;
using System.Collections.Generic;

namespace Lab1_IStaTP
{
    public partial class Compexities
    {
        public Compexities()
        {
            PatientRegistration = new HashSet<PatientRegistration>();
        }

        public int ComplexityId { get; set; }
        public string ComplexityNaming { get; set; }

        public virtual ICollection<PatientRegistration> PatientRegistration { get; set; }
    }
}
