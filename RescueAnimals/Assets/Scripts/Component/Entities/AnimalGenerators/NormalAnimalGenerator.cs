using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

namespace Entities.AnimalGenerators
{
    [CreateAssetMenu(menuName = "AnimalGenerator/NormalAnimalGenerator")]
    public class NormalAnimalGenerator : AnimalGenerator
    {
        public int animalCount = 5;

        public override void Generate(int rows, int cols, MapType[,] maps)
        {
            bool[,] isSelected = new bool[rows, cols];
            for (int i = 0; i < animalCount && i < maps.Length; i++)
            {
                int row;
                int col;
                do
                {
                    row = Random.Range(0, rows);
                    col = Random.Range(0, cols);
                } while (isSelected[row, col] || maps[row, col] != MapType.Blank);

                isSelected[row, col] = true;
                maps[row, col] = MapType.Animal;
            }
        }
    }
}