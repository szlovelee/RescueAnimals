using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class SinglePrefabObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Stack<T> _stack;

        public int PooledCount => _stack.Count;
        private GameObject _prefab;


        public T Pull()
        {
            T t;
            if (PooledCount > 0)
            {
                t = _stack.Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefab).AddComponent<T>();
                t.Initialize(Push);
            }

            t.gameObject.SetActive(true);
            return t;
        }
        
        public T Pull(Vector3 position)
        {
            var t = Pull();
            t.transform.position = position;
            return t;
        }

        public void Push(T obj)
        {
            obj.gameObject.SetActive(false);
            _stack.Push(obj);
        }

        public SinglePrefabObjectPool(GameObject prefab, int spawnNum = 0)
        {
            _prefab = prefab;
            _stack = new();
            Spawn(spawnNum);
        }

        private void Spawn(int n)
        {
            for (int i = 0; i < n; i++)
            {
                T t = GameObject.Instantiate(_prefab).AddComponent<T>();
                t.Initialize(Push);
                t.gameObject.SetActive(false);
            }
        }
    }
}