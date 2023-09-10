using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace AnimationComponent
{
    public class HorizontalAnimation : MonoBehaviour
    {
        private float _currentAnimatedValue;

        [FormerlySerializedAs("_transform")] [SerializeField]
        private Transform animatePivot;

        [FormerlySerializedAs("_MaxXAxisValue")] [SerializeField]
        private float maxXAxisValue = 1f;

        [FormerlySerializedAs("_animateValuePerRoutine")] [SerializeField]
        private float animateValuePerRoutine = 0.1f;

        [FormerlySerializedAs("_dir")] [SerializeField]
        private int dir = 1;

        [FormerlySerializedAs("_waitSecondsForNextRoutine")] [SerializeField]
        private float waitSecondsForNextRoutine = 0.1f;


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
                if (_currentAnimatedValue >= maxXAxisValue)
                {
                    dir *= -1;
                    _currentAnimatedValue = 0;
                }

                var dx = animateValuePerRoutine * dir;
                _currentAnimatedValue += animateValuePerRoutine;
                animatePivot.localPosition += new Vector3(x: dx, y: 0);
                yield return new WaitForSeconds(waitSecondsForNextRoutine);
            }
        }
    }
}