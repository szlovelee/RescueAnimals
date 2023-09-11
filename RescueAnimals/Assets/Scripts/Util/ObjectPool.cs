using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Stack<T> _objectPool = new();
        protected List<GameObject> _prefabs;
        public int SelectedIndex;
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
                t = GameObject.Instantiate(_prefabs[SelectedIndex]).GetComponent<T>();
            }
            
            t.gameObject.SetActive(true);
            t.Initialize(Push);


            return t;
        }
        
        public T Pull(int selectedIndex, Vector2 position, Quaternion rotation)
        {
            T t;
            if (PooledCount > 0)
            {
                t = _objectPool.Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefabs[selectedIndex]).GetComponent<T>();
            }

            var transform = t.transform;
            transform.localPosition = position;
            transform.localRotation = rotation;
            t.gameObject.SetActive(true);
            t.Initialize(Push);


            return t;
        }

        
        public void Push(T obj)
        {
            _objectPool.Push(obj);
            obj.gameObject.SetActive(false);
        }

        public ObjectPool(List<GameObject> prefabs)
        {
            _prefabs = prefabs;
        }
    }
}