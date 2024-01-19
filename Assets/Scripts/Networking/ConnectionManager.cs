using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    private Connection? connection;

    private static ConnectionManager? _instance;
    public static ConnectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ConnectionManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Task.Run(ReceiveLoop);
    }

    async void Update()
    {
        connection?.Update();
    }

    async Task ReceiveLoop()
    {
        while (true)
        {
            try
            {
                if (connection != null && connection.Client != null)
                {
                    Debug.Log("Waiting for message...");
                    await connection.ReadIncomingMessage();
                    Debug.Log("Got one!");
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError("SocketException: " + ex.ToString());
                break;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception: " + ex.ToString());
                break;
            }
        }

        Debug.Log("Receive loop ded");
    }

    public async Task StartHostConnection(Action onConnected)
    {
        Client client = new Client();
        connection = new HostConnection(client);
        await connection.Connect(onConnected);
    }

    public async Task StartClientConnection(Action onConnected)
    {
        Client client = new Client();
        connection = new ClientConnection(client);
        await connection.Connect(onConnected);
    }
}