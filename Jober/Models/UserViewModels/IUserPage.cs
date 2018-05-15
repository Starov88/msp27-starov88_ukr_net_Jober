using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models.UserViewModels
{
    interface IUserPage
    {
        [Required]
        [EmailAddress]
        string Email { get; set; }

        string Name { get; set; }

        [Phone]
        string PhoneNumber { get; set; }

        List<Order> Orders { get; set; }

        bool IsEmailConfirmed { get; set; }

        bool IsWorker { get; set; }
    }
}
