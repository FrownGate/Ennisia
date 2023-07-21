using UnityEngine;

public class ChatButton : MonoBehaviour
{
    public GameObject window;

    private void Start()
    {
        window.SetActive(false);
    }

    public void TogglePanel()
    {
        window.SetActive(!window.activeSelf);
    }
}
