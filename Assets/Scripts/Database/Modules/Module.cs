using UnityEngine;

public class Module : MonoBehaviour
{
    protected PlayFabManager _manager;

    public void Init(PlayFabManager manager) { _manager = manager; }
}