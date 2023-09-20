using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public int MenuId { get; set; }
        [Required]
        public int FoodId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int TimeId { get; set; }
        public ICollection<int> SideDishes { get; set; }
        public UserViewModel User { get; set; }
    }
}
