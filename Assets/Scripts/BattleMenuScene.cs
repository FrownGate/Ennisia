using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class BattleMenu : MonoBehaviour
    {
        public Button Settings;
        public Button Helmet;
        public Button Earrings;
        public Button Necklace;
        public Button Ring;
        public Button Chest;
        public Button Boots;
        public Button Return;

        public void OnEnable()
        {
            //Register Button Events
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Helmet.onClick.AddListener(() => buttonCallBack(Helmet));
            Earrings.onClick.AddListener(() => buttonCallBack(Earrings));
            Necklace.onClick.AddListener(() => buttonCallBack(Necklace));
            Ring.onClick.AddListener(() => buttonCallBack(Ring));
            Chest.onClick.AddListener(() => buttonCallBack(Chest));
            Boots.onClick.AddListener(() => buttonCallBack(Boots));
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

            if (buttonPressed == Earrings)
            {
                Debug.Log("Clicked: " + Earrings.name);
            } 
            
            if (buttonPressed == Helmet)
            {
                Debug.Log("Clicked: " + Helmet.name);
            }

            if (buttonPressed == Necklace)
            {
                Debug.Log("Clicked: " + Necklace.name);
            }

            if (buttonPressed == Ring)
            {
                Debug.Log("Clicked: " + Ring.name);
            }

            if (buttonPressed == Chest)
            {
                Debug.Log("Clicked: " + Chest.name);
            } 
            
            if (buttonPressed == Boots)
            {
                Debug.Log("Clicked: " + Boots.name);
            }

            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
