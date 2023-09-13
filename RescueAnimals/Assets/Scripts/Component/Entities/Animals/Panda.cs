using System.Collections;
using System.Collections.Generic;
using Component.Entities.Database;
using UnityEngine;
using EnumTypes;

public class Panda : Animal, IAnimalBehaviour
{
    private float _spawnTime = 100f;

    // Temp!!!
    [SerializeField] private AssistPanda assistPandaPrefab;
    [SerializeField] private Coefficient coef;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        for (int i = 0; i < reinforceLevel * coef.pandaCountPerLevel; i++)
        {
            AssistPanda newPanda = Instantiate(assistPandaPrefab);
            newPanda.transform.position = GameManager.Instance.player.transform.position;
            newPanda.SetAssistTime(_spawnTime + (3f));
        }
    }
}