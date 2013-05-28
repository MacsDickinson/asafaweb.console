using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using NUnit.Framework;
using asafaweb.console.Enums;
using asafaweb.console.Logic;

namespace asafaweb.tests
{
    [TestFixture]
    public class ProgramTestFixture
    {
        [TestCase("Pass", AsafaResult.Pass)]
        [TestCase("Warning", AsafaResult.Warning)]
        [TestCase("Fail", AsafaResult.Fail)]
        [TestCase("NotTested", AsafaResult.NotTested)]
        [TestCase("XXXXXX", AsafaResult.NotTested)]
        public void StatusLogic_GetStatus_ReturnsExpectedStatus(string input, AsafaResult expectedStatus)
        {
            // Arrange
            // Act
            AsafaResult actualStatus = StatusLogic.GetStatus(input);
            // Assert
            Assert.That(actualStatus, Is.EqualTo(expectedStatus));
        }

        [Test]
        public void HtmlGetLogic_LoadHtmlRespose_ValidUrl_LoadsResponse()
        {
            // Arrange
            HtmlGetLogic logic = new HtmlGetLogic();
            const string uri = "http://www.bing.com";
            // Act
            var result = logic.LoadHtmlResponse(uri);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(HtmlDocument)));
        }

        [Test]
        [ExpectedException(typeof(WebException), ExpectedMessage = "The remote name could not be resolved: 'fail'")]
        public void HtmlGetLogic_LoadHtmlRespose_InvalidUrl_DoesntLoadResponse()
        {
            // Arrange
            HtmlGetLogic logic = new HtmlGetLogic();
            const string uri = "http://fail";
            // Act
            logic.LoadHtmlResponse(uri);
            // Assert
        }

        [Test]
        public void StatusLogic_GetResults_ValidUrl_ReturnsListOfStatus()
        {
            // Arrange
            const string url = "http://www.google.com";
            // Act
            var result = StatusLogic.GetTestResults(url);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(Dictionary<string, AsafaResult>)));
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void StatusLogic_GetResults_InvalidUrl_ReturnsEmptyList()
        {
            // Arrange
            const string url = "http://fail";
            // Act
            var result = StatusLogic.GetTestResults(url);
            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(Dictionary<string, AsafaResult>)));
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
