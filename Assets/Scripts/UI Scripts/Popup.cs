using UnityEngine;

public class Popup : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject _screenwidePrefab;

    //Popup game object needs to have -1 to Pos Z + Box Collider

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
        _canvas.sortingOrder = 10;

        _screenwidePrefab = Resources.Load<GameObject>("Prefabs/UI/ScreenwideBackground");
        GameObject screenwide = Instantiate(_screenwidePrefab, transform);
        screenwide.transform.SetSiblingIndex(0);
    }
    /*private void Start()
    {
        Canvas mainCanvas = FindObjectOfType<Canvas>();

        if (mainCanvas != null)
        {
            EventTrigger eventTrigger = mainCanvas.gameObject.GetComponent<EventTrigger>();

            if (eventTrigger == null)
            {
                eventTrigger = mainCanvas.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener((data) => OnCanvasPointerClick());

            eventTrigger.triggers.Add(entry);
        }
        else
        {
            Debug.LogError("Main canvas not found in the scene.");
        }
    }*/

    /*private void OnCanvasPointerClick()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Scene popupScene = SceneManager.GetSceneByName(sceneName);

        if (popupScene.IsValid())
        {
            Scene[] scenes = new Scene[SceneManager.sceneCount];
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = SceneManager.GetSceneAt(i);
            }

            for (int i = sceneCount - 1; i >= 0; i--)
            {
                Scene scene = scenes[i];
                if (scene != popupScene && scene.isLoaded)
                {
                    ScenesManager.Instance.UnloadScene(scene.name);
                    break;
                }
            }
        }
    }*/
}
