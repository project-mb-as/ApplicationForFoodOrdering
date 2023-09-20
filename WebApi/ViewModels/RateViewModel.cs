using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class RateViewModel
    {
        
        [Required]
        public int FoodId { get; set; }
        [Required]
        public int Mark { get; set; }
        
    }
}
