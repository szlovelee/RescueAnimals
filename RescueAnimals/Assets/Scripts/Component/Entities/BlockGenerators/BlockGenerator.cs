using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.BlockGenerators
{
    public abstract class BlockGenerator : ScriptableObject
    {
        public abstract bool[,] Generate(int maxRow,int maxCol);
    }
}