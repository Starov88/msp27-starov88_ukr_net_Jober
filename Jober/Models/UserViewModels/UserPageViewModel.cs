using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jober.Models.UserViewModels
{
    public class UserPageViewModel
    {
        [EmailAddress]
        [Display (Name = "Логин/Почта")]
        public string Email { get; set; }

        [Display(Name = "Почта подтверждена")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Phone]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Город")]
        public string City { get; set; }

        [Required]
        public int CityId { get; set; }

        public float Balance { get; set; }

        [Display(Name = "Исполнитель")]
        public bool IsWorker { get; set; }

        public List<Order> UserOrders { get; set; }
        public List<City> Cities { get; set; }
    }
}
