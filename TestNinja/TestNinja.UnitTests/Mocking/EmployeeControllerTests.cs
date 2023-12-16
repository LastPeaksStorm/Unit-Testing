using NSubstitute;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        EmployeeController controller;
        IEmployeeStorage employeeStorage;

        [SetUp]
        public void SetUp()
        {
            employeeStorage = Substitute.For<IEmployeeStorage>();
            controller = new EmployeeController(employeeStorage);
        }

        [Test]
        public void DeleteEmployee_WhenCalled_RemovesEmployee()
        {
            controller.DeleteEmployee(1);

            employeeStorage.Received(1).RemoveEmployee(1);
        }

        [Test]
        public void DeleteEmployee_WhenCalled_RedirectsToTheEmployeePage()
        {
            var result = controller.DeleteEmployee(1);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

    }
}
