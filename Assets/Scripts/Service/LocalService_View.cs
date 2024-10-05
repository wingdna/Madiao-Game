using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts;


public class LocalService_View : EventView
{

    public int CurrentTurn,firstTurn;
    public string Landlord;
    public List<string> RoomMembers;
    public int ClaimCount;
    public bool isFight;
    public Dictionary<string, List<Card>> ComputerHandCards;
    public List<Card> BaseCards;
    public List<Card> DisCards;
    public Queue<RemoteCMD_Data> MessageBox;
    [Inject]
    public IMdData gameData { get; set; }
    public void Init()
    {
        MessageBox = new Queue<RemoteCMD_Data>();
        RoomMembers = new List<string>();
        DisCards = new List<Card>();
        ComputerHandCards = new Dictionary<string, List<Card>>();
        ClaimCount = 1;
        isFight = false;
        CurrentTurn = -1;
        firstTurn = -1;
        BaseCards = new List<Card>();
        Landlord = "";
        SoundManager.Instance.PlaySound("SoundEffect/StartFight");
        Debug.Log("Local Service is Init...");
    }
    public void SendDataToLocal(RemoteCMD_Data recData)
    {
        //HandleRecievedData(recData);
        MessageBox.Enqueue(recData);
    }
    private void Update()
    {
        if (MessageBox.Count > 0)
        {
            RemoteCMD_Data recData = MessageBox.Dequeue();
            HandleRecievedData(recData);
            //SendTurn();
        }


    }
    private void HandleRecievedData(RemoteCMD_Data recData)
    {

        switch (recData.cmd)
        {
            case RemoteCMD_Const.Match:
                {
                    HandleMatch(recData.player);
                }
                break;
            case RemoteCMD_Const.CallLandlord:
                {
                    //HandleCallLandlord(recData);
                    HandleDiscards(recData);
                }
                break;
            case RemoteCMD_Const.NotCall:
                {
                    HandleDiscards(recData);
                }
                break;
            case RemoteCMD_Const.Claim:
                {
                    HandleDiscards(recData);
                }
                break;
            case RemoteCMD_Const.NotClaim:
                {
                    HandleDiscards(recData);
                }
                break;
           /* case RemoteCMD_Const.ReturnRoom:
                {
                    HandleReturnRoom(recData);
                }
                break;*/
            case RemoteCMD_Const.GenerateCards:
                {
                    HandleGenerateCards((Card[])recData.cards.Clone());
                }
                break;
            case RemoteCMD_Const.Discards:
                {
                    HandleDiscards(recData);
                }
                break;
            case RemoteCMD_Const.Chong:
                {
                    HandleChong(recData);
                }
                break;
            case RemoteCMD_Const.Pass:
                {
                    //HandlePass(recData);
                    HandleDiscards(recData);
                }
                break;
            case RemoteCMD_Const.GamerOver:
                {
                    HandleGameOver(recData);
                }
                break;

        }
    }

    private void HandleMatch(PlayerInfo p)
    {

        Debug.Log("房间满员，开始游戏");
        RoomMembers.Clear();
        gameData.CurrentMode = MdMode.SinglePlayer;

        RoomMembers.Add(p.Name);
        RoomMembers.Add(StringFanConst.Computer1);
        RoomMembers.Add(StringFanConst.Computer2);
        RoomMembers.Add(StringFanConst.Computer3);
        //确定玩家顺序
        int startPlayer = 0;//从玩家起始
        CurrentTurn = startPlayer;
        int player2 = (startPlayer + 1) % 4;
        int player3 = (startPlayer + 2) % 4;
        int player4 = (startPlayer + 3) % 4;
        //发送匹配成功命令
        RemoteCMD_Data rec = new RemoteCMD_Data();
        rec.cmd = RemoteCMD_Const.MatchSuccess;
        dispatcher.Dispatch(ServiceConst.Service_MatchSuccess, rec);
        //将顺序转发给房间内的玩家
        rec.cmd = RemoteCMD_Const.StartPlayer;
        rec.player.Name = RoomMembers[startPlayer];
        dispatcher.Dispatch(ServiceConst.Service_StartPlayer, rec);
        //
        rec = new RemoteCMD_Data();
        rec.cmd = RemoteCMD_Const.Player2;
        rec.player.Name = RoomMembers[player2];
        rec.player.Score = gameData.Player2.Score;
        dispatcher.Dispatch(ServiceConst.Service_Player2, rec);
        //
        rec = new RemoteCMD_Data();
        rec.cmd = RemoteCMD_Const.Player3;
        rec.player.Score = gameData.Player3.Score;
        rec.player.Name = RoomMembers[player3];
        dispatcher.Dispatch(ServiceConst.Service_Player3, rec);

        rec = new RemoteCMD_Data();
        rec.cmd = RemoteCMD_Const.Player4;
        rec.player.Score = gameData.Player4.Score;
        rec.player.Name = RoomMembers[player4];
        dispatcher.Dispatch(ServiceConst.Service_Player4, rec);
       
    }

    private void HandleReturnRoom(RemoteCMD_Data recData)
    {
        //gameData.CurrentStatus = GameStatus.Claim;

    }
    private void HandleGenerateCards(Card[] cards)
    {
        Debug.Log("开始发牌...");
        Card[] cards1 = new Card[8];
        Array.Copy(cards, 0, cards1, 0, 8);
        Card[] cards2 = new Card[8];
        Array.Copy(cards, 8, cards2, 0, 8);
        Card[] cards3 = new Card[8];
        Array.Copy(cards, 16, cards3, 0, 8);
        Card[] cards4 = new Card[8];
        Array.Copy(cards, 24, cards4, 0, 8);
        Card[] baseCards = new Card[8];
        Array.Copy(cards, 32, baseCards, 0, 8);

        //升代 露面底牌为赏肩百趣者 则需挨近之牌代替 副极为极 肩升赏 次肩升肩 九十升百千万
        Card c = baseCards[baseCards.Length - 1];
        Card c0 = GameEngine.FindReplaceFace(c);
        if (c.Value != c0.Value)
        {
            cards1 = GameEngine.ReplaceCard(cards1, c0, c);
            cards2 = GameEngine.ReplaceCard(cards2, c0, c);
            cards3 = GameEngine.ReplaceCard(cards3, c0, c);
            cards4 = GameEngine.ReplaceCard(cards4, c0, c);
        }


        //手牌1
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.DealCards;
        recData.cards = (Card[])cards1.Clone();
        dispatcher.Dispatch(ServiceConst.Service_DealCards, recData);
        //手牌2
        ComputerHandCards.Clear();//情况电脑外家

        ComputerHandCards.Add(StringFanConst.Computer1, new List<Card>((Card[])cards2.Clone()));
        //手牌3

        ComputerHandCards.Add(StringFanConst.Computer2, new List<Card>((Card[])cards3.Clone()));

        //手牌4

        ComputerHandCards.Add(StringFanConst.Computer3, new List<Card>((Card[])cards4.Clone()));

        //底牌
        BaseCards = new List<Card>((Card[])baseCards.Clone());
        RemoteCMD_Data rec = new RemoteCMD_Data();
        rec.cmd = RemoteCMD_Const.BaseCards;
        rec.cards = baseCards;

        dispatcher.Dispatch(ServiceConst.Service_BaseCards, rec);
        //开始游戏                      
   

        SendTurn();
  


    }

    private void HandleCallLandlord(RemoteCMD_Data recData)
    {
        Console.WriteLine("玩家" + recData.player.Name + "叫地主");
        dispatcher.Dispatch(ServiceConst.Service_CallLandlord, new RemoteCMD_Data()
        {
            player = new PlayerInfo() { Name = recData.player.Name },
            cmd = RemoteCMD_Const.CallLandlord
        });
        //DecideLandlord(recData.player.Name, true);

        SendTurn();
    }

    private void HandleNotCall(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "不叫地主");
        dispatcher.Dispatch(ServiceConst.Service_NotCall, new RemoteCMD_Data()
        {
            player = new PlayerInfo() { Name = recData.player.Name },
            cmd = RemoteCMD_Const.NotCall
        });
        //DecideLandlord(recData.player.Name, false);
        SendTurn();
    }

    private void HandleClaim(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "抢地主");
        dispatcher.Dispatch(ServiceConst.Service_Claim, new RemoteCMD_Data()
        {
            player = new PlayerInfo() { Name = recData.player.Name },
            cmd = RemoteCMD_Const.Claim
        });
        //DecideLandlord(recData.player.Name, true);
        SendTurn();
    }

    private void HandleNotClaim(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "不抢地主");
        dispatcher.Dispatch(ServiceConst.Service_NotClaim, new RemoteCMD_Data()
        {
            player = new PlayerInfo() { Name = recData.player.Name },
            cmd = RemoteCMD_Const.NotClaim
        });
        //DecideLandlord(recData.player.Name,false);
        SendTurn();
    }
    /// <summary>
    /// 确定庄家
    /// </summary>
    /// <param name="name"></param>
    /// <param name="update"></param>
    private void DecideLandlord(string name, bool update)
    {
        if (update)
        {
            Landlord = name;//更新地主

        }

        //if (ClaimCount <= 0)    //开始出牌 庄家开始       
        Debug.Log("地主是:" + Landlord);
        CurrentTurn = gameData.BaseCards[gameData.BaseCards.Count - 1].Value % 4;
        //CurrentTurn = RoomMembers.IndexOf(Landlord);//起始玩家改为地主                                       

    }

    private void HandleDiscards(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "出牌");
        

        dispatcher.Dispatch(ServiceConst.Service_Discard, recData);
        SendTurn();

        ChongCards(recData);


    }

    private void ChongCards(RemoteCMD_Data recData)
    {
        if (gameData.IsFightOver() && gameData.CurrentStatus == GameStatus.FightLandlord)
        {
            isFight = false;
            gameData.CurrentStatus = GameStatus.ViewReslult;

            recData.cmd = RemoteCMD_Const.Chong;
            recData.cards = gameData.BaseCards.ToArray();
            dispatcher.Dispatch(ServiceConst.Service_Chong, recData);
          
            dispatcher.Dispatch(ServiceConst.Service_GameTurn, recData);//转发回合

        }
    }

    private void HandleChong(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "看冲");
        dispatcher.Dispatch(ServiceConst.Service_Chong, recData);
        SendTurn();
    }

    private void HandlePass(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "跳过");
        dispatcher.Dispatch(ServiceConst.Service_Pass, recData);
        SendTurn();
    }

    private void HandleGameOver(RemoteCMD_Data recData)
    {
        Debug.Log("玩家" + recData.player.Name + "获得胜利，游戏结束");
        dispatcher.Dispatch(ServiceConst.Service_Gameover, recData);

    }
    private void SendTurn()
    {
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.GameTurn;

        if (!isFight)
        {
            #region 检测免斗色样
            //出牌开始前检查免鬥色樣
            List<List<CardType>> llt = new List<List<CardType>> { null, null, null, null };
            gameData.Players[0].Cards = new List<Card>(gameData.HandCards);
            gameData.Players[1].Cards = new List<Card>(ComputerHandCards[RoomMembers[1]]);
            gameData.Players[2].Cards = new List<Card>(ComputerHandCards[RoomMembers[2]]);
            gameData.Players[3].Cards = new List<Card>(ComputerHandCards[RoomMembers[3]]);
            
            llt[0] = GameEngine.SeYangNoPlay(gameData.HandCards);
            llt[1] = GameEngine.SeYangNoPlay(ComputerHandCards[RoomMembers[1]]);
            llt[2] = GameEngine.SeYangNoPlay(ComputerHandCards[RoomMembers[2]]);
            llt[3] = GameEngine.SeYangNoPlay(ComputerHandCards[RoomMembers[3]]);

            List<Card> ltmp = new List<Card>();
            for (int i=0;i<gameData.Players.Count;i++)
            {
                if (llt[i] != null)//免鬥色樣開衝，極不看衝
                {
                    ltmp.AddRange(gameData.Players[i].Cards);
                    ltmp.RemoveAll(c => c.Value == PokerConst.JiValve);
                    gameData.Players[i].winCards.AddRange( ltmp);
                    HandleChong(recData);
                    break;
                }

            }
            gameData.PlayerSelf.Cards = gameData.Players[0].Cards;
            gameData.Player2.Cards = gameData.Players[1].Cards;
            gameData.Player3.Cards = gameData.Players[2].Cards;
            gameData.Player4.Cards = gameData.Players[3].Cards;


            //出牌开始前检查顺风旗 四趣 四肩

            gameData.PlayerSelf.EpicStatus = GameEngine.CheckEpicCards(gameData.PlayerSelf.Cards);
                gameData.Player2.EpicStatus = GameEngine.CheckEpicCards(gameData.Player2.Cards);
                gameData.Player3.EpicStatus = GameEngine.CheckEpicCards(gameData.Player3.Cards);
                gameData.Player4.EpicStatus = GameEngine.CheckEpicCards(gameData.Player4.Cards);
            #endregion

            if (string.IsNullOrEmpty(gameData.Landlord) ||
                string.IsNullOrEmpty(Landlord) )
            {
                int i = gameData.BaseCards[gameData.BaseCards.Count - 1].Value % 4;
                gameData.Landlord = RoomMembers[i];
            }
         
            DecideLandlord(gameData.Landlord, true);//赋值庄家和起手玩家
            isFight = true;
            /*if (gameData.Landlord == null || gameData.Landlord == "")
                CurrentTurn = gameData.BaseCards[BaseCards.Count - 1].Value % 4;
            else
                DecideLandlord(gameData.Landlord, true);
            */
            //List<CardType> lseyang = GameEngine.SeYangNoPlay(ComputerHandCards[RoomMembers[CurrentTurn]]);

            //if (!string.IsNullOrEmpty(gameData.Landlord))
            //    DecideLandlord(gameData.Landlord, true);
            //CurrentTurn = gameData.BaseCards[BaseCards.Count - 1].Value % 4;


            gameData.CurrentTurn = CurrentTurn;
            
        }

        
        Debug.Log("Local:" + CurrentTurn);
        int index = CurrentTurn;//当前回合索引
        
        recData.player.Name = RoomMembers[index];//当前玩家昵称
                                                 //string targetIP = roomManager[player_Room[ipname]].Members[index];//IP

        dispatcher.Dispatch(ServiceConst.Service_GameTurn, recData);//转发回合
                                                                    //如庄家未定，则先找出庄家

        recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.GameTurn;
        Debug.Log("Local:" + CurrentTurn);

        CurrentTurn = gameData.CurrentTurn;

        index = CurrentTurn;//当前回合索引
        recData.player.Name = RoomMembers[index];//当前玩家昵称
                                                 //string targetIP = roomManager[player_Room[ipname]].Members[index];//IP
        dispatcher.Dispatch(ServiceConst.Service_GameTurn, recData);//转发回合



        //Chong(recData);



        if (!RoomMembers[CurrentTurn].Equals(RoomMembers[0]))//不是玩家的回合
        {
            System.Threading.Thread.Sleep(1000);
            HandleComputerTurn();//处理电脑的操作
            ChongCards(recData);

        }
        else if (gameData.LastPlayer == RoomMembers[CurrentTurn] || //上次是自己
                 string.IsNullOrEmpty(gameData.LastPlayer) ||
                gameData.CurrentDiscards.Count <= 0)//自己第一个出牌
        {
            firstTurn = CurrentTurn;
            gameData.TheFirstTurn = firstTurn;
        }

            gameData.CurrentTurn = CurrentTurn;




      
    }

    private void Chong(RemoteCMD_Data recData)
    {
        if (gameData.IsFightOver())
        {
            dispatcher.Dispatch(ViewConst.ShowBaseCards_Chong, gameData.BaseCards.ToArray());//显示底牌
            int imax = gameData.CurrentTurn;
            List<Card> lBase = gameData.BaseCards.GetRange(0, 7);
            List<Card> lTotal, lWin;
            List<int> lScore = new List<int> { 0, 0, 0, 0 };
            while (lBase.Count > 0)
            {
                List<Card> lBaseOld = lBase;
                for (int i = imax; i < imax + 4; i++)
                {
                    //System.Threading.Thread.Sleep(1000);
                    if (gameData.Players[i].Name == gameData.PlayerSelf.Name)
                        lWin = gameData.PlayerSelf.winCards;
                    else
                        lWin = gameData.Players[i % 4].winCards;
                    if (lWin.Count < 1)
                        continue;

                    var tu = CardChong.KanChong(lWin, lBase, lWin);
                    if (tu.Item1 < 1)
                        continue;
                    lScore[i % 4] += tu.Item1;
                    lBase = tu.Item2;
                    lTotal = tu.Item3;


                    switch (i % 4)
                    {
                        case 1:
                            dispatcher.Dispatch(ViewConst.UpdateWinCards1, lTotal[1]);
                            break;
                        case 2:
                            dispatcher.Dispatch(ViewConst.UpdateWinCards2, lTotal[2]);
                            break;
                        case 3:
                            dispatcher.Dispatch(ViewConst.UpdateWinCards3, lTotal[3]);
                            break;
                        case 0:
                            dispatcher.Dispatch(ViewConst.UpdateWinCards4, lTotal[0]);
                            break;

                    }

                    Debug.Log("player" + i.ToString() + "'s score:" + lScore[i % 4]);
                }
                if (lBaseOld.Count == lBase.Count)
                    lBase.RemoveAt(0);
                dispatcher.Dispatch(ServiceConst.Service_GameTurn, recData);//转发回合

            }

        }
    }
   

    private void HandleComputerTurn()
    {
        #region 争夺地主（斗地主用）
        //if (string.IsNullOrEmpty(gameData.Landlord))//争夺地主阶段
        //{
        //    isFight = true;
        //    CurrentTurn = BaseCards[BaseCards.Count - 1].Value % 4;    //马吊分庄
        //    if (string.IsNullOrEmpty(Landlord))//玩家未抢地主
        //    {
        //        SendDataToLocal(new RemoteCMD_Data()
        //        {
        //            cmd = RemoteCMD_Const.Claim,
        //            player = new PlayerInfo() { Name = RoomMembers[CurrentTurn] }
        //        });
        //        //DecideLandlord(RoomMembers[CurrentTurn], true);
        //        return;
        //    }
        //    SendDataToLocal(new RemoteCMD_Data()//玩家抢了就不抢了
        //    {
        //        cmd = RemoteCMD_Const.NotClaim,
        //        player = new PlayerInfo() { Name = RoomMembers[CurrentTurn] }
        //    });
        //    gameData.CurrentStatus = GameStatus.FightLandlord;
        //    ClaimCount--;
        //    return;
        //}
        //if (string.IsNullOrEmpty(gameData.LastPlayer) && !RoomMembers[CurrentTurn].Equals(gameData.Landlord))
        //    return;
        #endregion

       
        DisRole iRols = DisRole.UnKownRole;
        //出牌阶段
        if (!gameData.IsFightOver())
        {
            if (gameData.LastPlayer == RoomMembers[CurrentTurn] || //上次是自己
                 string.IsNullOrEmpty(gameData.LastPlayer) ||
                gameData.CurrentDiscards.Count <= 0)//自己第一个出牌
            {
                

                firstTurn = CurrentTurn;
                gameData.TheFirstTurn = firstTurn;              
                iRols = gameData.getRoleCard(firstTurn, CurrentTurn);


                List<Card> disCards = gameData.
               GetEnabledDisCards(ComputerHandCards[RoomMembers[CurrentTurn]], RoomMembers[CurrentTurn], iRols);//获取出牌
                if (disCards.Count < 1)
                {
                    disCards.Add(ComputerHandCards[RoomMembers[CurrentTurn]][0]);//马吊放水，出单张            
                    ComputerHandCards[RoomMembers[CurrentTurn]].RemoveAt(0);
                }



              //  System.Threading.Thread.Sleep(1000);

                SendDataToLocal(new RemoteCMD_Data()//出牌
                {
                    cmd = RemoteCMD_Const.Discards,
                    player = new PlayerInfo() { Name = RoomMembers[CurrentTurn] },
                    cards = disCards.ToArray()
                });

                
            }
            else
            {

                iRols = gameData.getRoleCard(firstTurn, CurrentTurn);
                List<Card> disCards = gameData.
                GetEnabledDisCards(ComputerHandCards[RoomMembers[CurrentTurn]], RoomMembers[CurrentTurn], iRols);//获取出牌
                if (gameData.CurrentDiscards4.Count == 0 ||
                    (gameData.CurrentDiscards4.Count > 0 && disCards[0].GreatThan(gameData.CurrentDiscards4[gameData.CurrentCards4Top()])))//再比一次大小,最大出牌否则灭牌
                {
                    SendDataToLocal(new RemoteCMD_Data()//出牌
                    {
                        cmd = RemoteCMD_Const.Discards,
                        player = new PlayerInfo() { Name = RoomMembers[CurrentTurn] },
                        cards = disCards.ToArray()

                    });
                }
                else
                {
                    //灭牌
                    SendDataToLocal(new RemoteCMD_Data()//出牌
                    {
                        cmd = RemoteCMD_Const.Discards,                //Pass,
                        player = new PlayerInfo() { Name = RoomMembers[CurrentTurn] },
                        cards = disCards.ToArray()

                    });
                }
            }
        }

       
   
    }

    private string GetSeyangTip(List<Card> lwin, int i)
    {
        string sret = null;
        switch (i)
        {
            case 0:
                sret += gameData.PlayerSelf.Name + StringFanConst.GetSeyang + "\r\n";
                break;
            case 1:
                sret += gameData.PlayerSelf.Name + StringFanConst.GetSeyang + "\r\n";
                break;
            case 2:
                sret += gameData.PlayerSelf.Name + StringFanConst.GetSeyang + "\r\n";
                break;
            case 3:
                sret += gameData.PlayerSelf.Name + StringFanConst.GetSeyang + "\r\n";
                break;
        }
        List<string> ls = new List<string>();
        lwin.ForEach(c => ls.Add(c.SValue()));

        List<CardType> lct = GameEngine.SeYangPlayX(ls);
        string seyang = null;
        foreach (CardType c in lct)
        {
            seyang += c.CardKey + c.Name + c.Jifen.ToString();
        }
        if (seyang != null)
        {
            sret += seyang;
            return sret;
        }
        else
            return null;
    }


}