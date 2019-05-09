using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSModule
{
   public  class Billing
    {
        public string Sid { get; set; }
        public string CountUnit { get; set; }
        public string Count { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
    }
}
