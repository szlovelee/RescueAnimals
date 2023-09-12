using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Entities
{
    public class CharacterInputController : MonoBehaviour
    {
        private Camera _camera;
        private bool _isPointerOverUI = false;
        public event Action<Vector2> OnTouchPressEvent;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnTouchPosition(InputValue value)
        {
            if (_isPointerOverUI)
            {
                return;
            }

            var screenPos = value.Get<Vector2>();
            var worldPos = _camera.ScreenToWorldPoint(screenPos);
            //Debug.Log($"screenPos is {screenPos.x}, {screenPos.y}");
            //Debug.Log($"InvokeEvent::({worldPos.x}, {worldPos.y})");
            OnTouchPressEvent?.Invoke(worldPos);
        }

        private void Update()
        {
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }
}