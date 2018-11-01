using System;
using System.Text.RegularExpressions;
using Twilio.Clients;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SMSModule
{
    public class SMSClient : ISMSClient

    {
        public ITwilioRestClient _client;
        public MessageResource messageResponse;
        private string[] messageList;

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

        public string[] SendMessage(string from, string to, string body)
        {
            try
            {
                var messageResponse = MessageResource.Create(
                   to: new PhoneNumber(FormatPhoneNumber(to)),
                   from: new PhoneNumber(FormatPhoneNumber(from)),
                   body: body,
                   client: _client);

                messageList = new string[] { messageResponse.Sid
                                               ,messageResponse.DateCreated.ToString()
                                               ,messageResponse.To
                                               ,messageResponse.Body
                                               ,messageResponse.Status.ToString()
                                         };

                return messageList;

            }
            catch (ApiException ex)
            {
                throw ex;
            }
        }


        public string[] GetMessageStatus(string MessageID)
        {
            try
            {
                var messageHistory = MessageResource.Fetch(
                   pathSid: MessageID,
                   client: _client
                );

                messageList = new string[] {messageHistory.DateUpdated.ToString()
                                                ,messageHistory.Status.ToString()
                                          };


                return messageList;
            }

            catch (ApiException ex)
            {
                throw ex;
            }

        }

        public static string FormatPhoneNumber(string unformattedNumber)
        {
            if (String.IsNullOrWhiteSpace(unformattedNumber))
            {
                throw new ArgumentNullException("unformattedNumber");
            }

            var formattedNumber = Regex.Replace(unformattedNumber, "[^0-9\\.]", String.Empty);

            if (formattedNumber.Length < 10)
            {
                throw new ArgumentException("unformattedNumber must at least be a 10 digit phone number");
            }

            if (formattedNumber.Length == 10)
            {
                formattedNumber = String.Concat("1", formattedNumber);
            }

            return String.Concat("+", formattedNumber);
        }
    }
}
