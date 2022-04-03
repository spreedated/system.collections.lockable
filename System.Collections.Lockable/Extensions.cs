using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Lockable
{
    public static class Extensions
    {
        public static LockableLinkedList<T> ToLockableLinkedList<T>(this IEnumerable<T> obj)
        {
            LockableLinkedList<T> a = new();
            a.AddFirst(obj.First());
            foreach (T item in obj.Skip(1).Reverse())
            {
                a.AddAfter(a.First, item);
            }
            return a;
        }

        public static LockableList<T> ToLockableList<T>(this IEnumerable<T> obj)
        {
            LockableList<T> a = new();
            foreach (T item in obj)
            {
                a.Add(item);
            }
            return a;
        }

        public static LockableHashSet<T> ToLockableHashSet<T>(this IEnumerable<T> obj)
        {
            LockableHashSet<T> a = new();
            foreach (T item in obj)
            {
                if (!a.Contains(item))
                {
                    a.Add(item);
                }
            }
            return a;
        }

        public static LockableQueue<T> ToLockableQueue<T>(this IEnumerable<T> obj)
        {
            LockableQueue<T> a = new();
            foreach (T item in obj)
            {
                a.Enqueue(item);
            }
            return a;
        }

        public static LockableStack<T> ToLockableStack<T>(this IEnumerable<T> obj)
        {
            LockableStack<T> a = new();
            foreach (T item in obj)
            {
                a.Push(item);
            }
            return a;
        }

        public static LockableDictionary<int, T> ToLockableDictionary<T>(this IEnumerable<T> obj)
        {
            LockableDictionary<int, T> a = new();
            int c = 0;
            foreach (T item in obj)
            {
                if (!a.Any(x => x.Value.Equals(item)))
                {
                    a.Add(c, item);
                }
                c++;
            }
            return a;
        }
    }
}
