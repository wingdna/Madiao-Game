using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

public class GameOverUI_Mediator : EventMediator
{
    [Inject]
    public GameOverUI_View View { get; set; }
    
    public override void OnRegister()
    {
        View.Init();
        View.dispatcher.AddListener(ViewConst.Click_ReturnLobby, CallBack_ReturnLobby);//返回大厅
        View.dispatcher.AddListener(ViewConst.Click_ContinueGame, CallBack_ContinueGame);//返回大厅
        dispatcher.AddListener(ViewConst.UpdateGameOverResult, CallBack_UpdateGameOverResult);
    }
    private void CallBack_UpdateGameOverResult(IEvent evt)
    {
        View.UpdateResultMsg((string)evt.data);
        View.HideContinuePanel();
    }
    private void CallBack_ReturnLobby()//返回大厅
    {
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.ReturnLobby;
        dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);        
       // dispatcher.Dispatch(NotificationConst.Noti_ShowGameLobbyUI);
    }
    private void CallBack_ContinueGame()//返回大厅
    {
        View.ShowContinuePanel();
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.ReturnRoom;       
        dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);
        //RemoteCMD_Data recData = new RemoteCMD_Data();

        //recData.cmd = RemoteCMD_Const.Replay;

        //dispatcher.Dispatch(NotificationConst.Noti_ContinueGame);
    }
    public override void OnRemove()
    {
        View.dispatcher.RemoveListener(ViewConst.Click_ReturnLobby, CallBack_ReturnLobby);
        dispatcher.RemoveListener(ViewConst.UpdateGameOverResult, CallBack_UpdateGameOverResult);
        View.dispatcher.RemoveListener(ViewConst.Click_ContinueGame, CallBack_ContinueGame);
    }
}