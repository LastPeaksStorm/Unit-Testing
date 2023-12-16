using System.Diagnostics.SymbolStore;
using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private string _setupDestinationFile;
        private IFileRepository _fileRepository;
        public InstallerHelper(IFileRepository fileRepository, string setupDestinationFile)
        {
            _fileRepository = fileRepository;
            _setupDestinationFile = setupDestinationFile;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                var result = _fileRepository
                            .DownloadFile($"http://youtube.com/{customerName}/{installerName}",
                            _setupDestinationFile);

                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}