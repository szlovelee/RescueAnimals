using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;

public class HomeView : MonoBehaviour
{
    public event Action OnGameStartClicked;
    public event Action OnUpgradeOpen;
    public event Action OnRankOpen;
    public event Action OnPlayerOpen;
    public event Action OnPanelClose;
    public event Action OnGuideOff;
    public event Action OnReinforcePlayerAtk;
    public event Action OnReinforcePlayerSpd;
    public event Action<AnimalType> OnAnimalReinforce;

    public GameObject upgradePanel;
    public GameObject rankPanel;
    public GameObject playerPanel;
    public Text coin;

    public GameObject[] panels;

    public Transform Rank;
    public GameObject rankPrefab;

    public GameObject guide;

    [Header("Player")] public Slider playerAtkSlider;
    public Text playerAtkText;

    [Header("Retreiver")] public Text retreiverLevelText;
    public Text retreiverPriceText;
    public Text retreiverExplanationText;
    public GameObject retreiverNotActivePanel;

    [Header("Panda")] public Text pandaLevelText;
    public Text pandaPriceText;
    public Text pandaExplanationText;
    public GameObject pandaNotActivePanel;

    [Header("Dragon")] public Text dragonLevelText;
    public Text dragonPriceText;
    public Text dragonExplanationText;
    public GameObject dragonNotActivePanel;

    [Header("Cat")] public Text catLevelText;
    public Text catPriceText;
    public Text catExplanationText;
    public GameObject catNotActivePanel;

    [Header("Beagle")] public Text beagleLevelText;
    public Text beaglePriceText;
    public Text beagleExplanationText;
    public GameObject beagleNotActivePanel;

    private void Start()
    {
        panels = new GameObject[] { upgradePanel, rankPanel, playerPanel };
    }


    public void CallRetreiverReinforce()
    {
        OnAnimalReinforce?.Invoke(AnimalType.Retreiver);
    }

    public void CallPandaReinforce()
    {
        OnAnimalReinforce?.Invoke(AnimalType.Panda);
    }

    public void CallDragonReinforce()
    {
        OnAnimalReinforce?.Invoke(AnimalType.Dragon);
    }

    public void CallCatReinforce()
    {
        OnAnimalReinforce?.Invoke(AnimalType.BlackCat);
    }

    public void CallBeagleReinforce()
    {
        OnAnimalReinforce?.Invoke(AnimalType.Beagle);
    }


    public void CallPlayerSpdReinforce()
    {
        OnReinforcePlayerAtk?.Invoke();
    }

    public void CallPlayerAtkReinforce()
    {
        OnReinforcePlayerSpd?.Invoke();
    }

    public void CallGameStartClicked()
    {
        OnGameStartClicked?.Invoke();
    }

    public void CallUpgradeOpen()
    {
        OnUpgradeOpen?.Invoke();
    }

    public void CallRankOpen()
    {
        OnRankOpen?.Invoke();
    }

    public void CallPlayerOpen()
    {
        OnPlayerOpen?.Invoke();
    }

    public void CallPanelsClose()
    {
        OnPanelClose?.Invoke();
    }

    public void CallGuideOff()
    {
        OnGuideOff?.Invoke();
    }
}