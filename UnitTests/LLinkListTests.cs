using NUnit.Framework;
using System;
using System.Collections.Lockable;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class LLinkListTests
    {
        private LockableLinkedList<string> testList;

        [SetUp]
        public void SetUp()
        {
            this.testList = new();
        }

        [Test]
        public void SimpleLock()
        {
            Assert.IsFalse(this.testList.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testList.AddFirst("Hello");
                this.testList.AddAfter(this.testList.First, "World");
                this.testList.AddAfter(this.testList.First, "!");
            });

            this.testList.SimpleLock();

            Assert.IsTrue(this.testList.IsLocked);

            this.FailTamperprocess();

            Assert.IsTrue(this.testList.SimpleUnlock(null));

            this.NoThrowTamper();
        }

        [Test]
        public void LockTests()
        {
            byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
            byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

            Assert.IsFalse(this.testList.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testList.AddFirst("World");
                this.testList.AddAfter(this.testList.First, "!");
            });

            Assert.DoesNotThrow(() =>
            {
                this.testList.Lock(key, salt, distractionAfter: 5);
            });

            Assert.IsTrue(this.testList.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.SimpleUnlock(null);
            }, "Object is key locked");


            Assert.IsTrue(this.testList.IsLocked);

            this.FailTamperprocess();

            Assert.IsFalse(this.testList.Unlock(null, Encoding.UTF8.GetBytes("ThisIsNOTASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsTrue(this.testList.IsLocked);
            this.FailTamperprocess();

            Assert.IsFalse(this.testList.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMeNOT")));
            Assert.IsTrue(this.testList.IsLocked);
            this.FailTamperprocess();

            Assert.IsTrue(this.testList.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsFalse(this.testList.IsLocked);

            this.NoThrowTamper();

            Assert.IsNull(this.testList.UnlockReason);

            Assert.IsFalse(this.testList.Unlock("SomeReason", key, salt));

            Assert.IsNull(this.testList.UnlockReason);
        }

        private void NoThrowTamper()
        {
            Assert.DoesNotThrow(() =>
            {
                this.testList.Remove("Hello");
            });

            Assert.DoesNotThrow(() =>
            {
                this.testList.Clear();
            });
        }

        private void FailTamperprocess()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.AddLast("error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.Remove(this.testList.Last);
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.Remove("Hello");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testList.Clear();
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (this.testList != null && !this.testList.IsLocked)
            {
                this.testList.Clear();
            }
        }
    }
}
