# System.Collections.Lockable

---

Write-lockable collections, like List&lt;T&gt;, LinkedList&lt;T&gt;, etc.
Lock a collection with a simple lock or HMAC SHA512.

### Usage C#

```csharp
using System.Collections.Lockable;

private LockableDictionary<int, string> dict = new();

this.dict.SimpleLock();
this.dict.SimpleUnlock("Unlock reason");
```

or Lock with Key

```csharp
using System.Collections.Lockable;

private LockableDictionary<int, string> dict = new();

byte[] key = Encoding.UTF8.GetBytes("ThisIsASecret");
byte[] salt = Encoding.UTF8.GetBytes("RandomMe");

this.dict.Lock(key, salt);
this.dict.Unlock("SomeReason", key, salt)
```

---

### Wanna create your own lockable Object?

Inherit the `IObjectLockWritable` interface and create a private instance of `ObjectLockWritable`.
This way you can inherit from more than one class.

Implement the interface:

```csharp
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
```

### Enjoying this?
Just star the repo or make a donation.

[![Donate0](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=35WE5NU48AUMA&source=url)

Your help is valuable since this is a hobby project for all of us: we do development during out-of-office hours.

### Contribution
Pull requests are very welcome.