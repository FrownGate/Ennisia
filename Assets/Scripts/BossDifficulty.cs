using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaNormalNamespace
{
    public class BossDifficNormaly : MonoBehaviour
    {
        public Button Easy;
        public Button Peaceful;
        public Button Normal;
        public Button Hard;
        public Button Insane;
        public Button Ultimate;
        public Button Return;
        public Button Settings;


        public void OnEnable()
        {
            //Register Button Events
            Easy.onClick.AddListener(() => buttonCallBack(Easy));
            Peaceful.onClick.AddListener(() => buttonCallBack(Peaceful));
            Normal.onClick.AddListener(() => buttonCallBack(Normal));
            Hard.onClick.AddListener(() => buttonCallBack(Hard));
            Insane.onClick.AddListener(() => buttonCallBack(Insane));
            Ultimate.onClick.AddListener(() => buttonCallBack(Ultimate));
            Settings.onClick.AddListener(() => buttonCallBack(Settings));
            Return.onClick.AddListener(() => buttonCallBack(Return));
        }

        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Easy)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Easy.name);
            }

            if (buttonPressed == Peaceful)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Peaceful.name);
            }

            if (buttonPressed == Normal)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Normal.name);
            } 
            
            if (buttonPressed == Hard)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Hard.name);
            }
            
            if (buttonPressed == Insane)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Insane.name);
            } 
            
            if (buttonPressed == Ultimate)
            {
                //BATTLE CODE
                SceneManager.LoadScene("Battle");
                Debug.Log("Clicked: " + Ultimate.name);
            }

            if (buttonPressed == Settings)
            {
                //popup.SetActive(!popup.activeSelf);
                Debug.Log("Clicked: " + Settings.name);

            }
            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("RaidBoss");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
