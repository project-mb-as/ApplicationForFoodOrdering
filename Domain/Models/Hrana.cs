using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Hrana
    {
        public int HranaId { get; set; }
        public string Naziv { get; set; }
        public bool Stalna { get; set; }
        public string Image { get; set; }


        public virtual ICollection <Komentar> Komentari { get; set; }
        public virtual ICollection<Ocjena> Ocjene { get; set; }
        public virtual ICollection<HranaMeni> Menii { get; set; }
        public virtual ICollection<HranaPrilog> Prilozi { get; set; }

    }
}
