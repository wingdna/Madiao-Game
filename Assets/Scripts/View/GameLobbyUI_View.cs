using UnityEngine;
using strange.extensions.mediation.impl;
using UnityEngine.UI;
using Assets.Scripts;
using System.Collections.Generic;

public class GameLobbyUI_View : EventView
{
    private GameObject PlayerInfo_Panel;//人物信息
    private GameObject Matching_Panel;//匹配等待界面
    private GameObject Setting_Panel;//设置面板
    private GameObject Teach_Panel;//教学面板
    private GameObject RoomList_Panel;//房间面板

    private Button Btn_Match;//匹配
    private Button Btn_PVE;//人机
    private Button Btn_Quit;//退出
    private Button Btn_Cancel;//取消匹配
    private Button Btn_OK;//确认按钮
    private Button Btn_Close;//关闭按钮 教学窗口
    private Button Btn_Setting;//设置按钮
    private Button Btn_Teach;//教学按钮    
    private Button Btn_RoomList;//房间列表按钮

    RoomItem RoomItemPrefab;
    Button CreateRoomButton;
    Button UpdateListButton;
    Button CloseListButton;

    private Text Name_Txt;//昵称
    private Text Grade_Txt;//分数
    private Image Role_IMG;//头像
    private Text Msg_Txt;//匹配消息
    private InputField Input_IP;//IP
    private InputField Input_Port;//Port
    public bool isMusicOn;
    public bool isSoundOn;
    
    private bool _haveItem = false;
    private string strdata = "";
    public void Init()
    {
        Btn_Match = transform.Find("Btn_Match").GetComponent<Button>();
        Btn_Match.onClick.AddListener(Click_BtnMatch);//匹配
        Btn_PVE = transform.Find("Btn_PVE").GetComponent<Button>();
        Btn_PVE.onClick.AddListener(Click_BtnPVE); //人机
        Btn_Quit = transform.Find("Btn_Quit").GetComponent<Button>();
        Btn_Quit.onClick.AddListener(Click_BtnQuit);//退出
        Btn_Setting = transform.Find("Btn_Setting").GetComponent<Button>();
        Btn_Setting.onClick.AddListener(Click_Setting);//设置
        Btn_Teach = transform.Find("Btn_Teach").GetComponent<Button>();
        Btn_Teach.onClick.AddListener(Click_Teach);//教学
        Btn_RoomList = transform.Find("Btn_RoomList").GetComponent <Button>();//建立&加入房间
        Btn_RoomList.onClick.AddListener(Click_RoomList);//建立&加入房间

        RoomList_Panel = transform.Find("RoomListPanel").gameObject;
        InitRoomListPanel();
        
        //
        PlayerInfo_Panel = transform.Find("PlayerInfo_Panel").gameObject;
        Name_Txt = PlayerInfo_Panel.transform.Find("Name_Txt").GetComponent<Text>();
        Grade_Txt = PlayerInfo_Panel.transform.Find("Grade_Txt").GetComponent<Text>();
        Role_IMG = PlayerInfo_Panel.transform.Find("Role_IMG").GetComponent<Image>();
        //
        Matching_Panel = transform.Find("Matching_Panel").gameObject;
        Msg_Txt = Matching_Panel.transform.Find("Msg_Txt").GetComponent<Text>();
        Btn_Cancel = Matching_Panel.transform.Find("Btn_Cancel").GetComponent<Button>();
        Btn_Cancel.onClick.AddListener(Click_BtnCancel);//取消匹配
        //
        Setting_Panel = transform.Find("Setting_Panel").gameObject;
        Input_IP = Setting_Panel.transform.Find("Input_IP").GetComponent<InputField>();
        Input_Port = Setting_Panel.transform.Find("Input_Port").GetComponent<InputField>();

        Teach_Panel = transform.Find("Teach_Panel").gameObject;
        Btn_Close = Teach_Panel.transform.Find("Btn_Close").GetComponent<Button>();
        Btn_Close.onClick.AddListener(Click_BtnClose);//确认
        //isMusicOn = transform.Find("Music").GetComponent<Toggle>().isOn;
        //isSoundOn = transform.Find("Sound").GetComponent<Toggle>().isOn;
        

        Btn_OK = Setting_Panel.transform.Find("Btn_Ok").GetComponent<Button>();
        Btn_OK.onClick.AddListener(Click_OK);//确认
        //
        HideMatchPanel();//先隐藏
        Msg_Txt.text = StringFanConst.Matching;
//按钮文字
        Text txt_Match = Btn_Match.transform.Find("Text").GetComponent<Text>();
        txt_Match.text = StringFanConst.Match;
        Text txt_Pve = Btn_PVE.transform.Find("Text").GetComponent<Text>();
        txt_Pve.text = StringFanConst.PVE;
        Text txt_Teach = Btn_Teach.transform.Find("Text").GetComponent<Text>();
        txt_Teach.text = StringFanConst.Teach;
        Text txt_Setting = Btn_Setting.transform.Find("Text").GetComponent<Text>();
        txt_Setting.text = StringFanConst.Setting;
        Text txt_Quit = Btn_Quit.transform.Find("Text").GetComponent<Text>();
        txt_Quit.text = StringFanConst.Quit;
        //文本框文字
        Text txt_NickName = PlayerInfo_Panel.transform.Find("Text").GetComponent<Text>();
        txt_NickName.text = StringFanConst.NameTitle;
        Text txt_Jifen = PlayerInfo_Panel.transform.Find("Text (1)").GetComponent<Text>();
        txt_Jifen.text = StringFanConst.JifenName;
        Text txt_HeadPic = PlayerInfo_Panel.transform.Find("Text (2)").GetComponent<Text>();
        txt_HeadPic.text = StringFanConst.HeadPicName;
        Text txt_MsgIP = Setting_Panel.transform.Find("Msg_Txt").GetComponent<Text>();
        txt_MsgIP.text = StringFanConst.MsgIP;
        Text txt_Port = Setting_Panel.transform.Find("Msg_Port").GetComponent<Text>();
        txt_Port.text = StringFanConst.Port;



        //if (isMusicOn)
        //SoundManager.Instance.PlayBackground("SoundEffect/MusicGround");
    }

    private void InitRoomListPanel()
    {
        RoomItemPrefab = Resources.Load<RoomItem>("Item/RoomItem");
        CreateRoomButton = RoomList_Panel.transform.Find("CreateRoomButton").GetComponent<Button>();
        CreateRoomButton.onClick.AddListener(Click_BtnCreateRoom);
        UpdateListButton = RoomList_Panel.transform.Find("UpdateButton").GetComponent<Button>();
        UpdateListButton.onClick.AddListener(Click_BtnUpdateList);
        CloseListButton = RoomList_Panel.transform.Find("CloseButton").GetComponent<Button>();
        CloseListButton.onClick.AddListener(Click_BtnCloseListPanel);
        
        //base.Start();
    }
    public void UpdatePlayerSelfName(string name)
    {
        Name_Txt.text = name;
    }
    public void ShowMatchSuccessMsg()
    {
        Msg_Txt.text = StringFanConst.MatchSuccess;
        Btn_Cancel.interactable = false;
    }
    public void UpdatePlayerSelfGrade(int grade)
    {
        Grade_Txt.text = grade.ToString();
    }

    public void UpdatePlayersCount(int Count)
    {
        Msg_Txt.text = StringFanConst.Matching + Count.ToString() + "/4";
    }
    public void UpdatePlayerSelfRoleIMG(Sprite img)
    {
        Role_IMG.sprite = img;
    }
    public void ShowMatchPanel()
    {
        Matching_Panel.SetActive(true);
    }
    public void HideMatchPanel()
    {
        Matching_Panel.SetActive(false);
    }
    public void HideSettingPanel()
    {
        Setting_Panel.SetActive(false);
    }
    private void Click_BtnMatch()
    {
        Msg_Txt.text = StringFanConst.Matching;
        Btn_Cancel.interactable = true;

        if (!string.IsNullOrEmpty(StringFanConst.TestVersion))
        {
            Msg_Txt.text = StringFanConst.TestVersion;
            ShowMatchPanel();
            return;
        }

        if (Name_Txt.text.Equals(StringFanConst.Guest))
        {
            Msg_Txt.text = StringFanConst.RegisterPlease;
            ShowMatchPanel();
            return;
        }

        //IPAndPort ip = new IPAndPort();
        //if (string.IsNullOrEmpty(Input_IP.text) || string.IsNullOrEmpty(Input_Port.text) )
        //{ 
        //    ip.IPAddr = ServiceConst.IPAddress_Server;
        //    ip.Port = ServiceConst.Port_Server;
        //}
        //else
        //{
        //    ip.IPAddr = Input_IP.text;
        //    ip.Port = Input_Port.text;
        //}
        //dispatcher.Dispatch(ServiceConst.UpdateIPAndPort, ip);

        dispatcher.Dispatch(ViewConst.Click_Match);
    }
    private void Click_BtnPVE()
    {
        dispatcher.Dispatch(ViewConst.Click_PVE);
    }

    private void Click_RoomList()
    {
        RoomList_Panel.SetActive(true);
        //BasePanel panel;
        //Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();
        //bool isGet = panelDict.TryGetValue("",out panel);
    }
    private void Click_BtnUpdateList() {
        dispatcher.Dispatch(ViewConst.Click_UpdateRoomList);
    }
    private void Click_BtnCreateRoom() {
        dispatcher.Dispatch(ViewConst.Click_CreateRoom);
    }

    protected  void UpdateList()
    {
        //base.Update();
        if (strdata != "")
        {
            if (strdata == "0")
            {
                Debug.Log("房间列表为空");
                var content = RoomList_Panel.transform.Find("Content");
                for (int i = 0; i < content.childCount; i++)
                {
                    Destroy(content.GetChild(i).gameObject);
                }
                _haveItem = false;
            }
            else if (strdata.Contains("#") == true)
            {
                Debug.Log(strdata);
                var roomList = strdata.Split('#');
                if (roomList.Length < 4)
                {
                    AddItem(roomList.Length);
                }
                else if (roomList.Length >= 4)
                {
                    AddItem(4);
                }
                SetItems(roomList);
            }
            else if (strdata.Contains("#") == false)
            {
                if (strdata.Contains(";"))
                {
                    var array = strdata.Split(';');
                    AddItem(1);
                    SetItem(array[0], array[1], array[2], array[3]);
                }
            }
            strdata = "";
        }
    }


    public void AddItem(int count)
    {
        var content = transform.Find("Content");
        if (_haveItem == false)
        {
            for (int i = 0; i < count; i++)
            {
                var roomItem = Instantiate<RoomItem>(RoomItemPrefab, Vector3.zero, Quaternion.identity);
                roomItem.name = "RoomItem";
                roomItem.transform.SetParent(content);
            }
            _haveItem = true;
        }
    }

    public void SetItems(string[] roomList)
    {
        var content = transform.Find("Content");
        for (int i = 0; i < content.childCount; i++)
        {
            var array = roomList[i].Split(';');
            var roomItem = content.GetChild(i).GetComponent<RoomItem>();
            roomItem.SetText(array[0], array[1], array[2], array[3]);
        }
    }

    public void SetItem(string roomId, string userName, string totalCount, string winCount)
    {
        var content = transform.Find("Content");
        var roomItem = content.GetChild(0).GetComponent<RoomItem>();
        roomItem.SetText(roomId, userName, totalCount, winCount);
    }

    public void OnGetRoomListResponse(string data)
    {
        strdata = data;
    }

private void Click_BtnCloseListPanel() 
    { RoomList_Panel.SetActive(false); }

    private void Click_BtnQuit()
    {
        dispatcher.Dispatch(ViewConst.Click_Quit);
        Application.Quit();
    }
    private void Click_Setting()
    {
        Setting_Panel.SetActive(true);
        Input_IP.text = ServiceConst.IPAddress_Server;
        Input_Port.text = ServiceConst.Port_Server;
    }

    private void Click_Teach()
    {
        Teach_Panel.SetActive(true);
    }

    private void Click_OK()
    {
        IPAndPort ip = new IPAndPort();
        ip.IPAddr = Input_IP.text;
        ip.Port = Input_Port.text;
        dispatcher.Dispatch(ServiceConst.UpdateIPAndPort, ip);

        //Toggle togMusic = transform.Find("Music").GetComponent<Toggle>();
        //if (Input.GetMouseButtonDown(0))
        //    togMusic.isOn = !togMusic.isOn;
        //isMusicOn = togMusic.isOn;
        //Toggle togSound = transform.Find("Sound").GetComponent<Toggle>();
        //if (Input.GetMouseButtonDown(0))
        //    togSound.isOn = !togSound.isOn;
        //isSoundOn = togSound.isOn;

        HideSettingPanel();
    }


    private void Click_BtnCancel()
    {
        if (Name_Txt.text.Equals(StringFanConst.Guest))
        {
            HideMatchPanel();
            return;
        }
        dispatcher.Dispatch(ViewConst.Click_CancelMatch);
    }

    private void Click_BtnClose()
    {
        Teach_Panel.SetActive(false);
    }
}
public struct IPAndPort{
    public string IPAddr;
    public string Port;
}