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
}