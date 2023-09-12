using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BallRd;
    [SerializeField] private Collider2D BallCollider;
    [SerializeField] private GameObject ThrowPivot;
    [SerializeField] private GameObject ThrowPoint;

    private Vector2 direction = new Vector2(1, 1);

    [Range(50f, 200f)] public float speed = 100f;

    private bool _isPiercing = false;
    private float _piercingTimer = 0f;

    private bool _isShooting = true;

    private Touch touch;
    private Vector2 touchPos, ballDir;
    private Camera _camera;

    private void Awake()
    {
        BallRd = GetComponent<Rigidbody2D>();
        BallCollider = GetComponent<Collider2D>();
        ThrowPivot = transform.GetChild(0).gameObject;
        ThrowPoint = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        _camera = Camera.main;
    }

    void Start()
    {
        //BallRd.AddForce(direction * speed);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (_isShooting)
            {
                touch = Input.GetTouch(0);
                touchPos = new Vector2(_camera.ScreenToWorldPoint(touch.position).x 
                    , Mathf.Clamp(_camera.ScreenToWorldPoint(touch.position).y, -5f, (transform.position.y - 0.5f)));
                float rotZ = Mathf.Atan2(touchPos.y - ThrowPivot.transform.position.y, touchPos.x - ThrowPivot.transform.position.x) * Mathf.Rad2Deg;
                ThrowPivot.transform.rotation = Quaternion.AngleAxis(rotZ+90,Vector3.forward);
                ThrowPoint.transform.position = touchPos;

                if (touch.phase == UnityEngine.TouchPhase.Began)
                {
                    ThrowPivot.SetActive(true);

                }
                //if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Moved)
                //{
                    
                //}
                if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended)
                {
                    ballDir = ((Vector2)transform.position - touchPos).normalized;
                    BallRd.AddForce(ballDir * speed);
                    ThrowPivot.SetActive(false);
                    _isShooting = false;
                }
            }
        }
    }

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
