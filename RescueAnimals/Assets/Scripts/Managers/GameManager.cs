using Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
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