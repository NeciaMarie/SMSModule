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
        public MessageResource messageResponse;
        string[] messageList;

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

      public  SMSClient(ITwilioRestClient client)
        {
            _client = client;
           
        }

      public string SendMessage(string from, string to, string body)
        {
          try
            {
                var messageResponse =  MessageResource.Create(
                   to: new PhoneNumber(FormatPhoneNumber(to)),
                   from: new PhoneNumber(FormatPhoneNumber(from)),
                   body: body,
                   client: _client);
                 
                return messageResponse.Sid ;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
         }

        public string[] getMessages(string From, string To)
        {

            var messageHistory = MessageResource.Read(
               //dateSent: new DateTime(DateSent),
               from: new PhoneNumber(FormatPhoneNumber(From)),
               to: new PhoneNumber(FormatPhoneNumber(To)),
               client: _client
               );
            int i = 0;

            foreach (var record in messageHistory)
            {
              i = i++;
              messageList = new string[] { record.Sid,record.DateSent.ToString()
                                           ,record.DateUpdated.ToString()
                                           ,record.To
                                           ,record.Status.ToString()
                                           ,record.Status.ToString()
                                           ,record.Body
                                         };

                messageList[i] = record.ToString(); 
               
            }
         return messageList;
        }


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
