#pragma warning disable IDE0063

using System;
using System.Linq;
using System.Security.Cryptography;

namespace System.Collections.Lockable
{
    public sealed class ObjectLockWritable
    {
        public bool IsLocked { get; private set; }
        public string UnlockReason { get; private set; }
        internal byte[] lockKey;
        public void SimpleLock()
        {
            this.IsLocked = true;
        }
        public void Lock(byte[] key, byte[] salt, int distractionBefore = 47, int distractionAfter = 0)
        {
            if (this.IsLocked || this.lockKey != null)
            {
                throw new InvalidOperationException("Object is already locked, unlock to re-lock");
            }

            this.IsLocked = true;

            byte[] crypt = Crypt(key, salt, distractionBefore);
            this.lockKey = new byte[crypt.Length + distractionBefore + distractionAfter];
            crypt.CopyTo(this.lockKey, distractionBefore);

            AddRandomBytes(this.lockKey, 0, distractionBefore);
            AddRandomBytes(this.lockKey, this.lockKey.Length - distractionAfter, distractionAfter);
        }
        public bool SimpleUnlock(string reason)
        {
            if (this.lockKey != null)
            {
                throw new InvalidOperationException("Object is key locked");
            }
            this.IsLocked = false;
            this.UnlockReason = reason;
            return true;
        }
        public bool Unlock(string reason, byte[] key, byte[] salt, int distractionBefore = 47)
        {
            if (!this.IsLocked || this.lockKey == null)
            {
                return false;
            }

            byte[] crypt = Crypt(key, salt, distractionBefore);
            byte[] potentialKey = new byte[crypt.Length + distractionBefore];
            crypt.CopyTo(potentialKey, distractionBefore);

            if (!potentialKey.Skip<byte>(distractionBefore).Take<byte>(key.Length + salt.Length).SequenceEqual(this.lockKey.Skip<byte>(distractionBefore).Take<byte>(key.Length + salt.Length)))
            {
                return false;
            }

            this.IsLocked = false;
            this.lockKey = null;
            this.UnlockReason = reason;
            return true;
        }

        private static byte[] Crypt(byte[] key, byte[] salt, int begin)
        {
            byte[] container = new byte[key.Length + salt.Length + begin];
            key.CopyTo(container, 0);
            salt.CopyTo(container, key.Length);

            using (HMACSHA512 hmac = new(container))
            {
                hmac.Initialize();
                byte[] buffer = new byte[container.Length];
                byte[] keyBuffer = hmac.ComputeHash(buffer, begin, container.Length - begin);
                return keyBuffer;
            }
        }

        private static void AddRandomBytes(byte[] keyLock, int offset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Random random = new(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
                keyLock[i + offset] = (byte)random.Next(1, 255);
            }
        }
        internal void Lockcheck()
        {
            if (this.IsLocked && this.lockKey != null)
            {
                throw new InvalidOperationException("Object is writable locked");
            }
            if (this.IsLocked)
            {
                throw new InvalidOperationException("Object is simple locked");
            }
        }
    }
}
