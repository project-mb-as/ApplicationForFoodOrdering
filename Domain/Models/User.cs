using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string Roles { get; set; }
        [DefaultValue(0)]
        public int LocationId { get; set; }
        [DefaultValue(0)]
        public int TimeId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [DefaultValue(false)]
        public bool Activated { get; set; }

        [DefaultValue(true)]
        public bool ReceiveOrderConfirmationEmails { get; set; }

        [DefaultValue(true)]
        public bool ReceiveOrderWarningEmails { get; set; }


        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

    }
}
