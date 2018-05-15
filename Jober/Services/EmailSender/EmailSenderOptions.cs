using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Services
{
    public class EmailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Key { get; set; }
    }
}
