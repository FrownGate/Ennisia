using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    public string Params { get; private set; }

    private Scene _activeScene;
    private Scene _previousScene;
    private LoadSceneMode _sceneMode;
    private string _sceneToLoad;

    //private bool _loading;
    //public float minLoadingTime = 2f;

    //private float startTime;

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
            PlayFabManager.OnLoadingEnd += StopLoading;

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
        PlayFabManager.OnLoadingEnd -= StopLoading;
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
        SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
        //startTime = Time.time;

        //switch (_sceneToLoad)
        //{
        //    case "MainMenu":
        //    case "Battle":
        //        StartCoroutine(Loading());
        //        break;

        //    default:
        //        //if (IsPopupLoaded()) UnloadScene(_sceneToLoad);
        //        SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
        //        break;
        //}
    }

    public void ClosePopup()
    {
        SceneManager.UnloadSceneAsync(_activeScene);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _activeScene = scene.name.Contains("Loading") ? _activeScene : scene;
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
        //TODO -> prevent load if already loaded
        SceneManager.LoadScene("Loading_Big", LoadSceneMode.Additive);
    }

    private void MiniLoading()
    {
        //TODO -> prevent load if already loaded
        SceneManager.LoadScene("Loading_Mini", LoadSceneMode.Additive);
    }

    private void StopLoading()
    {
        if (SceneManager.GetSceneByName("Loading_Big").isLoaded) SceneManager.UnloadSceneAsync("Loading_Big");
        if (SceneManager.GetSceneByName("Loading_Mini").isLoaded) SceneManager.UnloadSceneAsync("Loading_Mini");
    }

    //private IEnumerator Loading()
    //{
    //    string loadScene = GetSceneName("Loading_Big");
    //    Debug.Log($"Going to scene {loadScene}");
    //    startTime = Time.time;

    //    AsyncOperation loadingScreenOperation = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

    //    while (!loadingScreenOperation.isDone)
    //    {
    //        yield return null;
    //    }

    //    Scene loadingScene = SceneManager.GetSceneByName(loadScene);
    //    if (!loadingScene.IsValid())
    //    {
    //        Debug.LogError("Loading scene is not valid.");
    //        yield break;
    //    }

    //    Slider progressBar = null;

    //    foreach (GameObject rootObject in loadingScene.GetRootGameObjects())
    //    {
    //        progressBar = rootObject.GetComponentInChildren<Slider>();
    //        if (progressBar != null)
    //        {
    //            break;
    //        }
    //    }

    //    if (progressBar == null)
    //    {
    //        Debug.LogError("Slider component not found in the loading scene.");
    //        yield break;
    //    }

    //    AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());

    //    while (!sceneOperation.isDone)
    //    {
    //        progressBar.value = sceneOperation.progress;
    //        yield return null;
    //    }
    //}

    public bool IsPopupLoaded()
    {
        return _sceneMode == LoadSceneMode.Additive;
    }

    /*public void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }*/
}