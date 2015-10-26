using System.Threading.Tasks;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.Search;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Models;

namespace DigipostClientLibWebapp.Services.Digipost
{
    public interface IDigipostService
    {
        Task<IIdentificationResult> Identify(Identification identification);
        Task<ISearchDetailsResult> Search(string searchText);
        Task<IMessageDeliveryResult> Send(byte[] fileContent, string filetype, SendModel sendModel);
    }
}