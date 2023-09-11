using System;
using System.Collections.Generic;
using Entities.BlockGenerators;
using EnumTypes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities
{
    [CreateAssetMenu(menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        [SerializeField] private float intervalX = 1f;
        [SerializeField] private float intervalY = -0.3f;

        private Vector2 _startPosition = new(-1.7f, 2.5f);
        [SerializeField] private float generationTime = 100f;
        public int maxRow = 8;
        public int maxCol = 8;

        public static AnimalType[] AnimalTypes = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        public static BlockPattern[] BlockPatterns =
            (BlockPattern[])Enum.GetValues(typeof(BlockPattern));

        public static BlockType[] BlockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));

        [SerializeField] public BlockGenerator blockGenerator;
        [SerializeField] public AnimalGenerator animalGenerator;

        [SerializeField] public GameObject blockPrefab; 
        [SerializeField] public GameObject animalPrefab;

        private MapType[,] _mapTypes;
        public List<KeyValuePair<BlockType, Vector2>> BlockPositions;
        public List<KeyValuePair<AnimalType, Vector2>> AnimalPositions;

        public StageType stage = StageType.None;
        public float BricksGenTime => CalcBrickGenTime();

        public bool IsStageOver => AnimalPositions.Count <= 0;

        private void OnEnable()
        {
            _mapTypes = new MapType[maxRow, maxCol];
            CreateBlocks();
            CreateAnimals();
        }

        private float CalcBrickGenTime()
        {
            var time = generationTime - (float)stage;
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
            animalGenerator.Generate(5, maxRow, maxCol, maps: _mapTypes);
        }

        public void InstantiateObjects()
        {
            for (var row = 0; row < maxRow; row++)
            {
                for (var col = 0; col < maxCol; col++)
                {
                    var mapType = _mapTypes[row, col];
                    if (mapType == MapType.Blank) continue;

                    var position = new Vector2(
                        x: _startPosition.x + intervalX * col,
                        y: _startPosition.y + intervalY * row);

                    var prefab = mapType switch
                    {
                        MapType.Block => blockPrefab,
                        MapType.Animal => animalPrefab,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    //todo set prefab's data to apply random block, animalType

                    var block = CalcBlockPercentage();
                    var animal = CalcAnimalPercentage();

                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }

        public void SetStartPosition(Vector2 startPosition)
        {
            _startPosition = startPosition;
        }
        //todo to manipulate retry make function that clear and init  
    }
}