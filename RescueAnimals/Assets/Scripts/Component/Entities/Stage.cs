using System;
using System.Collections.Generic;
using System.Numerics;
using Entities.BlockGenerators;
using EnumTypes;
using UnityEngine;
using UnityEngine.Serialization;
using Util;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Entities
{
    //확인좀 부탁 
    [CreateAssetMenu(menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        [SerializeField] private float intervalX = 1f;
        [SerializeField] private float intervalY = -0.3f;

        private Vector2 _startPosition = Vector2.zero;

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
        [SerializeField] private List<GameObject> animalPrefabs;
        private ObjectPool<Animal> _animalPool;
        public Vector2 BoxScale = new(0.5f, 0.5f);

        private MapType[,] _mapTypes;

        public StageType stage = StageType.None;
        public float BricksGenTime => CalcBrickGenTime();

        public bool IsStageOver => false; // todo : logic

        private void OnEnable()
        {
            _mapTypes = new MapType[maxRow, maxCol];
            blockPrefab.transform.localScale = BoxScale;
            _animalPool = new ObjectPool<Animal>(animalPrefabs);
            CreateBlocks();
            CreateAnimals();
        }

        private float CalcBrickGenTime()
        {
            var time = generationTime - (float)stage;
            return time < 20 ? 20 : time;
        }

        //
        private int CalcAnimalPercentage()
        {
            //todo calculate animal percentage and return AnimalType
            return Random.Range(0, AnimalTypes.Length);
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
        }


        private void CreateBlocks()
        {
            //todo : set blockGenerator from pattern 
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

                    if (mapType == MapType.Animal)
                    {
                        var selectedIdx = CalcAnimalPercentage();
                        _animalPool.SelectedIndex = selectedIdx;
                        _animalPool.Pull(selectedIdx, position, Quaternion.identity);
                        continue;
                    }

                    //todo make block pool
                    var prefab = mapType switch
                    {
                        MapType.Block => blockPrefab,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    //todo set prefab's data to apply random block, animalType

                    var block = CalcBlockPercentage();

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