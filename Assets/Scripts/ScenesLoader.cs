using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public enum ButtonType
    {
        SETTING_BTN, 
        BATTLE_BTN,
        SUMMON_BTN,
        SHOP_BTN,
        STUFF_BTN,
        RETURN_BTN
    }

    public class MainMenu : MonoBehaviour
    {
        public Button Settings;
        public Button Summon;
        public Button Battle;
        public Button Shop;
        public Button Stuff;
        public Button Return;
        /*public void LoadGame()
        {
            
        }*/
        //public void EndGame() => Application.Quit();

        public void OnEnable()
        {
            //Register Button Events
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Summon.onClick.AddListener(() => buttonCallBack(Summon));
            Battle.onClick.AddListener(() => buttonCallBack(Battle));
            Shop.onClick.AddListener(() => buttonCallBack(Shop));
            Stuff.onClick.AddListener(() => buttonCallBack(Stuff));
            Return.onClick.AddListener(() => buttonCallBack(Return));
        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Settings)
            {
                //Your code for button 1
                //SceneManager.LoadScene("");
                Debug.Log("Clicked: " + Settings.name);
            }

            if (buttonPressed == Summon)
            {
                SceneManager.LoadScene("SummonMenu");

                Debug.Log("Clicked: " + Summon.name);
            }

            if (buttonPressed == Battle)
            {
                SceneManager.LoadScene("Battle");
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
            
            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}



