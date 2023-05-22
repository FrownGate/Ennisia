using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class RaidBoss : MonoBehaviour
    {
        public Button Betala;
        public Button Shado;
        public Button Laijande;
        public Button Return;
        public Button Settings;


        private void OnEnable()
        {
            //Register Button Events
            Betala.onClick.AddListener(() => buttonCallBack(Betala));
            Shado.onClick.AddListener(() => buttonCallBack(Shado));
            Laijande.onClick.AddListener(() => buttonCallBack(Laijande));
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Return.onClick.AddListener(() => buttonCallBack(Return));
        }

        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Betala)
            {
                //BATTLE CODE
                SceneManager.LoadScene("RaidDifficulty");
                Debug.Log("Clicked: " + Betala.name);
            }

            if (buttonPressed == Shado)
            {
                //BATTLE CODE
                SceneManager.LoadScene("RaidDifficulty");
                Debug.Log("Clicked: " + Shado.name);
            }

            if (buttonPressed == Laijande)
            {
                //BATTLE CODE
                SceneManager.LoadScene("RaidDifficulty");
                Debug.Log("Clicked: " + Laijande.name);
            }

            if (buttonPressed == Settings)
            {
                //popup.SetActive(!popup.activeSelf);
                Debug.Log("Clicked: " + Settings.name);

            }
            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
