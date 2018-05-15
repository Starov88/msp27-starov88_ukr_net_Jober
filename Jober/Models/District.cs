using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class District : IEquatable<District>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public City City { get; set; }
        public int CityId { get; set; }

        public List<Location> Locations { get; set; }

       
        public bool Equals(District other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name) && Id.Equals(other.Id);
        }
    }
}
