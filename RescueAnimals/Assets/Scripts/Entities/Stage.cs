using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Stage : MonoBehaviour
{
    public StageType stage;

    public List<Block> bricks;
    public List<Animal> animals;

    [SerializeField]
    private float bricksGenTime;

    public void CalcAnimalPercentage()
    {

    }

    public void CalcBrickPercentage()
    {

    }

    public void OnClearStage()
    {

    }
    
    public void CreateBricks()
    {

    }
}
