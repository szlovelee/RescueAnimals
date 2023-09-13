using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Component.Entities;
using Entities;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

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
    [SerializeField] private AnimalData animalData;
    [SerializeField] private ParticleSystem ballParticle;


    private Camera cam;

    public event Action OnStageClear;
    public event Action OnGameEnd;
    public event Action OnScoreAdded; // todo make Action<int> send score value to presenter 

    private SinglePrefabObjectPool<Ball> _ballObjectPool;
    float ballSpeed = 0f;
    public float gameOverLine = 0f;
    private Vector2 ballPos = new Vector2(0, -2.8f);

    private bool isPlaying = true;
    private int addedScore;

    private bool IsStageClear => addedScore > 1000 + currentStage.stageNum || currentStage.aliveCount <= 0;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            cam = Camera.main;
            _ballObjectPool = new(prefab: ballPrefab, 1);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SetGame();
    }

    private void Update()
    {
        if (IsGameOver())
        {
            GameOver();
        }
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
        isPlaying = false;
        UpdateRank();
        OnGameEnd?.Invoke();
        DataManager.Instance.SavePlayer(player, animalData, Rank);
    }

    private bool IsGameOver()
    {
        Scene scene = SceneManager.GetActiveScene();
        return player.balls.Count == 0 && scene.name == "GameScene" && isPlaying;
    }


    private void CreateBall()
    {
        //todo ball make pool-able
        Ball newBall = Instantiate(ballPrefab, ballPos, Quaternion.identity).GetComponent<Ball>();
        player.balls.Add(newBall);
    }

    private void SetGame()
    {
        currentStage.Initialize();
        Time.timeScale = 1f;
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
        if (IsStageClear)
        {
            ResetBall();
            currentStage.StageClear();
            addedScore = 0;
            SoundManager.instance.PlayStageClear();
            OnStageClear?.Invoke();
        }
    }

    private void ResetBall()
    {
        for (int i = 0; i < player.balls.Count; i++)
        {
            player.balls[i].OnBallCollide -= ShowParticle;
            player.balls[i].gameObject.SetActive(false);
        }

        player.balls.Clear();
        CreateBall();
    }

    private void UpdateRank()
    {
        Rank rank = new Rank(score, currentStage.stageNum);
        Rank.AddRank(rank);
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
        for (int i = 0; i < ballCount; i++)
        {
            var ball = _ballObjectPool.Pull();
            ball.transform.position = position;
            ball.SetBonusBall();
            ball.OnBallCollide += ShowParticle;
            player.balls.Add(ball);
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
}