using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
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
}