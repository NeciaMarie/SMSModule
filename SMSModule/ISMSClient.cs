using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010;
using Twilio.Types;
using Twilio.Clients;

namespace SMSModule
{
    interface ISMSClient
    {
      string[] SendMessage(string from, string to, string body,string subaccountsid);
      string[] GetMessageStatus(string MessageID,string subaccountsid);
      int IncomingMessagesCount(string subaccountsid, string to,string date);
      object IncomingMessages(string subaccountsid, string to,string date);
   }
}


