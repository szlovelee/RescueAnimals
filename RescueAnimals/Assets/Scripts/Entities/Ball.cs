using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BallRd;

    private Vector2 direction = new Vector2(2, 1);

    public float speed = 10f;

    private void Awake()
    {
        BallRd = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        MoveBall();
    }

    private void MoveBall()
    {
        direction.Normalize();
        direction = direction * speed;
        BallRd.velocity = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            MoveBallReversal();
        }
    }

    private void MoveBallReversal()
    {
        direction.x = -direction.x;
    }
}
