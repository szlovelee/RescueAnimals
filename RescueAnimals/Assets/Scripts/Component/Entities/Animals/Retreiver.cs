using System;
using System.Collections;
using System.Collections.Generic;
using Component.Entities.Database;
using UnityEngine;
using EnumTypes;

public class Retreiver : Animal, IAnimalBehaviour
{
    private int BallCount => 1 + (reinforceLevel - 1) * coef.ballCloneCountPerLevel;
    [SerializeField] private AnimalData reinforceData;
    [SerializeField] private Coefficient coef;

    private void Start()
    {
        
    }

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        reinforceLevel = reinforceData
            .AnimalReinforceData
            .Find(data => data.animalType == AnimalType.Retreiver)
            .reinforceLevel;
        GameManager.Instance.AddBalls(transform.position, BallCount);
    }
}