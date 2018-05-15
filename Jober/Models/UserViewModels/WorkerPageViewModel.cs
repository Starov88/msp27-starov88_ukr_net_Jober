using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models.UserViewModels
{
    public class WorkerPageViewModel
    {
        public string UserId { get; set; }

        public int WorkerId { get; set; }

        public City City { get; set; }

        public WorkerSetting WorkerSetting { get; set; }

        public List<District> Districts { get; set; }

        public List<Category> Categories { get; set; }

        public List<Order> OrdersNew { get; set; }

        public List<Order> OrdersInWork { get; set; }
    }
}
