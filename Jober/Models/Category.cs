using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class Category : IEquatable<Category>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Родительская категория")]
        public int? ParentId { get; set; }

        [Display(Name = "грн/час")]
        public float? Price { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [MaxLength(100)]
        public string ImgFilePath { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        [MaxLength(1000)]
        public string Description { get; set; }

        public List<Service> Services { get; set; }
        public List<Order> Orders { get; set; }
        public List<CategoryWorker> CategoryWorkers { get; set; }

        [NotMapped]
        public List<Category> AllCategories { get; set; }

        public bool Equals(Category other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            if (Object.ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name) && Id.Equals(other.Id);
        }
    }
}
