using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using EnumTypes;
using TMPro;
using Component.Entities;

public class HomePresenter : MonoBehaviour
{
    public AnimalData reinforceData;
    public RankSystem rankSystem;

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
        _view.OnBeagleReinforce += ReinforceAnimal;

        UpdateCoin();
    }

    private void SetCurrentAnimalReinforceState()
    {
        foreach(var data in reinforceData.AnimalReinforceData)
        {
            ShowReinforceState(data);
        }
    }

    private void ShowReinforceState(AnimalReinforce data)
    {
        switch (data.animalType)
        {
            case AnimalType.Beagle:
                _view.beagleNotActivePanel.SetActive(!data.isActive);
                _view.beagleLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.beagleExplanationText.text = $"공이 {data.reinforceLevel * data.bonusStatRate}초 동안 관통!";
                break;
            case AnimalType.Panda:
                _view.pandaNotActivePanel.SetActive(!data.isActive);
                _view.pandaLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.pandaExplanationText.text = $"공이 {data.reinforceLevel * data.bonusStatRate}초 동안 관통!";
                break;
            case AnimalType.Dragon:
                _view.dragonNotActivePanel.SetActive(!data.isActive);
                _view.dragonLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.dragonExplanationText.text = $"공이 {data.reinforceLevel * data.bonusStatRate}초 동안 관통!";
                break;
        }
    }

    private void ReinforceAnimal(AnimalType animalType)
    {
        var data = reinforceData.AnimalReinforceData.Find(x=>x.animalType == animalType);

        data.reinforceLevel += 1;
        ShowReinforceState(data);
    }

    private void LoadGame()
    {
        StartCoroutine(LoadGameSceneAsync());
        SoundManager.instance.PlayAcceptEffect();
    }
    
    private void UpgradePanelOpen()
    {
        SetCurrentAnimalReinforceState();
        ActivatePanel(_view.panels, _view.upgradePanel);
    }

    private void RankPanelOpen()
    {
        rankSystem.CreateRankUI(_view.rankPrefab, _view.Rank);
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

        foreach (Transform child in _view.Rank)
        {
            Destroy(child.gameObject);
        }
        SoundManager.instance.PlayReturnEffect();
    }

    private void UpdateCoin()
    {
        SaveData gameData = DataManager.Instance.LoadPlayerInfo(reinforceData, rankSystem);
        _view.coin.text = gameData.Gold.ToString();
        Debug.Log($"UpdateCoin, {DataManager.Instance.LoadPlayerInfo(reinforceData, rankSystem).Gold}");
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

        SoundManager.instance.PlayClickEffect();
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
