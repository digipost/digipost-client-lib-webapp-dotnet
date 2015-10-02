using System;
using System.Collections.Generic;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;

namespace DigipostClientLibWebapp.Tests
{
    public class TestHelper
    {

        public static MessageDeliveryResult SendResponse()
        {
            return new MessageDeliveryResult
            {
                DeliveryTime = DateTime.Now,
                DeliveryMethod = DeliveryMethod.Digipost
            };
        }

        public static SearchDetailsResult GetSearchDetailsResult()
        {
            var searchDetailsResult = new SearchDetailsResult {PersonDetails = new List<SearchDetails>()};
            var searchDetails = new SearchDetails
            {
                DigipostAddress = "testulf#123",
                FirstName = "testulf",
                MiddleName = "the king",
                LastName = "testesen",
                MobileNumber = "12312312",
                OrganizationName = "123456",
                SearchDetailsAddress = new SearchDetailsAddress
                {
                    AdditionalAddressLine = "Testveien 1",
                    City = "Andeby",
                    HouseLetter = "1",
                    HouseNumber = "1",
                    Street = "Testveien",
                    ZipCode = "1212"
                }
            };
            
            searchDetailsResult.PersonDetails.Add(searchDetails);
            return searchDetailsResult;
        }
    }
}