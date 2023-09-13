using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Util;

//todo make IPoolable
public class Ball : MonoBehaviour, IAttackable, IPoolable<Ball>
{
    [SerializeField] private Rigidbody2D BallRd;
    [SerializeField] private Collider2D BallCollider;
    [SerializeField] private GameObject ThrowPivot;
    [SerializeField] private GameObject ThrowPoint;
    [SerializeField] private ParticleSystem BallParticle;


    [Range(100f, 400f)] public float speed = 200f;

    private bool _isPiercing = false;
    private float _piercingTimer = 0f;

    [SerializeField] private bool _isShooting = true;

    private Touch touch;
    private Vector2 touchPos, ballDir;
    private Vector2 _prevVelocity;
    private Camera _camera;

    private Vector2[] ballTransform = new Vector2[2];

    private Action<Ball> _returnAction;
    public Action<Vector2> OnBallCollide;
    public int Atk { get; set; }


    private void Awake()
    {
        BallRd = GetComponent<Rigidbody2D>();
        BallCollider = GetComponent<Collider2D>();
        ThrowPivot = transform.GetChild(0).gameObject;
        ThrowPoint = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        _camera = Camera.main;

        Atk = 10;
    }

    private void Update()
    {
        if (transform.position.y <= GameManager.Instance.gameOverLine)
        {
            GameManager.Instance.player.balls.Remove(this);
            gameObject.SetActive(false);
        }

        if (Input.touchCount > 0)
        {
            if (_isShooting)
            {
                touch = Input.GetTouch(0);
                touchPos = new Vector2(_camera.ScreenToWorldPoint(touch.position).x
                    , Mathf.Clamp(_camera.ScreenToWorldPoint(touch.position).y, -5f, (transform.position.y - 0.5f)));
                float rotZ = Mathf.Atan2(touchPos.y - ThrowPivot.transform.position.y,
                    touchPos.x - ThrowPivot.transform.position.x) * Mathf.Rad2Deg;
                ThrowPivot.transform.rotation = Quaternion.AngleAxis(rotZ + 90, Vector3.forward);
                ThrowPoint.transform.position = touchPos;

                if (touch.phase == TouchPhase.Began)
                {
                    ThrowPivot.SetActive(true);
                
                if (touch.phase == TouchPhase.Ended)
                {
                    ballDir = ((Vector2)transform.position - touchPos).normalized;
                    BallRd.AddForce(ballDir * speed);
                    ThrowPivot.SetActive(false);
                    _isShooting = false;
                }
            }
        }
    }

    public void SetBonusBall()
    {
        _isShooting = false;

        int rand = UnityEngine.Random.Range(0, 2);

        if (rand == 0)
            ballDir = new Vector2(1, 1).normalized;
        else
            ballDir = new Vector2(-1, 1).normalized;

        // BallRd.AddForce(ballDir * speed);
        ThrowPivot.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var position = collision.GetContact(0).point;
        OnBallCollide?.Invoke(position);
        
        if (_isPiercing)
            return;

        if (PreviousVelocityEqualToCurrent(collision.GetContact(0).relativeVelocity))
        {
            if (_prevVelocity.x <= 0.25f)
            {
                BallRd.velocity += new Vector2(2f, 0);
            }
            else
            {
                BallRd.velocity += new Vector2(0f, -2f);
            }
        }

        _prevVelocity = collision.GetContact(0).relativeVelocity;

        if (ballTransform[0] == null)
        {
            ballTransform[0] = this.gameObject.transform.position;
        }
        else
        {
            ballTransform[1] = ballTransform[0];
            ballTransform[0] = this.gameObject.transform.position;
        }

        if (ballTransform[0] != null && ballTransform[1] != null)
        {
            Vector2 BP = (ballTransform[0] - ballTransform[1]);
            BP.Normalize();
            if (Mathf.Abs(BP.x) < 0.1f)
            {
                if(BP.x < 0)
                {
                    BallRd.AddForce(new Vector2(-50f, 0));
                }
                else
                {
                    BallRd.AddForce(new Vector2(50f, 0));
                }
            }
            else if (Mathf.Abs(BP.y) < 0.1f)
            {
                if (BP.y < 0)
                {
                    BallRd.AddForce(new Vector2(0, -50f));
                }
                else
                {
                    BallRd.AddForce(new Vector2(0, 50f));
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPiercing)
            return;

        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("Ʈ�����浹");
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

    private bool PreviousVelocityEqualToCurrent(Vector2 velocity)
    {
        return (_prevVelocity + velocity).magnitude <= 0.5f;
    }

    public void Initialize(Action<Ball> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        _returnAction?.Invoke(this);
    }

    private void OnDisable()
    {
        ReturnToPool();
    }
}