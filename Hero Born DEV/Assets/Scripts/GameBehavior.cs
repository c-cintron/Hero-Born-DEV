using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{

    private string _state;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool showWinScreen = false;
    public bool showLossScreen = false;

    public string labelText = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;

    private int _itemsCollected = 0;
    public int Items
    {
        get { return _itemsCollected; }

        set
        {
            _itemsCollected = value;

            if (_itemsCollected >= maxItems)
            {
                labelText = "You've found all the items!";
                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Items found, only " + (maxItems - _itemsCollected) + " more to go!";
            }
        }
    }

    private int _playerHP = 10;
    public int HP
    {
        get { return _playerHP; }

        set
        {
            _playerHP = value;
            Debug.LogFormat("Lives: {0}", _playerHP);

            if (_playerHP <= 0)
            {
                labelText = "this text is at the bottom of the screen why are you even reading this you weirdo";
                showLossScreen = true;
                Time.timeScale = 0;
            }
            else
            {
                labelText = "oof";
            }
        }
    }

    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }

        set
        {
            _lives = value;

            if (_lives <= 0)
            {
                showWinScreen = true;
                Time.timeScale = 0;
            }
        }
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _state = "Manager initialized..";
        _state.FancyDebug();
        Debug.Log(_state);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health:" + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected:" + _itemsCollected);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!"))
            {
                Utilities.RestartLevel(0);
            }
        }
        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Try Again Buddy! :)"))
            {
                Utilities.RestartLevel();
            }
        }
    }
}
