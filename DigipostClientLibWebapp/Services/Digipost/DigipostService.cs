using System;
using System.Reflection;
using System.Threading.Tasks;
using Digipost.Api.Client;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Exceptions;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Properties;
using log4net;

namespace DigipostClientLibWebapp.Services.Digipost
{
    public class DigipostService
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DigipostClient _digipostClient;

        public virtual async Task<IIdentificationResult> Identify(Identification identification)
        {
            IIdentificationResult result = null;
            Logger.Debug("Inside Identify("+identification.ToString()+")");
            try
            {
                result = await GetClient().IdentifyAsync(identification);
            }
            catch (ClientResponseException cre)
            {
                Logger.Error(cre.Message, cre);
                throw;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
                throw;
            }
            return result;

        } 

        public virtual async Task<ISearchDetailsResult> Search(string searchText)
        {
            ISearchDetailsResult result = null;
            Logger.Debug("inside Search(" + searchText + ")");
            try
            {
                Logger.Debug("Search async init()");
                result = await GetClient().SearchAsync(searchText);
                Logger.Debug("Search async done()");
            }
            catch (ClientResponseException e)
            {
                Logger.Error(e.Message, e);
                throw;
            }

            return result;
        }

        public virtual async Task<IMessageDeliveryResult> Send(byte[] fileContent, string filetype, SendModel sendModel)
        {
            var recipient = new RecipientById(IdentificationType.DigipostAddress, sendModel.DigipostAddress);

            var primaryDocument = new Document(sendModel.Subject, filetype, fileContent)
            {
                SensitivityLevel = sendModel.SensitivityOption,
                AuthenticationLevel = sendModel.AuthenticationOption
            };
            if (sendModel.SmsAfterHour)
                primaryDocument.SmsNotification = new SmsNotification(int.Parse(sendModel.SmsAfterHours));

            var m = new Message(recipient, primaryDocument);
            
            IMessageDeliveryResult result = null;
            try {
                result = await GetClient().SendMessageAsync(m);
            }
            catch (ClientResponseException e)
            {
                var errorMessage = e.Error;
                Logger.Error("> Error." + errorMessage);
                throw;
            }
         

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
                Logger = (severity, traceId, metode, message) =>
                {
                    Logger.Debug(string.Format("[{0}]:[{1}]:[{2}]", traceId, metode, message));
                }
            };


            _digipostClient = new DigipostClient(config, Settings.Default.thumbprint);

            return _digipostClient;
        }
    }
}
