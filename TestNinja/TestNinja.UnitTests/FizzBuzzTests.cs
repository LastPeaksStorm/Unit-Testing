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
    public class FizzBuzzTests
    {
        [Test]
        public void GetOutPut_IsDivisibleByThreeAndFive_FizzBuzz()
        {
            string result = FizzBuzz.GetOutput(0);

            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        public void GetOutPut_IsDivisibleByThreeOnly_Fizz()
        {
            string result = FizzBuzz.GetOutput(3);

            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        public void GetOutPut_IsDivisibleByFiveOnly_Buzz()
        {
            string result = FizzBuzz.GetOutput(5);

            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        [TestCase(4)]
        [TestCase(7)]
        [TestCase(-14)]
        public void GetOutPut_IsNotDivisibleByThreeAndFive_ReturnTheNumberItself(int number)
        {
            string result = FizzBuzz.GetOutput(number);

            Assert.That(result, Is.EqualTo(number.ToString()));
        }
    }
}
