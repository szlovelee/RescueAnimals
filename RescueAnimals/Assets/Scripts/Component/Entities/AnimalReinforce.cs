using EnumTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalReinforce
{
    public AnimalType animalType;
    public int reinforceLevel;
    public int reinforcePrice;
    public float bonusStatRate;
    public bool isActive = false;
}
