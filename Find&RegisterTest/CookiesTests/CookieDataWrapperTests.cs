using Find_Register.Cookies;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Find_RegisterTest.CookiesTests;

public class CookieDataWrapperTests
{
    [Fact]
    public void BooleanCookieDataWrapperWorks()
    {
        var mockResponseCookies = new Mock<IResponseCookies>();

        var cookieDataWrapper = CookieDataWrapper<bool>.GenerateBoolCookieDataWrapper(
            true,
            "True",
            "test-cookie",
            new CookieOptions(),
            mockResponseCookies.Object
            );

        Assert.NotNull(cookieDataWrapper);
        Assert.True(cookieDataWrapper.IsSet);
        Assert.True(cookieDataWrapper.Value);

        cookieDataWrapper.UnSet();
        mockResponseCookies.Verify(m => m.Delete("test-cookie"), Times.Once);
        Assert.False(cookieDataWrapper.IsSet);
        // Assert that unsetting the cookie makes the expected changes

        cookieDataWrapper.Value = false;
        mockResponseCookies.Verify(m => m.Append("test-cookie", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        Assert.True(cookieDataWrapper.IsSet);
        Assert.False(cookieDataWrapper.Value);
        // Assert that changing the cookie value makes the expected changes
    }

    [Fact]
    public void StringCookieDataWrapperWorks()
    {
        var mockResponseCookies = new Mock<IResponseCookies>();

        var cookieDataWrapper = CookieDataWrapper<string>.GenerateStringCookieDataWrapper(
            true,
            "aaabbbccc",
            "test-cookie",
            new CookieOptions(),
            mockResponseCookies.Object
            );

        Assert.NotNull(cookieDataWrapper);
        Assert.True(cookieDataWrapper.IsSet);
        Assert.Equal("aaabbbccc", cookieDataWrapper.Value);

        cookieDataWrapper.UnSet();
        mockResponseCookies.Verify(m => m.Delete("test-cookie"), Times.Once);
        Assert.False(cookieDataWrapper.IsSet);
        // Assert that unsetting the cookie makes the expected changes

        cookieDataWrapper.Value = "dddeeefff";
        mockResponseCookies.Verify(m => m.Append("test-cookie", It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Once);
        Assert.True(cookieDataWrapper.IsSet);
        Assert.Equal("dddeeefff", cookieDataWrapper.Value);
        // Assert that changing the cookie value makes the expected changes
    }


    [Fact]
    public void JsonObjectSerializationWorks()
    {
        var mockResponseCookies = new Mock<IResponseCookies>();

        var cookieDataWrapper = CookieDataWrapper<TestJsonSerializable>.GenerateJsonResponsesCookieDataWrapper<TestJsonSerializable>(
            false,
            "",
            "test-cookie",
            new CookieOptions(),
            mockResponseCookies.Object
            );

        var underlayingObject = new TestJsonSerializable { SomeField = "some value" };
        cookieDataWrapper.Value = underlayingObject;

        mockResponseCookies.Verify(m => m.Append("test-cookie", "{\"SomeField\":\"some value\"}", It.IsAny<CookieOptions>()), Times.Once);
    }

    [Fact]
    public void JsonObjectDeserializationWorks()
    {
        var mockResponseCookies = new Mock<IResponseCookies>();

        var cookieDataWrapper = CookieDataWrapper<TestJsonSerializable>.GenerateJsonResponsesCookieDataWrapper<TestJsonSerializable>(
            true,
            "{\"SomeField\":\"testValue\"}",
            "test-cookie",
            new CookieOptions(),
            mockResponseCookies.Object
            );
        Assert.Equal("testValue", cookieDataWrapper.Value.SomeField);
    }

    private struct TestJsonSerializable
    {
        public string SomeField;
    }
}
