using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Stack<T> _objectPool = new();
        public GameObject _prefab;
        public int PooledCount => _objectPool.Count;
        
        public T Pull()
        {
            T t;
            if (PooledCount > 0)
            {
                t = _objectPool.Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefab).GetComponent<T>();
            }
            t.gameObject.SetActive(true);
            t.Initialize(Push);
            
            
            return t;
        }

        public void Push(T obj)
        {
            _objectPool.Push(obj);
            obj.gameObject.SetActive(false);
        }
    }
}