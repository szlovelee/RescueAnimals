using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Component.Entities;
using Component.Entities.Database;
using Entities;
using EnumTypes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Stage currentStage;
    public RankSystem Rank;
    public int score;
    public int coin;
    public float _lastTimeRegenerateBlock = 0f;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject beaglePrefab;
    [SerializeField] private AnimalData animalData;
    [SerializeField] private Coefficient reinforceCoef;
    [SerializeField] private ParticleSystem ballParticle;
    [SerializeField] private GameObject satellitePrefab;


    private Camera cam;

    public event Action OnStageClear;
    public event Action OnGameEnd;
    public event Action OnScoreAdded; // todo make Action<int> send score value to presenter 

    private SinglePrefabObjectPool<Ball> _ballObjectPool;
    private SinglePrefabObjectPool<Satellite> _satellitePool;
    private SinglePrefabObjectPool<RunningBeagle> _beaglePool;
    private List<Satellite> _satellites = new();
    private List<GameObject> _beagles = new();
    float ballSpeed = 0f;
    public float gameOverLine = 0f;
    private Vector2 ballPos = new Vector2(0, -2.8f);
    private SaveData gameData;
    [SerializeField] private int _ballCount;
    [SerializeField] private float _timeScale = 1f;
    private bool isPlaying = true;
    private int addedScore;

    private bool IsStageClear => addedScore > 1000 + currentStage.stageNum || currentStage.aliveCount <= 0;
    public static GameManager Instance;

    public bool IsStarted { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            cam = Camera.main;
            _ballObjectPool = new(prefab: ballPrefab, 5);
            _satellitePool = new(prefab: satellitePrefab, 0);
            _beaglePool = new(prefab: beaglePrefab, 0);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Time.timeScale = _timeScale;
        SetGame();
        Debug.Log(Rank);
    }

    private void FixedUpdate()
    {
        if (IsGameOver())
        {
            GameOver();
            return;
        }

        if (!IsStageClear) return;
        GamePause();
        ResetBall();
        ClearBeagles();
        ClearSatellites();
        addedScore = 0;
        SoundManager.instance.PlayStageClear();
        currentStage.StageClear();
        OnStageClear?.Invoke();
    }

    private void OnDestroy()
    {
        currentStage.OnBlockDestroyed -= AddBlockPoint;
        currentStage.OnAnimalSaved -= AddAnimalPoint;
        currentStage.OnBlockMoved -= OnBlockMoved;
        StopCoroutine(RegenerateBlockOnTime());
    }

    private void GameOver()
    {
        player.gold += coin;
        UpdateRank();
        isPlaying = false;
        DataManager.Instance.SavePlayer(player, animalData, Rank.GetRankings());
        OnGameEnd?.Invoke();
    }

    private bool IsGameOver()
    {
        Scene scene = SceneManager.GetActiveScene();
        return _ballCount <= 0 && scene.name == "GameScene" && isPlaying;
    }


    private void CreateBall()
    {
        //todo ball make pool-able
        _ballCount = 1;
        Ball newBall = Instantiate(ballPrefab, ballPos, Quaternion.identity).GetComponent<Ball>();
        player.balls.Add(newBall);
    }

    private void SetGame()
    {
        gameData = DataManager.Instance.LoadPlayerInfo(animalData);
        currentStage.Initialize();
        Time.timeScale = _timeScale;
        MakeWalls();
        SetBlockStartPosition();
        currentStage.ResetStage();
        score = 0;
        isPlaying = true;
        OnScoreAdded += ScoreCheck;
        OnGameEnd += ResetBall;
        OnGameEnd += GamePause;
        currentStage.InstantiateObjects();
        InstantiateCharacter();
        CreateBall();
        StartCoroutine(RegenerateBlockOnTime());
        ListenStageEvent();
        DataSetting();
    }

    private void DataSetting()
    {
        player.level = gameData.Level;
        player.exp = gameData.Exp;
        player.atk = gameData.Atk;
        player.gold = gameData.Gold;
        //동물 강화 기능 추가
        Rank.rankings = gameData.RankSystemData;
    }


    private IEnumerator RegenerateBlockOnTime()
    {
        while (isPlaying)
        {
            yield return new WaitForNextFrameUnit();
            _lastTimeRegenerateBlock += Time.deltaTime;
            if (_lastTimeRegenerateBlock >= currentStage.BricksGenTime)
            {
                currentStage.AddBlockLine();
                _lastTimeRegenerateBlock = 0;
            }
        }
    }

    private void ListenStageEvent()
    {
        currentStage.OnBlockDestroyed += AddBlockPoint;
        currentStage.OnAnimalSaved += AddAnimalPoint;
        currentStage.OnBlockMoved += OnBlockMoved;
    }


    private void AddScoreAndMoney(int addedScore, int addedCoin)
    {
        score += addedScore;
        coin += addedCoin;
        this.addedScore += addedScore;
        OnScoreAdded?.Invoke();
    }

    private void AddBlockPoint()
    {
        AddScoreAndMoney(10, 2);
        SoundManager.instance.PlayBallEffect();
    }

    private void AddAnimalPoint(AnimalType t)
    {
        AddScoreAndMoney(50, 25);
        SoundManager.instance.PlayBallEffectOnCage();
    }

    private void ScoreCheck()
    {
    }

    private void ResetBall()
    {
        for (int i = 0; i < player.balls.Count; i++)
        {
            player.balls[i].OnBallCollide -= ShowParticle;
            player.balls[i].gameObject.SetActive(false);
        }

        _ballCount = 0;
        player.balls.Clear();
        IsStarted = false;
        CreateBall();
    }

    private void ClearSatellites()
    {
        foreach (var satellite in _satellites)
        {
            satellite.gameObject.SetActive(false);
        }

        _satellites.Clear();
    }

    private void ClearBeagles()
    {
        foreach (var beagle in _beagles)
        {
            beagle.SetActive(false);
        }

        _beagles.Clear();
    }


    private void UpdateRank()
    {
        Rank rank = new Rank(score, currentStage.stageNum);
        Rank.AddRank(rank);
        Debug.Log("Rank Added On GameEnd");
    }

    public void StartStage()
    {
        _lastTimeRegenerateBlock = 0f;
        currentStage.InstantiateObjects();
        GameResume();
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
    }

    public void AddBalls(Vector2 position, int ballCount)
    {
        for (var i = 0; i < ballCount; i++)
        {
            var ball = _ballObjectPool.Pull();
            ball.SetBonusBall();
            ball.transform.position = position;
            ball.OnBallCollide += ShowParticle;
            player.balls.Add(ball);
            _ballCount++;
        }
    }


    private void SetBlockStartPosition()
    {
        //todo Be camera in member variable 
        if (cam != null)
        {
            var worldRect = cam.ViewportToWorldPoint(new Vector3(1, 1));
            var contentWidth = currentStage.BoxScale.x * currentStage.maxCol;

            var startPosition = new Vector2(
                x: -contentWidth * currentStage.BoxScale.x + currentStage.BoxScale.x * 0.5f,
                y: worldRect.y * 1 * 0.5f); //set 3 / 4
            currentStage.SetStartPosition(startPosition);
        }
    }


    private void MakeWalls()
    {
        if (cam == null) return;

        var worldRect = cam.ViewportToWorldPoint(new Vector3(1, 1));
        var width = worldRect.x * 2;
        var height = worldRect.y * 2;

        var widths = new[] { 1, width, 1, width };
        var heights = new[] { height, 1, height, 1 };

        //position 0이어야 Transform.scale: 1 일 때, -0.5,+0.5 씩 확장함.
        var startXList = new[] { worldRect.x, 0, -worldRect.x, 0 };
        var startYList = new[] { 0, -worldRect.y, 0, worldRect.y };
        //유니티 좌표계 (x, y) 원점은 정가운데, 1사분면 : (+,+), 2사분면 : (-, +), 3: (-, -), 4:(+, -)
        var dx = new[] { 1, 0, -1, 0 };
        var dy = new[] { 0, -1, 0, 1 };
        // list에 담긴 벽들의 정보
        // [0]  오른쪽 수직선
        // [1]  아래쪽 수평선
        // [2]  왼쪽 수직선
        // [3]  위쪽 수평선  
        for (var i = 0; i < startXList.Length; i++)
        {
            Vector2 position = new(
                x: startXList[i] + widths[i] * 0.5f * dx[i], // 너비의 절반 이동해서 피봇값 상쇄 offsetX
                y: startYList[i] + heights[i] * 0.5f * dy[i]); // 높이의 절반 이동해서 피봇값 상쇄 offsetY 
            var go = Instantiate(wallPrefab, position, Quaternion.identity);
            go.transform.localScale = new Vector2(widths[i], heights[i]);
        }

        float baseY = startYList[1];
        gameOverLine = baseY + heights[1] * 0.5f * dy[1] * -1;
    }

    private void InstantiateCharacter()
    {
        // var characterType  =  CharacterType.
        var halfHeight = cam.ViewportToWorldPoint(new Vector2(1, 1)).y;
        var y = halfHeight * 0.7f * -1;
        player = Instantiate(playerPrefab, new Vector3(0, y, 0), Quaternion.identity)
            .GetComponent<Player>();
    }

    private void ShowParticle(Vector2 position)
    {
        var particle = Instantiate(ballParticle);
        particle.transform.position = position;
    }

    private void OnBlockMoved(Vector2 position)
    {
        if (!isPlaying) return;
        var playerTransform = player.gameObject.transform;
        if (position.y <= playerTransform.position.y - playerTransform.localScale.y * 0.5f)
        {
            GameOver();
        }
    }

    public void SatelliteEffect()
    {
        var idx = UnityEngine.Random.Range(0, player.balls.Count);
        var pivot = player.balls[idx];
        if (pivot == null) return;
        var reinforceLevel = animalData
            .AnimalReinforceData
            .Find(it => it.animalType == AnimalType.BlackCat)
            .reinforceLevel;
        var count = reinforceCoef.satelliteCountPerLevel * reinforceLevel;
        for (int i = 0; i < count; i++)
        {
            var satellite = _satellitePool.Pull();
            satellite.SetLastingTime(3f);
            satellite.Attack = reinforceCoef.satelliteAtkPerLevel * reinforceLevel;
            _satellites.Add(satellite);
            satellite.Radian = (Mathf.PI / 4) * i;
            satellite.Pivot = pivot.transform;
        }
    }

    public void BeagleTime()
    {
        var beagleLevel = animalData.AnimalReinforceData
            .Find(it => it.animalType == AnimalType.Beagle)
            .reinforceLevel;

        var count = beagleLevel * reinforceCoef.beagleCountPerLevel;
        var worldPoint = cam.ViewportToWorldPoint(new Vector3(1, 1));
        var x = worldPoint.x;
        var y = worldPoint.y;

        for (int i = 0; i < count; i++)
        {
            var randomY = Random.Range(-y, y);
            var beagle = _beaglePool.Pull(new Vector3(-x, randomY));
            beagle.attack = beagleLevel * reinforceCoef.beagleAtkPerLevel;
            _beagles.Add(beagle.gameObject);
        }

        StartCoroutine(BeagleMovement(_beagles, seconds: beagleLevel * 0.5f));
    }

    private IEnumerator BeagleMovement(List<GameObject> beagles, float seconds)
    {
        var time = 0f;

        while (seconds >= time && !IsStageClear)
        {
            time += Time.deltaTime;
            foreach (var beagle in beagles)
            {
                if (IsStageClear) break;

                var beagleTransform = beagle.transform;
                beagleTransform.position += new Vector3(1, 0) * Time.deltaTime * 5f;
            }

            yield return new WaitForNextFrameUnit();
        }

        foreach (var beagle in beagles)
        {
            beagle.SetActive(false);
        }
    }

    public void DecreaseBallCount()
    {
        _ballCount--;
    }
}