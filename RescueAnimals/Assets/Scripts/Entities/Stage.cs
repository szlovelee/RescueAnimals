using System;
using System.Collections.Generic;
using System.Linq;
using Entities.BlockGenerators;
using EnumTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [CreateAssetMenu(menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        public static AnimalType[] AnimalTypes = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        public static BlockPattern[] BlockPatterns =
            (BlockPattern[])Enum.GetValues(typeof(BlockPattern));

        public static BlockType[] BlockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));

        [SerializeField] public BlockGenerator blockGenerator;
        public List<KeyValuePair<BlockType, Vector2>> BlockPositions;
        [SerializeField] private GameObject _prefab;
        public List<Animal> animals;
        public StageType stage = StageType.None;
        private Dictionary<AnimalType, float> _animalPercentages;
        private Dictionary<BlockType, float> _brickPercentages;
        public float BricksGenTime => CalcBrickGenTime();

        public bool IsClear => animals.Count <= 0;

        private void OnEnable()
        {
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
            CalcAnimalPercentage();
            CreateBlocks();
            CreateAnimals();
        }


        public void CreateBlocks()
        {
            BlockPositions = blockGenerator
                .Generate()
                .Select(vector2 =>
                {
                    var block = CalcBlockPercentage();
                    return new KeyValuePair<BlockType, Vector2>(block, vector2);
                }).ToList();
        }


        private void CreateAnimals()
        {
        }

        public void InstantiateBlocks()
        {
            foreach (var (blockType, position) in BlockPositions)
            {
                Instantiate(_prefab, position, Quaternion.identity);
            }
        }
    }
}