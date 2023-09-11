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
    public event Action<AnimalType> OnBeagleReinforce;

    public GameObject upgradePanel;
    public GameObject rankPanel;
    public GameObject playerPanel;
    public Text coin;

    public GameObject[] panels;

    [Header("Beagle")]
    public Text beagleLevelText;
    public Text beaglePriceText;
    public Text beagleExplanationText;

    private void Start()
    {
        panels = new GameObject[] { upgradePanel, rankPanel, playerPanel };
    }

    private void Update()
    {
        coin.text = "2000";  //Retreive information from the GameManager
    }

    public void CallBeagleReinforce()
    {
        OnBeagleReinforce?.Invoke(AnimalType.Beagle);
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
}
