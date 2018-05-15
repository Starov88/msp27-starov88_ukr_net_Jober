using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Models
{
    interface IMenuItem
    {
        int Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string ImgFilePath { get; set; }
    }
}
