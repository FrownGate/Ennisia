using System.Collections;
using UnityEngine;
using PlayFab;
using System;
using PlayFab.Networking;
using System.Collections.Generic;
using PlayFab.MultiplayerAgent.Model;
using Mirror;

public class ServerStartUp : MonoBehaviour
{
    public Configuration Configuration;

    private List<ConnectedPlayer> _connectedPlayers;
    public UnityNetworkServer UNetServer;

    public NetworkManager NetworkManager;

    void Start()
    {
        switch (Configuration.buildType)
        {
            case BuildType.REMOTE_SERVER:
                StartRemoteServer();
                break;
            case BuildType.LOCAL_SERVER:
                NetworkManager.StartServer();
                break;
        }
    }

    public void OnStartLocalServerButtonClick()
    {
        if (Configuration.buildType == BuildType.LOCAL_SERVER)
        {
            NetworkManager.StartServer();
        }
    }

    private void StartRemoteServer()
    {
        Debug.Log("[ServerStartUp].StartRemoteServer");
        _connectedPlayers = new List<ConnectedPlayer>();
        //PlayFabMultiplayerAgentAPI.Start();
        //PlayFabMultiplayerAgentAPI.IsDebugging = Configuration.playFabDebugging;
        //PlayFabMultiplayerAgentAPI.OnMaintenanceCallback += OnMaintenance;
        //PlayFabMultiplayerAgentAPI.OnShutDownCallback += OnShutdown;
        //PlayFabMultiplayerAgentAPI.OnServerActiveCallback += OnServerActive;
        //PlayFabMultiplayerAgentAPI.OnAgentErrorCallback += OnAgentError;

        UNetServer.OnPlayerAdded.AddListener(OnPlayerAdded);
        UNetServer.OnPlayerRemoved.AddListener(OnPlayerRemoved);

        StartCoroutine(ReadyForPlayers());
        StartCoroutine(ShutdownServerInXTime());
    }

    IEnumerator ShutdownServerInXTime()
    {
        yield return new WaitForSeconds(300f);
        StartShutdownProcess();
    }

    IEnumerator ReadyForPlayers()
    {
        yield return new WaitForSeconds(.5f);
        //PlayFabMultiplayerAgentAPI.ReadyForPlayers();
    }

    private void OnServerActive()
    {
        Debug.Log("Server Started From Agent Activation");

        TelepathyTransport telepathyTransport = (TelepathyTransport)Transport.active;
        if (telepathyTransport != null)
        {
            telepathyTransport.port = Configuration.port;
            //var connectionInfo = PlayFabMultiplayerAgentAPI.GetGameServerConnectionInfo();
            //if (connectionInfo != null)
            //{
            //    // Set the server to the first available port
            //    foreach (var port in connectionInfo.GamePortsConfiguration)
            //    {
            //        telepathyTransport.port = (ushort)port.ServerListeningPort;
            //        Debug.LogFormat("Server listening port = {0}, client connection port = {1}",
            //            port.ServerListeningPort, port.ClientConnectionPort);
            //        break;
            //    }
            //}
        }

        NetworkManager.StartServer();
    }

    private void OnPlayerRemoved(string playfabId)
    {
        ConnectedPlayer player =
            _connectedPlayers.Find(x => x.PlayerId.Equals(playfabId, StringComparison.OrdinalIgnoreCase));
        _connectedPlayers.Remove(player);
        //PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
        CheckPlayerCountToShutdown();
    }

    private void CheckPlayerCountToShutdown()
    {
        if (_connectedPlayers.Count <= 0)
        {
            StartShutdownProcess();
        }
    }

    private void OnPlayerAdded(string playfabId)
    {
        //_connectedPlayers.Add(new ConnectedPlayer(playfabId));
        //PlayFabMultiplayerAgentAPI.UpdateConnectedPlayers(_connectedPlayers);
    }

    private void OnAgentError(string error)
    {
        Debug.Log(error);
    }

    private void OnShutdown()
    {
        StartShutdownProcess();
    }

    private void StartShutdownProcess()
    {
        Debug.Log("Server is shutting down");
        foreach (var conn in UNetServer.Connections)
        {
            conn.Connection.Send(new ShutdownMessage());
        }

        StartCoroutine(ShutdownServer());
    }

    IEnumerator ShutdownServer()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }

    private void OnMaintenance(DateTime? NextScheduledMaintenanceUtc)
    {
        Debug.LogFormat("Maintenance scheduled for: {0}", NextScheduledMaintenanceUtc.Value.ToLongDateString());
        foreach (var conn in UNetServer.Connections)
        {
            conn.Connection.Send(new MaintenanceMessage()
            {
                ScheduledMaintenanceUTC = (DateTime)NextScheduledMaintenanceUtc
            });
        }
    }
}