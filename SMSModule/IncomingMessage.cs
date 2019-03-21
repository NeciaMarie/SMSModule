using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSModule
{
 public class IncomingMessage
    {
        public string Sid { get; set; }
        public string DateCreated { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }
}
