using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class HranaPrilog
    {
        public int HranaPrilogId { get; set; }
        public int HranaId { get; set; }
        public Hrana Hrana { get; set; }
        public int PrilogId { get; set; }
        public Prilog Prilog { get; set; }
        public int Varijanta { get; set; }
    }
}
