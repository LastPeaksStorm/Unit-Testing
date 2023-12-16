using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void Push_AddedObjectIsNull_ThrowArgumentNullException()
        {
            var stack = new Stacks<object>();

            Assert.That(() => stack.Push(null), Throws.ArgumentNullException);
        }

        [Test]
        [TestCase(new int[] { 1, 3, 2 })]
        [TestCase(new char[] { 'f', 'b', 'b', 't' })]
        public void Push_WhenCalled_AddTheObjectToTheStack<T>(T[] tS)
        {
            var stack = new Stacks<object>();

            foreach (var item in tS)
            {
                stack.Push(item);
            }
            Assert.That(stack.Count, Is.EqualTo(tS.Length));
        }

        [Test]
        public void Pop_NoElementsInTheStack_ThrowInvalidOperationException()
        {
            var stack = new Stacks<object>();

            Assert.That(() => stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        [TestCase(new int[] { 1, 3, 2 }, 2)]
        [TestCase(new string[] { "gang", "bang", "bam", "temp" }, "temp")]
        public void Pop_WhenCalled_PopTheStackAndReturnThePoppedElement<T>(T[] tS, T expectedResult)
        {
            var stack = new Stacks<object>();

            foreach (var item in tS)
            {
                stack.Push(item);
            }

            var result = stack.Pop();

            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(stack.Count, Is.EqualTo(tS.Length-1));
        }

        [Test]
        public void Peek_NoElementsInTheStack_ThrowInvalidOperationException()
        {
            var stack = new Stacks<object>();

            Assert.That(() => stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        [TestCase(new int[] { 1, 3, 2 }, 2)]
        [TestCase(new string[] { "gang", "bang", "bam", "temp" }, "temp")]
        public void Peek_WhenCalled_ReturnTheLastElementAndDoesNotPopTheStack<T>(T[] tS, T expectedResult)
        {
            var stack = new Stacks<object>();

            foreach (var item in tS)
            {
                stack.Push(item);
            }

            var result = stack.Peek();

            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(stack.Count, Is.EqualTo(tS.Length));
        }
    }
}
