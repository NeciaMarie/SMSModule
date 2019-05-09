using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Twilio.Clients;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Newtonsoft.Json;
using Twilio.Rest.Api.V2010.Account.Usage;

namespace SMSModule
{
    public class SMSClient : ISMSClient

    {
        public ITwilioRestClient _client;
        public MessageResource messageResponse;
        private string[] outgoingMessageList;
        public List<IncomingMessage> incomingMessageList;
        public List<Billing> BillingInfoList;

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

       public object IncomingMessages(string subaccountsid, string to, string date)
        {
            try

            {
                  var messages = MessageResource.Read(
                               pathAccountSid: subaccountsid,
                               dateSent: DateTimeParser(date),
                               to: new PhoneNumber(FormatPhoneNumber(to)), client: _client);
                incomingMessageList = new List<IncomingMessage>();
                foreach (var record in messages)
                {
                    IncomingMessage r = new IncomingMessage()
                    {
                        Sid = record.Sid,
                        DateCreated = record.DateCreated.ToString(),
                        From = record.From.ToString(),
                        Body = record.Body,
                        Status = record.Status.ToString()
                    };

                    incomingMessageList.Add(r);
                }
               object JsonincomingMessageList = JsonConvert.SerializeObject(incomingMessageList);

                return JsonincomingMessageList;

            }
            catch (ApiException)
            {
                throw;
            }
        }

        public object GetBillingInfo(string subaccountsid,string datefrom, string dateto)
        {
            try
            {
           var billinginfo = RecordResource.Read(
                                  pathAccountSid:subaccountsid,
                                  category: RecordResource.CategoryEnum.SmsOutbound,
                                  startDate: DateTimeParser(datefrom),
                                  endDate: DateTimeParser(dateto),
                                  includeSubaccounts:true,
                                  client: _client);
                BillingInfoList = new List<Billing>();

                foreach (RecordResource record in billinginfo)
                {
                    Billing r = new Billing()
                    {
                        Sid = record.AccountSid,
                        Count = record.Count,
                        CountUnit = record.CountUnit,
                        Price = record.Price.ToString(),
                        Description = record.Description,
                        Usage = record.Usage
                    };

                    BillingInfoList.Add(r);
                }
                object JsonBillingInfoList = JsonConvert.SerializeObject(BillingInfoList);

                return JsonBillingInfoList;
            }
            catch(ApiException)
            {
                throw;
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


        public static DateTime DateTimeParser(string date)
        {
            DateTime dateReceived = new DateTime(DateTime.Parse(date).Year, DateTime.Parse(date).Month, DateTime.Parse(date).Day
                                                   , DateTime.Parse(date).Hour, DateTime.Parse(date).Minute, DateTime.Parse(date).Second);

            var ParsedDate = DateTime.Parse(date);

            return ParsedDate;
        }

    }


}
