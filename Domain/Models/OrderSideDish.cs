using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class OrderSideDish
    {
        public int OrderSideDishId { get; set; }
        public int NarudzbaId { get; set; }
        public Narudzba Narudzba { get; set; }
        public int PrilogId { get; set; }
        public Prilog Prilog { get; set; }
    }
}
