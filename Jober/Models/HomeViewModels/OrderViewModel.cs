using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jober.Models.HomeViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "№")]
        public string Number { get; set; }

        public int SelectedCategoryId { get; set; }

        public string SelectedCategoryName { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Не указан город")]
        public int CityId { get; set; }
        [Display(Name = "Город")]
        public City City { get; set; }

        [Range(1, 10000, ErrorMessage = "Не выбран район")]
        public int DistrictId { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Не указан адрес")]
        public string Address { get; set; }

        [Display(Name = "Дата")]
        [Required(ErrorMessage = "Не указана дата")]
        public System.DateTime Date { get; set; }

        [Display(Name = "Время")]
        [Required(ErrorMessage = "Не указано время")]
        public int Time { get; set; }

        [Required(ErrorMessage = "Не выбраны сервисы")]
        [Display(Name = "Доступные сервисы")]
        public List<int> ServiceIds { get; set; }
        [Display(Name = "Сервисы")]
        public List<Service> Services { get; set; }

        public Dictionary<int, byte> Quantity { get; set; }
        public List<string> QuantityString { get; set; }

        [Display(Name = "Описание")]
        [StringLength(1000, ErrorMessage = "Длина сообщения должна быть до 1000 символов")]
        public string Description { get; set; }

        public double TotalCost { get; set; }
    }
}
