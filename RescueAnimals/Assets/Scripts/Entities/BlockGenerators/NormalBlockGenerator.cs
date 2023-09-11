using System;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.BlockGenerators
{
    [CreateAssetMenu(menuName = "BlockGenerator/NormalBlockGenerator")]
    public class NormalBlockGenerator : BlockGenerator
    {
        public override bool[,] Generate(int maxRow, int maxCol)
        {
            var ret = new bool[maxRow, maxCol];
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    ret[i, j] = (j & 1) == 0;
                }
            }

            return ret;
        }
    }
}