using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableQueue<T> : Queue<T>, IObjectLockWritable
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

        public new void Enqueue(T item)
        {
            this.ObjectLock.Lockcheck();
            base.Enqueue(item);
        }
        public new T Dequeue()
        {
            this.ObjectLock.Lockcheck();
            return base.Dequeue();
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
        public new void TryDequeue(out T result)
        {
            this.ObjectLock.Lockcheck();
            base.TryDequeue(out result);
        }
#endif
    }
}
