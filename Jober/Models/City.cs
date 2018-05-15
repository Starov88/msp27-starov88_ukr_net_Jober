using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Location> Locations { get; set; }
        public List<District> Districts { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
