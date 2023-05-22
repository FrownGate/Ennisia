using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TitleScreen : MonoBehaviour
    {
        public Button Start;


        private void OnEnable()
        {
            //Register Button Events
            Start.onClick.AddListener(() => buttonCallBack(Start));

        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Start)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Start.name);
            }  
        }
    }
}
