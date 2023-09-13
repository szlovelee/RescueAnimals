using System;
using Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Component.Entities
{
    public class RunningBeagle : MonoBehaviour, IAttackable, IPoolable<RunningBeagle>
    {
        [HideInInspector] public int attack;

        public int Atk
        {
            get
            {
                var ret = attack;
                gameObject.SetActive(false);
                return ret;
            }
        }

        private Action<RunningBeagle> _returnAction;

        public void Initialize(Action<RunningBeagle> returnAction)
        {
            _returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            _returnAction?.Invoke(this);
        }

        private void OnDisable()
        {
            ReturnToPool();
        }
    }
}