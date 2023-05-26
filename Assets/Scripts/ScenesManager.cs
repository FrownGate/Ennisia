using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private bool SceneIsValid(string scene)
    {
        return SceneManager.GetSceneByName(scene).IsValid();
    }

    public void SetScene(string scene)
    {
        Debug.Log($"Going to scene {scene}");

        if (SceneIsValid(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Scene not found");
        }
    }
}
