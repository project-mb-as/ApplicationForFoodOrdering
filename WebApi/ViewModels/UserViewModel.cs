using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Email { get; set; }
        public bool Activated { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public int LocationId { get; set; }
        public int TimeId { get; set; }
    }
}
