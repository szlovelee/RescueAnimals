using UnityEngine;

namespace Util
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                T ret;
                if (_instance != null)
                {
                    ret = _instance;
                }
                else
                {
                    var go = new GameObject
                    {
                        name = typeof(T).Name,
                        // hideFlags = HideFlags.HideAndDontSave
                    };
                    ret = _instance = go.AddComponent<T>();
                }

                return ret;
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}