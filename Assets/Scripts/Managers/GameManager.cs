using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : MonoBehaviour
{
    private const string GameManagerKey = "GameManager";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiateGameManager()
    {
        Addressables.InstantiateAsync(GameManagerKey).Completed += operationHandle =>
        {
            DontDestroyOnLoad(operationHandle.Result);
        };
    }
    
}
    