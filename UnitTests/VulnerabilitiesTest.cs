using NUnit.Framework;
using ProtoBuf;
using System;
using System.Collections.Lockable;
using System.IO;

namespace UnitTests
{
    [TestFixture]
    public class VulnerabilitiesTest
    {
        private LockableList<string> testList;

        [SetUp]
        public void SetUp()
        {
            this.testList = new();
            this.testList.Add("Hello");
            this.testList.Add("World");
            this.testList.Add("!");
        }

        [Test]
        [Description("Serializing the object and de-serialzing removes the lock, since protocol buffers does not serialize inheritance.")]
        public void VulnerabilityExample()
        {
            this.testList.SimpleLock();

            Assert.IsTrue(this.testList.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.Add("Something");
            });

            byte[] buffed;

            using (MemoryStream ms = new())
            {
                Serializer.Serialize(ms, this.testList);
                buffed = ms.ToArray();
            }

            this.testList = null;

            using (MemoryStream ms = new(buffed))
            {
                this.testList = Serializer.Deserialize<LockableList<string>>(ms);
            }

            Assert.DoesNotThrow(() =>
            {
                this.testList.Add("Something");
            });

            Assert.AreEqual(4, this.testList.Count);
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
