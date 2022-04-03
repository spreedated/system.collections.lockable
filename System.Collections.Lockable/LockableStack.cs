using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableStack<T> : Stack<T>, IObjectLockWritable
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

        public new T Pop()
        {
            this.ObjectLock.Lockcheck();
            return base.Pop();
        }
        public new void Push(T item)
        {
            this.ObjectLock.Lockcheck();
            base.Push(item);
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
#if NET5_0_OR_GREATER
        public new void TryPop(out T result)
        {
            this.ObjectLock.Lockcheck();
            base.TryPop(out result);
        }
#endif
    }
}
