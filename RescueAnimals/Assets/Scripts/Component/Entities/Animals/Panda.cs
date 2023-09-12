using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Panda : Animal, IAnimalBehaviour
{
    public void OnResqueMove()
    {
        jailObj.SetActive(false);
    }
    public void OnResqueEffect()
    {

    }
}
