using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


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
        _view.OnUpgradeOpen += UpgradePanelOpen;
        _view.OnRankOpen += RankPanelOpen;
        _view.OnPlayerOpen += PlayerPanelOpen;
        _view.OnPanelClose += PanelClose;
    }

    private void LoadGame()
    {
        StartCoroutine(LoadGameSceneAsync());
    }
    
    private void UpgradePanelOpen()
    {
        ActivatePanel(_view.panels, _view.upgradePanel);
    }

    private void RankPanelOpen()
    {
        ActivatePanel(_view.panels, _view.rankPanel);
    }

    private void PlayerPanelOpen()
    {
        ActivatePanel(_view.panels, _view.playerPanel);
    }

    private void PanelClose()
    {
        foreach (var panel in _view.panels)
        {
            panel.SetActive(false);
        }
    }

    private void ActivatePanel(GameObject[] panels, GameObject targetPanel)
    {
        foreach (var panel in panels)
        {
            if (panel == targetPanel)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
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
