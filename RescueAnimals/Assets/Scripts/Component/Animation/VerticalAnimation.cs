using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VerticalAnimation : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private float _currentAnimatedValue;

    [SerializeField] private float _MaxYAxisValue = 1f;
    [SerializeField] private float _animateValuePerRoutine = 0.1f;

    [SerializeField] private int _dir = 1;


    private void Start()
    {
        StartCoroutine(AnimateOffsetVertical());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator AnimateOffsetVertical()
    {
        while (isActiveAndEnabled)
        {
            if (_currentAnimatedValue >= _MaxYAxisValue)
            {
                _dir *= -1;
                _currentAnimatedValue = 0;
            }

            var dy = _animateValuePerRoutine * _dir;
            _currentAnimatedValue += _animateValuePerRoutine;
            _transform.localPosition += new Vector3(0, dy);
            yield return new WaitForSeconds(0.1f);
        }
    }
}