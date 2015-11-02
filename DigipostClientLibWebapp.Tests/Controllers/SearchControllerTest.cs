using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Digipost.Api.Client.Domain.Search;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Controllers;
using DigipostClientLibWebapp.Services.Digipost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DigipostClientLibWebapp.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            Mock<IDigipostService> digipostServiceMock = new Mock<IDigipostService>();
            var controller = new SearchController(digipostServiceMock.Object);

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Search()
        {
            // Arrange
            const string searchString = "testulf testesen oslo";
            var searchDetailsResult = TestHelper.GetSearchDetailsResult();
            var controller = SearchControllerWithMockedDigipostServiceAndSessionState(searchString, searchDetailsResult);

            // Act
            var result = controller.Search(searchString).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as List<SearchDetails>;
            var viewName = result.ViewName;
            Assert.AreEqual("Index", viewName);
            Assert.AreEqual(searchDetailsResult.PersonDetails, viewModel);
        }

        private static SearchController SearchControllerWithMockedDigipostServiceAndSessionState(string searchString,
            SearchDetailsResult searchDetailsResult)
        {
            var digipostService = new Mock<DigipostService>();
            digipostService.Setup(x => x.Search(searchString)).ReturnsAsync(searchDetailsResult);
            var controller = new SearchController(digipostService.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            return controller;
        }

        [TestMethod]
        public void GoToSendTest()
        {
            // Arrange
            var searchDetailsResult = TestHelper.GetSearchDetailsResult();
            var controller = SearchControllerWithMockedSessionState(searchDetailsResult);
            
            // Act
            var result =
                controller.GoToSend(searchDetailsResult.PersonDetails[0].DigipostAddress) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Send", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        private static SearchController SearchControllerWithMockedSessionState(ISearchDetailsResult searchDetailsResult)
        {
            var digipostServiceMock =new Mock<IDigipostService>();
            var controller = new SearchController(digipostServiceMock.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(x => x[SessionConstants.PersonDetails]).Returns(searchDetailsResult);
            
            context.Setup(x => x.Session).Returns(session.Object);
            
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            
            return controller;
        }
    }
}