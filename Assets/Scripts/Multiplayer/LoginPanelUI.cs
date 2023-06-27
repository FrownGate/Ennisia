using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelUI : MonoBehaviour
{

	public ClientStartUp ClientStartUp;
	public ServerStartUp ServerStartUp;

	public Button LoginButton;
	public Button StartLocalServerButton;

    private void Start()
    {
		LoginButton.onClick.AddListener(ClientStartUp.OnLoginUserButtonClick);
		StartLocalServerButton.onClick.AddListener(ServerStartUp.OnStartLocalServerButtonClick);
	}
}
