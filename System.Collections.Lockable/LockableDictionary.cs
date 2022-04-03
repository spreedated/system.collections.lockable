using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IObjectLockWritable
    {
        #region Inherit ObjectLock
        private readonly ObjectLockWritable ObjectLock = new();
        public bool IsLocked
        {
            get
            {
                return this.ObjectLock.IsLocked;
            }
        }

        public string UnlockReason
        {
            get
            {
                return this.ObjectLock.UnlockReason;
            }
        }
        public void SimpleLock()
        {
            this.ObjectLock.SimpleLock();
        }

        public bool SimpleUnlock(string reason)
        {
            return this.ObjectLock.SimpleUnlock(reason);
        }

        public void Lock(byte[] key, byte[] salt, int distractionBefore = 47, int distractionAfter = 0)
        {
            this.ObjectLock.Lock(key, salt, distractionBefore, distractionAfter);
        }

        public bool Unlock(string reason, byte[] key, byte[] salt, int distractionBefore = 47)
        {
            return this.ObjectLock.Unlock(reason, key, salt, distractionBefore);
        }
        #endregion

        public new void Add(TKey key, TValue value)
        {
            this.ObjectLock.Lockcheck();
            base.Add(key, value);
        }
#if NET5_0_OR_GREATER
        public new bool TryAdd(TKey key, TValue value)
        {
            this.ObjectLock.Lockcheck();
            return base.TryAdd(key, value);
        }
        public new void TrimExcess()
        {
            this.ObjectLock.Lockcheck();
            base.TrimExcess();
        }
        public new void TrimExcess(int capacity)
        {
            this.ObjectLock.Lockcheck();
            base.TrimExcess(capacity);
        }
#endif
        public new void Clear()
        {
            this.ObjectLock.Lockcheck();
            base.Clear();
        }
        public new void Remove(TKey key)
        {
            this.ObjectLock.Lockcheck();
            base.Remove(key);
        }

    }
}
