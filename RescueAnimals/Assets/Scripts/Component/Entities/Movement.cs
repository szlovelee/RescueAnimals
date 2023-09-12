using System;
using UnityEngine;

namespace Entities
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform transform;
        private float _destination;

        private bool _isArrive;

        public float Speed = 20f;

        private void Start()
        {
            _isArrive = true;
        }

        public void MoveTo(Vector2 dest)
        {
            _destination = dest.x;
            _isArrive = false;
        }

        private void FixedUpdate()
        {
            if (_isArrive)
            {
                return;
            }

            Vector2 position = transform.position;
            var dir = _destination > position.x ? 1 : -1;
            var dx = Speed * Time.deltaTime * dir;
            position.x += dx;
            transform.position = position;

            float diff = position.x - _destination;
            if (Mathf.Abs(diff) <= 0.2)
            {
                _isArrive = true;
            }
        }
    }
}