using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class HranaPrilogViewModel
    {
        public HranaViewModel Hrana { get; set; }
        public PrilogViewModel Prilog { get; set; }
        public int Varijanta { get; set; }
    }
}
