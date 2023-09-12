using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using EnumTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _level;
    private float _exp;
    public BallType BallType = BallType.Baseball;

    //todo make Object pool
    public List<Ball> balls = new();

    private CharacterInputController _controller;
    private Movement _movement;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Animator animator;
    private static readonly int IsHit = Animator.StringToHash("IsHit");

    private void Awake()
    {
        var i = GameManager.Instance;
        _movement = GetComponent<Movement>();
        _controller = GetComponent<CharacterInputController>();
    }

    private void Start()
    {
        _controller.OnTouchPressEvent += MoveTo;
    }

    private void MoveTo(Vector2 dest)
    {
        //calc max y , maxX send movcement
        _movement.MoveTo(dest);
    }

    //todo migrate to manager
    public void InstantiateBall()
    {
        var ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        balls.Add(ball.GetComponent<Ball>());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetBool(IsHit, true);
        StartCoroutine(TransitionToIdle());
    }

    private IEnumerator TransitionToIdle()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool(IsHit, false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    /*
     todo migrate to manager
     *private void InstantiateCharacter()
    {
        // var characterType  =  CharacterType.
        var position = cam.ViewportToWorldPoint(new Vector2(0.5f, 0.15f));
        position.z = 0;
        var go = Instantiate(_playerPrefab, position, Quaternion.identity);
        _player = go.GetComponent<Player>();
        _player.InstantiateBall();
    }
     *
     */
}