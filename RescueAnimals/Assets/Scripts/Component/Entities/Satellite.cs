using System;
using System.Collections;
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
        private float lastingTime;
        private Action<Satellite> _returnAction;
        public int Attack = 1;

        public void SetLastingTime(float time)
        {
            lastingTime = time;
        }

        private void Update()
        {
            if (Pivot == null)
            {
                ReturnToPool();
                return;
            }

            if (lastingTime <= 0f)
            {
                gameObject.SetActive(false);
                return;
            }

            lastingTime -= Time.deltaTime;
            _timeSinceLastAttack += Time.deltaTime;
            Radian += Mathf.PI * 10f * Time.deltaTime;
            
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
                if (_timeSinceLastAttack >= 0.5f)
                {
                    _timeSinceLastAttack = 0f;
                    _canAttack = true;
                }

                return _canAttack ? Attack : 0;
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