using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }

    enum OrderStatus { Active = 1, Complete, Canceled, Failed, InProgress, NaN };
}
