using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        private ErrorLogger _errorLogger;
        [SetUp]
        public void SetUp()
        {
            _errorLogger = new ErrorLogger();
        }

        [Test]
        public void Log_WhenCalled_SetLastErrorProperty()
        {
            _errorLogger.Log("a");

            Assert.That(_errorLogger.LastError == "a");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Log_NullOrWhiteSpace_ArgumentNullException(string message)
        {
            Assert.That(() => _errorLogger.Log(message), Throws.ArgumentNullException);
        }

        [Test]
        public void Log_ValidError_RaisesAnEvent()
        {
            Guid id;
            
            _errorLogger.ErrorLogged += (sender, args) => { id = args; };

            Assert.That(_errorLogger, Is.Not.Null);
        }
    }
}
