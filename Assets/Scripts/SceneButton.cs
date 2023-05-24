using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        try
		{
            Debug.Log(gameObject.name);
            SceneManager.LoadScene(gameObject.name);
        }
		catch
		{
			Debug.LogError("Scene not found");
		}
    }
}