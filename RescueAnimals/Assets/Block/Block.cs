using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private double MaxHp = 10;
    [SerializeField] private double Hp = 10;

    [SerializeField] private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            // Hp -= Ball.Atk;
        }

        switch (Hp / MaxHp)
        {
            case <= 0:
                _animator.SetTrigger("0%");
                GameObject.Destroy(gameObject);
                break;
            case <= 0.33:
                _animator.SetTrigger("33%");
                break;
            case <= 0.66:
                _animator.SetTrigger("66%");
                break;
        }

    }

    ////Å×½ºÆ®
    //private void Update()
    //{
    //    switch (Hp / MaxHp)
    //    {
    //        case <= 0:
    //            _animator.SetTrigger("0%");
    //            GameObject.Destroy(gameObject);
    //            break;
    //        case <= 0.33:
    //            _animator.SetTrigger("33%");
    //            break;
    //        case <= 0.66:
    //            _animator.SetTrigger("66%");
    //            break;
    //    }
    //}
}
