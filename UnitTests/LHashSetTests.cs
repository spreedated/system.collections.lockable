using NUnit.Framework;
using System;
using System.Collections.Lockable;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class LHashSetTests
    {
        private LockableHashSet<string> testHash;

        [SetUp]
        public void SetUp()
        {
            this.testHash = new();
        }

        [Test]
        public void SimpleLock()
        {
            Assert.IsFalse(this.testHash.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testHash.Add("Hello");
                this.testHash.Add("World");
                this.testHash.Add("!");
            });

            this.testHash.SimpleLock();

            Assert.IsTrue(this.testHash.IsLocked);

            this.FailTamperprocess();

            Assert.IsTrue(this.testHash.SimpleUnlock(null));

            this.NoThrowTamper();
        }

        [Test]
        public void LockTests()
        {
            byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
            byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

            Assert.IsFalse(this.testHash.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.testHash.Add("Hello");
                this.testHash.Add("World");
                this.testHash.Add("!");
            });

            Assert.DoesNotThrow(() =>
            {
                this.testHash.Lock(key, salt, distractionAfter: 5);
            });

            Assert.IsTrue(this.testHash.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.SimpleUnlock(null);
            }, "Object is key locked");


            Assert.IsTrue(this.testHash.IsLocked);

            this.FailTamperprocess();

            Assert.IsFalse(this.testHash.Unlock(null, Encoding.UTF8.GetBytes("ThisIsNOTASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsTrue(this.testHash.IsLocked);
            this.FailTamperprocess();

            Assert.IsFalse(this.testHash.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMeNOT")));
            Assert.IsTrue(this.testHash.IsLocked);
            this.FailTamperprocess();

            Assert.IsTrue(this.testHash.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsFalse(this.testHash.IsLocked);

            this.NoThrowTamper();

            Assert.IsNull(this.testHash.UnlockReason);

            Assert.IsFalse(this.testHash.Unlock("SomeReason", key, salt));

            Assert.IsNull(this.testHash.UnlockReason);
        }

        private void NoThrowTamper()
        {
            Assert.DoesNotThrow(() =>
            {
                this.testHash.Remove("Hello");
            });

            Assert.DoesNotThrow(() =>
            {
                this.testHash.RemoveWhere(x => { return x == "World"; });
            });

            Assert.DoesNotThrow(() =>
            {
                this.testHash.Clear();
            });
        }

        private void FailTamperprocess()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.Add("error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.Remove("Hello");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.RemoveWhere(x => { return x == "Hello"; });
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.TrimExcess();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.testHash.Clear();
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (this.testHash != null && !this.testHash.IsLocked)
            {
                this.testHash.Clear();
            }
        }
    }
}
