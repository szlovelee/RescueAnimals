using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumTypes;
using Component.Entities.Database;

public class Dragon : Animal, IAnimalBehaviour
{
    private float _bombScale = 30f;
    private float _bombDamage = 1f;

    [SerializeField] private ParticleSystem _bombEffect;
    [SerializeField] private Coefficient coef;
    [SerializeField] private AnimalData animalData;
    ParticleSystem bomb;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        bomb = Instantiate(_bombEffect);
        var dragonLevel = animalData.AnimalReinforceData
           .Find(it => it.animalType == AnimalType.Dragon)
           .reinforceLevel;
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _bombScale);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Block"))
            {
                var block = hit.gameObject.GetComponent<Block>();
                block.GetDamaged(dragonLevel * coef.dragonBreathAtkPerLevel);
            }
            else if (hit.CompareTag("Animal"))
            {
                var animal = hit.gameObject.GetComponent<Animal>();
                animal.GetDamaged(dragonLevel * coef.dragonBreathAtkPerLevel);
            }
        }

        bomb.Stop();
        var mainModule = bomb.main;
        bomb.transform.localScale *= 5f;
        mainModule.duration = 3f;
        bomb.Play();
    }
}