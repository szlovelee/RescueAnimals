using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistPanda : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private int _pattern = 0;
    private float _assistTime = 0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(PandaMove());
    }

    private void LateUpdate()
    {
        _assistTime -= Time.deltaTime;

        if (_assistTime < 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        switch (_pattern)
        {
            case 0:
                Move(-1f);
                break;
            case 1:
                Move(1f);
                break;
            case 2:
                Stop();
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            _animator.SetBool("IsMove", false);
        }
    }

    public void SetAssistTime(float time)
    {
        _assistTime = time;
    }

    private void Move(float direction)
    {
        _animator.SetBool("IsMove", true);

        if (direction > 0f)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 180f, 0f);
        }

        _rigidbody.velocity = new Vector2(direction, _rigidbody.velocity.y);
    }

    private void Stop()
    {
        _animator.SetBool("IsMove", false);

        _rigidbody.velocity = Vector2.zero;
    }

    private IEnumerator PandaMove()
    {
        while (true)
        {
            _pattern = Random.Range(0, 3);

            float nextMoveTimer = Random.Range(2f, 5f);

            yield return new WaitForSeconds(nextMoveTimer);
        }
    }
}