using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class StaffyDog : Animal, IAnimalBehaviour
{
    

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }
    public void OnResqueEffect()
    {

    }
}
