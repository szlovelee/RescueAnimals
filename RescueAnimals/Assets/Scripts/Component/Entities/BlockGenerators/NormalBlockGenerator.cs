using System;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.BlockGenerators
{
    [CreateAssetMenu(menuName = "BlockGenerator/NormalBlockGenerator")]
    public class NormalBlockGenerator : BlockGenerator
    {
        [Multiline(12)] public string map;
        public char blankCharacter = '@';
        public override bool[,] Generate(int maxRow, int maxCol)
        {
            var ret = new bool[maxRow, maxCol];
            var mapArray = map.Split("\n");
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxCol; j++)
                {
                    if (i < mapArray.Length &&
                        j < mapArray[i].Length &&
                        mapArray[i][j] != ' ' &&
                        mapArray[i][j] != blankCharacter)
                    {
                        ret[i, j] = true;
                    }
                    else
                    {
                        ret[i, j] = false;
                    }
                }
            }


            return ret;
        }
    }
}