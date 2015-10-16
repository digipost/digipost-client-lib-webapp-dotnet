using Digipost.Api.Client.Domain;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using Digipost.Api.Client.Domain.SendMessage;
using DigipostClientLibWebapp.Controllers;
using DigipostClientLibWebapp.Services.Digipost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            Identification identification = new Identification(new RecipientById(IdentificationType.DigipostAddress, digipostAddress));
            IdentificationResult identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress, digipostAddress);
            IdentifyController controller =  IdentifyControllerWithMockedDigipostServiceAndSessionState(identification,identificationResult);
            
            
            // Act
            ViewResult result = controller.Identify(identification).Result as ViewResult;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(IdentificationResult));
            var resultModel = result.Model as IdentificationResult;
            Assert.AreEqual(digipostAddress, resultModel.Data);
        }

        private static IdentifyController IdentifyControllerWithMockedDigipostServiceAndSessionState(Identification identification,
         IdentificationResult identificationResult)
        {
            var digipostService = new Mock<DigipostService>();
            digipostService.Setup(x => x.Identify(identification)).ReturnsAsync(identificationResult);
            var controller = new IdentifyController(digipostService.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            return controller;
        }

    }
}
