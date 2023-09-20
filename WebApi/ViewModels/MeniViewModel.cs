using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class MeniViewModel
    {
        [Required]
        public int MenuId { get; set; }
        public DateTime Date { get; set; }
        //public List<HranaMeniViewModel> Hrana { get; set; }
        public List<int> Food { get; set; }
        public bool CanOrder { get; set; }
    }
}
