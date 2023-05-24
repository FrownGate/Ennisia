using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("MainMenu");
    }
}