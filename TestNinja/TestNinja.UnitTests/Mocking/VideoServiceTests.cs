using NSubstitute;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private VideoService _videoService;
        private IFileReader _fileReader;
        private IVideoRepository _videoRepository;

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnErrorMessage()
        {
            _fileReader = Substitute.For<IFileReader>();
            _videoService = new VideoService(_fileReader);
            _fileReader.Read("video.txt").Returns("");

            var result = _videoService.ReadVideoTitle();

            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosProcessed_ReturnEmptyString()
        {
            _videoRepository = Substitute.For<IVideoRepository>();
            _videoService = new VideoService(_videoRepository);

            _videoRepository.GetUnprocessedVideos().Returns(new Video[] { });
            var result = _videoService.GetUnprocessedVideosAsCsv();

            _videoRepository.Received().GetUnprocessedVideos();
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AFewUnprocessedVideos_ReturnIdsInStringOfUnprocessedObjects()
        {
            _videoRepository = Substitute.For<IVideoRepository>();
            _videoService = new VideoService(_videoRepository);

            _videoRepository.GetUnprocessedVideos().Returns(new Video[]
            {
                new Video
                {
                    Id = 1,
                    IsProcessed = true,
                    Title = "Twenty One Pilots"
                },
                new Video
                {
                    Id = 2,
                    IsProcessed = false,
                    Title = "Twenty Two Pilots"
                },
                new Video
                {
                    Id = 3,
                    IsProcessed = false,
                    Title = "Twenty Three Pilots"
                }});
            var result = _videoService.GetUnprocessedVideosAsCsv();

            _videoRepository.Received().GetUnprocessedVideos();
            Assert.That(result, Is.EqualTo("2,3"));
        }
    }
}
