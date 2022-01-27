using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hendry_Mason_HW5.Models
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "First name is required.")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Last name is required.")]
        public String LastName { get; set; }

        public List<Order> Orders { get; set; }

        public AppUser()
        {
            if (Orders == null)
            {
                Orders = new List<Order>();
            }
        }
    }
}
