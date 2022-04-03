using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Lockable;
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class ExtensionTests
    {
        private string[] testStrings;

        [SetUp]
        public void SetUp()
        {
            testStrings = new string[] { "Hello", "World", "!" };
        }

        [Test]
        public void ListTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);

            LockableList<string> llist = list.ToLockableList();

            Assert.AreEqual(3, llist.Count);

            Assert.IsTrue(llist.Intersect<string>(testStrings).Any());
        }

        [Test]
        public void HashSetTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);
            list.Add("Hello");

            LockableHashSet<string> llist = list.ToLockableHashSet();

            Assert.AreEqual(3, llist.Count);

            Assert.IsTrue(llist.Intersect<string>(testStrings).Any());
        }

        [Test]
        public void LinkedListTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);
            list.Add("Another");
            list.Add("Test");
            list.Add("Here");
            list.Add("Sequence");

            LockableLinkedList<string> llist = list.ToLockableLinkedList();

            Assert.AreEqual(7, llist.Count);

            Assert.IsTrue(llist.Intersect<string>(testStrings).Any());

            Assert.AreEqual("Hello", llist.First.Value);
            Assert.AreEqual("Sequence", llist.Last.Value);
        }

        [Test]
        public void QueueTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);
            list.Add("Another");
            list.Add("Test");
            list.Add("Here");
            list.Add("Sequence");

            LockableQueue<string> llist = list.ToLockableQueue();

            Assert.AreEqual(7, llist.Count);

            Assert.IsTrue(llist.Intersect<string>(testStrings).Any());

            Assert.AreEqual("Hello", llist.Peek());

            Assert.AreEqual("Hello", llist.Dequeue());
            Assert.AreEqual("World", llist.Dequeue());
            Assert.AreEqual("!", llist.Dequeue());
            Assert.AreEqual("Another", llist.Dequeue());

            Assert.AreEqual(3, llist.Count);
        }

        [Test]
        public void StackTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);
            list.Add("Another");
            list.Add("Test");
            list.Add("Here");
            list.Add("Sequence");

            LockableStack<string> llist = list.ToLockableStack();

            Assert.AreEqual(7, llist.Count);

            Assert.IsTrue(llist.Intersect<string>(testStrings).Any());

            Assert.AreEqual("Sequence", llist.Peek());

            Assert.AreEqual("Sequence", llist.Pop());
            Assert.AreEqual("Here", llist.Pop());
            Assert.AreEqual("Test", llist.Pop());
            Assert.AreEqual("Another", llist.Pop());

            Assert.AreEqual(3, llist.Count);
        }

        [Test]
        public void DictionaryTests()
        {
            List<string> list = new();
            list.AddRange(testStrings);
            list.Add("Another");
            list.Add("Another");
            list.Add("Test");
            list.Add("Test");
            list.Add("Here");
            list.Add("Here");
            list.Add("Sequence");
            list.Add("Sequence");

            LockableDictionary<int, string> llist = list.ToLockableDictionary();

            Assert.AreEqual(7, llist.Count);

            Assert.AreEqual("Hello", llist.Values.ToArray()[0]);
            Assert.AreEqual("Sequence", llist.Values.ToArray()[llist.Count - 1]);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
