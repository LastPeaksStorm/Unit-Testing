using NUnit.Framework;
using NSubstitute;
using TestNinja.Mocking;
using NSubstitute.ExceptionExtensions;
using System.Net;
using NSubstitute.Core.Arguments;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        InstallerHelper _installerHelper;
        IFileRepository _fileRepository;

        [Test]
        public void DownloadInstaller_WrongUrl_ReturnsFalse()
        {
            _fileRepository = Substitute.For<IFileRepository>();
            _installerHelper = new InstallerHelper(_fileRepository, "C:/downloads");

            _fileRepository.DownloadFile("http://youtube.com/MrBest/skeamers", "C:/downloads")
                                        .Throws<WebException>();

            var result = _installerHelper.DownloadInstaller("MrBest", "skeamers");

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void DownloadInstaller_CanDownloadSuccessfully_ReturnsTrue()
        {
            _fileRepository = Substitute.For<IFileRepository>();
            _installerHelper = new InstallerHelper(_fileRepository, "C:/downloads");

            /* _fileRepository.DownloadFile("http://youtube.com/MrBeast/smitters", "C:/downloads")
                                        .Returns(true); */

            var result = _installerHelper.DownloadInstaller("MrBeast", "smitters");

            Assert.That(result, Is.EqualTo(true));
        }
    }
}
