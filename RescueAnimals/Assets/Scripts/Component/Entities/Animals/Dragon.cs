using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

public class Dragon : Animal, IAnimalBehaviour
{
    private float _bombScale = 2f;
    private float _bombDamage = 1f;

    [SerializeField]
    private ParticleSystem _bombEffect;
    
    public void OnResqueMove()
    {
        this.gameObject.SetActive(false);
    }
    public void OnResqueEffect()
    {
        ParticleSystem effect = Instantiate(_bombEffect);
        effect.transform.position = this.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, _bombScale);

        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].tag == "Animal")
            {
                hits[i].GetComponent<Animal>().GetDamaged(_bombDamage + (reinforceLevel * 0.5f));
            }
            else if(hits[i].tag == "Block")
            {
                hits[i].GetComponent<Block>().GetDamaged(_bombDamage + (reinforceLevel * 0.5f));
            }
        }
    }
}
