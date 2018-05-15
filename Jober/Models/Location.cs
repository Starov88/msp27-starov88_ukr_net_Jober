using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан город")]
        [Display(Name = "Город")]
        public City City { get; set; }
        public int CityId { get; set; }

        [Required(ErrorMessage = "Не указан район")]
        [Display(Name = "Район")]
        public District District { get; set; }
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Не указан адрес")]
        [Display(Name = "Адрес")]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Address { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public int? ZipCode { get; set; }

        [Display(Name = "Домашний адрес")]
        [UIHint("Boolean")]
        public bool? IsResidence { get; set; }

        public List<Order> Orders { get; set; }
    }
}
