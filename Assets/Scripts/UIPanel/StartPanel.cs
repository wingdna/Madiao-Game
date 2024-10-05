using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//用户登录面板
public class StartPanel : BasePanel
{
	Button _loginButton;

	protected override void Start ()
	{
		_loginButton = GetComponent<Button> ();
		base.Start ();
		_loginButton.onClick.AddListener (() => {
			OnLoginClick ();
		});
	}

	void OnLoginClick()
	{
		Game.Instance.ShowPanel ("LoginPanel");
		Game.Instance.HidePanel (name);
	}
}
