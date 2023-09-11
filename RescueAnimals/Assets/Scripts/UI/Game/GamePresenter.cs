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
    }

    void PauseGame()
    {
        // pause logic
        ActivateUIElement(_view.PausePanel);
    }

    void ResumeGame()
    {
        // resume logic
        DeactivateUIElement(_view.PausePanel);
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
    }

    void DeactivateUIElement(GameObject obj)
    {
        obj.SetActive(false);
    }

    void LoadTargetScene(string sceneName)
    {
        StartCoroutine(LoadTargetSceneAsync(sceneName));
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
