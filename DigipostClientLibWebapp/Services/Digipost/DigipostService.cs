using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Api.Client;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Properties;
using log4net;

namespace DigipostClientLibWebapp.Services.Digipost
{
    public class DigipostService
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DigipostClient _digipostClient;

        public async Task<ISearchDetailsResult> Search(string searchText)
        {
            ISearchDetailsResult result = null;
            Logger.Debug("inside Search(" + searchText + ")");
            try
            {
                Logger.Debug("Search async init()");
                result = await GetClient().SearchAsync(searchText);
                Logger.Debug("Search async done()");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
                throw;
            }

            return result;
        }

        public async Task<IMessageDeliveryResult> Send(byte[] fileContent, string filetype, string suject, string digipostAddress, SensitivityLevel sensitivityOption,AuthenticationLevel authenticationOption, bool smsAfterHour, string smsAfterHours)
        {
            var recipient = new Recipient(IdentificationChoiceType.DigipostAddress, digipostAddress);

            var primaryDocument = new Document(suject, filetype, fileContent);
            primaryDocument.SensitivityLevel = sensitivityOption;
            primaryDocument.AuthenticationLevel = authenticationOption;
            if (smsAfterHour)
                primaryDocument.SmsNotification = new SmsNotification(Int32.Parse(smsAfterHours));

            IMessage m = new Message(recipient, primaryDocument);


            var result = await GetClient().SendMessageAsync(m);

            return result;
        }



        private static DigipostClient GetClient()
        {
            return _digipostClient ?? InitClient();
        }

        private static DigipostClient InitClient()
        {

            var config = new ClientConfig(Settings.Default.senderid, Settings.Default.url, Settings.Default.timeout, false)
            {
                Logger = (severity, traceID, metode, message) =>
                {
                    Logger.Debug(message);
                }
            };


            _digipostClient = new DigipostClient(config, Settings.Default.thumbprint);

            return _digipostClient;
        }



    }

}
