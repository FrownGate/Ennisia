using UnityEngine;
using Mirror;

public class Host : MonoBehaviour
{
    private string hostIPAddress;

    private void Start()
    {
        hostIPAddress = NetworkManager.singleton.networkAddress;
        Debug.Log("Host IP Address: " + hostIPAddress);

        NetworkManager.singleton.networkAddress = hostIPAddress;
    }
}
