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
    }
}
