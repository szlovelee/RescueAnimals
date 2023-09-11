using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameObject _viewObj;

    GameView _view;

    void Awake()
    {
        _view = _viewObj.GetComponent<GameView>(); 
    }

}
