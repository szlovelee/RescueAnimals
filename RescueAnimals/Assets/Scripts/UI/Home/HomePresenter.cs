using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HomePresenter : MonoBehaviour
{
    [SerializeField] private GameObject _viewObj;
    private HomeView _view;

    private void Awake()
    {
        _view = _viewObj.GetComponent<HomeView>(); 
    }
    private void Start()
    {
        _view.OnGameStartClicked += LoadGame;
    }

    private void LoadGame()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync()
    {
        var oper = SceneManager.LoadSceneAsync("GameScene");
        while (!oper.isDone)
        {
            yield return null;
        }
    }
}
