using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    //TODO -> use id instead of scenes names if possible

    public static ScenesManager Instance { get; private set; }
    public string Params { get; private set; }

    private Scene _activeScene;
    private Scene _previousScene;
    private LoadSceneMode _sceneMode; //Used ?
    private string _sceneToLoad;
    private bool _loading; //Used ?
    public float minLoadingTime = 2f; // Optional: Minimum duration to display the loading screen

    private float startTime;

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
            PlayFabManager.OnLoadingStart += MiniLoading;
            PlayFabManager.OnBigLoadingStart += BigLoading;
            PlayFabManager.OnLoginSuccess += StopLoading;

            Params = null;
            _activeScene = SceneManager.GetActiveScene();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        PlayFabManager.OnLoadingStart -= MiniLoading;
        PlayFabManager.OnBigLoadingStart -= BigLoading;
        PlayFabManager.OnLoginSuccess -= StopLoading;
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
        startTime = Time.time;

        switch (_sceneToLoad)
        {
            case "MainMenu":
            case "Battle":
                StartCoroutine(Loading());
                break;

            default:
                SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
                break;
        }
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
        Params = splittedName.Length > 1 ? splittedName[1] : null;
        return splittedName[0];
    }

    public bool HasParams()
    {
        return !string.IsNullOrEmpty(Params);
    }

    private void BigLoading()
    {
        SceneManager.LoadSceneAsync("Loading_Big", LoadSceneMode.Additive);
    }

    private void MiniLoading()
    {
        SceneManager.LoadSceneAsync("Loading_Mini", LoadSceneMode.Additive);
    }

    private void StopLoading()
    {
        if (SceneManager.GetSceneByName("Loading_Big").isLoaded) SceneManager.UnloadSceneAsync("Loading_Big");
        if (SceneManager.GetSceneByName("Loading_Mini").isLoaded) SceneManager.UnloadSceneAsync("Loading_Mini");
    }

    private IEnumerator Loading()
    {
        //TODO -> Replace AsyncOperation with events ?
        string loadScene = GetSceneName("Loading_Big");
        Debug.Log($"Going to scene {loadScene}");
        startTime = Time.time;

        AsyncOperation loadingScreenOperation = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

        while (!loadingScreenOperation.isDone)
        {
            yield return null;
        }

        Scene loadingScene = SceneManager.GetSceneByName(loadScene);
        if (!loadingScene.IsValid())
        {
            Debug.LogError("Loading scene is not valid.");
            yield break;
        }

        Slider progressBar = null;

        foreach (GameObject rootObject in loadingScene.GetRootGameObjects())
        {
            progressBar = rootObject.GetComponentInChildren<Slider>();
            if (progressBar != null)
            {
                break;
            }
        }

        if (progressBar == null)
        {
            Debug.LogError("Slider component not found in the loading scene.");
            yield break;
        }

        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
        //sceneOperation.allowSceneActivation = false;

        //TODO -> fix progress bar ui
        while (!sceneOperation.isDone)
        {
            // Update your loading progress UI here (e.g., progress bar, text)
            progressBar.value = sceneOperation.progress;

            //if (sceneOperation.progress >= 0.9f)
            //{
            //    // Optional: Ensure the loading screen is displayed for a minimum duration
            //    float elapsedTime = Time.time - startTime;

            //    if (elapsedTime >= minLoadingTime)
            //    {
            //        sceneOperation.allowSceneActivation = true; // Activate the  scene
            //    }
            //}

            yield return null;
        }
    }
}