using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models.AdminViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public List<Category> AllCategories { get; set; }

        public CategoryViewModel()
        {
            AllCategories = new List<Category>();
        }
        
    }
}
