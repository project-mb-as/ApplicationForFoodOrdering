using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class CommentViewModel
    {
        
        [Required]
        public int FoodId { get; set; }
        [Required]
        public string Content { get; set; }

        public string Image { get; set; }
        public string User { get; set; }
        public DateTime Time { get; set; }
        public string ImageBase64 { get; set; }



    }
}
