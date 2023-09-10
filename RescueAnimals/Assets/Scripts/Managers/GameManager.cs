using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class GameManager : MonoBehaviour
{
    public Stage currentStage;
    public int score;


    public void ChangeStage(Stage stage)
    {
        currentStage = stage;
    }
}
