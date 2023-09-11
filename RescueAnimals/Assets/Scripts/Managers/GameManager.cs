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
        SetBlockStartPosition();
        currentStage.InstantiateObjects();
    }

    private void SetBlockStartPosition()
    {
        //todo Be camera in member variable 
        Camera camera = Camera.main;
        if (camera == null) return;
        var worldRect = camera.ViewportToWorldPoint(new Vector3(1, 1));
        var contentWidth = 0.5f * currentStage.maxCol;
        
        var startPosition = new Vector2(
            x: -contentWidth * 0.5f + 0.25f,
            y: worldRect.y * 1 * 0.5f);
        currentStage.SetStartPosition(startPosition);
    }
}