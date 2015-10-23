using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Identify;
using DigipostClientLibWebapp.Controllers;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DigipostClientLibWebapp.Tests.Controllers
{
    [TestClass]
    public class IdentifyControllerTest
    {
        private const string DigipostAddress = "Testulf.testesen#8PWF";

        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new IdentifyController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Identify()
        {
            // Arrange
            var identification = new IdentifyModel
            {
                IdentificationType = IdentificationType.DigipostAddress,
                IdentificationValue = DigipostAddress
            };
            var identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress,
                DigipostAddress);
            var controller = IdentifyControllerWithMockedDigipostServiceAndSessionState(identificationResult);

            // Act
            var result = controller.Identify(identification).Result as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewName = result.ViewName;
            Assert.AreEqual("IdentificationResult", viewName);
            Assert.IsInstanceOfType(result.Model, typeof (IdentificationResult));
            var viewModel = result.Model as IdentificationResult;
            Assert.AreEqual(DigipostAddress, viewModel.Data);
        }

        [TestMethod]
        public void IdentifyByNameAndAddress()
        {
            // Arrange
            var identification = new IdentifyModel
            {
                FullName = "Testulf testesen",
                AddressLine1 = "Vegen bortafor vegen",
                PostalCode = "0401",
                City = "Mordor"
            };
            var identificationResult = new IdentificationResult(IdentificationResultType.DigipostAddress,
                DigipostAddress);
            var controller = IdentifyControllerWithMockedDigipostServiceAndSessionState(identificationResult);

            // Act
            var result = controller.IdentifyByNameAndAddress(identification).Result as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewName = result.ViewName;
            Assert.AreEqual("IdentificationResult", viewName);
            Assert.IsInstanceOfType(result.Model, typeof (IdentificationResult));
            var viewModel = result.Model as IdentificationResult;
            Assert.AreEqual(DigipostAddress, viewModel.Data);
        }

        private static IdentifyController IdentifyControllerWithMockedDigipostServiceAndSessionState(
            IdentificationResult identificationResult)
        {
            var digipostService = new Mock<DigipostService>();
            digipostService.Setup(x => x.Identify(It.IsAny<Identification>())).ReturnsAsync(identificationResult);
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