using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Intro
{
    public class IntroPresenter : MonoBehaviour
    {
        [SerializeField] private IntroView _view;

        private void Start()
        {
            _view.OnStartClicked += StartClicked;
            _view.OnExitClicked += ExitClicked;
        }

        private void StartClicked()
        {
            //TODO when added character and selectable them, add logic to select character
            StartCoroutine(LoadHomeSceneAsync());
            SoundManager.instance.PlayAcceptEffect();
        }

        private void ExitClicked()
        {
            Application.Quit();
        }

        private IEnumerator LoadHomeSceneAsync()
        {
            var oper = SceneManager.LoadSceneAsync("Scenes/HomeScene");
            while (!oper.isDone)
            {
                yield return null;
            }
        }
    }
}