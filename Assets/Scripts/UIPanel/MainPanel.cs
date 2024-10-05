using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class MainPanel : BasePanel
{
	Button _getResultInfoButton;
	Button _getRoomListButton;

	protected override void Start ()
	{
		_getResultInfoButton = transform.Find ("GetResultInfoButton").GetComponent<Button> ();//战绩按钮
		_getRoomListButton = transform.Find ("GetRoomListButton").GetComponent<Button> ();//房间列表按钮
		base.Start ();
		_getResultInfoButton.onClick.AddListener (() => {
			OnGetResultInfoClick ();
		});
		_getRoomListButton.onClick.AddListener (() => {
			OnGetRoomListClick ();
		});
	}

	void OnGetResultInfoClick()//点击战绩信息
	{
		if (Game.Instance.User == null) {
			return;
		}
		NetConn.Instance.Send (RequestCode.Result, ActionCode.GetResultInfo, Game.Instance.User.Id.ToString ());
	}

	void OnGetRoomListClick()//点击房间列表
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.GetRoomList, "");
	}
}
