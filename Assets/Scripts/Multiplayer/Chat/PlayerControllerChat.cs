using Mirror;
using UnityEngine;

public class PlayerControllerChat : NetworkBehaviour
{
    private ChatManager chatManager;

    private void Start()
    {
        chatManager = ChatManager.GetInstance();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            chatManager.SendMessage(chatManager.chatInput.text);
        }
    }

    // Reste du code du joueur...
}
