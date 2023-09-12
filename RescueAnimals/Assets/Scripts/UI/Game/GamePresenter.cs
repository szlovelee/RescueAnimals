using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameObject _viewObj;
    GameView _view;

    void Awake()
    {
        _view = _viewObj.GetComponent<GameView>();

        _view.OnGameStart += GameStart;
        _view.OnGamePause += PauseGame;
        _view.OnGameResume += ResumeGame;
        _view.OnGameOver += OpenGameOverPanel;
        _view.OnHomeClicked += ChangeToHomeScene;
        _view.OnRestartClicked += Restart;

        GameManager.Instance.OnGameEnd += GameOver;
        GameManager.Instance.OnScoreAdded += UpdateScoreUI;
        GameManager.Instance.OnScoreAdded += UpdateCoinUI;
        GameManager.Instance.OnStageClear += StageClearUI;

    }

    void GameStart()
    {
        ActivateUIElement(_view.GameUI);
        //if (true) ActivateUIElement(_view.Boost); // additional condition for boost needed;
        GameManager.Instance.CallGameStart();
    }

    void GameOver()
    {
        _view.CallGameOver();
    }

    void PauseGame()
    {
        GameManager.Instance.GamePause();
        ActivateUIElement(_view.PausePanel);
        SoundManager.instance.PauseBGM();
    }

    void ResumeGame()
    {
        GameManager.Instance.GameResume();
        DeactivateUIElement(_view.PausePanel);
        SoundManager.instance.ResumeBGM();
    }

    void OpenGameOverPanel()
    {
        Debug.Log("GameOverPanel Called");
        _view.FinalScoreTxt.text = GameManager.Instance.score.ToString();
        _view.FinalCoinTxt.text = GameManager.Instance.coin.ToString();
        ActivateUIElement(_view.GameOverPanel);
    }

    void Restart()
    {
        LoadTargetScene("GameScene");
    }

    void ChangeToHomeScene()
    {
        LoadTargetScene("HomeScene");
    }

    void UpdateScoreUI()
    {
        _view.ScoreTxt.text = GameManager.Instance.score.ToString();
    }

    void UpdateCoinUI()
    {
        _view.CoinTxt.text = GameManager.Instance.coin.ToString();
    }

    void StageClearUI()
    {
        _view.StageTxt.text = string.Format($"stage {GameManager.Instance.currentStage.stageNum}");
        StartCoroutine(PauseRoutine(3f));
    }

    void ActivateUIElement(GameObject obj)
    {
        obj.SetActive(true);
        SoundManager.instance?.PlayClickEffect();
    }

    void DeactivateUIElement(GameObject obj)
    {
        obj.SetActive(false);
        SoundManager.instance.PlayReturnEffect();
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
        GameManager.Instance.currentStage.ClearMap();
        GameManager.Instance.GamePause();

        ActivateUIElement(_view.ClearMessage);

        StartCoroutine(BlinkTextRoutine(duration));

        // duration 동안 대기
        float pauseEndTime = Time.realtimeSinceStartup + duration;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        DeactivateUIElement(_view.ClearMessage);

        GameManager.Instance.StartStage();

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
