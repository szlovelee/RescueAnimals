using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueAnimalsMagicShield : MonoBehaviour
{
    private RescueAnimalsCharacterController _controller;

    [SerializeField] private Transform projectileSpawnPosition;
    private Vector2 _aimDirection = Vector2.right;

    public GameObject MagicShield;


    private void Awake()
    {
        _controller = GetComponent<RescueAnimalsCharacterController>();
    }


    void Start()
    {
        _controller.OnMagicShieldEvent += OnMagicShield;
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
        Instantiate(MagicShield, projectileSpawnPosition.position, Quaternion.identity);
    }
}
