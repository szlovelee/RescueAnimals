using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class CharacterInputController : MonoBehaviour
    {
        private Camera _camera;

        private event Action<Vector2> OnTouchPressEvent;
        private event Action<Vector2> LookEvent; // todo manager manage this event (calculate in manager) 
        private event Action HitBallEvent; // todo manager manage this event (calculate in manager)

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnTouchPosition(InputValue value)
        {
            var screenPos = value.Get<Vector2>();
            var worldPos = _camera.ScreenToWorldPoint(screenPos);
            Debug.Log($"screenPos is {screenPos.x}, {screenPos.y}");
            Debug.Log($"InvokeEvent::({worldPos.x}, {worldPos.y})");
            OnTouchPressEvent?.Invoke(worldPos);
        }
    }
}