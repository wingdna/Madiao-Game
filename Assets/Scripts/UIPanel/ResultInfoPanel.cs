using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战绩信息面板
public class ResultInfoPanel : BasePanel
{
	Text _userNameText;//名字
	Text _totalCountText;//总场次
	Text _winCountText;//胜利场次
	Button _closeButton;//关闭按钮

	protected override void Start ()
	{
		_userNameText = transform.Find ("UserNameText").GetComponent<Text> ();
		_totalCountText = transform.Find ("TotalCountText").GetComponent<Text> ();
		_winCountText = transform.Find ("WinCountText").GetComponent<Text> ();
		_closeButton = transform.Find ("CloseButton").GetComponent<Button> ();
		base.Start ();
		_closeButton.onClick.AddListener (() => {
			OnCloseClick ();
		});
	}

	void OnCloseClick()
	{
		Game.Instance.HidePanel ("ResultInfoPanel");
	}

	protected override void Update ()
	{
		base.Update ();
		if (Game.Instance.User != null) {
			var user = Game.Instance.User;
			_userNameText.text = user.Name;
			_totalCountText.text = "总场数：" + user.TotalCount;
			_winCountText.text = "胜利：" + user.WinCount;
		}
	}

	public void OnGetResultInfoResponse(string data)
	{
		if (data.Contains(";")) {
			var array = data.Split (';');
			UserData user = new UserData ();
			user.Id = int.Parse (array [0]);
			user.Name = array [1];
			user.TotalCount = int.Parse (array [2]);
			user.WinCount = int.Parse (array [3]);
			Game.Instance.User = user;
		}
	}
}
