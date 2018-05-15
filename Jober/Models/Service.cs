using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class Service
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Сервис")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
