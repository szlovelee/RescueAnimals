using System.Collections;
using System.Collections.Generic;
using Entities;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour, IAttackable
{
    [SerializeField] private Rigidbody2D BallRd;
    [SerializeField] private Collider2D BallCollider;
    [SerializeField] private ParticleSystem BallParticle;
    
    [Range(50f, 200f)] public float speed = 100f;

    private bool _isPiercing = false;
    private float _piercingTimer = 0f;

    public Ball(int atk = 1)
    {
        Atk = atk;
    }
        
    void Start()
    {
        var direction = new Vector2(2f, 1f).normalized;
        BallRd.AddForce(direction * speed);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ParticleSystem effect = Instantiate(BallParticle);
        effect.transform.position = collision.contacts[0].point;

        if (_isPiercing)
            return;

        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("1212");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPiercing)
            return;

        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("1212");
        }
    }

    public void OnPiercingMode(bool isPiercing, float time)
    {
        if (isPiercing)
        {
            _isPiercing = true;
            BallCollider.isTrigger = true;
            _piercingTimer = time;
        }
        else
        {
            _isPiercing = false;
            BallCollider.isTrigger = false;
            _piercingTimer = 0f;
        }
    }

    public int Atk { get; }
}