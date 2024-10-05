using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
//房间信息面板
public class RoomPanel : BasePanel 
{
	Text _oneUserNameText;
	Text _oneTotalCountText;
	Text _oneWinCountText;

	Text _twoUserNameText;
	Text _twoTotalCountText;
	Text _twoWinCountText;

	Text _threeUserNameText;
	Text _threeTotalCountText;
	Text _threeWinCountText;

	Button _startGameButton;//开始游戏
	Button _leaveRoomButton;//离开房间

	protected override void Start ()
	{
		//玩家1
		_oneUserNameText = transform.Find ("OnePanel/UserNameText").GetComponent<Text> ();//用户名
		_oneTotalCountText = transform.Find ("OnePanel/TotalCountText").GetComponent<Text> ();//总场数
		_oneWinCountText = transform.Find ("OnePanel/WinCountText").GetComponent<Text> ();//胜利场数

		//玩家2
		_twoUserNameText = transform.Find ("TwoPanel/UserNameText").GetComponent<Text> ();
		_twoTotalCountText = transform.Find ("TwoPanel/TotalCountText").GetComponent<Text> ();
		_twoWinCountText = transform.Find ("TwoPanel/WinCountText").GetComponent<Text> ();

		//玩家3
		_threeUserNameText = transform.Find("ThreePanel/UserNameText").GetComponent<Text>();
		_threeTotalCountText = transform.Find("ThreePanel/TotalCountText").GetComponent<Text>();
		_threeWinCountText = transform.Find("ThreePanel/WinCountText").GetComponent<Text>();

		_startGameButton = transform.Find ("StartGameButton").GetComponent<Button> ();
		_leaveRoomButton = transform.Find ("LeaveRoomButton").GetComponent<Button> ();
		base.Start ();
		_startGameButton.onClick.AddListener (() => {
			OnStartGameClick ();
		});
		_leaveRoomButton.onClick.AddListener (() => {
			OnLeaveRoomClick ();
		});
	}

	void OnStartGameClick()
	{
	}

	void OnLeaveRoomClick()
	{
		NetConn.Instance.Send (RequestCode.Room, ActionCode.LeaveRoom, "");
	}
	//创建房间
	public void OnCreateRoomResponse(string data)
	{
		if (data == "1") {
			_userOne = Game.Instance.User;
			Game.Instance.ShowMessage ("创建房间成功");
			//SetOneText (user.Name, user.TotalCount.ToString (), user.WinCount.ToString ());
			//ClearTwoText ();
			_userTwo = null;
			_userThree = null;
			Game.Instance.HidePanel ("RoomListPanel");
		}
	}

	string _oneUserName = "";
	string _oneTotalCount = "";
	string _oneWinCount = "";
	string _twoUserName = "";
	string _twoTotalCount = "";
	string _twoWinCount = "";
	string _threeUserName = "";
	string _threeTotalCount = "";
	string _threeWinCount = "";
	//玩家1信息列表
	void SetOneText(string userName, string totalCount, string winCount)
	{
		_oneUserNameText.text = userName;
		_oneTotalCountText.text = "总场数\n" + totalCount;
		_oneTotalCountText.text.Replace ("\n", "\\n");
		_oneWinCountText.text = "胜利\n" + winCount;
		_oneWinCountText.text.Replace ("\n", "\\n");
	}
	//玩家2信息列表
	void SetTwoText(string userName, string totalCount, string winCount)
	{
		_twoUserNameText.text = userName;
		_twoTotalCountText.text = "总场数\n" + totalCount;
		_twoTotalCountText.text.Replace ("\n", "\\n");
		_twoWinCountText.text = "胜利\n" + winCount;
		_twoWinCountText.text.Replace ("\n", "\\n");
	}

	void SetThreeText(string userName, string totalCount, string winCount)
	{
		_threeUserNameText.text = userName;
		_threeTotalCountText.text = "总场数\n" + totalCount;
		_threeTotalCountText.text.Replace("\n", "\\n");
		_threeWinCountText.text = "胜利\n" + winCount;
		_threeWinCountText.text.Replace("\n", "\\n");
	}

	void ClearTwoText()
	{
		_twoUserNameText.text = "等待加入";
		_twoTotalCountText.text = "";
		_twoWinCountText.text = "";
	}

	void ClearThreeText()
	{
		_threeUserNameText.text = "等待加入";
		_threeTotalCountText.text = "";
		_threeWinCountText.text = "";
	}

	UserData _userOne = null;
	UserData _userTwo = null;
	UserData _userThree = null;

	protected override void Update ()
	{
		base.Update ();
		if (_userOne != null) {
			SetOneText (_userOne.Name, _userOne.TotalCount.ToString (), _userOne.WinCount.ToString ());
		}

		if (_userTwo != null) {
			SetTwoText (_userTwo.Name, _userTwo.TotalCount.ToString (), _userTwo.WinCount.ToString ());
		}
		else {
			ClearTwoText ();
		}

		if (_userThree != null)
		{
			SetTwoText(_userThree.Name, _userThree.TotalCount.ToString(), _userThree.WinCount.ToString());
		}
		else
		{
			ClearThreeText();
		}
	}

	public void OnJoinRoomResponse(string data)
	{
		if (data != "") {
			if (data.Contains ("#")) {//用户之间以#分隔
				Debug.Log (data);
				var room = data.Split ('#');
				var arrayOne = room [0].Split (';');//字段之间以;分隔
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);//用户ID
				_userOne.Name = arrayOne [1];//用户名
				_userOne.TotalCount = int.Parse (arrayOne [2]);//总场数
				_userOne.WinCount = int.Parse (arrayOne [3]);//胜利场数

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
				var arrayThree = room[2].Split(';');
				_userThree = new UserData();
				_userThree.Id = int.Parse(arrayThree[0]);
				_userThree.Name = arrayThree[1];
				_userThree.TotalCount = int.Parse(arrayThree[2]);
				_userThree.WinCount = int.Parse(arrayThree[3]);
			} 
			else {
				Debug.Log (data);
				if (data.Contains (";")) {
					var array = data.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;

					//ClearTwoText ();
				}
			}
			data = "";
		}
	}

	/*
	public void OnGetRoomInfoResponse(string data)
	{
		if (data != "") {
			if (data.Contains ("#")) {
				Debug.Log (data);
				var room = data.Split ('#');
				var arrayOne = room [0].Split (';');
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);
				_userOne.Name = arrayOne [1];
				_userOne.TotalCount = int.Parse (arrayOne [2]);
				_userOne.WinCount = int.Parse (arrayOne [3]);

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
			} 
			else {
				Debug.Log (data);
				if (data.Contains (";")) {
					var array = data.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;

					//ClearTwoText ();
				}
			}
		}
	}
	*/

	public void OnLeaveRoomResponse(string data)//离开房间
	{
		if (data != "") {
			if (data == "0") {
				Game.Instance.ShowMessage ("房主离开了房间");
				Game.Instance.HidePanel ("RoomPanel");
				Game.Instance.HidePanel ("RoomListPanel");
				_userOne = null;
				_userTwo = null;
				return;
			}
			else if (data.Contains ("#")) {
				var room = data.Split ('#');
				var arrayOne = room [0].Split (';');
				_userOne = new UserData ();
				_userOne.Id = int.Parse (arrayOne [0]);
				_userOne.Name = arrayOne [1];
				_userOne.TotalCount = int.Parse (arrayOne [2]);
				_userOne.WinCount = int.Parse (arrayOne [3]);

				//SetOneText (arrayOne [1], arrayOne [2], arrayOne [3]);

				var arrayTwo = room [1].Split (';');
				_userTwo = new UserData ();
				_userTwo.Id = int.Parse (arrayTwo [0]);
				_userTwo.Name = arrayTwo [1];
				_userTwo.TotalCount = int.Parse (arrayTwo [2]);
				_userTwo.WinCount = int.Parse (arrayTwo [3]);

				//SetTwoText (arrayTwo [1], arrayTwo [2], arrayTwo [3]);
				var arrayThree = room[2].Split(';');
				_userThree = new UserData();
				_userThree.Id = int.Parse(arrayTwo[0]);
				_userThree.Name = arrayTwo[1];
				_userThree.TotalCount = int.Parse(arrayTwo[2]);
				_userThree.WinCount = int.Parse(arrayTwo[3]);
			} 
			else {
				if (data.Contains (";")) {
					var array = data.Split (';');
					_userOne = new UserData ();
					_userOne.Id = int.Parse (array [0]);
					_userOne.Name = array [1];
					_userOne.TotalCount = int.Parse (array [2]);
					_userOne.WinCount = int.Parse (array [3]);

					//SetOneText (array [1], array [2], array [3]);

					_userTwo = null;
					_userThree = null;
					//ClearTwoText ();
				}
			}
			if (data == "2") {
				Game.Instance.ShowMessage ("离开房间成功");
				Game.Instance.HidePanel ("RoomPanel");
				Game.Instance.HidePanel ("RoomListPanel");
				_userOne = null;
				_userTwo = null;
				_userThree = null;
				return;
			}
			data = "";
		}
	}
}
