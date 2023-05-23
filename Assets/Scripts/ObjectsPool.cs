﻿using UnityEngine;

namespace Pang
{
    internal class ObjectsPool<T> : Pool<T> where T : Object
    {
        public ObjectsPool(T prefab) : base(() => Object.Instantiate(prefab))
        {
        }
    }
}