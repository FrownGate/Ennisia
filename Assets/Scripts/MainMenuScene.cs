using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class MainMenu : MonoBehaviour
    {
        public Button Settings;
        public Button Summon;
        public Button Battle;
        public Button Shop;
        public Button Stuff;

        private void OnEnable()
        {
            //Register Button Events
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Summon.onClick.AddListener(() => buttonCallBack(Summon));
            Battle.onClick.AddListener(() => buttonCallBack(Battle));
            Shop.onClick.AddListener(() => buttonCallBack(Shop));
            Stuff.onClick.AddListener(() => buttonCallBack(Stuff));
        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Settings)
            {
                Debug.Log("Clicked: " + Settings.name);
            }

            if (buttonPressed == Summon)
            {
                SceneManager.LoadScene("SummonMenu");
                Debug.Log("Clicked: " + Summon.name);
            }

            if (buttonPressed == Battle)
            {
                SceneManager.LoadScene("RaidBoss");
                Debug.Log("Clicked: " + Battle.name);
            }

            if (buttonPressed == Shop)
            {
                SceneManager.LoadScene("ShopMenu");
                Debug.Log("Clicked: " + Shop.name);
            }

            if (buttonPressed == Stuff)
            {
                SceneManager.LoadScene("StuffMenu");
                Debug.Log("Clicked: " + Stuff.name);
            } 
        }
    }
}
