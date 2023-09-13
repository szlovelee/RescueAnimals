using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Util;
using UnityEngine.Events;
using Entities;
using Unity.Mathematics;
using Unity.VisualScripting;

public class Animal : MonoBehaviour, IPoolable<Animal>
{
    [SerializeField] private double MaxHp = 10;
    [SerializeField] private double Hp = 10;
    //[SerializeField] private Animator _animator;

    public AnimalType animalType;
    public int reinforceLevel;
    public GameObject jailObj;
    private SpriteRenderer _renderer;
    public UnityEvent onResqueEvent;
    public event Action<Animal> OnAnimalSave;
    private bool _isSaved = false;

    public void SetAnimalReinforceLevel(int level)
    {
        reinforceLevel = level;
    }

    private Action<Animal> _returnAction;

    private void Awake()
    {
        _renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialize(Action<Animal> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _isSaved = false;
        jailObj.SetActive(true);
        _returnAction?.Invoke(this);
        Hp = MaxHp;
    }

    public void GetDamaged(float damage)
    {
        Hp -= damage;
        if (Hp <= 0 && !_isSaved)
        {
            _isSaved = true;
            jailObj.SetActive(false);
            StartCoroutine(Fadeout());
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Fadeout());
        ReturnToPool();
    }

    private void OnEnable()
    {
        jailObj.SetActive(true);
        var color = _renderer.color;
        color.a = 1f;
        _renderer.color = color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var attackable = collision.gameObject.GetComponent<IAttackable>();
        if (attackable == null) return;

        Hp -= attackable.Atk;
        if (Hp <= 0 && !_isSaved && gameObject.activeInHierarchy)
        {
            _isSaved = true;
            OnAnimalSave?.Invoke(this);
            onResqueEvent?.Invoke();
            jailObj.SetActive(false);
            StartCoroutine(Fadeout());
        }
    }

    private IEnumerator Fadeout()
    {
        var color = _renderer.color;
        for (var i = 10; i >= 0 && gameObject.activeSelf; i -= 2)
        {
            color.a = i * 0.1f;
            _renderer.color = color;
            yield return new WaitForSeconds(0.1f);
        }

        if (!gameObject.activeInHierarchy) yield break;
        gameObject.SetActive(false);
    }
}