using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class MeniForCalendarViewModel
    {
        public int MeniId { get; set; }
        public DateTime Datum { get; set; }

        public bool CanOrder { get; set; }
    }
}
