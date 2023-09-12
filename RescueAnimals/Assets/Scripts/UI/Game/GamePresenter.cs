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
    }

    void Start()
    {
        _view.OnGameStart += GameStart;
        _view.OnGamePause += PauseGame;
        _view.OnGameResume += ResumeGame;
        _view.OnGameOver += OpenGameOverPanel;
        _view.OnHomeClicked += ChangeToHomeScene;
        _view.OnRestartClicked += Restart;
    }

    void GameStart()
    {
        ActivateUIElement(_view.GameUI);
        if (true) ActivateUIElement(_view.Boost); // additional condition for boost needed;
        GameManager.Instance.CallGameStart();
    }

    void PauseGame()
    {
        // pause logic
        ActivateUIElement(_view.PausePanel);
        SoundManager.instance.PauseBGM();
    }

    void ResumeGame()
    {
        // resume logic
        DeactivateUIElement(_view.PausePanel);
        SoundManager.instance.ResumeBGM();
    }

    void OpenGameOverPanel()
    {
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

    void ActivateUIElement(GameObject obj)
    {
        obj.SetActive(true);
        SoundManager.instance.PlayClickEffect();
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

}
