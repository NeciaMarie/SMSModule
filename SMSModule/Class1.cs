using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Clients;
using System.Threading.Tasks;

namespace SMSModule
{
    public interface IRestClient
    {
        Task<MessageResource> SendMessage(string from, string to, string body);
    }

    public class SMSClient:IRestClient
    {
       private readonly TwilioRestClient _client;

        
        public async Task<MessageResource> SendMessage(string from, string to, string body)
        {
            var toPhoneNumber = new PhoneNumber(to);
            return await MessageResource.CreateAsync(
                toPhoneNumber,
                from: new PhoneNumber(from),
                body: body,
                client: _client);
        }

    }
    
}
