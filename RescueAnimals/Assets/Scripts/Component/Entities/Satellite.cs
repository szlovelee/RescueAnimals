using System;
using Entities;
using UnityEngine;
using Util;

namespace Component.Entities
{
    public class Satellite : MonoBehaviour, IAttackable, IPoolable<Satellite>
    {
        private bool _canAttack = true;
        private float _timeSinceLastAttack = float.MaxValue;
        [HideInInspector] public Transform Pivot;
        public float Radian;
        private float radius = 0.7f;
        private int _atk = 1;
        private Action<Satellite> _returnAction;

        public int Reinforce
        {
            set => _atk = Mathf.CeilToInt(value * 0.5f);
        }

        private void Update()
        {
            if (Pivot == null) return;
            
            _timeSinceLastAttack += Time.deltaTime;
            Radian += Mathf.PI * 10f * Time.deltaTime;
            radius += 0.1f;
            if (radius >= 3) radius = 0.7f;

            gameObject.transform.position = new Vector3(
                x: radius * Mathf.Cos(Radian),
                y: radius * Mathf.Sin(Radian)
            ) + Pivot.localPosition;

            if (Radian >= Mathf.PI * 2) Radian = 0;
        }

        public int Atk
        {
            get
            {
                if (_timeSinceLastAttack >= 0.01f)
                {
                    _timeSinceLastAttack = 0f;
                    _canAttack = true;
                }

                return _canAttack ? _atk : 0;
            }
        }

        public void Initialize(Action<Satellite> returnAction)
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