using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class HranaMeni
    {
        public int HranaId { get; set; }
        public Hrana Hrana { get; set; }
        public int MeniId { get; set; }
        public Meni Meni { get; set; }
    }
}
