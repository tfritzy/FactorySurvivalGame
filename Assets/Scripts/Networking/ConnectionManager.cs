using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    private Connection? connection;
    private string currentScene = "";

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

    void Update()
    {
        connection?.Update();
        WaitForClientConnectionToReceiveWorld();
        OnLoadGameSceneComplete();
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

    private void WaitForClientConnectionToReceiveWorld()
    {
        if (currentScene != "Game")
        {
            return;
        }

        if (!WorldMono.Instance.Context.HasWorld)
        {
            if (connection is ClientConnection clientConnection)
            {
                if (clientConnection.ConnectedWorld != null)
                {
                    WorldMono.Instance.SetWorld(clientConnection.ConnectedWorld);
                }
            }
        }
    }

    private void OnLoadGameSceneComplete()
    {
        if (SceneManager.GetActiveScene().name != currentScene)
        {
            currentScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            return;
        }

        if (currentScene == "Game")
        {
            if (connection?.ConnectedWorld == null)
            {
                throw new Exception("ConnectionManager's world should not be null on game load.");
            }

            WorldMono.Instance.SetWorld(connection.ConnectedWorld);
        }
    }

    private void LoadGameScene()
    {
        Debug.Log("On connected. Loading game scene");
        SceneManager.LoadScene("Game");
    }

    public async Task StartHostConnection()
    {
        Client client = new Client();
        connection = new HostConnection(client, LoadGameScene);

        Context context = new Context();
        Core.Terrain terrain = new Core.Terrain(new TerrainGenerator(100, 100, 5).GenerateFlatWorld(context), context);
        World world = new World(terrain, context);
        connection.SetWorld(world);

        await connection.Connect();
    }

    public async Task StartClientConnection()
    {
        Client client = new Client();
        connection = new ClientConnection(client, LoadGameScene);
        await connection.Connect();
    }
}