using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class HranaMeniViewModel
    {
        public int HranaId { get; set; }
        public int MeniId { get; set; } 
        public HranaViewModel Hrana { get; set; }
        public MeniViewModel Meni { get; set; }
    }
}
