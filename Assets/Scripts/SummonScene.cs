using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaBannerNamespace
{
    public class SummonScene : MonoBehaviour
    {
        public Button pull1;
        public Button pull10;
        //public Button Banner;
        public Button Return;


        private void OnEnable()
        {
            //Register Button Events
            pull1.onClick.AddListener(() => buttonCallBack(pull1));
            pull10.onClick.AddListener(() => buttonCallBack(pull10));
            //Banner.onClick.AddListener(() => buttonCallBack(Banner));
            Return.onClick.AddListener(() => buttonCallBack(Return));

        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == pull1)
            {
                //SUMMON CODE
                Debug.Log("Clicked: " + pull1.name);
            }

            if (buttonPressed == pull10)
            {
                //SUMMON CODE

                Debug.Log("Clicked: " + pull10.name);
            }

            /*if (buttonPressed == Banner)
            {
                //SUMMON CODE

                Debug.Log("Clicked: " + Banner.name);
            }*/

            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
