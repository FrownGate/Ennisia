using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    public string Params { get; private set; }

    private Scene _activeScene;
    private Scene _previousScene;
    private LoadSceneMode _sceneMode;
    private string _sceneToLoad;

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
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            _activeScene = SceneManager.GetActiveScene();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private LoadSceneMode SceneMode()
    {
        if (_sceneToLoad.Contains("Popup"))
        {
            return LoadSceneMode.Additive;
        }
        else
        {
            return LoadSceneMode.Single;
        }
    }

    public void SetScene(string scene)
    {
        _sceneToLoad = GetSceneName(scene);
        Debug.Log($"Going to scene {_sceneToLoad}");
        SceneManager.LoadScene(_sceneToLoad, SceneMode());
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _activeScene = scene;
        _sceneMode = mode;
        Debug.Log($"{_activeScene.name} loaded !");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _previousScene = scene;
        Debug.Log($"{_previousScene.name} unloaded !");
    }

    private string GetSceneName(string scene)
    {
        string[] splittedName = scene.Split('#');

        if (splittedName.Length > 1)
        {
            Params = splittedName[1];
        }

        return splittedName[0];
    }

    public bool HasParams()
    {
        return !string.IsNullOrEmpty(Params);
    }
}
