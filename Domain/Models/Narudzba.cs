using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Narudzba
    {
        public int NarudzbaId { get; set; }
        [Required]
        public int MeniId { get; set; }

        public Meni Meni { get; set; }
        [Required]
        public int HranaId { get; set; }
        public Hrana Hrana { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
        public int Dostavljena { get; set; }
        public string Note { get; set; }
        public int LocationId { get; set; }
        public int TimeId { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public virtual ICollection<OrderSideDish> SideDishes { get; set; }

        //public ICollection<SavedBook> BookUsers { get; set; }
    }
}
