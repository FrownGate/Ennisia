using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void OnClick()
    {
        FindObjectOfType<AudioManager>().Play("StartGame");
    }
}
