using UnityEngine;
using System.Collections.Generic;
using Common;

public class Game : MonoBehaviour
{
	static Game _instance;
	private const float REFERENCE_RESOLUTION_WIDTH = 1920f; // 参考分辨率宽度
	private const float REFERENCE_RESOLUTION_HEIGHT = 1080f; // 参考分辨率高度

	public static Game Instance { get { return _instance; } }

	void Awake()
	{
		if (_instance == null) {
			_instance = this;
		}
		InitPanel ();
	}

	void Start ()
	{
		NetConn.Instance.Connect ("111.67.200.250", 50000);
		NetConn.Instance.Send (RequestCode.Default, ActionCode.Default, "Hello,Client");
	}
	
	void Update ()
	{
	}

	public UserData User;
	public int RoomId;
	public void OnResponse(int requestCode, int actionCode, string data)
	{
		if (requestCode == RequestCode.Default) {
			if (actionCode == ActionCode.Default) {
				Debug.Log (data);
			}
		}
		else if (requestCode == RequestCode.User) {//用户登录
			if (actionCode == ActionCode.Login) {
				var loginPanel = ShowPanel ("LoginPanel") as LoginPanel;
				loginPanel.OnLoginResponse (data);
			}
			else if (actionCode == ActionCode.Register) {//用户注册
				var registerPanel = ShowPanel ("RegisterPanel") as RegisterPanel;
				registerPanel.OnRegisterResponse (data);
			}
		}
		else if (requestCode == RequestCode.Result) {//请求战绩信息
			if (actionCode == ActionCode.GetResultInfo) {
				var resultInfoPanel = ShowPanel ("ResultInfoPanel") as ResultInfoPanel;
				resultInfoPanel.OnGetResultInfoResponse (data);
			}
		}
		else if (requestCode == RequestCode.Room) {//房间面板
			if (actionCode == ActionCode.CreateRoom) {//创建房间
				var roomPanel = ShowPanel ("RoomPanel") as RoomPanel;
				roomPanel.OnCreateRoomResponse (data);
			}
			else if (actionCode == ActionCode.GetRoomList) {//获取房间列表
				var roomListPanel = ShowPanel ("RoomListPanel") as RoomListPanel;
				roomListPanel.OnGetRoomListResponse (data);
			}
			else if (actionCode == ActionCode.JoinRoom) {//加入房间
				HidePanel ("RoomListPanel");
				var roomPanel = ShowPanel ("RoomPanel") as RoomPanel;
				roomPanel.OnJoinRoomResponse (data);
			}
			else if (actionCode == ActionCode.LeaveRoom) {//离开房间
				var roomPanel = ShowPanel ("RoomPanel") as RoomPanel;
				roomPanel.OnLeaveRoomResponse (data);
			}
			/*
			else if (actionCode == ActionCode.GetRoomInfo) {
				var roomPanel = ShowPanel ("RoomPanel") as RoomPanel;
				roomPanel.OnGetRoomInfoResponse (data);
			}
			*/
		}
	}

	void OnDestroy()
	{
		if (NetConn.Instance.Connected == true) {
			NetConn.Instance.Close ();
		}
	}

	private void AdjustScreen()
	{
		float screenWidth = Screen.width; // 获取当前屏幕宽度
		float screenHeight = Screen.height; // 获取当前屏幕高度
		float screenAspectRatio = screenWidth / screenHeight; // 获取当前屏幕宽高比

		float referenceAspectRatio = REFERENCE_RESOLUTION_WIDTH / REFERENCE_RESOLUTION_HEIGHT; // 获取参考宽高比

		// 如果当前宽高比小于参考宽高比，说明是窄屏，需要将摄像机视野进行调整
		if (screenAspectRatio < referenceAspectRatio)
		{
			float targetWidth = screenHeight / REFERENCE_RESOLUTION_HEIGHT * REFERENCE_RESOLUTION_WIDTH;
			float widthDiff = screenWidth - targetWidth;

			Camera.main.orthographicSize *= targetWidth / screenWidth;

			Camera.main.transform.position += new Vector3(widthDiff / 2f, 0f, 0f);
		}
		// 如果当前宽高比大于参考宽高比，说明是宽屏，需要调整摄像机的位置
		else if (screenAspectRatio > referenceAspectRatio)
		{
			float targetHeight = screenWidth / REFERENCE_RESOLUTION_WIDTH * REFERENCE_RESOLUTION_HEIGHT;
			float heightDiff = screenHeight - targetHeight;

			Camera.main.transform.position += new Vector3(0f, heightDiff / 2f, 0f);
		}
	}



	#region UIManager
	Dictionary<string,BasePanel> _panelDict = new Dictionary<string, BasePanel> ();
	void InitPanel()
	{
		var startPanel = ViewManager.OpenPanel ("StartPanel", "Canvas");
		var loginPanel = ViewManager.OpenPanel ("LoginPanel", "Canvas");
		var registerPanel = ViewManager.OpenPanel ("RegisterPanel", "Canvas");
		var mainPanel = ViewManager.OpenPanel ("MainPanel", "Canvas");
		var resultInfoPanel = ViewManager.OpenPanel ("ResultInfoPanel", "Canvas");
		var messagePanel = ViewManager.OpenPanel ("MessagePanel", "Canvas");
		var roomListPanel = ViewManager.OpenPanel ("RoomListPanel", "Canvas");
		var roomPanel = ViewManager.OpenPanel ("RoomPanel", "Canvas");

		_panelDict.Add (startPanel.name, startPanel);
		_panelDict.Add (loginPanel.name, loginPanel);
		_panelDict.Add (registerPanel.name, registerPanel);
		_panelDict.Add (mainPanel.name, mainPanel);
		_panelDict.Add (resultInfoPanel.name, resultInfoPanel);
		_panelDict.Add (messagePanel.name, messagePanel);
		_panelDict.Add (roomListPanel.name, roomListPanel);
		_panelDict.Add (roomPanel.name, roomPanel);

		ShowPanel (startPanel.name);
		HidePanel (loginPanel.name);
		HidePanel (registerPanel.name);
		HidePanel (mainPanel.name);
		HidePanel (resultInfoPanel.name);
		HidePanel (messagePanel.name);
		HidePanel (roomListPanel.name);
		HidePanel (roomPanel.name);
	}
	public BasePanel ShowPanel(string panelName)
	{
		BasePanel panel;
		bool isGet = _panelDict.TryGetValue (panelName, out panel);
		if (isGet == true) {
			panel.OnEnter ();
		}
		return panel;
	}
	public void HidePanel(string panelName)
	{
		BasePanel panel;
		bool isGet = _panelDict.TryGetValue (panelName, out panel);
		if (isGet == true) {
			panel.OnExit ();
		}
	}
	public void HideAllPanel()
	{
		foreach (var item in _panelDict.Values) {
			item.OnExit ();
		}
	}
	public void ShowMessage(string message, float showMessageTime = 2)
	{
		var messagePanel = ShowPanel ("MessagePanel") as MessagePanel;
		messagePanel.ShowMessage (message, showMessageTime);
	}
	#endregion
}

public class UserData
{
	public int Id;
	public string Name;
	public int TotalCount;
	public int WinCount;
}
