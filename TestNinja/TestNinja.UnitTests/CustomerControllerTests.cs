using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    internal class CustomerControllerTests
    {
        private CustomerController _customerController;
        [SetUp]
        public void SetUp() 
        { 
            _customerController = new CustomerController();
        }

        [Test]
        public void GetCustomer_IdIsZero_ReturnNotFoundType()
        {
            var result = _customerController.GetCustomer(0);

            //its type of NotFound
            Assert.That(result, Is.TypeOf<NotFound>());

            //its type of NotFound or its derivatives
            // Assert.That(result, Is.InstanceOf<NotFound>());
        }

        [Test]
        public void GetCustomer_IdIsNotZero_ReturnOkType()
        {
            var result = _customerController.GetCustomer(1);

            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}
