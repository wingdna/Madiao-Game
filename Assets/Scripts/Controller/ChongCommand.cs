
using strange.extensions.command.impl;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts;

public class ChongCommand : EventCommand
{
    [Inject]

    public IMdData gameData { get; set; }
    public override void Execute()
    {
        /*
         * RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        Debug.Log("BaseCards:" + LitJson.JsonMapper.ToJson(recData));
        Card[] baseCard = new Card[recData.cards.Length];
        recData.cards.CopyTo(baseCard, 0);
        gameData.BaseCards.Clear();
        gameData.BaseCards.AddRange(baseCard);//保存底牌数据
        */


        if (gameData.IsFightOver())
        {

            gameData.CurrentStatus = GameStatus.ViewReslult;
            int imax = gameData.CurrentTurn;
            List<Card> lBase = new List<Card>( gameData.BaseCards.GetRange(0, 7));
            List<Card> lWin;
            List<int> lScore = new List<int> { 0, 0, 0, 0 };

            var names = from p in gameData.Players select p.Name;
            List<string> lNames = names.ToList();
            string selfName = gameData.PlayerSelf.Name;

            int iself = lNames.IndexOf(selfName);
            gameData.Players[iself].winCards = new List<Card>(gameData.PlayerSelf.winCards);
            gameData.Players[(iself + 1) % 4].winCards = new List<Card>(gameData.Player2.winCards);
            gameData.Players[(iself + 2) % 4].winCards = new List<Card>(gameData.Player3.winCards);
            gameData.Players[(iself + 3) % 4].winCards = new List<Card>(gameData.Player4.winCards);
            gameData.Players[iself].Score = gameData.PlayerSelf.Score;
            gameData.Players[(iself + 1) % 4].Score = gameData.Player2.Score;
            gameData.Players[(iself + 2) % 4].Score = gameData.Player3.Score;
            gameData.Players[(iself + 3) % 4].Score = gameData.Player4.Score;
            List<List<Card>> llWin = new List<List<Card>>(),
                            llTotals = new List<List<Card>>();
            var vWin = from p in gameData.Players select p.winCards;
            for (int i = 0; i < 4; i++)
            {
                llWin.Add(vWin.ToList()[i]);
                llTotals.Add(vWin.ToList()[i]);
            }

            while (lBase.Count > 0)
            {
                List<Card> lBaseOld = new List<Card>(lBase);
                for (int i = imax; i < imax + 4; i++)
                {
                    lWin = llWin[i % 4];
                    if (lWin.Count <= 1)
                        continue;

                    int twins = 0;
                    lBaseOld.Clear();
                    lBaseOld.AddRange(lBase);


                    //计算功千 万王之王等加成
                   
                        switch (gameData.Players[i % 4].twinsF)
                        {
                            case TwinsFlower.WinQian:
                            case TwinsFlower.WinWan:
                            twins = 1;
                                break;
                            case TwinsFlower.KingWan:
                            twins = 2;
                            break;
                        case TwinsFlower.NoTwins:
                            twins = 0;
                            break;
                        }
                    




                    var tu = CardChong.KanChong(lWin, lBase, llTotals[i % 4],twins);
                    if (tu.Item1 < 1)
                        continue;

                    lScore[i % 4] += tu.Item1;
                    lBase = tu.Item2;

                    llTotals[i % 4] = llTotals[i % 4].Union(tu.Item3).ToList();

                    Debug.Log("player" + i.ToString() + "'s score:" + lScore[i % 4]);
                    imax = i % 4;
                    break;
                }
                if (lBase.Count > 0 && lBaseOld.Count == lBase.Count)
                {
                    lBase.RemoveAt(0);
                }
            }

            List<int> lscoreseyang = new List<int> { 0, 0, 0, 0 };
            int thescore = 0, seyangscore = 0;
            int indexSelf = gameData.Players.FindIndex((p) => { return p.Name == gameData.PlayerSelf.Name; });//找到玩家本人所在的顺序
            int iLord = gameData.Players.FindIndex((p) => { return p.Name == gameData.Landlord; });//找到地主所在的顺序
            string stip = "";
            List<List<Card>> lchongs = new List<List<Card>> { null, null, null, null };
            int nbaseduo = 0, iwin = -1, ilosedouble = -1;
            string sduojinbiao = null;

            for (int i = iLord; i < iLord + gameData.Players.Count; i++)
            {
                int j = i % 4;
                //计算色样

                if (llWin[j].Where(c => c.Value == EngineTool.GetShangValve(c.Suit)).Count() == 3)
                {
                    var tuduo = CardChong.DuoJinBiao(llTotals, llWin,j);
                    if (tuduo.Item1 > 0)//夺錦标
                    {
                        nbaseduo = tuduo.Item1;
                        ilosedouble = tuduo.Item2;
                        iwin = tuduo.Item3;
                        sduojinbiao = tuduo.Item4;
                        break;

                    }
                    else
                    { 
                        //鐵門閂
                    }
                }
                //计算地绝色样
                //var vsenp = GameEngine.SeYangNoPlay(llTotals[j]);
                List<Card> lchong = CardChong.GetChongCards(llTotals[j], llWin[j]);
                //lchong.RemoveAll(llWin[j].Contains);
                int qingcount, qingpos = -1;
                if (lchong != null && lchong.Count > 0)
                {
                    for (int ipos = 0; ipos < lchong.Count; ipos++)
                    {
                        if (!EngineTool.isJinZhang(lchong[ipos]))
                        {
                            qingpos = ipos;
                            break;
                        }
                    }
                    if (qingpos >= 0)
                    {
                        qingcount = lchong.Count - qingpos;
                        lchong.RemoveRange(qingpos, qingcount);
                        lchong.AddRange(llWin[j]);
                        var vsenp = GameEngine.SeYangNoPlay(lchong);

                        if (vsenp != null)
                        {
                            foreach (var item in vsenp)
                            {
                                if (item.Jifen > 0)
                                {
                                    thescore = item.Jifen / 2;
                                    lscoreseyang[j] += item.Jifen / 2;//地绝积分减半
                                    stip += gameData.Players[j].Name + " : " + item.Name + "   " + thescore.ToString() + "\r\n";
                                    Debug.Log("冲出地绝色样:"+stip);
                                }
                            }
                        }
                    }
                }
                //计算冲成奇色
                var ltu = CardChong.GetChongSeYang(llTotals[j], llWin[j]);
                if (ltu != null)
                {
                    foreach (var item in ltu)
                    {
                        if (item.Item1 > 0)
                        {
                            lscoreseyang[j] += item.Item1;
                            stip += gameData.Players[j].Name + " : " + item.Item2 + "   " + item.Item1.ToString() + "\r\n";
                            Debug.Log("冲出色样:" + stip);
                        }
                    }
                }
                //计算阴阳四门色样
                List<Card> lchong2 = CardChong.GetChongCards(llTotals[j], llWin[j]);
                List<Card> lwin_chong0 = new List<Card>();
                //lchong2.RemoveAll(lwin.Contains);

                if (lchong2 != null && lchong2.Count > 0 &&
                    EngineTool.isJinZhang(lchong2[0]) &&
                    EngineTool.isJinZhang(gameData.BaseCards[0]) &&
                    lchong2[0].Value == gameData.BaseCards[0].Value &&
                    lchong2[0].Suit == gameData.BaseCards[0].Suit)
                {
                    lwin_chong0.AddRange(llWin[j]);
                    lwin_chong0.Add(lchong2[0]);
                    List<CardType> lt0 = GameEngine.GetSeYangPlay4(llWin[j]);
                    List<CardType> lt1 = GameEngine.GetSeYangPlay4(lwin_chong0);
                    if (lt1 != null && lt1.Count > 0 )                        
                    {
                        if (lt0 != null && lt0.Count > 0)
                            lt1.RemoveAll(lt0.Contains);
                        if (lt1 != null)
                        {
                            foreach (var item in lt1)
                            {
                                if (item.Jifen > 0)
                                {
                                    thescore = item.Jifen / 2 > 20 ? 20 : item.Jifen / 2;
                                    lscoreseyang[j] += thescore;
                                    stip += gameData.Players[j].Name + " : " + item.Name + "   " + thescore.ToString() + "\r\n";
                                    Debug.Log("冲出阴阳四门:" + stip);                                  
                                }
                            }
                        }
                    }
                }

            }

            //奪錦標勝負結算
            if (nbaseduo > 0)
            {
                for (int j = 0; j < gameData.Players.Count; j++)
                {
                    if (j == ilosedouble)
                        gameData.Players[j].Score -= nbaseduo * 2;
                    else if (j == iwin)
                    {
                        gameData.Players[j].Score += nbaseduo * 4;
                        stip += gameData.Players[j].Name + " : " + sduojinbiao + "   " + nbaseduo.ToString() + "\r\n";
                    }
                    else
                        gameData.Players[j].Score -= nbaseduo;

                    
                }
            }

            for (int j = 0; j < gameData.Players.Count; j++)
            {
                
                seyangscore = lscoreseyang[j] * 3 - lscoreseyang[(j + 1) % 4]
                                                                 - lscoreseyang[(j + 2) % 4]
                                                                 - lscoreseyang[(j + 3) % 4];
                //gameData.Players[j].Score += seyangscore;
                //gameData.Players[j].thechong += seyangscore;


                if (llTotals[j] != null && llWin[j] != null)
                    lchongs[j] = CardChong.GetChongCards(llTotals[j], llWin[j]);

                
                //庄闲对冲
                if (j == iLord)
                {
                    thescore = lScore[j] * 3 - lScore[(j + 1) % 4]
                                                                - lScore[(j + 2) % 4]
                                                                - lScore[(j + 3) % 4];
                    thescore += seyangscore;
                    gameData.Players[j].thechong = thescore;
                    gameData.Players[j].Score += thescore;
                    stip += gameData.Players[j].Name + " : " + thescore.ToString() + StringFanConst.Chong;
                    for (int x = 0; x < gameData.Players.Count; x++)
                    {
                        if (x == j && lScore[x] > 0)
                            stip += "  +" + (lScore[x] * 3).ToString();
                        else if (lScore[x] > 0)
                            stip += "  -" + lScore[x] .ToString();
                    }
                    stip += "\r\n";
                }
                else
                {
                    thescore = lScore[j] - lScore[iLord];
                    thescore += seyangscore;
                    gameData.Players[j].thechong = thescore;
                    gameData.Players[j].Score += thescore;
                    stip += gameData.Players[j].Name + " : " + thescore.ToString() + StringFanConst.Chong;
                    if (lScore[j] > 0)
                        stip += "   +" + lScore[j].ToString();
                    if (lScore[iLord] > 0)
                        stip += "    -" + lScore[iLord].ToString();
                    stip += "\r\n";
                }

            }

            int index = gameData.Players.FindIndex(
           (p) =>
           {
               return p.Name == gameData.PlayerSelf.Name;
           });//找到玩家本人所在的顺序
            gameData.PlayerSelf.Score = gameData.Players[index].Score;
            gameData.Player2.Score = gameData.Players[(index + 1) % 4].Score;
            gameData.Player3.Score = gameData.Players[(index + 2) % 4].Score;
            gameData.Player4.Score = gameData.Players[(index + 3) % 4].Score;


            dispatcher.Dispatch(ViewConst.ShowBaseCards_Chong, gameData.BaseCards.ToArray());//显示底牌           
            dispatcher.Dispatch(ViewConst.UpdateStatus_Base, gameData.Players[indexSelf].Score);//显示玩家分数
            dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipKanChong);//提示玩家冲出色样组合
            if (!string.IsNullOrEmpty(stip) ) 
            dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, stip);//提示玩家冲出色样组合

            if (lchongs[indexSelf % 4] != null && lchongs[indexSelf % 4].Count > 0)
                dispatcher.Dispatch(ViewConst.ShowHandCards, lchongs[indexSelf % 4].ToArray());
            if (lchongs[(indexSelf + 1) % 4] != null && lchongs[(indexSelf + 1) % 4].Count > 0)
                dispatcher.Dispatch(ViewConst.ShowChongCards2, lchongs[(indexSelf + 1) % 4].ToArray());
            if (lchongs[(indexSelf + 2) % 4] != null && lchongs[(indexSelf + 2) % 4].Count > 0)
                dispatcher.Dispatch(ViewConst.ShowChongCards3, lchongs[(indexSelf + 2) % 4].ToArray());
            if (lchongs[(indexSelf + 3) % 4] != null && lchongs[(indexSelf + 3) % 4].Count > 0)
                dispatcher.Dispatch(ViewConst.ShowChongCards4, lchongs[(indexSelf + 3) % 4].ToArray());


            //dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.ViewResult);          

        }
    }
}