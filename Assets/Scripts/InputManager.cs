

using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool hotkeysEnabled = true;
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
            }

            return _instance;
        }
    }

    public void SetEnabled(bool enabled)
    {
        hotkeysEnabled = enabled;
    }

    struct KeyDownListener
    {
        public int Priority;
        public System.Action Callback;
    }

    private Dictionary<KeyCode, List<KeyDownListener>> keyDownListeners = new();

    public void RegisterKeyDown(KeyCode key, System.Action callback, int priority = 0)
    {
        if (!keyDownListeners.ContainsKey(key))
        {
            keyDownListeners[key] = new List<KeyDownListener>();
        }

        keyDownListeners[key].Add(new KeyDownListener
        {
            Priority = priority,
            Callback = callback,
        });

        keyDownListeners[key].Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }

    void Update()
    {
        if (!hotkeysEnabled)
        {
            return;
        }

        foreach (var key in keyDownListeners.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                keyDownListeners[key][0].Callback();
            }
        }
    }
}