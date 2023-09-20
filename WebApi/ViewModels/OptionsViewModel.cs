using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class OptionsViewModel
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int TimeId { get; set; }
        [Required]
        public bool ReceiveOrderConfirmationEmails { get; set; }
        [Required]
        public bool ReceiveOrderWarningEmails { get; set; }
    }
}
