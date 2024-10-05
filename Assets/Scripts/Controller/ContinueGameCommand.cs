using UnityEngine;

using strange.extensions.command.impl;
using strange.extensions.context.api;

public class ContinueGameCommand : EventCommand
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject ContextView { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        //if (gameData.BaseCards.Count > 0)
        //    return;
        dispatcher.Dispatch(NotificationConst.Noti_ShowMainGameUI);//显示主界面

        if (gameData.CurrentMode == MdMode.SinglePlayer)
        {
            RemoteCMD_Data recData = new RemoteCMD_Data();
            recData.cmd = RemoteCMD_Const.Match;
            recData.player = gameData.PlayerSelf;
            dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);//开始单机模式
        }
        else
        {
            gameData.CurrentStatus = GameStatus.CallLandlord;

            //if (gameData.PlayerSelf.Name.Equals(gameData.Players[0].Name))//从原始首家开始
            //{
                Debug.Log("GenerateCards");
                gameData.CurrentStatus = GameStatus.CallLandlord;//模式为叫地主
              
                dispatcher.Dispatch(NotificationConst.Noti_SendRecData,
                    new RemoteCMD_Data()
                    {
                        //player = new PlayerInfo() { Name = gameData.Landlord },
                        cmd = RemoteCMD_Const.GenerateCards,
                        cards = gameData.GenerateCards().ToArray()
                    });//随机生成一副牌并发送给服务器
           // }
            //recData.cmd = RemoteCMD_Const.GameTurn;//执行新的回合
            //recData.player.Name = gameData.Players[0].Name;
            
            
        }
        Debug.Log("Game Start");
        

        Transform canvas = ContextView.transform.Find("Canvas");
        GameObject GameOverUI = null;
        if (canvas.Find("GameOverUI(Clone)") != null)//已经加载过了
        {

            GameOverUI = canvas.Find("GameOverUI(Clone)").gameObject;
            GameOverUI.transform.SetSiblingIndex(canvas.childCount - 1);//显示在最前面
            GameOverUI.GetComponent<GameOverUI_View>().HideContinuePanel();//先隐藏返回游戏界面
            return;
        }
        GameObject go = Resources.Load<GameObject>("Prefabs/GameOverUI");
        GameOverUI = Object.Instantiate(go) as GameObject;
        GameOverUI.AddComponent<GameOverUI_View>();
        GameOverUI.transform.SetParent(canvas, false);

        
    }
}