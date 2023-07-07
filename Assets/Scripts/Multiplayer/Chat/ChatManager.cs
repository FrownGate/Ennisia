using Mirror;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    public TMP_Text chatText;
    public TMP_InputField chatInput;

    private static ChatManager instance; // Instance statique du ChatManager

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static ChatManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        chatInput.onSubmit.AddListener(SendMessage);
    }

    private void SendMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            if (hasAuthority)
            {
                CmdSendMessage(message);
            }
            else
            {
                Debug.LogWarning("ChatManager does not have authority to send message.");
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
