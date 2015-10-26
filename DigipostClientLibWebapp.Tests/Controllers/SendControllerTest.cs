using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Digipost.Api.Client.Domain.DataTransferObjects;
using Digipost.Api.Client.Domain.Enums;
using Digipost.Api.Client.Domain.Search;
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
            var controller = SendControllerWithMockedRequestContext(person);
            var compareLogic = new CompareLogic();

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsTrue(compareLogic.Compare(expectedModel, result.Model).AreEqual);
        }

        private static SendController SendControllerWithMockedRequestContext(SearchDetails person)
        {
            var digipostServiceMock = new Mock<IDigipostService>();
            var controller = new SendController(digipostServiceMock.Object);
            var mockedRequestContext = MockedRequestContextWithSessionState(person);
            controller.ControllerContext = new ControllerContext(mockedRequestContext, controller);
            return controller;
        }

        private static RequestContext MockedRequestContextWithSessionState(SearchDetails person)
        {
            var mockedContext = new Mock<HttpContextBase>();
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(x => x[SessionConstants.PersonModel]).Returns(person);
            mockedContext.Setup(x => x.Session).Returns(session.Object);
            var requestContext = new RequestContext(mockedContext.Object, new RouteData());
            return requestContext;
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            var digipostServiceMock = new Mock<IDigipostService>();
            var controller = new SendController(digipostServiceMock.Object);
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
            var sendModel = SearchDetailsToSendModel();
            var controller = SendControllerWithMockedDigipostServiceAndRequestFile();

            // Act
            var result = controller.Send(sendModel).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var viewModel = result.Model as MessageDeliveryResult;
            var viewName = result.ViewName;
            Assert.AreEqual("SendStatus", viewName);
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(DeliveryMethod.Digipost, viewModel.DeliveryMethod);
        }

        private static SendController SendControllerWithMockedDigipostServiceAndRequestFile()
        {
            var mockedDigipostService = MockedDigipostService();
            var controller = new SendController(mockedDigipostService.Object);
            var mockedRequestContext = MockedRequestContext();
            controller.ControllerContext = new ControllerContext(mockedRequestContext, controller);
            return controller;
        }

        private static RequestContext MockedRequestContext()
        {
            var mockedContext = MockedContextWithFileRequest();
            var requestContext = new RequestContext(mockedContext.Object, new RouteData());
            return requestContext;
        }

        private static Mock<HttpContextBase> MockedContextWithFileRequest()
        {
            var context = new Mock<HttpContextBase>();
            var mockRequest = MockedFileRequest();
            context.Setup(x => x.Request).Returns(mockRequest.Object);
            return context;
        }


        private static Mock<DigipostService> MockedDigipostService()
        {
            var sendResponse = TestHelper.SendResponse();
            var digipostService = new Mock<DigipostService>();
            digipostService.Setup(x => x.Send(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<SendModel>()))
                .ReturnsAsync(sendResponse);
            return digipostService;
        }


        private static SendModel SearchDetailsToSendModel()
        {
            var sendModel = Converter.SearchDetailsToSendModel(TestHelper.GetSearchDetailsResult().PersonDetails[0]);
            sendModel.Subject = "Test subject";
            return sendModel;
        }


        private static Mock<HttpRequestBase> MockedFileRequest()
        {
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
            return mockRequest;
        }

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length*sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}