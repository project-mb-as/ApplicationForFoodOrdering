using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Korisnik
    {
        public int KorisnikId { get; set; }
        [Required]
        public string Ime { get; set; }
        public string Prezime { get; set; }
        [Required]
        public string Email { get; set; }
        public string Lozinka { get; set; }
        [DefaultValue(0)]
        public int LocationId { get; set; }
        [DefaultValue(0)]
        public int TimeId { get; set; }

    }
}
