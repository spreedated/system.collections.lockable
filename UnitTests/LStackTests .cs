using NUnit.Framework;
using System;
using System.Collections.Lockable;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class LStackTests
    {
        private LockableStack<string> testStack;

        [SetUp]
        public void SetUp()
        {
            this.testStack = new();
        }

        [Test]
        public void SimpleLock()
        {
            Assert.IsFalse(this.testStack.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testStack.Push("Hello");
                this.testStack.Push("World");
                this.testStack.Push("!");
            });

            this.testStack.SimpleLock();

            Assert.IsTrue(this.testStack.IsLocked);

            this.FailTamperprocess();

            Assert.IsTrue(this.testStack.SimpleUnlock(null));

            this.NoThrowTamper();
        }

        [Test]
        public void LockTests()
        {
            byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
            byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

            Assert.IsFalse(this.testStack.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testStack.Push("Hello");
                this.testStack.Push("World");
                this.testStack.Push("!");
            });

            Assert.DoesNotThrow(() =>
            {
                this.testStack.Lock(key, salt, distractionAfter: 5);
            });

            Assert.IsTrue(this.testStack.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testStack.SimpleUnlock(null);
            }, "Object is key locked");


            Assert.IsTrue(this.testStack.IsLocked);

            this.FailTamperprocess();

            Assert.IsFalse(this.testStack.Unlock(null, Encoding.UTF8.GetBytes("ThisIsNOTASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsTrue(this.testStack.IsLocked);
            this.FailTamperprocess();

            Assert.IsFalse(this.testStack.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMeNOT")));
            Assert.IsTrue(this.testStack.IsLocked);
            this.FailTamperprocess();

            Assert.IsTrue(this.testStack.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsFalse(this.testStack.IsLocked);

            this.NoThrowTamper();

            Assert.IsNull(this.testStack.UnlockReason);

            Assert.IsFalse(this.testStack.Unlock("SomeReason", key, salt));

            Assert.IsNull(this.testStack.UnlockReason);
        }

        private void NoThrowTamper()
        {
            Assert.DoesNotThrow(() =>
            {
                this.testStack.Pop();
            });

            Assert.DoesNotThrow(() =>
            {
                this.testStack.Clear();
            });
        }

        private void FailTamperprocess()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testStack.Push("error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testStack.Pop();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testStack.TrimExcess();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testStack.Clear();
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (this.testStack != null && !this.testStack.IsLocked)
            {
                this.testStack.Clear();
            }
        }
    }
}
