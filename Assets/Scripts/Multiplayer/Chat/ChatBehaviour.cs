//using System;
//using Mirror;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Rendering;

//public class ChatBehaviour : MonoBehaviour
//{
//    [SerializeField] private TMP_Text chatText = null;
//    [SerializeField] private TMP_InputField inputField = null;

//    private static event Action<string> OnMessage;

//    public void OnStartAuthority()
//    {
//        OnMessage += HandleNewMessage;
//    }

//    [ClientCallback]

//    private void OnDestroy()
//    {
//        if (!hasAuthority) { return; }

//        OnMessage -= HandleNewMessage;
//    }

//    private void HandleNewMessage(string message)
//    {
//        chatText.text = message;
//    }

//    [Client]
//    public void Send(string message)
//    {
//        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

//        if (string.IsNullOrEmpty(message)) { return; }

//        CmdSendMessage(inputField.text);

//        inputField.text = string.Empty;
//    }

//    [Command]
//    private void CmdSendMessage(string message)
//    {
//        RpcHandleMessage($"[{ConnectionToClient.connectionId}]: {message}");
//    }

//    [ClientRpc]
//    private void RpcHandleMessage(string message)
//    {
//        OnMessage?.Invoke($"\n{message}");
//    }
//}
