using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableList<T> : List<T>, IObjectLockWritable
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

        public new void Add(T item)
        {
            this.ObjectLock.Lockcheck();
            base.Add(item);
        }
        public new void AddRange(IEnumerable<T> collection)
        {
            this.ObjectLock.Lockcheck();
            base.AddRange(collection);
        }
        public new void Remove(T item)
        {
            this.ObjectLock.Lockcheck();
            base.Remove(item);
        }
        public new void RemoveAll(Predicate<T> match)
        {
            this.ObjectLock.Lockcheck();
            base.RemoveAll(match);
        }
        public new void RemoveAt(int index)
        {
            this.ObjectLock.Lockcheck();
            base.RemoveAt(index);
        }
        public new void RemoveRange(int index, int count)
        {
            this.ObjectLock.Lockcheck();
            base.RemoveRange(index, count);
        }
        public new void Insert(int index, T item)
        {
            this.ObjectLock.Lockcheck();
            base.Insert(index, item);
        }
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            this.ObjectLock.Lockcheck();
            base.InsertRange(index, collection);
        }
        public new void Clear()
        {
            this.ObjectLock.Lockcheck();
            base.Clear();
        }
        public new void Reverse()
        {
            this.ObjectLock.Lockcheck();
            base.Reverse();
        }
        public new void TrimExcess()
        {
            this.ObjectLock.Lockcheck();
            base.TrimExcess();
        }
    }
}
