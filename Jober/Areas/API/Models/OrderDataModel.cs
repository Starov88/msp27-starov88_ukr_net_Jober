using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Areas.API.Models
{
    public class OrderDataModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public string ClientCity { get; set; }
        public string ClientDistrict { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPhone { get; set; }
        public string Description { get; set; }
        public double TotalCost { get; set; }
        public string CategoryName { get; set; }
        public List<OrderDetailDataModel> OrderDetails { get; set; }
    }

    public class OrderDetailDataModel
    {
        public int Quantity { get; set; }
        public string SeviceName { get; set; }
    }
}
