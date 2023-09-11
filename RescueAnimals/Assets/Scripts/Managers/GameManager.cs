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
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;
        var worldRect = cam.ViewportToWorldPoint(new Vector3(1, 1));
        var contentWidth = currentStage.BoxScale.x * currentStage.maxCol;
        
        var startPosition = new Vector2(
            x: -contentWidth * currentStage.BoxScale.x + currentStage.BoxScale.x * 0.5f,
            y: worldRect.y * 1 * 0.5f); //set 3 / 4
        currentStage.SetStartPosition(startPosition);
    }
}