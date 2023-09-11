using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BallRd;
    [SerializeField] private Collider2D BallCollider;

    private Vector2 direction = new Vector2(2, 1).normalized;

    [Range(50f, 200f)] public float speed = 100f;

    private bool _isPiercing = false;
    private float _piercingTimer = 0f;

    private void Awake()
    {
        BallRd = GetComponent<Rigidbody2D>();
        BallCollider = gameObject.GetComponent<Collider2D>();
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
}
