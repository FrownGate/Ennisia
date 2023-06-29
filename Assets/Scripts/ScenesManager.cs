using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField, Scene] private string _loadingBig;
    [SerializeField, Scene] private string _loadingMini;

    public static ScenesManager Instance { get; private set; }
    public string Params { get; private set; }

    private Scene _activeScene;
    private Scene _previousScene;
    private LoadSceneMode _sceneMode;
    private string _sceneToLoad;

    private bool _loading;
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
            PlayFabManager.OnLoadingEnd += StopLoading;
            PlayFabManager.OnBigLoadingStart += BigLoading;
            PlayFabManager.OnLoginSuccess += StopBigLoading;

            Params = null;
            _activeScene = SceneManager.GetActiveScene();
            _loading = false;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        PlayFabManager.OnLoadingStart -= MiniLoading;
        PlayFabManager.OnLoadingEnd -= StopLoading;
        PlayFabManager.OnBigLoadingStart -= BigLoading;
        PlayFabManager.OnLoginSuccess -= StopBigLoading;
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
        StartCoroutine(LoadingScene());
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

        //Debug.Log($"{_activeScene.name} loaded !");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _previousScene = scene;
        //Debug.Log($"{_previousScene.name} unloaded !");

        if (_previousScene.name == _loadingMini) _loading = false;
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
        SceneManager.LoadScene(_loadingBig, LoadSceneMode.Additive);
    }

    private void MiniLoading()
    {
        if (_loading) return;
        _loading = true;
        SceneManager.LoadScene(_loadingMini, LoadSceneMode.Additive);
    }

    private void StopLoading()
    {
        if (SceneManager.GetSceneByName(_loadingMini).isLoaded) SceneManager.UnloadSceneAsync(_loadingMini);
    }

    private void StopBigLoading()
    {
        if (SceneManager.GetSceneByName(_loadingBig).isLoaded) SceneManager.UnloadSceneAsync(_loadingBig);
    }

    private IEnumerator LoadingScene()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
        MiniLoading();

        while (!loading.isDone) yield return null;
        StopLoading();
    }

    public bool IsPopupLoaded()
    {
        return _sceneMode == LoadSceneMode.Additive;
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
}