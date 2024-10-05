using UnityEngine;

using strange.extensions.command.impl;

public class Player4Command : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;        
        Debug.Log("Player4:" + recData.player.Name);

        gameData.Players.Add(recData.player);
        Debug.Log(gameData.PlayerSelf.Name);
      

        int selfIndex = gameData.Players.FindIndex(
            (p) => {
                Debug.Log(p.Name);
                return p.Name == gameData.PlayerSelf.Name;
            });//找到自己所在的顺序
        //以自己为准，其他玩家按游戏顺序逆时针排位置        

        Debug.Log("SelfIndex:" + selfIndex);
        selfIndex++;
        selfIndex %= 4;
        gameData.Player2 = gameData.Players[selfIndex];//玩家2
        dispatcher.Dispatch(ViewConst.UpdatePlayer2Name, gameData.Player2.Name);//显示玩家2名字

        selfIndex++;
        selfIndex %= 4;
        gameData.Player3 = gameData.Players[selfIndex];//玩家3
        dispatcher.Dispatch(ViewConst.UpdatePlayer3Name, gameData.Player3.Name);//显示玩家3名字
        //
        selfIndex++;
        selfIndex %= 4;
        gameData.Player4 = gameData.Players[selfIndex];//玩家4
        dispatcher.Dispatch(ViewConst.UpdatePlayer4Name, gameData.Player4.Name);//显示玩家4名字

        
        gameData.CurrentStatus = GameStatus.CallLandlord;
       
        if (gameData.PlayerSelf.Name.Equals(gameData.Players[0].Name))//从庄家开始
        {
            Debug.Log("GenerateCards");
            gameData.CurrentStatus = GameStatus.CallLandlord;//模式为叫地主
            dispatcher.Dispatch(NotificationConst.Noti_SendRecData,
                new RemoteCMD_Data()
                {
                //    player = new PlayerInfo() { Name = gameData.Landlord },
                    cmd = RemoteCMD_Const.GenerateCards,
                    cards = gameData.GenerateCards().ToArray()
                });//随机生成一副牌并发送给服务器
        }
        //recData.cmd = RemoteCMD_Const.GameTurn;//执行新的回合
        //recData.player.Name = gameData.Players[0].Name;
        Debug.Log("Game Start");
        dispatcher.Dispatch(NotificationConst.Noti_ShowMainGameUI);//显示主界面

    

        dispatcher.Dispatch(ViewConst.UpdateStatus_Base, gameData.PlayerSelf.Score);//显示玩家分数
    }
}