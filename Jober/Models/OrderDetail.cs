using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jober.Models
{
    public class OrderDetail
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [Required]
        [Display(Name = "Сервис")]
        public Service Service { get; set; }

        [Required]
        public byte Quantity { get; set; }
    }
}
