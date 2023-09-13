using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Panda : Animal, IAnimalBehaviour
{
    private float _spawnTime = 5f;

    // Temp!!!
    [SerializeField]
    private AssistPanda assistPandaPrefab;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }
    public void OnResqueEffect()
    {
        AssistPanda newPanda =  Instantiate(assistPandaPrefab);
        newPanda.transform.position = GameManager.Instance.player.transform.position;
        newPanda.SetAssistTime(_spawnTime + (reinforceLevel * 0.5f));
    }
}
