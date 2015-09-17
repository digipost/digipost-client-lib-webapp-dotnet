using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Digipost.Api.Client;
using Digipost.Api.Client.Api;
using Digipost.Api.Client.Domain.Search;
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
        
        private static DigipostClient GetClient()
        {
            return _digipostClient ?? InitClient();
        }

        private static DigipostClient InitClient()
        {
            const string thumbprint = "d6 5e 6c 4c 77 fc 0e 0d c5 f5 ac 32 bc 43 70 1f a8 b0 3d 21";
            const string senderId = "779052";
            const string url = "https://api.digipost.no";

            var config = new ClientConfig(senderId, url, 10000, false)
            {
                Logger = (severity, traceID, metode, message) =>
                {
                    Logger.Debug(message);
                }
            };


            _digipostClient = new DigipostClient(config, thumbprint);

            return _digipostClient;
        }


    }

}
