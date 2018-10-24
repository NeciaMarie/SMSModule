using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;


namespace SMSModule
{
  public class SMSResponse
    {
        public PhoneNumber PhoneNumber;
        public string MessageText = "";
        public string SendingStatus = "";
        public string DeliveryStatus = "";
        public DateTime SentDate;
        public DateTime DeliveryDate;
        public string MessageId = "";

        public enum ResponsePropertyId
        {
            MessageId = 1,
            PhoneNumber,
            MessageText,
            SendingStatus,
            DeliveryStatus,
            SentDate,
            DeliveryDate
           
        }
           

    }
}
