using System;
using Entities;
using UnityEngine;
using Util;


public class Block : MonoBehaviour, IPoolable<Block>

{
    [SerializeField] public double MaxHp = 10;

    [SerializeField] public double Hp = 10;

    [SerializeField] private Animator _animator;


    public Action<Block> OnBlockDestroyed;
    public Action<Vector2> OnHitBlock;

    private Action<Block> _returnToPool;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var attackable = collision.gameObject.GetComponent<IAttackable>();
        if (attackable == null) return;

        GetDamaged(attackable.Atk);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var attackable = other.gameObject.GetComponent<IAttackable>();
        if (attackable == null) return;
        GetDamaged(attackable.Atk);
    }

    public void GetDamaged(float atk)
    {
        Hp -= atk;
        OnHitBlock?.Invoke(transform.position);
        switch (Hp / MaxHp)
        {
            case <= 0:
                _animator.SetTrigger("0%");
                gameObject.SetActive(false);
                OnBlockDestroyed?.Invoke(this);
                break;
            case <= 0.33:
                _animator.SetTrigger("33%");
                break;
            case <= 0.66:
                _animator.SetTrigger("66%");
                break;
        }
    }

    public void Initialize(Action<Block> returnAction)

    {
        _returnToPool = returnAction;
    }


    public void ReturnToPool()
    {
        Hp = MaxHp;
        _returnToPool?.Invoke(this);
    }


    private void OnDisable()
    {
        OnHitBlock = null;
        ReturnToPool();
    }
}