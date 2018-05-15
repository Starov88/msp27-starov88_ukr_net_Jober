using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class Worker
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsActive { get; set; }

        public string WorkerSettingJson { get; set; }

        [Display(Name = "Хорошие отзывы")]
        public int EvaluationGood { get; set; }

        [Display(Name = "Плохие отзывы")]
        public int EvaluationBad { get; set; }

        public List<Order> Orders { get; set; }
        public List<CategoryWorker> CategoryWorkers { get; set; }
    }
}
