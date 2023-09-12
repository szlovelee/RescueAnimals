using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Util;

public class Animal : MonoBehaviour, IPoolable<Animal>
{
    public AnimalType animalType;
    public int reinforceLevel;

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
}