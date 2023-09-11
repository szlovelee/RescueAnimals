using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public abstract class AnimalGenerator : ScriptableObject
{
    public abstract List<Vector2> Generate(int animalCount);
}