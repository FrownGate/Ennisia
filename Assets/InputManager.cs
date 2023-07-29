using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    void Awake()
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
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var stat in PlayFabManager.Instance.Player.Stats)
            {
                Debug.Log(stat);
            }
        }
    }
}
