using UnityEngine;

using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GameStartUI_Mediator : EventMediator
{
    [Inject]
    public GameStartUI_View View { get; set; }
    public override void OnRegister()
    {
        View.Init();
        View.dispatcher.AddListener(ViewConst.Click_StartGame, CallBack_StartGame);//显示开始游戏
        View.dispatcher.AddListener(ViewConst.Click_RegisterOK, CallBack_RegisterOK);
        dispatcher.AddListener(ViewConst.Update_Message, CallBack_ShowMessage);

    }
    private void CallBack_StartGame(IEvent evt)
    {
        string str = (string)evt.data;
        if (!str.Contains("|") )
        { dispatcher.Dispatch(NotificationConst.Noti_ShowGameLobbyUI, (string)evt.data); return; }

        string[] strs = str.Split('|');
        string sname = strs[0];
        string spwd = strs[1];
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.Login;
        recData.player = new PlayerInfo() { Name = sname, Pwd = spwd };
        dispatcher.Dispatch(NotificationConst.Noti_SendUserinfo, recData);
        //dispatcher.Dispatch(NotificationConst.Noti_ShowGameLobbyUI, sname);//(string)evt.data);//显示游戏大厅
    }

    private void CallBack_RegisterOK(IEvent evt)
    {
        string str = (string)evt.data;
        string[] strs = str.Split('|');
        string sname = strs[0];
        string spwd = strs[1];
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.Register;
        recData.player = new PlayerInfo() { Name = sname, Pwd = spwd,Score = 1000 };
        dispatcher.Dispatch(NotificationConst.Noti_SendUserinfo, recData);
        View.HideRegisterPanel();//返回开始页面大厅
    }

    private void CallBack_ShowMessage(IEvent evt)
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        string strMessage = "";
        switch (recData.cmd)
        {
            case RemoteCMD_Const.LoginPwdErr:
                strMessage = StringFanConst.LoginPwdError;
                break;
            case RemoteCMD_Const.LoginUserErr:
                strMessage = StringFanConst.LoginUserError;
                break;
            case RemoteCMD_Const.RegAlready:
                strMessage = StringFanConst.UserRegAlready;
                break;
            case RemoteCMD_Const.RegError:
                strMessage = StringFanConst.UserRegError;
                break;
        }
        View.ShowMessagePanel(strMessage);
    }


    public override void OnRemove()
    {
        View.dispatcher.RemoveListener(ViewConst.Click_StartGame, CallBack_StartGame);
        View.dispatcher.RemoveListener(ViewConst.Click_RegisterOK, CallBack_RegisterOK);
        dispatcher.RemoveListener(ViewConst.Update_Message, CallBack_ShowMessage);
    }
}