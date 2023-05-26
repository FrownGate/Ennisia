using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        try
		{
            Debug.Log($"Going to scene {gameObject.name}");
            SceneManager.LoadScene(gameObject.name);
            //TODO -> add name to SceneManager
            //Add parent name if exist ?           
        }
        catch
		{
			Debug.LogError("Scene not found");
		}
    }
}