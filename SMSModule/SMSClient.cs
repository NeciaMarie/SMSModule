using System;
using Twilio.Clients;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SMSModule
{
    public class SMSClient : ISMSClient
    {
        public ITwilioRestClient _client;
       
        public SMSClient(string accountsid, string authtoken)
        {
            _client = new TwilioRestClient(Credentials.MasterAccountSid,Credentials.MasterAuthToken);

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

        public string AssignPhoneNumber(string pathaccountsid, string pathsid)
        {
            var incomingphonenumber = IncomingPhoneNumberResource.Update(pathAccountSid: pathaccountsid, pathSid: pathsid, client: _client);
            return incomingphonenumber.FriendlyName;
        }

        public async Task<MessageResource> SendMessage(string from, string to, string body)
        {
            var toPhoneNumber = new PhoneNumber(FormatPhoneNumber(to));
            var message = await MessageResource.CreateAsync(
                toPhoneNumber,
                from: new PhoneNumber(FormatPhoneNumber(from)),
                body: body,
                client: _client);
            return await Task.FromResult(message);
        }

        public static string FormatPhoneNumber(string unformattedNumber)
        {
            if (String.IsNullOrWhiteSpace(unformattedNumber)) throw new ArgumentNullException("unformattedNumber");
            var formattedNumber = Regex.Replace(unformattedNumber, "[^0-9\\.]", String.Empty);
            if (formattedNumber.Length < 10)
                throw new ArgumentException("unformattedNumber must at least be a 10 digit phone number");
            if (formattedNumber.Length == 10)
            formattedNumber = String.Concat("1", formattedNumber);
            return String.Concat("+", formattedNumber);
        }

    }
}
