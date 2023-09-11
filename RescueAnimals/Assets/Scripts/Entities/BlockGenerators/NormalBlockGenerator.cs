using System;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.BlockGenerators
{
    [CreateAssetMenu(menuName = "BlockGenerator/NormalBlockGenerator")]
    public class NormalBlockGenerator : BlockGenerator
    {
        public override List<Vector2> Generate()
        {
            var ret = new List<Vector2>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ret.Add(new Vector2(-1 + j, 3.5f + i * (0.3f)));
                }
            }
            return ret;
        }
    }
}