using Core;
using UnityEngine;

public static class Managers
{
    private static GameObject _player;
    public static GameObject Player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.Find("Player");
            }

            return _player;
        }
    }

    private static WorldMono _world;
    public static WorldMono World
    {
        get
        {
            if (_world == null)
            {
                _world = GameObject.Find("World").GetComponent<WorldMono>();
            }

            return _world;
        }
    }

    private static Camera _mainCamera;
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            return _mainCamera;
        }
    }

    private static Canvas _canvas;
    public static Canvas Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            }

            return _canvas;
        }
    }
}