using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public abstract class AnimalGenerator : ScriptableObject
{
    public abstract void Generate(int rows, int cols, MapType[,] maps);
}