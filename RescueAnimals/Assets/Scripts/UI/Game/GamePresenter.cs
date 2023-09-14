using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameObject _viewObj;
    GameView _view;

    GameManager gameManager;

    float regenTime;
    float remainingTime;

    void Awake()
    {
        _view = _viewObj.GetComponent<GameView>();

        _view.OnGameStart += GameStart;
        _view.OnGamePause += PauseGame;
        _view.OnGameResume += ResumeGame;
        _view.OnGameOver += OpenGameOverPanel;
        _view.OnHomeClicked += ChangeToHomeScene;
        _view.OnRestartClicked += Restart;

        gameManager = GameManager.Instance;
        gameManager.OnGameEnd += GameOver;
        gameManager.OnScoreAdded += UpdateScoreUI;
        gameManager.OnScoreAdded += UpdateCoinUI;
        gameManager.OnStageClear += StageClearUI;
        gameManager.OnStageClear += ChangeBackground;
    }

    void Update()
    {
        regenTime = gameManager.currentStage.BricksGenTime;
        remainingTime = regenTime - gameManager._lastTimeRegenerateBlock;
        float fillAmount = remainingTime / regenTime;
        _view.Timer.fillAmount = fillAmount;

        if (fillAmount < 0.3f)
        {
            _view.Timer.color = new Color32(255, 0, 31, 174);
        }
        else
        {
            _view.Timer.color = new Color32(204, 255, 153, 174);
        }
    }


    void GameStart()
    {
        ActivateUIElement(_view.GameUI);
        //if (true) ActivateUIElement(_view.Boost); // additional condition for boost needed;
    }

    void GameOver()
    {
        _view.CallGameOver();
        gameManager.Rank.CreateRankUI(_view.rankPrefab, _view.Rank);
    }

    void PauseGame()
    {
        gameManager.GamePause();
        ActivateUIElement(_view.PausePanel);
        SoundManager.instance.PlayClickEffect();
        SoundManager.instance.PauseBGM();
    }

    void ResumeGame()
    {
        gameManager.GameResume();
        DeactivateUIElement(_view.PausePanel);
        SoundManager.instance.PlayReturnEffect();
        SoundManager.instance.ResumeBGM();
    }

    void OpenGameOverPanel()
    {
        Debug.Log("GameOverPanel Called");
        _view.FinalScoreTxt.text = gameManager.score.ToString();
        _view.FinalCoinTxt.text = gameManager.coin.ToString();
        ActivateUIElement(_view.GameOverPanel);
        SoundManager.instance.PlayGameOver();
        SoundManager.instance.StopBGM();

    }

    void ChangeBackground()
    {
        int stage = gameManager.currentStage.stageNum;
        if (stage < 5) _view.BackgroundSprite.sprite = _view.Background[0];
        else if (stage < 10) _view.BackgroundSprite.sprite = _view.Background[1];
        else if (stage < 15) _view.BackgroundSprite.sprite = _view.Background[2];
        else if (stage < 20) _view.BackgroundSprite.sprite = _view.Background[3];
        else if (stage < 25) _view.BackgroundSprite.sprite = _view.Background[4];
        else if (stage < 30) _view.BackgroundSprite.sprite = _view.Background[5];
        else if (stage < 35) _view.BackgroundSprite.sprite = _view.Background[6];
        else _view.BackgroundSprite.sprite = _view.Background[6];
        Debug.Log($"stage: {stage}");
    }

    void Restart()
    {
        LoadTargetScene("GameScene");
        SoundManager.instance.PlayAcceptEffect();
    }

    void ChangeToHomeScene()
    {
        gameManager.SaveData();
        LoadTargetScene("HomeScene");
        SoundManager.instance.PlayAcceptEffect();
    }

    void UpdateScoreUI()
    {
        _view.ScoreTxt.text = gameManager.score.ToString();
    }

    void UpdateCoinUI()
    {
        _view.CoinTxt.text = gameManager.coin.ToString();
    }

    void StageClearUI()
    {
        
        StartCoroutine(PauseRoutine(0.5f));
    }

    void ActivateUIElement(GameObject obj)
    {
        obj.SetActive(true);
    }

    void DeactivateUIElement(GameObject obj)
    {
        obj.SetActive(false);
    }

    void LoadTargetScene(string sceneName)
    {
        StartCoroutine(LoadTargetSceneAsync(sceneName));
        SoundManager.instance.PlayAcceptEffect();
    }

    private IEnumerator LoadTargetSceneAsync(string sceneName)
    {
        var oper = SceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator PauseRoutine(float duration)
    {
        gameManager.currentStage.ClearMap();
        gameManager.GamePause();

        ActivateUIElement(_view.ClearMessage);

        StartCoroutine(BlinkTextRoutine(duration));

        // duration 동안 대기
        float pauseEndTime = Time.realtimeSinceStartup + duration;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        DeactivateUIElement(_view.ClearMessage);
        _view.StageTxt.text = string.Format($"stage {gameManager.currentStage.stageNum}");
        gameManager.StartStage();
    }
    private IEnumerator BlinkTextRoutine(float duration)
    {
        Color32 clearColor = new Color32(21, 217, 171, 0);
        Color32 defaultColor = new Color32(21, 217, 171, 225);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            _view.NextMessage.color = defaultColor;
            yield return new WaitForSecondsRealtime(0.5f);
            _view.NextMessage.color = clearColor;
            yield return new WaitForSecondsRealtime(0.5f);

            elapsedTime += 1.0f;
        }

        _view.NextMessage.color = clearColor;   //  원래 컬러로 복구
    }

}
