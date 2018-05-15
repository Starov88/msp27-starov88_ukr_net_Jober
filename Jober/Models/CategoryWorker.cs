using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class CategoryWorker
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
