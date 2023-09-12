using System;
using UnityEngine;

namespace Entities
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform transform;
        private Vector2 _destination = Vector2.zero;

        private bool _isArrive;

        public float Speed = 5f;

        private void Start()
        {
            _isArrive = true;
        }

        public void MoveTo(Vector2 dest)
        {
            _destination = dest;
            _isArrive = false;
        }

        private void FixedUpdate()
        {
            if (_isArrive)
            {
                return;
            }

            Vector2 position = transform.position;
            var dir = (_destination - position).normalized;
            position += dir * Speed * Time.deltaTime;
            transform.position = position;
            Vector2 diff = position - _destination;

            if (Mathf.Abs(diff.x) <= 0.2 && Mathf.Abs(diff.y) <= 0.2)
            {
                _isArrive = true;
            }
        }
    }
}