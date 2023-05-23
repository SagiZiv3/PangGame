using System;
using System.Collections.Generic;

namespace Pang.Pools
{
    internal class Pool<T>
    {
        private readonly Stack<T> pool;
        private readonly Func<T> creationMethod;

        public Pool(Func<T> creationMethod)
        {
            pool = new Stack<T>();
            this.creationMethod = creationMethod;
        }

        public bool TryGet(out T value)
        {
            if (pool.Count == 0)
            {
                value = default;
                return false;
            }

            value = pool.Pop();
            return true;
        }

        public T GetOrCreate()
        {
            if (!TryGet(out T value))
            {
                value = creationMethod();
            }

            return value;
        }

        public void Return(T value)
        {
            pool.Push(value);
        }
    }
}