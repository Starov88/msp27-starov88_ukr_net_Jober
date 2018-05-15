using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Areas.API.Models
{
    public class PutOrderDataModel
    {
        public string Guid { get; set; }
        public int WorkerId { get; set; }
        public int Status { get; set; }
    }
}
