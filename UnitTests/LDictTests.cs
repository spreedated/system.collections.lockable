using NUnit.Framework;
using System;
using System.Collections.Lockable;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class LDictTests
    {
        private LockableDictionary<int, string> dict;

        [SetUp]
        public void SetUp()
        {
            this.dict = new();
        }

        [Test]
        public void SimpleLock()
        {
            Assert.IsFalse(this.dict.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.dict.Add(1, "Hello");
                this.dict.Add(2, "World");
                this.dict.Add(3, "!");
            });

            this.dict.SimpleLock();

            Assert.IsTrue(this.dict.IsLocked);

            this.FailTamperprocess();

            Assert.IsTrue(this.dict.SimpleUnlock(null));

            this.NoThrowTamper();
        }

        [Test]
        public void LockTests()
        {
            byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
            byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

            Assert.IsFalse(this.dict.IsLocked);

            Assert.DoesNotThrow(() =>
            {
                this.dict.Add(1, "Hello");
                this.dict.Add(2, "World");
                this.dict.Add(3, "!");
            });

            Assert.DoesNotThrow(() =>
            {
                this.dict.Lock(key, salt, distractionAfter: 5);
            });

            Assert.IsTrue(this.dict.IsLocked);

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.SimpleUnlock(null);
            }, "Object is key locked");


            Assert.IsTrue(this.dict.IsLocked);

            this.FailTamperprocess();

            Assert.IsFalse(this.dict.Unlock(null, Encoding.UTF8.GetBytes("ThisIsNOTASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsTrue(this.dict.IsLocked);
            this.FailTamperprocess();

            Assert.IsFalse(this.dict.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMeNOT")));
            Assert.IsTrue(this.dict.IsLocked);
            this.FailTamperprocess();

            Assert.IsTrue(this.dict.Unlock(null, Encoding.UTF8.GetBytes("ThisIsASecret"), Encoding.UTF8.GetBytes("RandomMe")));
            Assert.IsFalse(this.dict.IsLocked);

            this.NoThrowTamper();

            Assert.IsNull(this.dict.UnlockReason);

            Assert.IsFalse(this.dict.Unlock("SomeReason", key, salt));

            Assert.IsNull(this.dict.UnlockReason);
        }

        private void NoThrowTamper()
        {
            Assert.DoesNotThrow(() =>
            {
                this.dict.Remove(1);
            });

            Assert.DoesNotThrow(() =>
            {
                this.dict.Clear();
            });
        }

        private void FailTamperprocess()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.Add(4, "error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.TryAdd(4, "error");
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.Remove(1);
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.TrimExcess();
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.TrimExcess(2);
            });

            Assert.Throws<InvalidOperationException>(() =>
            {
                this.dict.Clear();
            });
        }

        [TearDown]
        public void TearDown()
        {
            if (this.dict != null && !this.dict.IsLocked)
            {
                this.dict.Clear();
            }
        }
    }
}
