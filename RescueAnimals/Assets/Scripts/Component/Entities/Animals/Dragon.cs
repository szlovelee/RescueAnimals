using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumTypes;

public class Dragon : Animal, IAnimalBehaviour
{
    private float _bombScale = 30f;
    private float _bombDamage = 1f;

    [SerializeField] private ParticleSystem _bombEffect;

    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }

    public void OnResqueEffect()
    {
        var bomb = Instantiate(_bombEffect);
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _bombScale);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Block"))
            {
                var block = hit.gameObject.GetComponent<Block>();
                block.GetDamaged(15);
            }
            else if (hit.CompareTag("Animal"))
            {
                var animal = hit.gameObject.GetComponent<Animal>();
                animal.GetDamaged(15);
            }
        }

        bomb.Stop();
        var mainModule = bomb.main;
        bomb.transform.localScale *= 5f;
        mainModule.duration = 3f;
        bomb.Play();
    }
}