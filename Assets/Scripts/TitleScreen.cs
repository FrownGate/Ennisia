using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TitileScreen : MonoBehaviour
    {
        public Button Start;


        public void OnEnable()
        {
            //Register Button Events
            Start.onClick.AddListener(() => buttonCallBack(Start));

        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Start)
            {
                //Your code for button 1
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Start.name);
            }  
        }
    }
}
