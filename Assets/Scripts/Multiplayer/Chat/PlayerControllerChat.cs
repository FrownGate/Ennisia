using Mirror;
using UnityEngine;

public class PlayerControllerChat : NetworkBehaviour
{
    private ChatManager chatManager;

    private void Start()
    {
        chatManager = ChatManager.GetInstance();
        Debug.Log($"Player is LocalPlayer : {isLocalPlayer}");
        Debug.Log($"Player is Owned : {isOwned}");
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Return)) SendMessage(chatManager.chatInput.text);
    }

    private new void SendMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            if (isOwned)
            {
                CmdSendMessage(message);
            }
            else
            {
                Debug.LogWarning("ChatManager does not have authority to send message.");
            }

            chatManager.chatInput.text = "";
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
        chatManager.chatText.text += message + "\n";
    }
}