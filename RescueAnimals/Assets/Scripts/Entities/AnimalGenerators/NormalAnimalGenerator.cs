using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.AnimalGenerators
{
    [CreateAssetMenu(menuName = "AnimalGenerator/NormalAnimalGenerator")]
    public class NormalAnimalGenerator : AnimalGenerator
    {
        private bool[,] isSelected = new bool[4, 4];
        [SerializeField] private Vector2 startPosition = new Vector2(0.2f, 0.4f);

        //todo migrate to scriptable Object BlockSetup -> in manager or !![stage]!!
        [SerializeField] private float intervalX = 1f;

        [SerializeField] private float intervalY = 1f;

        //
        public override List<Vector2> Generate(int animalCount)
        {
            var ret = new List<Vector2>();
            for (int i = 0; i < animalCount || i < 16; i++)
            {
                int row;
                int col;

                do
                {
                    row = Random.Range(0, 4);
                    col = Random.Range(0, 4);
                } while (!isSelected[row, col]);

                isSelected[row, col] = true;
                ret.Add(startPosition + new Vector2(x: col * intervalX, y: row * intervalY));
            }

            return ret;
        }
    }
}