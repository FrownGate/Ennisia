using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField, Scene] private string _loadingBig;
    [SerializeField, Scene] private string _loadingMini;

    public static ScenesManager Instance { get; private set; }
    public string Params { get; private set; } //TODO -> remove

    private Scene _activeScene;
    private Scene _previousScene;
    private string _sceneToLoad;
    private bool _isPopupLoaded;

    private bool _isMiniLoading;
    private bool _isBigLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

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
        _isMiniLoading = false;
        _isBigLoading = false;
        _isPopupLoaded = false;
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

    public void SetScene(string scene)
    {
        _sceneToLoad = GetSceneName(scene);
        if (_isPopupLoaded && _sceneToLoad.Contains("Popup")) return;
        Debug.Log($"Going to scene {_sceneToLoad}");
        StartCoroutine(LoadScene());
    }

    private string GetSceneName(string scene)
    {
        string[] splittedName = scene.Split('#');
        Params = splittedName.Length > 1 ? splittedName[1] : null;
        return splittedName[0];
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(_sceneToLoad, SceneMode());
        MiniLoading();

        while (!loading.isDone) yield return null;
        StopLoading();
    }

    private LoadSceneMode SceneMode()
    {
        if (_sceneToLoad.Contains("Popup"))
        {
            _isPopupLoaded = true;
            return LoadSceneMode.Additive;
        }
        else
        {
            return LoadSceneMode.Single;
        }
    }

    public void UnloadPopup(string scene)
    {
        Scene popup = SceneManager.GetSceneByName(scene);
        UnloadPopup(popup);
    }

    public void UnloadPopup(Scene scene)
    {
        _activeScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _activeScene = scene.name.Contains("Loading") ? _activeScene : scene;

        //Debug.Log($"{_activeScene.name} loaded !");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _previousScene = scene;
        //Debug.Log($"{_previousScene.name} unloaded !");

        if (_previousScene.name == _loadingMini) _isMiniLoading = false;
        if (_previousScene.name.Contains("Popup")) _isPopupLoaded = false;
    }

    private void BigLoading()
    {
        _isBigLoading = true;
        SceneManager.LoadScene(_loadingBig, LoadSceneMode.Additive);
    }

    private void MiniLoading()
    {
        if (_isMiniLoading || _isBigLoading) return;
        _isMiniLoading = true;
        SceneManager.LoadScene(_loadingMini, LoadSceneMode.Additive);
    }

    private void StopLoading()
    {
        if (SceneManager.GetSceneByName(_loadingMini).isLoaded) SceneManager.UnloadSceneAsync(_loadingMini);
    }

    private void StopBigLoading()
    {
        _isBigLoading = false;
        if (SceneManager.GetSceneByName(_loadingBig).isLoaded) SceneManager.UnloadSceneAsync(_loadingBig);
    }
}