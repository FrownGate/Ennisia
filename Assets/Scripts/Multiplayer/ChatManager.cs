using Mirror;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public TMP_Text chatText;
    public TMP_InputField chatInput;

    private void Start()
    {
        chatInput.onSubmit.AddListener(SendMessage);
    }

    private void SendMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            if (isServer)
            {
                RpcReceiveMessage(message);
            }
            else
            {
                CmdSendMessage(message);
            }

            chatInput.text = "";
        }
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcReceiveMessage(message);
    }

    [ClientRpc]
    private void RpcReceiveMessage(string message)
    {
        chatText.text += message + "\n";
    }
}
