using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class HtmlFormatterTests
    {
        private HtmlFormatter _htmlFormatter;

        [SetUp]
        public void SetUp()
        {
            _htmlFormatter = new HtmlFormatter();
        }

        [Test]
        public void FormatAsBold_WhenCalled_ReturnTheStringWithStrongTagCover()
        {
            var result = _htmlFormatter.FormatAsBold("Heathens");

            Assert.That(result, Is.EqualTo("<strong>Heathens</strong>"));
        }
    }
}
