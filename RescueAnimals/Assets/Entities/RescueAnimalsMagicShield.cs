using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueAnimalsMagicShield : MonoBehaviour
{
    private RescueAnimalsCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public GameObject Wizard;


    private void Awake()
    {
        _controller = GetComponent<RescueAnimalsCharacterController>();
    }


    void Start()
    {
        _controller.OnMagicShield += OnMagicShield;
        _controller.OnLookEvent += OnAim;
    }


    private void OnAim(Vector2 newAimDirection)
    {
        _aimDirection = newAimDirection;
    }


    private void OnMagicShield()
    {
        CreateProjectile();
    }


    private void CreateProjectile()
    {
        //Debug.Log("Fire");
        Instantiate(Wizard, projectileSpawnPosition.position, Quaternion.identity);
    }
}
