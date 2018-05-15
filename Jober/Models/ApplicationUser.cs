using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Jober.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public float Balance { get; set; }

        public List<Location> Locations { get; set; }
        public List<Order> Orders { get; set; }
    }
}
