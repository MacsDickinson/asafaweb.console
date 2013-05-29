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

        [Test]
        public void StatusLogic_AnalyseResults_AllPass_NoneFailed()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Pass},
                    {"Key2", AsafaResult.Pass},
                    {"Key3", AsafaResult.Pass},
                    {"Key4", AsafaResult.Pass},
                    {"Key5", AsafaResult.Pass},
                };
            StatusLogic logic = new StatusLogic();
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void StatusLogic_AnalyseResults_AllFail_AllFailed()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Fail},
                    {"Key2", AsafaResult.Fail},
                    {"Key3", AsafaResult.Fail},
                    {"Key4", AsafaResult.Fail},
                    {"Key5", AsafaResult.Fail},
                };
            StatusLogic logic = new StatusLogic();
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(5));
        }

        [Test]
        public void StatusLogic_AnalyseResults_SomeFail_SomeFailed()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Fail},
                    {"Key2", AsafaResult.Pass},
                    {"Key3", AsafaResult.Fail},
                    {"Key4", AsafaResult.Pass},
                    {"Key5", AsafaResult.Fail},
                };
            StatusLogic logic = new StatusLogic();
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(3));
        }

        [Test]
        public void StatusLogic_AnalyseResults_FailOnWarning_SomeWarning_IncludedInFails()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Warning}
                };
            StatusLogic logic = new StatusLogic {FailOnWarning = true};
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(1));
        }

        [Test]
        public void StatusLogic_AnalyseResults_DontFailOnWarning_SomeWarning_NotIncluded()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Warning}
                };
            StatusLogic logic = new StatusLogic { FailOnWarning = false };
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(0));
        }
        [Test]
        public void StatusLogic_AnalyseResults_FailOnNotTested_SomeNotTested_IncludedInFails()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.NotTested}
                };
            StatusLogic logic = new StatusLogic { FailOnNotTested = true };
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(1));
        }

        [Test]
        public void StatusLogic_AnalyseResults_DontFailOnNotTested_SomeNotTested_NotIncluded()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.NotTested}
                };
            StatusLogic logic = new StatusLogic { FailOnNotTested = false };
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void StatusLogic_AnalyseResults_DontFailOnFailure_SomeFailed_NotIncluded()
        {
            // Arrange
            Dictionary<string, AsafaResult> results = new Dictionary<string, AsafaResult>
                {
                    {"Key1", AsafaResult.Fail}
                };
            StatusLogic logic = new StatusLogic { FailOnFailure = false };
            // Act
            Dictionary<string, AsafaResult> updatedResults = logic.AnalyseResults(results);
            // Assert
            Assert.That(updatedResults.Count, Is.EqualTo(0));
        }

        [Test]
        public void StatusLogic_IsTestIgnored_TestIsIgnored_ReturnsTrue()
        {
            // Arrange
            StatusLogic logic = new StatusLogic { IgnoredTests = new List<string>{"TestA"} };
            // Act
            bool result = logic.IsTestIgnored("TestA");
            // Assert
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void StatusLogic_IsTestIgnored_TestNotIgnored_ReturnsFalse()
        {
            // Arrange
            StatusLogic logic = new StatusLogic { IgnoredTests = new List<string> { "TestA" } };
            // Act
            bool result = logic.IsTestIgnored("TestB");
            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void StatusLogic_IsTestIgnored_TestIsOneOfMany_ReturnsTrue()
        {
            // Arrange
            StatusLogic logic = new StatusLogic { IgnoredTests = new List<string> { "TestA","TestB","TestC" } };
            // Act
            bool result = logic.IsTestIgnored("TestB");
            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
    }
}
