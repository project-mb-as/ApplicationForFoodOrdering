using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models
{
    public class Meni
    {
        public int MeniId { get; set; }
        public DateTime Datum { get; set; }

        [DefaultValue(false)]
        public bool Locked { get; set; }
        public virtual ICollection<HranaMeni> Hrana { get; set; }
        public virtual ICollection<Narudzba> Narudzbe { get; set; }

    }
}
