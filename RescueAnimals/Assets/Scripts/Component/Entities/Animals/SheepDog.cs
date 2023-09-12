using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class SheepDog : Animal, IAnimalBehaviour
{
    public void OnResqueMove()
    {
        jailObj.SetActive(false);
    }
    public void OnResqueEffect(Player player)
    {

    }
}
