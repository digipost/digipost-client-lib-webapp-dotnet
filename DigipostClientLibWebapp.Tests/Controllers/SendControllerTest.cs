using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using DigipostClientLibWebapp.Constants;
using DigipostClientLibWebapp.Controllers;
using DigipostClientLibWebapp.Models;
using DigipostClientLibWebapp.Services.Digipost;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DigipostClientLibWebapp.Tests.Controllers
{
    [TestClass]
    public class SendControllerTest
    {
        [TestMethod]
        public void IndexFromSearchController()
        {
            // Arrange
            var person = TestHelper.GetSearchDetailsResult().PersonDetails[0];
            var expectedModel = Converter.SearchDetailsToSendModel(person);
            var controller = new SendController();
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(x => x[SessionConstants.PersonModel]).Returns(person);
            context.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);
            var compareLogic = new CompareLogic();

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsTrue(compareLogic.Compare(expectedModel, result.Model).AreEqual);
        }

        [TestMethod]
        public void Index()
        {
            // Arragne
            var controller = new SendController();
            var person = TestHelper.GetSearchDetailsResult().PersonDetails[0];
            var sendModel = Converter.SearchDetailsToSendModel(person);

            // Act
            var result = controller.Index(sendModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(result.Model, sendModel);
        }

        [TestMethod]
        public void Send()
        {
            // Arrange
            var sendModel = Converter.SearchDetailsToSendModel(TestHelper.GetSearchDetailsResult().PersonDetails[0]);
            sendModel.Subject = "Test subject";
            var sendResponse = new MessageDeliveryResult
            {
                DeliveryTime = DateTime.Now,
                DeliveryMethod = DeliveryMethod.Digipost
            };
            var digipostService = new Mock<DigipostService>();
            digipostService.Setup(x => x.Send(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<SendModel>())).ReturnsAsync(sendResponse);
            var controller = new SendController(digipostService.Object);
            var context = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            context.Setup(x => x.Session).Returns(session.Object);

            var fileContent = GetBytes("filecontent");
            var stream = new MemoryStream(fileContent);
            var mockFile = new Mock<HttpPostedFileBase>();
            var mockFiles = new Mock<HttpFileCollectionBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockFile.Setup(f => f.InputStream).Returns(stream);
            mockFile.Setup(x => x.ContentLength).Returns(fileContent.Length);
            mockFile.Setup(x => x.ContentType).Returns("application/pdf");
            mockFiles.Setup(f => f.Count).Returns(1);
            mockFiles.Setup(f => f[0]).Returns(mockFile.Object);
            mockRequest.Setup(r => r.Files).Returns(() => mockFiles.Object);
            context.Setup(x => x.Request).Returns(mockRequest.Object);
            var requestContext = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(requestContext, controller);

            // Act
            var result = controller.Send(sendModel).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as MessageDeliveryResult;
            var viewName = result.ViewName;
            Assert.AreEqual("SendStatus", viewName);
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(DeliveryMethod.Digipost,viewModel.DeliveryMethod);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
