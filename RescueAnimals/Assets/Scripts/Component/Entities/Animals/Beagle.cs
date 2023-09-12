using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Beagle : Animal, IAnimalBehaviour
{
    private float _reinforceTime = 3f;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }
    public void OnResqueEffect()
    {
        Player player = GameManager.Instance.player;

        for(int i = 0; i < player.balls.Count; i++)
        {
            player.balls[i].OnPiercingMode(true, _reinforceTime + (reinforceLevel * 0.2f));
        }
    }
}
