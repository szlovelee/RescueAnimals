using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using System;

public class Animal : MonoBehaviour
{
    public AnimalType animalType;
    public int reinforceLevel;

    public void SetAnimalReinforceLevel(int level)
    {
        reinforceLevel = level;
    }
}
