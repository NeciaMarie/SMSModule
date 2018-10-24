using System;
using Twilio.Clients;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Exceptions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMSModule
{
    public class SMSClient : SMSResponse ,ISMSClient

    {
        public ITwilioRestClient _client;
        public MessageResource messageResponse;
       
        public  SMSClient()

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

        public bool SendMessage(string from, string to, string body)
        {
            bool returnValue = true ;
            try
            {
                var messageResponse =  MessageResource.Create(
                   to: new PhoneNumber(FormatPhoneNumber(to)),
                   from: new PhoneNumber(FormatPhoneNumber(from)),
                   body: body,
                   client: _client);

                if  (messageResponse.Sid == "")
                {
                    returnValue = false;
                }

                return returnValue;
            }
            catch (ApiException ex)
            {
                throw ex;
            }
         }

        //public async Task SendMessage(string from, string to, string body)
        //  {
        //    try
        //      {
        //       messageResponse = await MessageResource.CreateAsync(
        //          to: new PhoneNumber(FormatPhoneNumber(to)),
        //          from: new PhoneNumber(FormatPhoneNumber(from)),
        //          body: body,
        //          client: _client);

        //      await Task.FromResult(true);
        //      }
        //      catch (ApiException ex)
        //      {
        //          throw ex;
        //      }
        //      finally
        //      {
        //          await Task.FromResult(true);
        //      }

        //  }

        public string getResponseProperty(ResponsePropertyId iResponsePropertyId)
        {
            var responseString = "";

            switch (iResponsePropertyId)
            {
                case ResponsePropertyId.MessageId:
                    responseString = messageResponse.Sid;
                    break;

                case ResponsePropertyId.PhoneNumber:
                    responseString = messageResponse.To;
                    break;

                case ResponsePropertyId.MessageText:
                    responseString = messageResponse.Body;
                    break;

                case ResponsePropertyId.DeliveryStatus:
                    responseString = messageResponse.Status.ToString();
                    break;

                case ResponsePropertyId.SendingStatus:
                    responseString =messageResponse.Status.ToString(); ;
                    break;

                case ResponsePropertyId.SentDate:
                    responseString = messageResponse.DateCreated.ToString();
                    break;

               case ResponsePropertyId.DeliveryDate:
                    responseString = messageResponse.DateSent.ToString();
                    break;

               default:
                    responseString = "";
                    break;
            }
            return responseString;
        }

        public  string FetchMessages(long dateSent, string from, string to)
        {
            var messageHistory = MessageResource.Read(
               dateSent: new DateTime(dateSent),
               from: new PhoneNumber(FormatPhoneNumber(from)),
               to: new PhoneNumber(FormatPhoneNumber(to))
               );

            foreach (var record in messageHistory)
            {

            }
            return messageHistory.ToString();
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
