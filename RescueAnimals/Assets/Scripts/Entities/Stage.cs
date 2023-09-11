using System;
using System.Collections.Generic;
using System.Linq;
using Entities.BlockGenerators;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [CreateAssetMenu(menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        private const float intervalX = 0.5f;
        private const float intervalY = 0.2f;

        private float startX = -1.5f;
        private float startY = 4f;

        [SerializeField] private int maxRow = 8;
        [SerializeField] private int maxCol = 8;

        public static AnimalType[] AnimalTypes = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        public static BlockPattern[] BlockPatterns =
            (BlockPattern[])Enum.GetValues(typeof(BlockPattern));

        public static BlockType[] BlockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));

        [SerializeField] public BlockGenerator blockGenerator;
        [SerializeField] public AnimalGenerator animalGenerator;

        [SerializeField] private GameObject _prefab; // block prefab => todo change name
        [SerializeField] private GameObject _animalPrefab; // block prefab => todo change name

        private MapType[,] _mapTypes;
        public List<KeyValuePair<BlockType, Vector2>> BlockPositions;
        public List<KeyValuePair<AnimalType, Vector2>> AnimalPositions;

        public StageType stage = StageType.None;
        public float BricksGenTime => CalcBrickGenTime();

        public bool IsClear => AnimalPositions.Count <= 0;

        private void OnEnable()
        {
            _mapTypes = new MapType[maxRow, maxCol];
            CreateBlocks();
            CreateAnimals();
        }

        private float CalcBrickGenTime()
        {
            var time = 100f - (float)stage;
            return time < 20 ? 20 : time;
        }

        public AnimalType CalcAnimalPercentage()
        {
            //todo calculate animal percentage and return AnimalType
            return AnimalType.Beagle;
        }

        public BlockType CalcBlockPercentage()
        {
            //todo how to set in integer...
            var randomValue = Random.Range(0, (int)stage);
            foreach (var block in BlockTypes)
            {
                if ((int)block > randomValue)
                    return block;
            }

            return BlockTypes[^1];
        }

        public void OnClearStage()
        {
            // update stage information to move next stage
            CalcAnimalPercentage();
            CreateBlocks();
            CreateAnimals();
        }


        public void CreateBlocks()
        {
            var blocks = blockGenerator.Generate(maxRow, maxCol);
            for (int row = 0; row < maxRow; row++)
            {
                for (int col = 0; col < maxCol; col++)
                {
                    _mapTypes[row, col] = blocks[row, col] ? MapType.Block : MapType.Blank;
                }
            }
        }


        private void CreateAnimals()
        {
            var animalPosList = animalGenerator
                .Generate(2)
                .Select(pos => new KeyValuePair<AnimalType, Vector2>(CalcAnimalPercentage(), pos))
                .ToList();

            AnimalPositions = animalPosList;
        }

        public void InstantiateObjects()
        {
            for (var row = 0; row < maxRow; row++)
            {
                for (var col = 0; col < maxCol; col++)
                {
                    var mapType = _mapTypes[row, col];
                    if (mapType == MapType.Blank) continue;
                    var position = new Vector2(startX + col * intervalX, startY + intervalY * col);
                    var prefab = mapType switch
                    {
                        MapType.Block => _prefab,
                        MapType.Animal => _prefab,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    //todo set prefab's data to apply random block, animalType
                    var block = CalcBlockPercentage();
                    var animal = CalcAnimalPercentage();

                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }
    }
}