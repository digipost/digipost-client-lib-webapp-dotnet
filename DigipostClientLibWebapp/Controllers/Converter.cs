using System;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Models;

namespace DigipostClientLibWebapp.Controllers
{
    public class Converter
    {
        public static SendModel SearchDetailsToSendModel(SearchDetails searchDetails)
        {
            var sendModel = new SendModel();
            if (searchDetails.SearchDetailsAddress != null)
            {
                sendModel.AdditionalAddressLine = searchDetails.SearchDetailsAddress.AdditionalAddressLine;
                sendModel.City = searchDetails.SearchDetailsAddress.City;
                sendModel.HouseLetter = searchDetails.SearchDetailsAddress.HouseLetter;
                sendModel.HouseNumber = searchDetails.SearchDetailsAddress.HouseNumber;
                sendModel.Street = searchDetails.SearchDetailsAddress.Street;
                sendModel.ZipCode = searchDetails.SearchDetailsAddress.ZipCode;
            }
            sendModel.DigipostAddress = searchDetails.DigipostAddress;
            sendModel.FirstName = searchDetails.FirstName;
            sendModel.MiddleName = searchDetails.MiddleName;
            sendModel.LastName = searchDetails.LastName;
            sendModel.MobileNumber = searchDetails.MobileNumber;
            sendModel.OrganizationName = searchDetails.OrganizationName;
            return sendModel;
        }

        public static string MimeTypeToDigipostFileType(string mimeType)
        {
            if (mimeType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return "pdf";
            if (mimeType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
                return "txt";
            if(mimeType.Equals("text/html",StringComparison.OrdinalIgnoreCase))
                return "html";

            return "";
        }

    }
}