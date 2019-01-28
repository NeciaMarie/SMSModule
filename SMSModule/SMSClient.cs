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
        private string[] outgoingMessageList;
        private string[] incomingMessageList;
        private string[] accountProperties;

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

        public string[] GetAccountInfo(string clinicID)
        {
            try
            {
                var account = Twilio.Rest.Api.V2010.AccountResource.Read(friendlyName: clinicID.Trim()
                                                                         , client: _client);
                accountProperties = new string[]
                {

                };

                return accountProperties;
            }

            catch (ApiException ex)
            {
                throw ex;
            }

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

                outgoingMessageList = new string[] { messageResponse.Sid
                                               ,messageResponse.DateCreated.ToString()
                                               ,messageResponse.To
                                               ,messageResponse.Body
                                               ,messageResponse.Status.ToString()
                                         };

                return outgoingMessageList;

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

                outgoingMessageList = new string[] {messageHistory.DateUpdated.ToString()
                                                ,messageHistory.Status.ToString()
                                          };

                return outgoingMessageList;
            }

            catch (ApiException ex)
            {
                throw ex;
            }
        }


        public string[] IncomingMessage(string to)
        {
            try

             {
                DateTime dateReceived = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,0,0,0);
                var messages = MessageResource.Read(
                               dateSent: dateReceived,
                               to: new PhoneNumber(FormatPhoneNumber(to)), client: _client);
                foreach (var record in messages)
                {
                    incomingMessageList = new string[] {record.Sid
                                                        ,record.DateCreated.ToString()
                                                        ,record.From.ToString()
                                                        ,record.Body
                                                        ,record.Status.ToString()};
                                                                 
                }

                return incomingMessageList;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
        }

        public static string FormatDatetime(string unformatteddatetime)
        {

            return unformatteddatetime;
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
