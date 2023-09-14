using System.Collections;
using System.Collections.Generic;
using Component.Entities.Database;
using UnityEngine;
using EnumTypes;

public class Panda : Animal, IAnimalBehaviour
{
    // Temp!!!
    [SerializeField] private AssistPanda assistPandaPrefab;
    [SerializeField] private Coefficient coef;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        var count = reinforceLevel * coef.pandaCountPerLevel;
        var assistTime = reinforceLevel * coef.pandaCountPerLevel;
        for (int i = 0; i < count; i++)
        {
            AssistPanda newPanda = Instantiate(assistPandaPrefab);
            newPanda.transform.position = GameManager.Instance.player.transform.position;
            newPanda.SetAssistTime((assistTime));
        }
    }
}