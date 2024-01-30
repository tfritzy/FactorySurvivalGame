using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class NetworkDebugger : MonoBehaviour
{
    private static NetworkDebugger? _instance;
    public static NetworkDebugger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<NetworkDebugger>();
            }

            return _instance;
        }
    }
    private LinkedList<string> logMessages = new LinkedList<string>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LogSentMessage(new IPEndPoint(IPAddress.Parse("192.168.0.1"), 1234), "Hello");
        LogReceivedMessage(new IPEndPoint(IPAddress.Parse("192.168.0.1"), 1234), "Fuck you");
    }

    public void LogSentMessage(IPEndPoint endpoint, string message)
    {
        lock (logMessages)
        {
            logMessages.AddLast($"-> {endpoint}: {message}");

            if (logMessages.Count > 70)
            {
                logMessages.RemoveFirst();
            }
        }
    }

    public void LogReceivedMessage(IPEndPoint endpoint, string message)
    {
        lock (logMessages)
        {
            logMessages.AddLast($"<- {endpoint}: {message}");

            if (logMessages.Count > 70)
            {
                logMessages.RemoveFirst();
            }
        }
    }

    void OnGUI()
    {
        lock (logMessages)
        {
            int i = 0;
            foreach (string message in logMessages)
            {
                GUI.Label(new Rect(0, i * 20, 1000, 20), message);
                i++;
            }
        }
    }
}