using System;
using System.Collections.Generic;
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
        bool SendMessage(string from, string to, string body);
        string getResponseProperty(SMSResponse.ResponsePropertyId iResponsePropertyId);
        string FetchMessages(long dateSent, string from, string to);
        //Task SendMessage(string from, string to, string body)
    }
}
