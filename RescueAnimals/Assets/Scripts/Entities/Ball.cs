using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BallRd;

    private Vector2 direction = new Vector2(2, 1).normalized;

    [Range(50f, 200f)] public float speed = 100f;

    private void Awake()
    {
        BallRd = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        BallRd.AddForce(direction * speed);
    }

    void Update()
    {
        //MoveBall();
    }

    //private void MoveBall()
    //{
    //    direction.Normalize();
    //    direction = direction * speed;
    //    BallRd.velocity = direction;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("1212");
        }
    }
}
