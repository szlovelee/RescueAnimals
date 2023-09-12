using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Dictionary<int, Stack<T>> _objectPool = new();
        protected List<GameObject> _prefabs;
        public int SelectedIndex;

        public int PooledCount(int idx)
        {
            var isValid = _objectPool.TryGetValue(idx, out var stack);
            return isValid ? stack.Count : 0;
        }

        public T Pull()
        {
            T t;
            if (PooledCount(SelectedIndex) > 0)
            {
                t = _objectPool[SelectedIndex].Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefabs[SelectedIndex]).GetComponent<T>();
            }

            t.gameObject.SetActive(true);
            t.Initialize(r => Push(r, SelectedIndex));
            return t;
        }

        public T Pull(int selectedIndex, Vector2 position, Quaternion rotation)
        {
            T t;
            if (PooledCount(selectedIndex) > 0)
            {
                t = _objectPool[selectedIndex].Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefabs[selectedIndex]).GetComponent<T>();
            }

            t.gameObject.SetActive(true);
            var transform = t.transform;
            transform.localPosition = position;
            transform.localRotation = rotation;
            t.Initialize(v => { Push(v, selectedIndex); });
            return t;
        }


        public void Push(T obj)
        {
            if (!_objectPool.TryGetValue(SelectedIndex, out var stack))
            {
                stack = new();
                _objectPool[SelectedIndex] = stack;
            }

            stack.Push(obj);
            obj.gameObject.SetActive(false);
        }

        public void Push(T obj, int selectedIndex)
        {
            SelectedIndex = selectedIndex;
            Push(obj);
        }
        
        public void Clear()
        {
            _objectPool.Clear();
        }

        public ObjectPool(List<GameObject> prefabs, int spawnNum = 0)
        {
            _prefabs = prefabs;
        }

        
    }
}