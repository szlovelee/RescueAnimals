using System;
using Entities;
using UnityEngine;
using Util;

public class GameManager : Singleton<GameManager>
{
    public Stage currentStage;
    public int score;
    
    
    public void ChangeStage(Stage stage)
    {
        currentStage = stage;
    }

    private void Start()
    {
        currentStage.InstantiateObjects();
    }
}