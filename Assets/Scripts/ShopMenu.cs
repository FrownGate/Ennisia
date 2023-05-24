using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Defapack3Namespace
{
    public class SummonScene : MonoBehaviour
    {
        public Button pack1;
        public Button pack2;
        public Button pack3;
        public Button pack4;
        public Button Settings;
        public Button Return;


        private void OnEnable()
        {
            //Register Button Events
            pack1.onClick.AddListener(() => buttonCallBack(pack1));
            pack2.onClick.AddListener(() => buttonCallBack(pack2));
            pack3.onClick.AddListener(() => buttonCallBack(pack3));
            pack4.onClick.AddListener(() => buttonCallBack(pack4));
            Return.onClick.AddListener(() => buttonCallBack(Return));
            Settings.onClick.AddListener(() => buttonCallBack(Settings));

        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == pack1)
            {
                //SUMMON CODE
                Debug.Log("Clicked: " + pack1.name);
            }

            if (buttonPressed == pack2)
            {
                //SUMMON CODE

                Debug.Log("Clicked: " + pack2.name);
            }

            if (buttonPressed == Settings)
            {
                //SUMMON CODE

                Debug.Log("Clicked: " + Settings.name);
            }

            if (buttonPressed == pack3)
            {
                //SUMMON CODE
                Debug.Log("Clicked: " + pack3.name);
            }
            
            if (buttonPressed == pack4)
            {
                //SUMMON CODE
                Debug.Log("Clicked: " + pack4.name);
            }

            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
