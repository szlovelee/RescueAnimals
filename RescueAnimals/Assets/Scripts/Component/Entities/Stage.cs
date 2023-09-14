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

        [SerializeField] private float generationTime = 30f;
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

        private List<GameObject> aliveObjects = new();
        private int _blockGenRowIndex;
        public float BricksGenTime => generationTime;

        public int aliveCount => aliveObjects.Count;
        [SerializeField] private string onlyGenAnimal;
        public event Action OnBlockDestroyed;
        public event Action<AnimalType> OnAnimalSaved;
        public event Action<Vector2> OnBlockMoved;
        public event Action<Vector2> OnAnimalHit;
        public event Action<Vector2> OnBlockHit;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            blockGenerator = blockGenerators[0];
            _mapTypes = new MapType[maxRow, maxCol];
            _animalPool = new ObjectPool<Animal>(animalPrefabs);
            _blockPool = new ObjectPool<Block>(blockPrefabs);
            aliveObjects.Clear();
            CreateBlocks();
            CreateAnimals();
        }

        private float CalcBrickGenTime()
        {
            var time = 20 - ((float)stageNum * 5);
            Debug.Log($"{time} / {stageNum}");
            if (time < 5)
            {
                time = 5;
            }

            return time;
        }

        private int CalcAnimalPercentage()
        {
            //todo calculate animal percentage and return AnimalType
            if (Enum.TryParse(onlyGenAnimal, out AnimalType animalType))
            {
                return (int)animalType;
            }
            else
            {
                return Random.Range(0, AnimalTypes.Length);
            }
        }

        private int CalcBlockPercentage()
        {
            return 0;
        }

        public void StageClear()
        {
            ClearMap();
            stageNum++;
            generationTime = CalcBrickGenTime();
            BlockGenerator blockGen = blockGenerators[(stageNum - 1) % blockGenerators.Count];
            AnimalGenerator animalGen = animalGenerator; // temp
            ChangePattern(blockGen, animalGen);
            CreateBlocks();
            CreateAnimals();
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
            animalGenerator.Generate(maxRow, maxCol, maps: _mapTypes);
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
                            InstantiateBlock(position);
                            break;
                        case MapType.Animal:
                            InstantiateAnimal(position);
                            break;
                    }

                    //todo set prefab's data to apply random block, animalType
                }
            }
        }

        private void InstantiateAnimal(Vector2 position)
        {
            var selectedIdx = CalcAnimalPercentage();
            _animalPool.SelectedIndex = selectedIdx;
            var newAnimal = _animalPool.Pull(selectedIdx, position, Quaternion.identity);
            aliveObjects.Add(newAnimal.gameObject);
            newAnimal.MaxHp = 1f * stageNum;
            newAnimal.Hp = newAnimal.MaxHp;
            newAnimal.OnAnimalSave += AnimalSaved;
            newAnimal.OnHit += OnAnimalHit;
            SetAnimalReinforceState(newAnimal);
        }

        private void InstantiateBlock(Vector2 position)
        {
            var idx = CalcBlockPercentage();
            var newBlock = _blockPool.Pull(idx, position, Quaternion.identity);
            newBlock.MaxHp = 1f * stageNum;
            newBlock.Hp = newBlock.MaxHp;
            aliveObjects.Add(newBlock.gameObject);
            newBlock.OnHitBlock += OnBlockHit;
            newBlock.OnBlockDestroyed += BlockDestroyed;
        }


        public void ClearMap()
        {
            for (int i = aliveCount - 1; i >= 0; i--)
            {
                var block = aliveObjects[i];
                if (block.gameObject != null && block.activeSelf)
                {
                    block.SetActive(false);
                }
            }

            aliveObjects.Clear();
        }

        public void AddBlockLine()
        {
            _blockGenRowIndex = (_blockGenRowIndex + 1) % maxRow;

            for (int col = 0; col < maxCol; col++)
            {
                var mapType = _mapTypes[_blockGenRowIndex, col];
                if (mapType != MapType.Block) continue;

                //todo extract method to refactor
                var position = _startPosition;
                position.x += col * intervalX;
                InstantiateBlock(position);
            }

            Vector2 minYPos = Vector2.positiveInfinity;
            foreach (var obj in aliveObjects)
            {
                obj.transform.localPosition += new Vector3(0, intervalY);
                if (obj.transform.position.y <= minYPos.y)
                {
                    minYPos = obj.transform.position;
                }
            }

            OnBlockMoved?.Invoke(minYPos);
        }

        public void ResetStage()
        {
            stageNum = 1;
            BlockGenerator blockGen = blockGenerators[(stageNum - 1) % 4];
            // AnimalGenerator animalGen = animalGenerators[stageNum - 1];
            AnimalGenerator animalGen = animalGenerator; // temp
            ChangePattern(blockGen, animalGen);
            generationTime = 30f;
        }

        private void SetAnimalReinforceState(Animal animal)
        {
            AnimalReinforce data = animalData.AnimalReinforceData.Find(x => x.animalType == animal.animalType);

            animal.reinforceLevel = data.reinforceLevel;
        }

        public void SetStartPosition(Vector2 startPosition)
        {
            _startPosition = startPosition;
        }

        private void BlockDestroyed(Block block)
        {
            aliveObjects.Remove(block.gameObject);
            OnBlockDestroyed?.Invoke();
            block.OnBlockDestroyed -= BlockDestroyed;
        }

        private void AnimalSaved(Animal animal)
        {
            aliveObjects.Remove(animal.gameObject);
            OnAnimalSaved?.Invoke(animal.animalType);
            animal.OnAnimalSave -= AnimalSaved;
        }
    }
}