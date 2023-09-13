using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Retreiver : Animal, IAnimalBehaviour
{
    private int BallCount => 1 + (reinforceLevel - 1);

    // Temporary
    [SerializeField] private Ball ballPrefab;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        GameManager.Instance.AddBalls(transform.position, BallCount);
    }
}