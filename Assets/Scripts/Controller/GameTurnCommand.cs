using UnityEngine;
using System.Linq;
using strange.extensions.command.impl;
using Assets.Scripts.Model;

public class GameTurnCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        Debug.Log("ClaimCount:"+gameData.ClaimCount);
        
        RemoteCMD_Data rec = evt.data as RemoteCMD_Data;
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.player.Name = rec.player.Name;
        recData.cmd = rec.cmd;
        string recName = recData.player.Name;
        Debug.Log(LitJson.JsonMapper.ToJson(recData));
      
        //if (gameData.ClaimCount > 0  )//只剩下一次争夺
        //{
        //    //int count = gameData.BaseCards.Count;
        //    //recData.cmd = RemoteCMD_Const.Discards;//.Claim;

        //    //if (gameData.Landlord == null || gameData.Landlord == "")
        //    //{
        //    //    //switch (gameData.BaseCards[count - 1].Value % 4)
        //    //    gameData.Baijia = GameEngine.Whoisbai(gameData.PlayerSelf.Cards,
        //    //        gameData.Player2.Cards,
        //    //        gameData.Player3.Cards,
        //    //        gameData.Player4.Cards);
        //    //    switch (gameData.Baijia)
        //    //    {
        //    //        case -1:
        //    //        case 0:
        //    //            recData.player = gameData.PlayerSelf;                        
        //    //            break;
        //    //        case 1:
        //    //            recData.player = gameData.Player2;                        
        //    //            break;
        //    //        case 2:
        //    //            recData.player = gameData.Player3;
        //    //            break;
        //    //        case 3:
        //    //            recData.player = gameData.Player4;
        //    //            break;
        //    //    }
        //    //    gameData.Landlord = recData.player.Name;
        //    //}
        //    //else
        //    //{
        //    //    if (gameData.Landlord == gameData.PlayerSelf.Name)
        //    //        recData.player = gameData.PlayerSelf;
        //    //    if (gameData.Landlord == gameData.Player2.Name)
        //    //        recData.player = gameData.Player2;
        //    //    if (gameData.Landlord == gameData.Player3.Name)
        //    //        recData.player = gameData.Player3;
        //    //    if (gameData.Landlord == gameData.Player4.Name)
        //    //        recData.player = gameData.Player4;
        //    //}

        //    //    dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);//最后一次如果是自己默认抢一次

        //    //    //if (gameData.Landlord.Equals(recData.player.Name))//没人抢地主
        //    //{
        //    //    Debug.Log("没人抢地主");
        //    //    recData.cmd = RemoteCMD_Const.Claim;
        //    //    recData.player = gameData.PlayerSelf;
        //    //    dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);//最后一次如果是自己默认抢一次
        //    //    return;
        //    //}
        //    }

        if (gameData.ClaimCount > 0  )
        {
            recData.cmd = RemoteCMD_Const.Discards;//.Claim;
            if (string.IsNullOrEmpty(gameData.Landlord))
            {
                gameData.Landlord = gameData.Players[gameData.BaseCards[gameData.BaseCards.Count - 1].Value % 4].Name;//如庄家未定 则依底牌值确定
                int index = gameData.Players.FindIndex(
                        (p) =>
                            {
                                return p.Name == gameData.Landlord;
                            });//找到地主所在的顺序
                gameData.CurrentTurn = index;//当前回合变成地主
            }
            else
                gameData.CurrentTurn = gameData.BaseCards[gameData.BaseCards.Count - 1].Value % 4;

            var vcards  = from p in gameData.Players select p.Cards;
            for (int idx = 0; idx < gameData.Players.Count; idx++)
            {
                if (vcards.ToList()[idx].Exists(c => c.Value == PokerConst.ShangValve - 2 &&
                                                   c.Suit == PokerConst.Spade))
                    gameData.Players[idx].isBai = true;
                else
                    gameData.Players[idx].isBai = false;
                switch (idx)
                {
                    case 0:
                        gameData.PlayerSelf.isBai = gameData.Players[idx].isBai;
                        break;
                    case 1:
                        gameData.Player2.isBai = gameData.Players[idx].isBai;
                        break;
                    case 2:
                        gameData.Player3.isBai = gameData.Players[idx].isBai;
                        break;
                    case 3:
                        gameData.Player4.isBai = gameData.Players[idx].isBai;
                        break;
                }
            }

            recData.cmd = RemoteCMD_Const.Discards;//.Claim;
            gameData.CurrentStatus = GameStatus.FightLandlord;

            /*
             * int index = gameData.Players.FindIndex(
            (p) =>
            {
                return p.Name == gameData.Landlord;
            });//找到地主所在的顺序
            gameData.CurrentTurn = index;//当前回合变成地主
            */
           
            dispatcher.Dispatch(ViewConst.ShowBaseCards_Value, gameData.BaseCards.ToArray());//显示底牌    
            dispatcher.Dispatch(NotificationConst.Noti_UpdatePlayerIdentity);//更新地主头像           

        }
        


        if (recName.Equals(gameData.PlayerSelf.Name) )//当前回合是自己
        {
            switch (gameData.CurrentStatus)
            {
                case GameStatus.CallLandlord:
                    {
                        //显示叫地主
                        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.CallLandlord);
                    }
                    break;
                case GameStatus.Claim:
                    {
                        //显示抢地主
                        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.ClaimLandlord);
                    }
                    break;
                case GameStatus.FightLandlord:
                    {
                        //显示斗地主
                        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.FightLandlord);
                        if (gameData.FourDoor.Count > 0)
                        {
                            string str = " " + EngineTool.SuitToMd(gameData.FourDoor[0]);
                            if (gameData.FourDoor.Count >= 4)                                
                            {
                                str += " " + EngineTool.SuitToMd(gameData.FourDoor[1]);
                                if (gameData.Flower3 >= 3)
                                    str += " " + EngineTool.SuitToMd(gameData.FourDoor[2]);
                            }
                            dispatcher.Dispatch(ViewConst.UpdateStatus_OpenDoors,str );//提示玩家色样组合
                        }
                        if (gameData.RestCardNum >= 8 )
                        {
                            var llcards = from p in gameData.Players select p.winCards;
                            var lnames = from p in gameData.Players select p.Name;
                            string startInfo = GameEngine.GetStartPlayerInfo(llcards.ToList(), lnames.ToList(), gameData.BaseCards[gameData.BaseCards.Count - 1]);
                            
                            dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipQiShou);//提示名稱改為起手
                            if (!string.IsNullOrEmpty(startInfo))
                                dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, startInfo);//提示面張信息
                        }
                        else if (gameData.HandCards.Count < 8)
                        {
                            //string stip = gameData.TipSeyang();
                           // if (!string.IsNullOrEmpty(stip))
                           //  dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, stip);//提示玩家色样组合
                            //dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipSeyang);//提示玩家色样组合
                        }

                    }
                    break;
 /*               case GameStatus.Chong:
                    {
                        gameData.CurrentStatus = GameStatus.ViewReslult;
                        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.Chong);
                    }
                    break;
                case GameStatus.ViewReslult:
                    { //显示看冲
                        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.ViewResult);
                    }
                    break;
 */           
            }
        }
        
        switch (gameData.CurrentStatus)
        {
            case GameStatus.Chong:
                {
                  
                    dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.Chong);
                }
                break;
            case GameStatus.ViewReslult:
                { //显示看冲
                    dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.ViewResult);
                }
                break;
        }

        if (gameData.CurrentStatus == GameStatus.ViewReslult ||
            gameData.CurrentStatus == GameStatus.Chong)
            return;
        if (recName.Equals(gameData.Player2.Name))//当前回合是玩家2
        {
            dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.HideAll);
            //dispatcher.Dispatch(ServiceConst.Service_Discard, recData);            
        }
        if (recName.Equals(gameData.Player3.Name))//当前回合是玩家3
        {
            dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.HideAll);
            //dispatcher.Dispatch(ServiceConst.Service_Discard, recData);
        }
        if (recName.Equals(gameData.Player4.Name))//当前回合是玩家4
        {
            dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.HideAll);
            //dispatcher.Dispatch(ServiceConst.Service_Discard, recData);
        }
        if (gameData.CurrentStatus== GameStatus.FightLandlord)
        {        
            dispatcher.Dispatch(NotificationConst.Noti_UpdatetTimer, true);//更新游戏中的计时器
        }
        else
        {
            dispatcher.Dispatch(NotificationConst.Noti_UpdatetTimer, false);//更新游戏前的计时器
        }
       
            gameData.ClaimCount--;


      
    }
}