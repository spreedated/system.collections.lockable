using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableHashSet<T> : HashSet<T>, IObjectLockWritable
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

        public new bool Add(T item)
        {
            this.ObjectLock.Lockcheck();
            return base.Add(item);
        }
        public new bool Remove(T item)
        {
            this.ObjectLock.Lockcheck();
            return base.Remove(item);
        }
        public new int RemoveWhere(Predicate<T> match)
        {
            this.ObjectLock.Lockcheck();
            return base.RemoveWhere(match);
        }
        public new bool SetEquals(IEnumerable<T> other)
        {
            this.ObjectLock.Lockcheck();
            return base.SetEquals(other);
        }
        public new void Clear()
        {
            this.ObjectLock.Lockcheck();
            base.Clear();
        }
        public new void TrimExcess()
        {
            this.ObjectLock.Lockcheck();
            base.TrimExcess();
        }
    }
}
