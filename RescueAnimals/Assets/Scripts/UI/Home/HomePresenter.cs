using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using EnumTypes;

public class HomePresenter : MonoBehaviour
{
    public AnimalData reinforceData;

    [SerializeField] private GameObject _viewObj;
    private HomeView _view;

    private void Awake()
    {
        _view = _viewObj.GetComponent<HomeView>(); 
    }
    private void Start()
    {
        SetCurrentAnimalReinforceState();

        _view.OnGameStartClicked += LoadGame;
        _view.OnUpgradeOpen += UpgradePanelOpen;
        _view.OnRankOpen += RankPanelOpen;
        _view.OnPlayerOpen += PlayerPanelOpen;
        _view.OnPanelClose += PanelClose;
        _view.OnBeagleReinforce += ReinforceAnimal;
    }

    private void SetCurrentAnimalReinforceState()
    {
        foreach(var data in reinforceData.AnimalReinforceData)
        {
            ShowReinforceState(data.animalType, data.reinforceLevel);
        }
    }

    private void ShowReinforceState(AnimalType animalType, int level)
    {
        switch (animalType)
        {
            case AnimalType.Beagle:
                _view.beagleLevelText.text = $"레벨 {level}";
                _view.beagleExplanationText.text = $"공이 {level * 0.2f}초 동안 관통!";
                break;
        }
    }

    private void ReinforceAnimal(AnimalType animalType)
    {
        var data = reinforceData.AnimalReinforceData.Find(x=>x.animalType == animalType);

        data.reinforceLevel += 1;
        ShowReinforceState(data.animalType, data.reinforceLevel);
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
