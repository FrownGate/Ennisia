using Mirror;
using UnityEngine;

public class PlayerM : NetworkBehaviour
{
    private ChatManager chatManager;

    private void Start()
    {
        if (isLocalPlayer)
        {
            chatManager = FindObjectOfType<ChatManager>();
            chatManager.chatInput.interactable = true;
        }
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
}
