using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using strange.extensions.mediation.impl;

//房主房间信息面板
public class RoomItem :EventView 
{
	int _roomId = 0;
	Text _userNameText;
	Text _totalCountText;
	Text _winCountText;
	Button _joinRoomButton;

	protected override void Start ()
	{
		_userNameText = transform.Find ("UserNameText").GetComponent<Text> ();//用户名
		_totalCountText = transform.Find ("TotalCountText").GetComponent<Text> ();//总场数
		_winCountText = transform.Find ("WinCountText").GetComponent<Text> ();//胜利场数
		_joinRoomButton = transform.Find ("JoinRoomButton").GetComponent<Button> ();//加入房间
		base.Start ();
		_joinRoomButton.onClick.AddListener (() => {
			OnJoinRoomClick ();
		});
	}

	void OnJoinRoomClick()
	{
		//Game.Instance.RoomId = _roomId;
		//NetConn.Instance.Send (RequestCode.Room, ActionCode.JoinRoom, Game.Instance.RoomId.ToString ());
		RemoteCMD_Data recData = new RemoteCMD_Data();
		recData.cmd = RemoteCMD_Const.JoinRoom;
		recData.MatchCounts = _roomId;
		dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);
	}

	string _userName = "";
	string _totalCount = "";
	string _winCount = "";
	protected /*override*/ void  Update ()
	{		
		//base.Update ();
		if (_userName != "" && _totalCount != "" && _winCount != "") {
			_userNameText.text = _userName;
			_totalCountText.text = "总场数\n" + _totalCount;
			_totalCountText.text.Replace ("\n", "\\n");
			_winCountText.text = "胜利\n" + _winCount;
			_winCountText.text.Replace ("\n", "\\n");
		}
	}

	public void SetText(string roomId, string userName, string totalCount, string winCount)
	{
		_roomId = int.Parse (roomId);
		_userName = userName;
		_totalCount = totalCount;
		_winCount = winCount;
	}
}
