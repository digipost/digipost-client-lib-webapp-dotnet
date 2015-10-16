using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DigipostClientLibWebapp.Tests.Controllers
{
    [TestClass]
    public class IdentifyControllerTest
    {
        private const string digipostAddress = "kristian.sæther.enge#8PWF";

        [TestMethod]
        public void Index()
        {
            // Arrange
            IdentifyController controller = new IdentifyController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Identify()
        {
            // Arrange
            IdentifyController controller = new IdentifyController();
            
            Identification identification = new Identification(new RecipientById(IdentificationType.DigipostAddress, digipostAddress));
            // Act
            ViewResult result = controller.Identify(identification) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(IdentificationResult));
            var resultModel = result.Model as IdentificationResult;
            Assert.AreEqual(digipostAddress, resultModel.Data);
        }

    }
}
