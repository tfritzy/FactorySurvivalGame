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
}