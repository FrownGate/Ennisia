using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class StuffMenu : MonoBehaviour
    {
        public Button Settings;
        public Button Helmet;
        public Button Earrings;
        public Button Necklace;
        public Button Ring;
        public Button Chest;
        public Button Boots;
        public Button Weapon;
        public Button Supp1;
        public Button Supp2;
        public GameObject PopupEmpty;
        public Button Return;
        public Button Close;

        private void OnEnable()
        {
            //Register Button Events
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Helmet.onClick.AddListener(() => buttonCallBack(Helmet));
            Earrings.onClick.AddListener(() => buttonCallBack(Earrings));
            Necklace.onClick.AddListener(() => buttonCallBack(Necklace));
            Ring.onClick.AddListener(() => buttonCallBack(Ring));
            Chest.onClick.AddListener(() => buttonCallBack(Chest));
            Boots.onClick.AddListener(() => buttonCallBack(Boots));
            Weapon.onClick.AddListener(() => buttonCallBack(Weapon));
            Supp1.onClick.AddListener(() => buttonCallBack(Supp1));
            Supp2.onClick.AddListener(() => buttonCallBack(Supp2));
            Return.onClick.AddListener(() => buttonCallBack(Return));
            Close.onClick.AddListener(() => buttonCallBack(Close));
        }

        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Settings)
            {
                Debug.Log("Clicked: " + Settings.name);
            }

            if (buttonPressed == Earrings)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Earrings.name);
            } 
            
            if (buttonPressed == Helmet)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Helmet.name);
            }

            if (buttonPressed == Necklace)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Necklace.name);
            }

            if (buttonPressed == Ring)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Ring.name);
            }

            if (buttonPressed == Chest)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Chest.name);
            } 
            
            if (buttonPressed == Boots)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Boots.name);
            } 
            
            if (buttonPressed == Weapon)
            {
                Debug.Log("Clicked: " + Weapon.name);
            } 
            
            if (buttonPressed == Supp1)
            {
                Debug.Log("Clicked: " + Supp1.name);
            } 
            
            if (buttonPressed == Supp2)
            {
                Debug.Log("Clicked: " + Supp2.name);
            }

            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }  
            
            if (buttonPressed == Close)
            {
                PopupEmpty.SetActive(!PopupEmpty.activeSelf);
                Debug.Log("Clicked: " + Close.name);
            }
        }
    }
}
