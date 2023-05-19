using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class BattleScene : MonoBehaviour
    {
        public Button Skill1;
        public Button Skill2;
        public Button Ult;
        public Button Return;


        public void OnEnable()
        {
            //Register Button Events
            Skill1.onClick.AddListener(() => buttonCallBack(Skill1));
            Skill2.onClick.AddListener(() => buttonCallBack(Skill2));
            Ult.onClick.AddListener(() => buttonCallBack(Ult));
            Return.onClick.AddListener(() => buttonCallBack(Return));

        }


        private void buttonCallBack(Button buttonPressed)
        {
            if (buttonPressed == Skill1)
            {
                //BATTLE CODE
                Debug.Log("Clicked: " + Skill1.name);
            } 
            
            if (buttonPressed == Skill2)
            {
                //BATTLE CODE
                
                Debug.Log("Clicked: " + Skill2.name);
            }
            
            if (buttonPressed == Ult)
            {
                //BATTLE CODE
                
                Debug.Log("Clicked: " + Ult.name);
            }

            if (buttonPressed == Return)
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Clicked: " + Return.name);
            }
        }
    }
}
