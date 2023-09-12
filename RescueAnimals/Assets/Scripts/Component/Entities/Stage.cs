using System;
using System.Collections.Generic;
using Entities.BlockGenerators;
using EnumTypes;
using UnityEngine;
using Util;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Entities
{
    [CreateAssetMenu(menuName = "Stage")]
    public class Stage : ScriptableObject
    {
        public int stageNum = 1;

        [SerializeField] private AnimalData animalData;

        [SerializeField] private float intervalX = 1f;
        [SerializeField] private float intervalY = -0.3f;

        private Vector2 _startPosition = Vector2.zero;

        [SerializeField] private float generationTime = 100f;
        public int maxRow = 8;
        public int maxCol = 8;

        public static AnimalType[] AnimalTypes = (AnimalType[])Enum.GetValues(typeof(AnimalType));

        public static BlockType[] BlockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));


        [SerializeField] public AnimalGenerator animalGenerator;

        [SerializeField] private List<BlockGenerator> blockGenerators;
        [SerializeField] private List<AnimalGenerator> animalGenerators;
        private BlockGenerator blockGenerator;


        [SerializeField] private List<GameObject> blockPrefabs;
        [SerializeField] private List<GameObject> animalPrefabs;

        private ObjectPool<Animal> _animalPool;
        private ObjectPool<Block> _blockPool;
        public Vector2 BoxScale = new(0.5f, 0.5f);

        private MapType[,] _mapTypes;

        public StageType stage = StageType.None;

        private List<GameObject> instantiatedObjects = new List<GameObject>();

        public float BricksGenTime => CalcBrickGenTime();

        public bool IsStageOver => false; // todo : write logic to move next stage

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            blockGenerator = blockGenerators[0];
            _mapTypes = new MapType[maxRow, maxCol];
            _animalPool = new ObjectPool<Animal>(animalPrefabs);
            _blockPool = new ObjectPool<Block>(blockPrefabs);
            CreateBlocks();
            CreateAnimals();
        }

        private float CalcBrickGenTime()
        {
            var time = generationTime - (float)stage;
            return time < 20 ? 20 : time;
        }

        private int CalcAnimalPercentage()
        {
            //todo calculate animal percentage and return AnimalType
            return Random.Range(0, AnimalTypes.Length);
        }

        private int CalcBlockPercentage()
        {
            return 0;
        }

        public void UpdateStageSettings()
        {
            stageNum++;
            Debug.Log($"New Stage: {stageNum}");
            BlockGenerator blockGen = blockGenerators[(stageNum - 1) % blockGenerators.Count];
            // AnimalGenerator animalGen = animalGenerators[stageNum - 1];
            AnimalGenerator animalGen = animalGenerator; // temp

            ChangePattern(blockGen, animalGen);

            CreateBlocks();
            CreateAnimals();

            ClearMap();
        }

        public void ChangePattern(BlockGenerator blockGen, AnimalGenerator animalGen)
        {
            blockGenerator = blockGen;
            animalGenerator = animalGen;
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
            animalGenerator.Generate( maxRow, maxCol, maps: _mapTypes);
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

                    switch (mapType)
                    {
                        case MapType.Block:
                            //todo calc this index block
                            var idx = CalcBlockPercentage();
                            var newBlock = _blockPool.Pull(idx, position, Quaternion.identity);
                            instantiatedObjects.Add(newBlock.gameObject);
                            break;
                        case MapType.Animal:
                            var selectedIdx = CalcAnimalPercentage();
                            _animalPool.SelectedIndex = selectedIdx;
                            var newAnimal = _animalPool.Pull(selectedIdx, position, Quaternion.identity);
                            instantiatedObjects.Add(newAnimal.gameObject);
                            SetAnimalReinforceState(newAnimal);
                            break;
                    }

                    //todo set prefab's data to apply random block, animalType
                }
            }
        }


        public void ClearMap()
        {
            foreach (var block in instantiatedObjects)
            {
                if(block.gameObject != null)
                {
                    block.SetActive(false);
                }
            }
        }

        public void ResetStage()
        {
            Initialize();
            stageNum = 1;
            BlockGenerator blockGen = blockGenerators[(stageNum - 1) % 4];
            // AnimalGenerator animalGen = animalGenerators[stageNum - 1];
            AnimalGenerator animalGen = animalGenerator; // temp

            ChangePattern(blockGen, animalGen);
        }

        private void SetAnimalReinforceState(Animal animal)
        {
            AnimalReinforce data = animalData.AnimalReinforceData.Find(x=>x.animalType == animal.animalType);

            animal.reinforceLevel = data.reinforceLevel;
        }

        public void SetStartPosition(Vector2 startPosition)
        {
            _startPosition = startPosition;
        }
        //todo to manipulate retry make function that clear and init  
    }
}