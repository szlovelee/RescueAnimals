using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
