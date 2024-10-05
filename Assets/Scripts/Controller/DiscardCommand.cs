using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using Assets.Scripts;
using Assets.Scripts.Model;

using System.Linq;

public class DiscardCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {

        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        Debug.Log("DiscardCommand is Executing...:" + recData.player.Name);

        gameData.LastPlayer = recData.player.Name;//
        bool bEnd = false;


        //从手牌中删除
        for (int i = 0; i < recData.cards.Length; i++)
        {
            for (int j = 0; j < gameData.HandCards.Count; j++)
            {
                if (recData.cards[i].Value == gameData.HandCards[j].Value &&
                    recData.cards[i].Suit.Equals(gameData.HandCards[j].Suit))
                {
                    gameData.HandCards.RemoveAt(j);
                    SoundManager.Instance.PlaySound("SoundEffect/Discard");
                    break;//找到了就跳出
                }
            }
        }

        //if (gameData.CurrentDiscards.Count >= 4)
        gameData.CurrentDiscards.Clear();//马吊 清除上一轮的出牌       

        gameData.CurrentDiscards.AddRange(recData.cards);//保存出牌

 //       if (gameData.Baijia < 0 && recData.cards[0].Suit == PokerConst.Spade)
 //           gameData.Baijia = gameData.Players.FindIndex((p) => { return p.Name == recData.player.Name; });//找到告百者所在的顺序;    



        //前7牌无上桌则第八牌不得上桌
        int num = gameData.RestCardNum;
        string recName = recData.player.Name;
     /*   if ((num == 0 ||
             (num == 1 && gameData.HandCards.Count < 1)) &&
                 ((recName == gameData.PlayerSelf.Name && gameData.PlayerSelf.winCards.Count < 1)
                || (recName == gameData.Player2.Name && gameData.Player2.winCards.Count < 1)
                || (recName == gameData.Player3.Name && gameData.Player3.winCards.Count < 1)
                || (recName == gameData.Player4.Name && gameData.Player4.winCards.Count < 1))
                && gameData.CurrentDiscards4.Count > 0)*/
     if (num == 0 &&  gameData.CurrentDiscards4.Count > 0 &&
                 ((recName == gameData.PlayerSelf.Name && gameData.PlayerSelf.winCards.Count < 1)
                || (recName == gameData.Player2.Name && gameData.Player2.winCards.Count < 1)
                || (recName == gameData.Player3.Name && gameData.Player3.winCards.Count < 1)
                || (recName == gameData.Player4.Name && gameData.Player4.winCards.Count < 1))            )

            gameData.CurrentDiscards4.Add(gameData.CurrentDiscards4[0]);
        else
            gameData.CurrentDiscards4.AddRange(recData.cards); //保存到本轮马吊出牌列表

        //保存出过的非灭牌
        int itop = gameData.CurrentCards4Top();
        if (gameData.CurrentDiscards4.Count < 1|| 
            gameData.CurrentDiscards4.Count >= 4||
            recData.cards[0].Equals(gameData.CurrentDiscards4[itop]) )
            gameData.AllDiscards.Add(recData.cards[0]);
       


        if (gameData.CurrentMode == MdMode.MutiPlayer)
        {
            if (recName == gameData.PlayerSelf.Name)
                gameData.PlayerSelf.Cards.AddRange(recData.cards);
            if (recName == gameData.Player2.Name)
                gameData.Player2.Cards.AddRange(recData.cards);
            if (recName == gameData.Player3.Name)
                gameData.Player3.Cards.AddRange(recData.cards);
            if (recName == gameData.Player4.Name)
                gameData.Player4.Cards.AddRange(recData.cards);
        }
        //显示本輪所有玩家出牌
        dispatcher.Dispatch(ViewConst.ShowDiscards, gameData.CurrentDiscards4.ToArray());//recData.cards);//显示出牌


        #region 斗满一轮 本轮由最大者得桌 并在下轮出牌       
        if (gameData.CurrentDiscards4.Count >= 4)
        {
            Debug.Log(gameData.CurrentDiscards4.ToString());

            gameData.Zhuoji = gameData.IsZhuoji(gameData.CurrentTurn);
            int i = gameData.CurrentCards4Top();
            gameData.CurrentTurn -= 3 - i;
            gameData.CurrentTurn = gameData.CurrentTurn < 0 ? (gameData.CurrentTurn + 4) % 4
                                 : gameData.CurrentTurn % 4;

            TwinsFlower tf = TwinsFlower.NoTwins;
            int nflower = gameData.CurrentDiscards4.GetRange(0, i + 1).Where(c => c.Suit == PokerConst.Spade &&
                                                       c.Value >= PokerConst.ShangValve - 2).Count();//万千百数量
            //万捉千 万捉百 千捉百
            if (nflower >= 2 &&
               (gameData.CurrentDiscards4[i].Suit == PokerConst.Spade &&
               gameData.CurrentDiscards4[i].Value >= PokerConst.ShangValve - 1))//千僧 红万得桌
            {
                if (nflower == 3)
                    tf = TwinsFlower.KingWan;
                else if (gameData.CurrentDiscards4[i].Value == PokerConst.ShangValve)
                    tf = TwinsFlower.WinWan;
                else
                    tf = TwinsFlower.WinQian;
            }
            else
                tf = TwinsFlower.NoTwins;
            //三花现身数量
            if (recData.cards.ToList().Exists(c => c.Value >= EngineTool.GetShangValve(PokerConst.Spade) &&
                                                 c.Suit == PokerConst.Spade))
                gameData.Flower3++;


            if (gameData.CurrentMode == MdMode.MutiPlayer)//多人模式
            {
                int lastindex = gameData.Players.FindIndex((p) =>
                {
                    return p.Name == gameData.LastPlayer;
                });//找到最后出牌玩家本人所在的顺序
                int jidx = (lastindex - (3 - i) + 4) % 4;
                if (gameData.Players[jidx].twinsF == TwinsFlower.NoTwins)
                    gameData.Players[jidx].twinsF = tf;

                if (gameData.LastPlayer == gameData.PlayerSelf.Name)
                {
                    switch (i)
                    {
                        case 0:
                            gameData.Player2.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 1:
                            gameData.Player3.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 2:
                            gameData.Player4.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 3:
                            gameData.PlayerSelf.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                    }
                }

                if (gameData.LastPlayer == gameData.Player2.Name)
                {
                    switch (i)
                    {
                        case 0:
                            gameData.Player3.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 1:
                            gameData.Player4.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 2:
                            gameData.PlayerSelf.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 3:
                            gameData.Player2.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                    }
                }

                if (gameData.LastPlayer == gameData.Player3.Name)
                {
                    switch (i)
                    {
                        case 0:
                            gameData.Player4.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 1:
                            gameData.PlayerSelf.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 2:
                            gameData.Player2.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 3:
                            gameData.Player3.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                    }
                }

                if (gameData.LastPlayer == gameData.Player4.Name)
                {
                    switch (i)
                    {
                        case 0:
                            gameData.PlayerSelf.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 1:
                            gameData.Player2.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 2:
                            gameData.Player3.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                        case 3:
                            gameData.Player4.winCards.Add(gameData.CurrentDiscards4[i]);
                            break;
                    }
                }


            }
            else//单机模式 、人机
            {
                if (gameData.Players[gameData.CurrentTurn].twinsF == TwinsFlower.NoTwins)
                    gameData.Players[gameData.CurrentTurn].twinsF = tf;
                
                switch (gameData.CurrentTurn)
                {
                    case 0:
                        gameData.PlayerSelf.winCards.Add(gameData.CurrentDiscards4[i]);
                        gameData.Players[gameData.CurrentTurn].winCards = gameData.PlayerSelf.winCards;
                        break;
                    case 1:
                        gameData.Player2.winCards.Add(gameData.CurrentDiscards4[i]);
                        gameData.Players[gameData.CurrentTurn].winCards = gameData.Player2.winCards;
                        break;
                    case 2:
                        gameData.Player3.winCards.Add(gameData.CurrentDiscards4[i]);
                        gameData.Players[gameData.CurrentTurn].winCards = gameData.Player3.winCards;
                        break;
                    case 3:
                        gameData.Player4.winCards.Add(gameData.CurrentDiscards4[i]);
                        gameData.Players[gameData.CurrentTurn].winCards = gameData.Player4.winCards;
                        break;
                }
            }

            gameData.FirstDiscards.Add(gameData.CurrentDiscards4[0]);      
            gameData.CurrentDiscards.Clear();
            gameData.CurrentDiscards4.Clear();
            if (num == 0)
                bEnd = true;
        }
        else
        {
            gameData.CurrentTurn++;
            gameData.CurrentTurn %= 4;
        }
        #endregion
        //string cardType = gameData.GetCardsType(recData.cards);//获得牌型
        //dispatcher.Dispatch(ViewConst.ShowDiscardMsg, caerdType);//显示牌型

        if (recName.Equals(gameData.PlayerSelf.Name))
        {
            dispatcher.Dispatch(ViewConst.RemoveAllDiscards);//清除自己要出的牌

        }
        if (recName.Equals(gameData.Player2.Name))
        {
            dispatcher.Dispatch(ViewConst.UpdatePlayer2Cards, -recData.cards.Length);//更新玩家2牌数

        }
        if (recName.Equals(gameData.Player3.Name))
        {
            dispatcher.Dispatch(ViewConst.UpdatePlayer3Cards, -recData.cards.Length);//更新玩家3牌数

        }
        if (recName.Equals(gameData.Player4.Name))
        {
            dispatcher.Dispatch(ViewConst.UpdatePlayer4Cards, -recData.cards.Length);//更新玩家4牌数

        }
        dispatcher.Dispatch(ViewConst.UpdateWinCards1, (Card[])gameData.PlayerSelf.winCards.ToArray().Clone());
        dispatcher.Dispatch(ViewConst.UpdateWinCards2, (Card[])gameData.Player2.winCards.ToArray().Clone());
        dispatcher.Dispatch(ViewConst.UpdateWinCards3, (Card[])gameData.Player3.winCards.ToArray().Clone());
        dispatcher.Dispatch(ViewConst.UpdateWinCards4, (Card[])gameData.Player4.winCards.ToArray().Clone());

       

        if (gameData.IsFightOver() && bEnd)// && gameData.CurrentMode == MdMode.MutiPlayer）
        {

            recData.cmd = RemoteCMD_Const.Chong;
            recData.cards = gameData.BaseCards.ToArray();
            dispatcher.Dispatch(ServiceConst.Service_Chong, recData);


            dispatcher.Dispatch(ServiceConst.Service_GameTurn, recData);//转发回合

        }
        #region 改写到viewChongResult中
        /*
        clientService.SendDataToServer(new RemoteMsg(new RemoteCMD_Data()
            {
                cmd = RemoteCMD_Const.GamerOver,
                player = new PlayerInfo()
                {
                    Name = gameData.LastPlayer == gameData.PlayerSelf.Name ? gameData.Player2.Name :
                                                   gameData.LastPlayer == gameData.Player2.Name ? gameData.Player3.Name :
                                                   gameData.LastPlayer == gameData.Player3.Name ? gameData.Player4.Name :
                                                   gameData.PlayerSelf.Name
                }
            }));//游戏结束
            */
        #endregion

        dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.HideAll);//回合结束隐藏界面

 



    }




}
