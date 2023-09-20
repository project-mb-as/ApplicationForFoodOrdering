using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class PrilogViewModel
    {
        public int PrilogId { get; set; }
        public int Varijanta { get; set; }
        public string Naziv { get; set; }
    }
}
