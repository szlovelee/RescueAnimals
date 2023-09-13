using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Entities;
using EnumTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Stage currentStage;
    public int score;
    public int coin;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    private Camera cam;

    public event Action OnGameStart;
    public event Action OnStageClear;
    public event Action OnGameEnd;

    public event Action OnBlockBreak;
    public event Action OnAnimalRescue;
    public event Action OnScoreAdded;

    float ballSpeed = 0f;
    public float gameOverLine = 0f;
    Vector2 ballPos = new Vector2(0, -2.8f);
    bool isPlaying = true;
    int addedScore;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            cam = Camera.main;
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

        OnBlockBreak += AddBlockPoint;
        OnAnimalRescue += AddAnimalPoint;
        OnScoreAdded += ScoreCheck;

        OnGameEnd += ResetBall;
        OnGameEnd += GamePause;
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (player.balls.Count == 0 && scene.name == "GameScene" && isPlaying)
        {
            CallGameEnd();
            isPlaying = false;
        }
    }

    private void CreateBall()
    {
        Ball newBall = Instantiate(ballPrefab, ballPos, Quaternion.identity).GetComponent<Ball>();
        player.balls.Add(newBall);
    }

    private void SetGame()
    {
        Time.timeScale = 1f;
        InstantiateCharacter();
        CreateBall();
        MakeWalls();
        SetBlockStartPosition();
        currentStage.ResetStage();
        currentStage.InstantiateObjects();
        score = 0;
        isPlaying = true;
    }

    public void CallGameStart()
    {
        OnGameStart?.Invoke();
    }

    public void CallStageClear()
    {
        OnStageClear?.Invoke();
    }

    public void CallGameEnd()
    {
        //if (!DataManager.Instance.IsWrite)
        //{
        //    DataManager.Instance.SavePlayer(player);
        //}

        OnGameEnd?.Invoke();
    }

    public void CallBlockBreak()
    {
        OnBlockBreak?.Invoke();
    }

    public void CallAnimalRescue()
    {
        OnAnimalRescue?.Invoke();
    }

    public void CallScoreAdded()
    {
        OnScoreAdded?.Invoke();
    }

    private void AddBlockPoint()
    {
        score += 10;
        coin += 2;
        addedScore += 10;
        CallScoreAdded();
    }

    private void AddAnimalPoint()
    {
        score += 50;
        coin += 20;
        addedScore += 50;
        CallScoreAdded();
    }

    private void ScoreCheck()
    {
        if (addedScore > 10 + currentStage.stageNum)    // goal score for stage clear should be set
        {
            currentStage.UpdateStageSettings();
            CallStageClear();
            SoundManager.instance.PlayStageClear();
            ResetBall();
            addedScore = 0;
        }
    }

    private void ResetBall()
    {
        while (player.balls.Count > 1) 
        {
            Destroy(player.balls[0].gameObject);
            player.balls.RemoveAt(0);
        }
    }

    public void StartStage()
    {
        GameResume();
        currentStage.InstantiateObjects();
    }

    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        Time.timeScale = 1;
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

        Debug.Log("WallCreated");
    }

    private void InstantiateCharacter()
    {
        // var characterType  =  CharacterType.
        var halfHeight = cam.ViewportToWorldPoint(new Vector2(1, 1)).y;
        var y = halfHeight * 0.7f * -1;
        player = Instantiate(playerPrefab, new Vector3(0, y, 0), Quaternion.identity)
            .GetComponent<Player>();
    }
}