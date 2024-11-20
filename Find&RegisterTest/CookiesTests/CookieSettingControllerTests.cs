using Find_Register.Controllers;
using Find_Register.Cookies;
using Find_Register.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Find_RegisterTest.CookiesTests
{
    public class CookieSettingControllerTests
    {
        private Mock<ICookieHelper> _mockCookieHelper;
        private Mock<IConfiguration> _mockConfiguration;
        private readonly EligibilityController _controller;
        private Mock<IUrlHelper> _mockUrlHelper;
        private ILogger<EligibilityController> _mockLogger => new Mock<ILogger<EligibilityController>>().Object;

        public CookieSettingControllerTests()
        {
            _mockCookieHelper = new Mock<ICookieHelper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUrlHelper = new Mock<IUrlHelper>();

            _controller = new EligibilityController(
                _mockLogger,
                _mockCookieHelper.Object,
                _mockConfiguration.Object
            );
            // Set up the HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Set up RouteData
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            routeData.Values["controller"] = "Eligibility";
            routeData.Values["action"] = "CookieSettings";

            var ModelState = new ModelStateDictionary();
            var actionDescriptor = new ControllerActionDescriptor()
            {
                ControllerName = "Eligibility",
                ActionName = "CookieSettings"
            };

            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor, ModelState);
            _controller.ControllerContext = new ControllerContext(actionContext);
            _controller.Url = _mockUrlHelper.Object;
        }

        [Fact]
        public void CookieSettings_RedirectsToLocalUrl_WhenBackUrlIsLocal()
        {
            // Arrange
            var model = new CookieSettings()
            {
                BackUrl = "/home",
                AcceptAnalyticsCookies = true
            };

            SetupCookieHelper();

            // Act
            _mockUrlHelper.Setup(x => x.IsLocalUrl(model.BackUrl)).Returns(true);
            var result = _controller.CookieSettings(model) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.BackUrl, result.Url);
        }

        [Fact]
        public void CookieSettings_RedirectsToHome_WhenBackUrlIsExternal()
        {
            // Arrange
            var model = new CookieSettings
            {
                AcceptAnalyticsCookies = true,
                BackUrl = "http://malicious.com"
            };

            SetupCookieHelper();

            // Act
            var result = _controller.CookieSettings(model) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(".", result.Url);
        }

        [Fact]
        public void CookieSettings_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");
            var model = new CookieSettings();

            // Act
            var result = _controller.CookieSettings(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model, result.Model);
        }

        private void SetupCookieHelper()
        {
            var applicationCookieMock = new MockCookieHelper();
            var mockRequestCookies = new RequestCookiesTestCollection();
            var mockResponseCookies = new Mock<IResponseCookies>();
            var applicationCookie = applicationCookieMock.GetApplicationCookieData(mockRequestCookies, mockResponseCookies.Object);
            applicationCookie.AnalyticSettings.Value = new AnalyticSettings { AcceptAnalytics = true };
            this._mockCookieHelper
                .Setup(ch => ch.GetApplicationCookieData(It.IsAny<IRequestCookieCollection>(), It.IsAny<IResponseCookies>()))
                .Returns(applicationCookie);
        }
    }
}