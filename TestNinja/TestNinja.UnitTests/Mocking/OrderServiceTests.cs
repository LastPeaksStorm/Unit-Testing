using NSubstitute;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class OrderServiceTests
    {
        private IStorage _storage;
        private OrderService _service;
        [SetUp]
        public void SetUp()
        {
            _storage = Substitute.For<IStorage>();
            _service = new OrderService(_storage);
        }
        [Test]
        public void PlaceOrder_WhenCalled_PlaceTheOrderIntTheStorage()
        {
            var order = new Order();

            _service.PlaceOrder(order);

            _storage.Received().Store(order);
        }
    }
}
