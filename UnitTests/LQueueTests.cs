using NUnit.Framework;
using System;
using System.Collections.Lockable;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class LQueueTests
    {
        private LockableQueue<string> testQueue;

        [SetUp]
        public void SetUp()
        {
            this.testQueue = new();
        }

        [Test]
        public void SimpleLock()
        {
            Assert.IsFalse(this.testQueue.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Enqueue("Hello");
                this.testQueue.Enqueue("World");
                this.testQueue.Enqueue("!");
            });

            this.testQueue.SimpleLock();

            Assert.IsTrue(this.testQueue.IsLocked);

            this.FailTamperprocess();

            Assert.IsTrue(this.testQueue.SimpleUnlock(null));

            this.NoThrowTamper();
        }

        [Test]
        public void LockTests()
        {
            byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
            byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

            Assert.IsFalse(this.testQueue.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Enqueue("Hello");
                this.testQueue.Enqueue("World");
                this.testQueue.Enqueue("!");
            });

            string a = null;
            string b = null;
            string c = null;

            Assert.DoesNotThrow(() =>
            {
                a = this.testQueue.Dequeue();
                b = this.testQueue.Dequeue();
                c = this.testQueue.Dequeue();
            });

            Assert.IsNotNull(a);
            Assert.IsNotNull(b);
            Assert.IsNotNull(c);

            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Lock(key, salt, distractionAfter: 5);
            });

            Assert.IsTrue(this.testQueue.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.SimpleUnlock(null);
            }, "Object is key locked");


            Assert.IsTrue(this.testQueue.IsLocked);

            this.FailTamperprocess();

            Assert.IsFalse(this.testQueue.Unlock(null, Encoding.UTF8.GetBytes("ThisIsNOTASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsTrue(this.testQueue.IsLocked);
            this.FailTamperprocess();

            Assert.IsFalse(this.testQueue.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMeNOT")));
            Assert.IsTrue(this.testQueue.IsLocked);
            this.FailTamperprocess();

            Assert.IsTrue(this.testQueue.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsFalse(this.testQueue.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Enqueue("Hello");
                this.testQueue.Enqueue("World");
                this.testQueue.Enqueue("!");
            });

            this.NoThrowTamper();

            Assert.IsNull(this.testQueue.UnlockReason);

            Assert.IsFalse(this.testQueue.Unlock("SomeReason", key, salt));

            Assert.IsNull(this.testQueue.UnlockReason);
        }

        private void NoThrowTamper()
        {
            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Dequeue();
            });

            Assert.DoesNotThrow(() =>
            {
                this.testQueue.Clear();
            });
        }

        private void FailTamperprocess()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.Enqueue("error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.Dequeue();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.TryDequeue(out _);
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.TrimExcess();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testQueue.Clear();
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (this.testQueue != null && !this.testQueue.IsLocked)
            {
                this.testQueue.Clear();
            }
        }
    }
}
