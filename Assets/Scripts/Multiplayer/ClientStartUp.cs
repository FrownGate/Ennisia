using System;
using System.Collections.Generic;
using Mirror;
using PlayFab;
using PlayFab.MultiplayerModels;
using UnityEngine;

public class ClientStartUp : MonoBehaviour
{
    public Configuration Configuration;
    public ServerStartUp ServerStartUp;
    public NetworkManager NetworkManager;
    public TelepathyTransport TelepathyTransport;
    public ApathyTransport ApathyTransport;

    public void OnLoginUserButtonClick()
    {
        switch (Configuration.buildType)
        {
            case BuildType.REMOTE_CLIENT when Configuration.buildId == "":
                throw new Exception(
                    "A remote client build must have a buildId. Add it to the Configuration. Get this from your Multiplayer Game Manager in the PlayFab web console.");
            case BuildType.REMOTE_CLIENT when PlayFabManager.Instance.LoggedIn:
                OnPlayFabLoginSuccess();
                break;
            case BuildType.REMOTE_CLIENT:
                OnLoginError();
                break;
            case BuildType.LOCAL_CLIENT:
                NetworkManager.StartClient();
                break;
        }
    }


    private void OnLoginError()
    {
        Debug.Log("not Logged IN");
    }

    private void OnPlayFabLoginSuccess()
    {
        Debug.Log("[ClientStartUp].LoginRemoteUser");
        if (Configuration.ipAddress == "")
            //We need to grab an IP and Port from a server based on the buildId. Copy this and add it to your Configuration.
            RequestMultiplayerServer();
        else
            ConnectRemoteClient();
    }

    private void RequestMultiplayerServer()
    {
        Debug.Log("[ClientStartUp].RequestMultiplayerServer");
        RequestMultiplayerServerRequest requestData = new()
        {
            BuildId = Configuration.buildId,
            SessionId = Guid.NewGuid().ToString(),
            PreferredRegions = new List<string> { AzureRegion.NorthEurope.ToString() }
        };
        PlayFabMultiplayerAPI.RequestMultiplayerServer(requestData, OnRequestMultiplayerServer,
            OnRequestMultiplayerServerError);
    }

    private void OnRequestMultiplayerServer(RequestMultiplayerServerResponse response)
    {
        Debug.Log(response.ToString());
        ConnectRemoteClient(response);
    }

    private void ConnectRemoteClient(RequestMultiplayerServerResponse response = null)
    {
        if (response == null)
        {
            NetworkManager.networkAddress = Configuration.ipAddress;
            TelepathyTransport.port = Configuration.port;
            ApathyTransport.Port = Configuration.port;
        }
        else
        {
            Configuration.ipAddress = response.IPV4Address;
            NetworkManager.networkAddress = response.IPV4Address;
            Configuration.port = (ushort)response.Ports[0].Num;
            TelepathyTransport.port = (ushort)response.Ports[0].Num;
            ApathyTransport.Port = (ushort)response.Ports[0].Num;
        }

        NetworkManager.StartClient();
    }

    private void OnRequestMultiplayerServerError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }
}