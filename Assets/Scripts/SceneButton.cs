using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        SceneManager.LoadScene(gameObject.name);
    }
}