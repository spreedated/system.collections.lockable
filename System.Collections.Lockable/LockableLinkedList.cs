using System.Collections.Generic;

namespace System.Collections.Lockable
{
    public class LockableLinkedList<T> : LinkedList<T>, IObjectLockWritable
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

        public new void AddFirst(T value)
        {
            this.ObjectLock.Lockcheck();
            base.AddFirst(value);
        }
        public new void AddLast(T value)
        {
            this.ObjectLock.Lockcheck();
            base.AddLast(value);
        }
        public new void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            this.ObjectLock.Lockcheck();
            base.AddAfter(node, newNode);
        }
        public new void AddAfter(LinkedListNode<T> node, T value)
        {
            this.ObjectLock.Lockcheck();
            base.AddAfter(node, value);
        }
        public new void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            this.ObjectLock.Lockcheck();
            base.AddBefore(node, newNode);
        }
        public new void AddBefore(LinkedListNode<T> node, T value)
        {
            this.ObjectLock.Lockcheck();
            base.AddBefore(node, value);
        }
        public new void Remove(T value)
        {
            this.ObjectLock.Lockcheck();
            base.Remove(value);
        }
        public new void Remove(LinkedListNode<T> node)
        {
            this.ObjectLock.Lockcheck();
            base.Remove(node);
        }
        public new void RemoveFirst()
        {
            this.ObjectLock.Lockcheck();
            base.RemoveFirst();
        }
        public new void RemoveLast()
        {
            this.ObjectLock.Lockcheck();
            base.RemoveLast();
        }
        public new void Clear()
        {
            this.ObjectLock.Lockcheck();
            base.Clear();
        }
    }
}
