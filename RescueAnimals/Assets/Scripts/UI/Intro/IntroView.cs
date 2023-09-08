using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Intro
{
    public class IntroView : MonoBehaviour
    {
        public event Action OnStartClicked;
        public event Action OnExitClicked;

        public void CallStart()
        {
            OnStartClicked?.Invoke();
        }

        public void CallExit()
        {
            OnExitClicked?.Invoke();
        }
    }
}