using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Prilog
    {
        public int PrilogId { get; set; }
        public string Naziv { get; set; }
        public ICollection<OrderSideDish> Orders { get; set; }
        public virtual ICollection<HranaPrilog> Hrana { get; set; }
    }
}
