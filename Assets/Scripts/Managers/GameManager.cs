using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    private const string GameManagerKey = "GameManager";
    public static GameManager Instance = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateGameManager()
    {
        if (Instance == null)
        {
            Addressables.InstantiateAsync(GameManagerKey).Completed += operationHandle =>
            {
                Instance = operationHandle.Result.GetComponent<GameManager>();
                DontDestroyOnLoad(operationHandle.Result);
            };
        }

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
}
    