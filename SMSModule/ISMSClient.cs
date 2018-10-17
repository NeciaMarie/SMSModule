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
     Task<MessageResource> SendMessage(string from, string to, string body);
    AccountResource CreateAccount(string ClinicID);
     string AssignPhoneNumber(string AccountSid, string PathSid);
    }
}
