using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Retreiver : Animal, IAnimalBehaviour
{
    private int _ballCount = 1;

    // Temporary
    [SerializeField]
    private Ball ballPrefab;
    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }
    public void OnResqueEffect()
    {
        GameManager.Instance.player.balls.AddRange(CreateNewBall());
        //player.balls.Add(CreateNewBall());
    }

    public List<Ball> CreateNewBall()
    {
        List<Ball> newBalls = new List<Ball>();
        int ballCount = _ballCount + (reinforceLevel - 1);

        for (int i = 0; i < ballCount; i++)
        {
            Ball newBall = Instantiate(ballPrefab);
            newBall.transform.position = this.transform.position;
            newBall.SetBonusBall();

            newBalls.Add(newBall);
        }

        return newBalls;
    }
}
