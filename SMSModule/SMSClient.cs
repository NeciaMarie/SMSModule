using System;
using Twilio.Clients;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Exceptions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SMSModule
{
    public class SMSClient : ISMSClient

    {
        public ITwilioRestClient _client;


        public SMSClient()
        {
            try
            {
                _client = new TwilioRestClient(Credentials.masterAccountSid, Credentials.masterAuthToken);
            }
            catch (TwilioException ex)
            {
                throw ex;
            }
        }

        public SMSClient(ITwilioRestClient client)
        {
            _client = client;
        }

        public string CreateAccount(string clinicID)
        {
            var account = AccountResource.Create(friendlyName: clinicID, client: _client);
            return account.Sid;
        }

        public AccountResource.StatusEnum UpdateAccount(string accountSid, string _status)
        {
            var account = AccountResource.Update(pathSid: accountSid, status: _status, client: _client);
            return account.Status;

        }

   
        public async Task<MessageResource> SendMessage(string from, string to, string body)
        {
            var message = await MessageResource.CreateAsync(
                to:  new PhoneNumber(FormatPhoneNumber(to)),
                from: new PhoneNumber(FormatPhoneNumber(from)),
                body: body,
                client: _client);
            return await Task.FromResult(message);
        }


        //public MessageResource FetchMessages(long dateSent, string from, string to)
        //{
           
        //    var messages = MessageResource.Read(
        //       dateSent: new DateTime(dateSent),
        //       from: new PhoneNumber(FormatPhoneNumber(from)),
        //       to: new PhoneNumber (FormatPhoneNumber(to))
        //       );

        //    foreach (var record in messages)
        //    {

        //    }
         
        //}

        public static string FormatPhoneNumber(string unformattedNumber)
        {
            if (String.IsNullOrWhiteSpace(unformattedNumber))
                throw new ArgumentNullException("unformattedNumber");

            var formattedNumber = Regex.Replace(unformattedNumber, "[^0-9\\.]", String.Empty);

            if (formattedNumber.Length < 10)
                throw new ArgumentException("unformattedNumber must at least be a 10 digit phone number");

            if (formattedNumber.Length == 10)
                formattedNumber = String.Concat("1", formattedNumber);

            return String.Concat("+", formattedNumber);
        }

    }
}
