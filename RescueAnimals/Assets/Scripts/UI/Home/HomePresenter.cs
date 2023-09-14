using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using EnumTypes;
using TMPro;
using Component.Entities;
using Component.Entities.Database;

public class HomePresenter : MonoBehaviour
{
    public AnimalData reinforceData;
    public RankSystem rankSystem;

    private SaveData _savedData;
    [SerializeField] private GameObject _viewObj;
    private HomeView _view;
    public Coefficient coefficient;


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
        _view.OnGuideOff += GuideOff;
        _view.OnAnimalReinforce += ReinforceAnimal;
        _view.OnReinforcePlayerAtk += ReinforcePlayerAtk;
        _view.OnReinforcePlayerSpd += ReinforcePlayerSpd;

        LoadSaveData();
    }

    private void SetCurrentAnimalReinforceState()
    {
        foreach (var data in reinforceData.AnimalReinforceData)
        {
            ShowReinforceState(data);
        }
    }

    private void ShowReinforceState(AnimalReinforce data)
    {
        switch (data.animalType)
        {
            case AnimalType.Retreiver:
                _view.retreiverNotActivePanel.SetActive(!data.isActive);
                _view.retreiverLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.retreiverPriceText.text = $"${data.reinforcePrice}";
                _view.retreiverExplanationText.text =
                    $"보너스 공을 {data.reinforceLevel * coefficient.ballCloneCountPerLevel}개 만큼 생성!";
                break;
            case AnimalType.Panda:
                _view.pandaNotActivePanel.SetActive(!data.isActive);
                _view.pandaLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.pandaPriceText.text = $"${data.reinforcePrice}";
                _view.pandaExplanationText.text =
                    $"{5 + (data.reinforceLevel * data.bonusStatRate)}초 동안 도와주는 판다 {data.reinforceLevel * coefficient.pandaCountPerLevel}마리를 부름!";
                break;
            case AnimalType.Dragon:
                _view.dragonNotActivePanel.SetActive(!data.isActive);
                _view.dragonLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.dragonPriceText.text = $"${data.reinforcePrice}";
                _view.dragonExplanationText.text =
                    $"모든 블록에 데미지 -{data.reinforceLevel * coefficient.dragonBreathAtkPerLevel} 입힘!";
                break;
            case AnimalType.BlackCat:
                _view.catNotActivePanel.SetActive(!data.isActive);
                _view.catLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.catPriceText.text = $"${data.reinforcePrice}";
                _view.catExplanationText.text =
                    $"위성 공 {data.reinforceLevel * coefficient.satelliteCountPerLevel}개 생성 & 공격력 + {data.reinforceLevel * coefficient.satelliteAtkPerLevel} 증가!";
                break;
            case AnimalType.Beagle:
                _view.beagleNotActivePanel.SetActive(!data.isActive);
                _view.beagleLevelText.text = $"레벨 {data.reinforceLevel}";
                _view.beaglePriceText.text = $"${data.reinforcePrice}";
                _view.beagleExplanationText.text =
                    $"비글 친구들 {data.reinforceLevel * coefficient.beagleCountPerLevel}마리가 공격력 {data.reinforceLevel * coefficient.beagleAtkPerLevel}으로 함께 공격!";
                break;
        }
    }

    private void ReinforceAnimal(AnimalType animalType)
    {
        var data = reinforceData.AnimalReinforceData.Find(x => x.animalType == animalType);
        var price = data.reinforcePrice * data.reinforceLevel * 1.5f;
        if (_savedData.Gold < price)
        {
            ShowGuide();
            return;
        }

        data.reinforceLevel += 1;
        _savedData.Gold -= data.reinforcePrice;
        UpdateCoin(_savedData.Gold);
        ShowReinforceState(data);
        var player = GameManager.Instance.player;
        player.atk = _savedData.Atk;
        player.exp = _savedData.Exp;
        player.gold = _savedData.Gold;
        player.level = _savedData.Level;
        DataManager.Instance.SavePlayer(player, reinforceData, rankSystem.rankings);
    }

    private void SetCurrentPlayerState()
    {
        _view.playerAtkText.text = $"({_savedData.Atk} / 10)";
        _view.playerAtkSlider.value = _savedData.Atk * 0.1f;
    }

    private void ReinforcePlayerAtk()
    {
        if (_savedData.Gold < 1)
        {
            ShowGuide();

            return;
        }

        _savedData.Atk += 1;
    }

    private void ReinforcePlayerSpd()
    {
        if (_savedData.Gold < 1)
        {
            ShowGuide();

            return;
        }

        // No Speed Data
        //_savedData. += 1;
    }

    private void LoadGame()
    {
        DataManager.Instance.SavePlayer(_savedData);

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
        SetCurrentPlayerState();

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

    private void ShowGuide()
    {
        _view.guide.SetActive(true);
    }

    private void GuideOff()
    {
        _view.guide.SetActive(false);
    }

    private void LoadSaveData()
    {
        _savedData = DataManager.Instance.LoadPlayerInfo(reinforceData);

        _view.coin.text = _savedData.Gold.ToString();
        Debug.Log($"UpdateCoin, {_savedData.Gold}");
        Debug.Log($"UpdateAtk, {_savedData.Atk}");
    }

    private void UpdateCoin(int setCoin)
    {
        _savedData.Gold = setCoin;
        _view.coin.text = _savedData.Gold.ToString();
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