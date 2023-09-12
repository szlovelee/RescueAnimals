using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Util;
using UnityEngine.Events;
using Entities;
using Unity.Mathematics;

public class Animal : MonoBehaviour, IPoolable<Animal>
{
    [SerializeField] private double MaxHp = 10;
    [SerializeField] private double Hp = 10;
    //[SerializeField] private Animator _animator;

    public AnimalType animalType;
    public int reinforceLevel;

    public UnityEvent onResqueEvent;

    public void SetAnimalReinforceLevel(int level)
    {
        reinforceLevel = level;
    }

    private Action<Animal> _returnAction;

    public void Initialize(Action<Animal> returnAction)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var attackable = collision.gameObject.GetComponent<IAttackable>();
        if (attackable == null) return;
        Hp -= attackable.Atk;
        if (Hp <= 0)
        {
            onResqueEvent.Invoke();
            // gameObject.SetActive(false);
        }
    }
}