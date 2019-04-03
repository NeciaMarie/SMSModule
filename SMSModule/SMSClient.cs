using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using Twilio.Clients;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Linq;

namespace SMSModule
{
    public class SMSClient : ISMSClient

    {
        public ITwilioRestClient _client;
        public MessageResource messageResponse;
        private string[] outgoingMessageList;
        private string[] incomingmessage;
        

        public SMSClient()

        {
            try
            {
                _client = new TwilioRestClient(Credentials.masterAccountSid, Credentials.masterAuthToken);

            }
            catch (TwilioException)
            {
                throw ;
            }
        }
        
        public SMSClient(ITwilioRestClient client)
        {
            _client = client;

        }

        public string[] SendMessage(string from, string to, string body, string subaccountsid)
        {
            try
            {
                var messageResponse = MessageResource.Create(
                   to: new PhoneNumber(FormatPhoneNumber(to)),
                   pathAccountSid: subaccountsid,
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
            catch (ApiException)
            {
                throw ;
            }
        }

        public string[] GetMessageStatus(string MessageID, string subaccountsid)
        {
            try
            {
                var messageHistory = MessageResource.Fetch(
                   pathSid: MessageID,
                   pathAccountSid: subaccountsid,
                   client: _client
                );

                outgoingMessageList = new string[] {messageHistory.DateUpdated.ToString()
                                                ,messageHistory.Status.ToString()
                                          };

                return outgoingMessageList;
            }

            catch (ApiException)
            {
                throw ;
            }
        }


        public int IncomingMessagesCount(string subaccountsid, string to, string date)
        {
            try

            {
                DateTime dateReceived = new DateTime(DateTime.Parse(date).Year, DateTime.Parse(date).Month, DateTime.Parse(date).Day
                                                    , DateTime.Parse(date).Hour, DateTime.Parse(date).Minute, DateTime.Parse(date).Second);
                var messages = MessageResource.Read(
                               pathAccountSid: subaccountsid,
                               dateSent: dateReceived,
                               to: new PhoneNumber(FormatPhoneNumber(to)), client: _client);
                int MessageCount = 0;
                foreach (var record in messages)
                {
                     MessageCount++;
                }
               
                return MessageCount;
            }
            catch (ApiException)
            {
                throw ;
            }
        }

       public string[] IncomingMessages(string subaccountsid, string to, string date)
        {
            try

            {
                DateTime dateReceived = new DateTime(DateTime.Parse(date).Year, DateTime.Parse(date).Month, DateTime.Parse(date).Day
                                                     , DateTime.Parse(date).Hour, DateTime.Parse(date).Minute, DateTime.Parse(date).Second);
                var messages = MessageResource.Read(
                               pathAccountSid: subaccountsid,
                               dateSent: dateReceived,
                               to: new PhoneNumber(FormatPhoneNumber(to)), client: _client);
                foreach (var record in messages)
                {
                    incomingmessage = new string[] {record.Sid
                                                        ,record.DateCreated.ToString()
                                                        ,record.From.ToString()
                                                        ,record.Body
                                                        ,record.Status.ToString()};
                }

                return incomingmessage;
            }
            catch (ApiException )
            {
                throw ;
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
