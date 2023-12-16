using System.Net;

namespace TestNinja.Mocking
{
    public interface IFileRepository
    {
        bool DownloadFile(string url, string setupDestinationFile);
    }
    internal class FileRepository : IFileRepository
    {
        public bool DownloadFile(string url, string setupDestinationFile)
        {
            var client = new WebClient();

            client.DownloadFile(
                string.Format("url"),
                setupDestinationFile);

            return true;
        }
    }
}
