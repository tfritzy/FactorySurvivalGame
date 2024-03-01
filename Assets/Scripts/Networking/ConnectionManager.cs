using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    public Connection? Connection { get; private set; }
    public int UnhandledPacketsCount => messagesToHandle.Count;
    private string cachedScene = "";
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private Client client;
    private Queue<UdpReceiveResult> messagesToHandle = new();
    public ulong SelfId;
    public Task? ListenLoopTask;

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
        client = new Client();
        client.OnMessageSent = (IPEndPoint endpoint, byte[] message) =>
        {
            NetworkDebugger.Instance.LogSentMessage(endpoint, message);
        };
        client.OnMessageReceived = (IPEndPoint endpoint, byte[] message) =>
        {
            NetworkDebugger.Instance.LogReceivedMessage(endpoint, message);
        };
        StartListener();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != cachedScene)
        {
            cachedScene = SceneManager.GetActiveScene().name;

            if (cachedScene == "Game")
            {
                OnEnterGame();
            }
        }

        if (Connection != null)
        {
            while (messagesToHandle.Count > 0)
            {
                UdpReceiveResult result = messagesToHandle.Dequeue();
                Connection.HandleMessage(result.RemoteEndPoint, result.Buffer);
            }
            Connection?.Update();
        }

        WaitForClientConnectionToReceiveWorld();
    }

    private void StartListener()
    {
        cancellationTokenSource = new CancellationTokenSource();
        ListenLoopTask = Task.Run(ReceiveLoop);
    }

    async Task ReceiveLoop()
    {
        while (true)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                Debug.Log("Receive loop cancelled");
                break;
            }

            try
            {
                if (Connection != null && client != null)
                {
                    UdpReceiveResult result = await client.ReceiveAsync(cancellationTokenSource.Token);
                    messagesToHandle.Enqueue(result);
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError("SocketException: " + ex.ToString());
                break;
            }
            catch (TaskCanceledException ex)
            {
                Debug.Log("Task cancelled.");
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

    private void WaitForClientConnectionToReceiveWorld()
    {
        if (cachedScene != "Game")
        {
            return;
        }

        if (!WorldMono.Instance.Context.HasWorld)
        {
            if (Connection is ClientConnection clientConnection)
            {
                if (clientConnection.ConnectedWorld != null)
                {
                    WorldMono.Instance.SetWorld(clientConnection.ConnectedWorld);
                }
            }
        }
    }

    private void OnEnterGame()
    {
        if (Connection?.ConnectedWorld == null)
        {
            throw new Exception("ConnectionManager's world should not be null on game load.");
        }

        WorldMono.Instance.SetWorld(Connection.ConnectedWorld);
    }

    private void LoadGameScene()
    {
        Debug.Log("On connected. Loading game scene");
        SceneManager.LoadScene("Game");
    }

    public async Task StartHostConnection()
    {
        Connection = new HostConnection(client, LoadGameScene);
        Context context = new Context();
        Core.Terrain terrain = new Core.Terrain(new TerrainGenerator(100, 100, 5).GenerateFlatWorld(context), context);
        World world = new World(terrain, context);
        Connection.SetWorld(world);
        SelfId = world.Characters.Values.First(
            (c) => c is Player player && player.PlayerId == Connection.PlayerId).Id;
        await Connection.Connect();
    }

    public async Task StartClientConnection()
    {
        var connection = new ClientConnection(client, LoadGameScene);
        connection.OnSetWorld = (World world) =>
        {
            SelfId = world.Characters.Values.First(
                (c) => c is Player player && player.PlayerId == connection.PlayerId).Id;
        };
        Connection = connection;
        await Connection.Connect();
    }

    public void ResetState()
    {
        Connection = null;
    }
}