namespace System.Collections.Lockable
{
    public interface IObjectLockWritable
    {
        public bool IsLocked { get; }
        public string UnlockReason { get; }
        public void SimpleLock();
        public bool SimpleUnlock(string reason);
        public void Lock(byte[] key, byte[] salt, int distractionBefore = 47, int distractionAfter = 0);
        public bool Unlock(string reason, byte[] key, byte[] salt, int distractionBefore = 47);
    }
}
