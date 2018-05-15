using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jober.Models
{
    public class Order : IComparable<Order>
    {
        [ScaffoldColumn(false)]
        [Display(Name = "№")]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "№")]
        public string Number { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Не указана локация")]
        [Display(Name = "Адрес")]
        public Location Location { get; set; }
        public int LocationId { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }

        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        [MaxLength(1000)]
        [StringLength(1000, ErrorMessage = "Длина сообщения должна быть до 1000 символов")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Сумма")]
        public double TotalCost { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int StatusId { get; set; }
        [Required]
        [Display(Name = "Статус")]
        public Status Status { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Display(Name = "Детали")]
        public List<OrderDetail> OrderDetails { get; set; }

        public int CompareTo(Order order)
        {
            return this.Date.CompareTo(order.Date);
        }
    }
}
