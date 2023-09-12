using System;
using System.Drawing;
using Entities;
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
    float gameOverLine = 0f;

    GameObject ball;

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
        OnGameStart += SetGame;
        OnStageClear += UpdateStage;

        OnBlockBreak += AddBlockPoint;
        OnAnimalRescue += AddAnimalPoint;
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (ball != null && scene.name == "GameScene")
        {
            if (ball.transform.position.y <= gameOverLine)
            {
                CallGameEnd();
                Destroy(ball);
            }
        }
    }

    private void CreateBall()
    {
        if (ball != null)
        {
            Destroy(ball);
        }

        Vector2 ballPos = new Vector2(0, -3);
        ball = Instantiate(ballPrefab, ballPos, Quaternion.identity);
    }

    private void SetGame()
    {
        score = 0;

        Debug.Log("SetGameCalled");
        CreateBall();

        MakeWalls();
        SetBlockStartPosition();
        currentStage.ResetStage();
        currentStage.InstantiateObjects();
        InstantiateCharacter();
        Debug.Log(ball);
        score = 0;
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


    private void UpdateStage()
    {
        currentStage.InstantiateObjects();
    }

    private void AddBlockPoint()
    {
        score += 10;
        coin += 2;
        CallScoreAdded();
    }

    private void AddAnimalPoint()
    {
        score += 50;
        coin += 20;
        CallScoreAdded();
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
        Instantiate(playerPrefab, new Vector3(0, y, 0), Quaternion.identity);
    }
}