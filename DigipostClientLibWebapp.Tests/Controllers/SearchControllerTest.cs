using System.Collections.Generic;
using System.Security.Principal;
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
            var controller = new SearchController();

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Search()
        {
            // Arrange
            const string searchString = "kristian sæther enge oslo";
            var digipostService = new Mock<DigipostService>();
            var searchDetailsResult = TestHelper.GetSearchDetailsResult();
            digipostService.Setup(x => x.Search(searchString)).ReturnsAsync(searchDetailsResult);
            var controller = new SearchController(digipostService.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            
            // Act
            var result =  controller.Search(searchString).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as List<SearchDetails>;
            var viewName = result.ViewName;
            Assert.AreEqual("Index",viewName);
            Assert.AreEqual(searchDetailsResult.PersonDetails, viewModel);
        }

        [TestMethod]
        public void GoToSendTest()
        {
            // Arrange
            var controller = new SearchController();
            var searchDetailsResult = TestHelper.GetSearchDetailsResult();
            
            
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(x => x[SessionConstants.PersonDetails]).Returns(searchDetailsResult);
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            // Act
            var result = controller.GoToSend(searchDetailsResult.PersonDetails[0].DigipostAddress) as RedirectToRouteResult ;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Send", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
