using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueAnimalsCharacterController : MonoBehaviour
{
    // event 외부에서는 호출하지 못하게 막는다
    public event Action<Vector2> OnMoveEvent;
    public event Action OnMagicShieldEvent;

    private float _timeSinceLastAttack = float.MaxValue;

    protected bool IsMagicShield { get; set; }


    protected virtual void Update()
    {
        HandleAttackDelay();
    }


    private void HandleAttackDelay()
    {
        if (IsMagicShield && _timeSinceLastAttack > 0.2f)
        {
            _timeSinceLastAttack = 0;
            CallMagicShieldEvent();
        }
    }


    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallMagicShieldEvent()
    {
        OnMagicShieldEvent?.Invoke();
    }
}