using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class HranaViewModel
    {
        public int HranaId { get; set; }
        [Required]
        public string Naziv { get; set; }
        public bool Stalna { get; set; }
        public List<PrilogViewModel> Prilozi { get; set; }
        public bool Narucena { get; set; }
        public double Rating { get; set; }
        public int NumberOfComments { get; set; }
        public string Image { get; set; }
    }
}
