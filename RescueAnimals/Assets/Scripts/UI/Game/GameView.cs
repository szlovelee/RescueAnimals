using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public event Action OnGameStart;
    public event Action OnGamePause;
    public event Action OnGameResume;
    public event Action OnGameOver;
    public event Action OnHomeClicked;
    public event Action OnRestartClicked;


    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public GameObject GameUI;
    public GameObject Boost;
    public Transform Rank;
    public GameObject rankPrefab;

    public Image Timer;

    public List<Sprite> Background;
    public SpriteRenderer BackgroundSprite;

    public Text ScoreTxt;
    public Text CoinTxt;
    public Text StageTxt;
    public Text FinalScoreTxt;
    public Text FinalCoinTxt;

    public GameObject ClearMessage;
    public Text NextMessage;

    private void Start()
    {
        CallGameStart();
    }
    void CallGameStart()
    {
        OnGameStart?.Invoke();
    }

    public void CallGamePause()
    {
        OnGamePause?.Invoke();
    }


    public void CallGameResume()
    {
        OnGameResume?.Invoke();
    }

    public void CallGameOver()
    {
        OnGameOver?.Invoke();
    }

    public void CallHomeClicked()
    {
        OnHomeClicked?.Invoke();
    }

    public void CallRestartClicked()
    {
        OnRestartClicked?.Invoke();
    }
}
