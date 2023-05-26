using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
	public class MainMenu : MonoBehaviour
	{
		private void OnMouseDown()
		{
			try
			{
				//gameObject.SetActive(!gameObject.activeSelf);
			}
			catch
			{
				Debug.LogError("gameObject not found");
			}
		}
	}
}
