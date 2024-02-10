using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Core;
using NUnit.Framework.Constraints;
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
    }

    private string getMessageString(byte[] bytes)
    {
        string message = "";
        try
        {
            Schema.OneofRequest request = Schema.OneofRequest.Parser.ParseFrom(bytes);
            if (request.Heartbeat != null && request.Heartbeat.MissedPacketIds.Count > 0)
            {
                message = "Missed: " + string.Join(", ", request.Heartbeat!.MissedPacketIds);
            }
        }
        catch { }

        if (message == "")
        {
            try
            {
                Schema.Packet packet = Schema.Packet.Parser.ParseFrom(bytes);
                message = "packet " + packet.Id.ToString();
            }
            catch { }
        }

        if (message == "")
        {
            message = Encoding.UTF8.GetString(bytes);
        }

        return message;
    }

    public void LogSentMessage(IPEndPoint endpoint, byte[] bytes)
    {
        string message = getMessageString(bytes);

        lock (logMessages)
        {
            logMessages.AddLast($"{message} => {endpoint}");

            if (logMessages.Count > 70)
            {
                logMessages.RemoveFirst();
            }
        }
    }

    public void LogReceivedMessage(IPEndPoint endpoint, byte[] bytes)
    {
        string message = getMessageString(bytes);

        lock (logMessages)
        {
            logMessages.AddLast($"{message} <= {endpoint}");

            if (logMessages.Count > 70)
            {
                logMessages.RemoveFirst();
            }
        }
    }

    private string BuildDebugString()
    {
        StringBuilder sb = new StringBuilder();

        if (ConnectionManager.Instance.Connection is HostConnection)
        {
            HostConnection connection = (HostConnection)ConnectionManager.Instance.Connection;
            sb.AppendLine("Host");
            sb.AppendLine($"  Own id: {connection.PlayerId}");
            sb.AppendLine($"  Listener status: {ConnectionManager.Instance.ListenLoopTask?.Status}");
            sb.AppendLine($"  Unhandled messages: {ConnectionManager.Instance.UnhandledPacketsCount}");
            sb.AppendLine($"  Clients:");
            foreach (var client in connection.ConnectedPlayers)
            {
                sb.AppendLine($"    {client.EndPoint}");
                sb.AppendLine($"      Id: {client.Id}");
                sb.AppendLine($"      Ping: {client.Ping.Milliseconds}ms");
                sb.AppendLine($"      Sent packets: {client.NumSentPackets}");
                sb.AppendLine($"      Missed packets: {client.NumMissedPackets}");
                sb.AppendLine($"      PacketLoss: {(client.PacketLoss).ToString("0.00")}%");
            }
        }
        else if (ConnectionManager.Instance.Connection is ClientConnection)
        {
            ClientConnection connection = (ClientConnection)ConnectionManager.Instance.Connection;
            sb.AppendLine("Client");
            sb.AppendLine($"  Listener status: {ConnectionManager.Instance.ListenLoopTask?.Status}");
            sb.AppendLine($"  Unhandled messages: {ConnectionManager.Instance.UnhandledPacketsCount}");
            sb.AppendLine($"  Own Id: {connection.PlayerId}");
            sb.AppendLine($"  Server: {connection.HostEndPoint}");
            sb.AppendLine($"  Received packets: {connection.NumPacketsReceived}");
        }

        return sb.ToString();
    }

    void OnGUI()
    {
        lock (logMessages)
        {
            GUILayout.BeginArea(new Rect(10, 10, 400, 400));
            GUILayout.BeginVertical();
            GUILayout.Label(BuildDebugString());

            // foreach (var message in logMessages)
            // {
            //     GUILayout.Label(message);
            // }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}