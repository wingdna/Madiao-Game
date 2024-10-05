using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts;

public class GameOverCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    [Inject(ContextKeys.CONTEXT_VIEW)]

    public GameObject ContextView { get; set; }
    //public static GameEngine engine;
    public override void Execute()
    {
        if (gameData.ClaimCount > 0)//本局斗牌结算是否已结束并初始化
            return;
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        string result = "";
        SoundManager.Instance.PlaySound("SoundEffect/GameOver");
        
        //if (recData.player.Name.Equals(gameData.Landlord))//赢的是地主
        //{
        //    result = StringFanConst.Lose;
        //    if (recData.player.Name.Equals(gameData.PlayerSelf.Name))//自己是地主
        //    {
        //        result = StringFanConst.Win;
        //    }
        //    else//不是地主
        //    {
        //        result = StringFanConst.Lose;
        //    }
        //}
        //else//赢的是农民
        //{
        //    if (gameData.PlayerSelf.Name.Equals(gameData.Landlord))//自己是地主
        //    {
        //        result = StringFanConst.Lose;
        //    }
        //    else//自己是农民，获胜
        //    {
        //        result = StringFanConst.Win;
        //    }
        //}


        #region 各家吊数统计     
        int i1 = gameData.PlayerSelf.winCards.Count;//先计算各家得桌数 得2桌正本 <2则-1吊 >2则得1吊
        int i2 = gameData.Player2.winCards.Count;
        int i3 = gameData.Player3.winCards.Count;
        int i4 = gameData.Player4.winCards.Count;
        
        List<int> ldiao = EngineTool.CalcBaseJifen(i1, i2, i3, i4);//计算各家吊数
        List<int> ljifen = new List<int> { 0, 0, 0, 0 };//公贺 三家齐贺
        List<int> ljifenB = new List<int> { 0, 0, 0, 0 };//庄散之间
        List<int> ljifenC = new List<int> { 0, 0, 0, 0 };//敲门 散家与另二散家之间
        string zhuang1 = ":", zhuang2 = ":", zhuang3 = ":", zhuang4 = ":";
        if (gameData.PlayerSelf.Name.Equals(gameData.Landlord))
            zhuang1 = StringFanConst.Zhuang;
        else if (gameData.Player2.Name.Equals(gameData.Landlord))
            zhuang2 = StringFanConst.Zhuang;
        else if (gameData.Player3.Name.Equals(gameData.Landlord))
            zhuang3 = StringFanConst.Zhuang;
        else if (gameData.Player4.Name.Equals(gameData.Landlord))
            zhuang4 = StringFanConst.Zhuang;
        result = gameData.PlayerSelf.Name + zhuang1 + ldiao[0].ToString() + StringFanConst.Diao
               + gameData.Player2.Name + zhuang2 + ldiao[1].ToString() + StringFanConst.Diao
               + gameData.Player3.Name + zhuang3 + ldiao[2].ToString() + StringFanConst.Diao
               + gameData.Player4.Name + zhuang4 + ldiao[3].ToString() + StringFanConst.Diao + "\r\n";
        #endregion
        string txt1 = null, txt2 = null, txt3 = null, txt0 = null;
        List<CardType> lt0 = new List<CardType>(),
                    lt1 = new List<CardType>(),
                    lt2 = new List<CardType>(),
                    lt3 = new List<CardType>();
        //计算免斗色样,无须出牌
        if (gameData.RestCardNum == 8 && !gameData.IsFightOver() )
        {
            lt0 = GameEngine.SeYangNoPlay(gameData.PlayerSelf.Cards);
            lt1 = GameEngine.SeYangNoPlay(gameData.Player2.Cards);
            lt2 = GameEngine.SeYangNoPlay(gameData.Player3.Cards);
            lt3 = GameEngine.SeYangNoPlay(gameData.Player4.Cards);
        }
        else//斗上色样
        {
            
            if (i1 > 1) lt0 = GameEngine.GetSeyang(gameData.PlayerSelf.winCards);
            if (i2 > 1) lt1 = GameEngine.GetSeyang(gameData.Player2.winCards);
            if (i3 > 1) lt2 = GameEngine.GetSeyang(gameData.Player3.winCards);
            if (i4 > 1) lt3 = GameEngine.GetSeyang(gameData.Player4.winCards);
            #region 顺风旗  香炉脚 捉极献极&献百        
            if (gameData.PlayerSelf.EpicStatus >= EpicCards.ShunFengQi)
                lt0.Add(GameEngine.ShunFeng( gameData.PlayerSelf.winCards));
            if (gameData.Player2.EpicStatus >= EpicCards.ShunFengQi)
                lt1.Add(GameEngine.ShunFeng(gameData.Player2.winCards));
            if (gameData.Player3.EpicStatus >= EpicCards.ShunFengQi)
                lt2.Add(GameEngine.ShunFeng(gameData.Player3.winCards));
            if (gameData.Player4.EpicStatus >= EpicCards.ShunFengQi)
                lt3.Add(GameEngine.ShunFeng(gameData.Player4.winCards));

            //四极在手 福禄寿喜
            if (gameData.PlayerSelf.EpicStatus == EpicCards.FourQu ||
                gameData.PlayerSelf.EpicStatus == EpicCards.ShunFeng4Qu)
                lt0.Add(GameEngine.FuLuShouXi(gameData.PlayerSelf.winCards));
            if (gameData.Player2.EpicStatus == EpicCards.FourQu ||
                gameData.Player2.EpicStatus == EpicCards.ShunFeng4Qu)
                lt1.Add(GameEngine.FuLuShouXi(gameData.Player2.winCards));
            if (gameData.Player3.EpicStatus == EpicCards.FourQu ||
                gameData.Player3.EpicStatus == EpicCards.ShunFeng4Qu)
                lt2.Add(GameEngine.FuLuShouXi(gameData.Player3.winCards));
            if (gameData.Player4.EpicStatus == EpicCards.FourQu ||
                gameData.Player4.EpicStatus == EpicCards.ShunFeng4Qu)
                lt3.Add(GameEngine.FuLuShouXi(gameData.Player4.winCards));
            //四肩在手 
            if (gameData.PlayerSelf.EpicStatus == EpicCards.FourJian ||
                gameData.PlayerSelf.EpicStatus == EpicCards.ShunFeng4Jian)
                lt0.Add(GameEngine.FourJian(gameData.PlayerSelf.winCards));
            if (gameData.Player2.EpicStatus == EpicCards.FourJian ||
                gameData.PlayerSelf.EpicStatus == EpicCards.ShunFeng4Jian)
                lt1.Add(GameEngine.FourJian(gameData.Player2.winCards));
            if (gameData.Player3.EpicStatus == EpicCards.FourJian ||
                gameData.PlayerSelf.EpicStatus == EpicCards.ShunFeng4Jian)
                lt2.Add(GameEngine.FourJian(gameData.Player3.winCards));
            if (gameData.Player4.EpicStatus == EpicCards.FourJian ||
                gameData.PlayerSelf.EpicStatus == EpicCards.ShunFeng4Jian)
                lt3.Add(GameEngine.FourJian(gameData.Player4.winCards));
            //香炉脚
            if (i1 > 0 && i2 > 0 && i3 > 0 && i4 > 0)//香炉脚必须每家得桌
            {
                CardType ctxlj = GameEngine.XiangLuJiao(gameData.PlayerSelf.winCards,
                                          gameData.Player2.winCards,
                                          gameData.Player3.winCards,
                                          gameData.Player4.winCards);
                if (ctxlj != null)
                {
                    if (gameData.PlayerSelf.winCards.Count == 5)
                        lt0.Add(ctxlj);
                    if (gameData.Player2.winCards.Count == 5)
                        lt1.Add(ctxlj);
                    if (gameData.Player3.winCards.Count == 5)
                        lt2.Add(ctxlj);
                    if (gameData.Player4.winCards.Count == 5)
                        lt3.Add(ctxlj);
                }
            }
            //捉极献极&捉极献百
            if (gameData.Zhuoji )
            {
                if (gameData.PlayerSelf.Zhuoji >= 0 && GameEngine.ZhuoXian(gameData.PlayerSelf.winCards,gameData.PlayerSelf.Zhuoji) != null) 
                    lt0.Add(GameEngine.ZhuoXian(gameData.PlayerSelf.winCards, gameData.PlayerSelf.Zhuoji));
                if (gameData.Player2.Zhuoji >= 0 && GameEngine.ZhuoXian(gameData.Player2.winCards, gameData.Player2.Zhuoji)!=null)
                    lt1.Add(GameEngine.ZhuoXian(gameData.Player2.winCards, gameData.Player2.Zhuoji));
                if (gameData.Player3.Zhuoji >= 0 && GameEngine.ZhuoXian(gameData.Player3.winCards, gameData.Player3.Zhuoji) != null)
                    lt2.Add(GameEngine.ZhuoXian(gameData.Player3.winCards, gameData.Player3.Zhuoji));
                if (gameData.Player4.Zhuoji >= 0 && GameEngine.ZhuoXian(gameData.Player4.winCards, gameData.Player4.Zhuoji) != null)
                    lt3.Add(GameEngine.ZhuoXian(gameData.Player4.winCards, gameData.Player4.Zhuoji));

            }

            //佛赤脚           
                if (i1 == 1 && GameEngine.FoChiJiao(gameData.PlayerSelf.winCards) != null)
                    lt0.Add(GameEngine.FoChiJiao(gameData.PlayerSelf.winCards));
                if (i2 == 1 && GameEngine.FoChiJiao(gameData.Player2.winCards) != null)
                    lt1.Add(GameEngine.FoChiJiao(gameData.Player2.winCards));
                if (i3 == 1 && GameEngine.FoChiJiao(gameData.Player3.winCards) != null)
                    lt2.Add(GameEngine.FoChiJiao(gameData.Player3.winCards));
                if (i4 == 1 && GameEngine.FoChiJiao(gameData.Player4.winCards) != null)
                    lt3.Add(GameEngine.FoChiJiao(gameData.Player4.winCards));


            #endregion
        }
        #region 四八家本轮积分计算
        //统计四八家有无色样        
        if ( lt0 != null && lt0.Count > 0  )
        {
            foreach (CardType c in lt0)//计入各色样名称 牌组 与开注
            {
                ljifen[0] += c.Jifen;//色样为公贺，三家齐贺
                SoundManager.Instance.PlaySound("SoundSY/" + c.Name);
                txt0 += GameEngine.ConvertMdName(c.CardKey) + "  " + c.Name + "  " + c.Jifen.ToString() + StringFanConst.ZhuHe + "\r\n";
            }
            
        }
        //以下大小突散家开庄家，庄家开其一散家， 大活百、大活百突、全突大活散家另敲2散家
        CardType ct = new CardType();
        if (gameData.PlayerSelf.isBai == true)
        {
            ct = GameEngine.JifenBaiTu(gameData.PlayerSelf.winCards,true);         
        }
        if (i1 > 0 && gameData.PlayerSelf.winCards.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))//四八家手持百老
        {
            ct = GameEngine.JifenBaiTu(gameData.PlayerSelf.winCards);//各种含百老牌组
        }
          
        if (ct != null && ct.Jifen > 0)
            {
                ljifenB[0] += ct.Jifen;
                if (!gameData.Landlord.Equals(gameData.PlayerSelf.Name))//四八家有百老 非庄家                {
                {
                    if (ct.Name == CardsTypeMdFan.QuanTuDaHuo)//全突大活
                        ljifenC[0] = 16;
                    if (ct.Name == CardsTypeMdFan.DaHuoBaiTu)//大活百突
                        ljifenC[0] = 2;
                    if (ct.Name == CardsTypeMdFan.DaHuoBai)//大活百
                        ljifenC[0] = 1;
                    txt0 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                          + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu
                          + StringFanConst.QiaoMen + ljifenC[0].ToString() + StringFanConst.Zhu + "\r\n";
                }
                else
                    txt0 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                         + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            ct = null;
            }
           
        

        //小突：九十 + （五 六 八） 万之一
        ct = CardSeyang.Xiaotu(gameData.PlayerSelf.winCards);
        if (ct != null)
        {
            ljifenB[0] += ct.Jifen;
            txt0 += GameEngine.ConvertMdName(ct.CardKey) + "  " + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
        }

        //无正色 按赏一肩二趣三百四 庄散之间  必须正本才算
        if (i1 > 1 && GameEngine.GetSeyang4(gameData.PlayerSelf.winCards) == null )
        {
            ct = GameEngine.JifenSJBQ(gameData.PlayerSelf.winCards);
            if (ct != null)
            {
                ljifenB[0] += ct.Jifen;
                txt0 += ct.CardKey + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            }
                
        }
       
        if (txt0 != null)
            result += gameData.PlayerSelf.Name + ": " + txt0 ;
        #endregion

        #region 一五九家本轮积分计算
        //统计一五九家有无色样        
        if (lt1 != null && lt1.Count > 0 )
        {
            foreach (CardType c in lt1)//计入各色样名称 牌组 与开注
            {
                ljifen[1] += c.Jifen;//色样为公贺，三家齐贺
                SoundManager.Instance.PlaySound("SoundSY/" + c.Name);
                txt1 += GameEngine.ConvertMdName(c.CardKey) + "  " + c.Name + "  " + c.Jifen.ToString() + StringFanConst.ZhuHe + "\r\n";
            }

        }
        //以下大小突散家开庄家，庄家开三家， 大活百、大活百突、全突大活散家另敲2散家
        ct = null;
        if (gameData.Player2.isBai == true)
        {
            ct = GameEngine.JifenBaiTu(gameData.Player2.winCards, true);
        }
        if (i2 > 0 && gameData.Player2.winCards.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))//四八家手持百老
        {
            ct = GameEngine.JifenBaiTu(gameData.Player2.winCards);//各种含百老牌组
        }

        if (ct != null && ct.Jifen > 0)
        {
                ljifenB[1] += ct.Jifen;
                if (!gameData.Landlord.Equals(gameData.Player2.Name))//一五九家为百老之家 非庄
                {   //大活敲门(散家敲2散家)
                    if (ct.Name == CardsTypeMdFan.QuanTuDaHuo) //全突大活
                        ljifenC[1] = 16;
                    if (ct.Name == CardsTypeMdFan.DaHuoBaiTu)  //大活百突
                        ljifenC[1] = 2;
                    if (ct.Name == CardsTypeMdFan.DaHuoBai)    //大活百
                        ljifenC[1] = 1;
                    txt1 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                          + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu
                          + StringFanConst.QiaoMen + ljifenC[1].ToString() + StringFanConst.Zhu + "\r\n";
                }
                else //庄家开三散家
                    txt1 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                         + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            ct = null;

        }
        //无正色 按赏一肩二趣三百四 庄散之间
        if (i2 > 1 && GameEngine.GetSeyang4(gameData.Player2.winCards) == null)
        {
            ct = GameEngine.JifenSJBQ(gameData.Player2.winCards);
            if (ct != null)
            {
                ljifenB[1] += ct.Jifen;
                txt1 += ct.CardKey + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            }

        }
        //小突：九十 + （五 六 八） 万之一  庄散之间
        ct = CardSeyang.Xiaotu(gameData.Player2.winCards);
        if (ct != null)
        {
            ljifenB[1] += ct.Jifen;
            txt1 += GameEngine.ConvertMdName(ct.CardKey) + "  " + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
        }
        if (txt1 != null)
            result += gameData.Player2.Name + ": " + txt1 ;
        #endregion

        #region 二六家本轮积分计算
        //统计二六家有无色样        
        if (lt2 != null && lt2.Count > 0)
        {
            foreach (CardType c in lt2)//计入各色样名称 牌组 与开注
            {
                ljifen[2] += c.Jifen;//色样为公贺，三家齐贺
                SoundManager.Instance.PlaySound("SoundSY/" + c.Name);
                txt2 += GameEngine.ConvertMdName(c.CardKey) + "  " + c.Name + "  " + c.Jifen.ToString() + StringFanConst.ZhuHe + "\r\n";
            }

        }
        //以下大小突散家开庄家，庄家开三家， 大活百、大活百突、全突大活散家另敲2散家
        ct = null;
        if (gameData.Player3.isBai == true)
        {
            ct = GameEngine.JifenBaiTu(gameData.Player3.winCards, true);
        }
        if (i3 > 0 && gameData.Player3.winCards.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))//四八家手持百老
        {
            ct = GameEngine.JifenBaiTu(gameData.Player3.winCards);//各种含百老牌组
        }
        if (ct != null && ct.Jifen > 0)
        {
                ljifenB[2] += ct.Jifen;
                if (!gameData.Landlord.Equals(gameData.Player3.Name))//百老之家非庄
                {   //大活敲门(散家敲2散家)
                    if (ct.Name == CardsTypeMdFan.QuanTuDaHuo) //全突大活
                        ljifenC[2] = 16;
                    if (ct.Name == CardsTypeMdFan.DaHuoBaiTu)  //大活百突
                        ljifenC[2] = 2;
                    if (ct.Name == CardsTypeMdFan.DaHuoBai)    //大活百
                        ljifenC[2] = 1;
                    txt2 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                          + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu
                          + StringFanConst.QiaoMen + ljifenC[2].ToString() + StringFanConst.Zhu + "\r\n";
                }
                else //庄家开三散家
                    txt2 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                         + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            ct = null;
        }
           
        
        //无正色 按赏一肩二趣三百四 庄散之间
        if (i3 > 1 && GameEngine.GetSeyang4(gameData.Player3.winCards) == null)
        {
            ct = GameEngine.JifenSJBQ(gameData.Player3.winCards);
            if (ct != null)
            {
                ljifenB[2] += ct.Jifen;
                txt2 += ct.CardKey + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            }

        }
        //小突：九十 + （五 六 八） 万之一  庄散之间
        ct = CardSeyang.Xiaotu(gameData.Player3.winCards);
        if (ct != null)
        {
            ljifenB[2] += ct.Jifen;
            txt2 += GameEngine.ConvertMdName(ct.CardKey) + "  " + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
        }
        if (txt2 != null)
            result += gameData.Player3.Name + ": " + txt2 ;
        #endregion

        #region 三七家本轮积分计算
        //统计三七家有无色样        
        if (lt3 != null && lt3.Count > 0)
        {
            foreach (CardType c in lt3)//计入各色样名称 牌组 与开注
            {
                ljifen[3] += c.Jifen;//色样为公贺，三家齐贺
                SoundManager.Instance.PlaySound("SoundSY/" + c.Name);
                Debug.Log(StringFanConst.ConvertMadiao + c.CardKey);
                txt3 += GameEngine.ConvertMdName(c.CardKey) + "  " + c.Name + "  " + c.Jifen.ToString() + StringFanConst.ZhuHe + "\r\n";
            }

        }
        //以下大小突散家开庄家，庄家开三家， 大活百、大活百突、全突大活散家另敲2散家
        ct = null;
        if (gameData.Player4.isBai == true)
        {
            ct = GameEngine.JifenBaiTu(gameData.Player4.winCards, true);
        }
        if (i4 > 0 && gameData.Player4.winCards.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))//四八家手持百老
        {
            ct = GameEngine.JifenBaiTu(gameData.Player4.winCards);//各种含百老牌组
        }

        if (ct != null && ct.Jifen > 0)
        {
                ljifenB[3] += ct.Jifen;
                if (!gameData.Landlord.Equals(gameData.Player4.Name))//百老之家非庄
                {   //大活敲门(散家敲2散家)
                    if (ct.Name == CardsTypeMdFan.QuanTuDaHuo) //全突大活
                        ljifenC[3] = 16;
                    if (ct.Name == CardsTypeMdFan.DaHuoBaiTu)  //大活百突
                        ljifenC[3] = 2;
                    if (ct.Name == CardsTypeMdFan.DaHuoBai)    //大活百
                        ljifenC[3] = 1;
                    txt3 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                          + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu
                          + StringFanConst.QiaoMen + ljifenC[3].ToString() + StringFanConst.Zhu + "\r\n";
                }
                else //庄家开三散家
                    txt3 += GameEngine.ConvertMdName(ct.CardKey) + "  "
                         + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            ct = null;
        }
         
        
        //无正色 按赏一肩一趣二百四 庄散之间
        if (i4 > 1 && GameEngine.GetSeyang4(gameData.Player4.winCards) == null)
        {
            ct = GameEngine.JifenSJBQ(gameData.Player4.winCards);
            if (ct != null)
            {
                ljifenB[3] += ct.Jifen;
                txt3 += ct.CardKey + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
            }

        }
        //小突：九十 + （五 六 八） 万之一  庄散之间
        ct = CardSeyang.Xiaotu(gameData.Player4.winCards);
        if (ct != null)
        {
            ljifenB[3] += ct.Jifen;
            txt3 += GameEngine.ConvertMdName(ct.CardKey) + "  " + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
        }
        if (txt3 != null)
            result += gameData.Player4.Name + ": " + txt3 ;
        #endregion

        int score0 = 0, score1 = 0, score2 = 0, score3 = 0;

        #region 四八家最终得分结算

        ljifenB[0] += ldiao[0]; //赢筹为所得吊数
        
        if (ljifen[0] != 0)//公贺 三家齐贺
        {
            score0 += ljifen[0] * 3;
            score1 -= ljifen[0] ;
            score2 -= ljifen[0];
            score3 -= ljifen[0];
        }

        //庄家大活及单项积分 赢三散家
        if (gameData.PlayerSelf.Name.Equals( gameData.Landlord) )
        {
            score0 += ljifenB[0] * 3;
            score1 -= ljifenB[0];
            score2 -= ljifenB[0];
            score3 -= ljifenB[0];
        }
        else //散家大活 赢庄家 敲二散家
        {
            score0 += ljifenB[0] ;//四八家开庄家
            score0 += ljifenC[0] * 2;//敲二散家

            if (gameData.Player2.Name.Equals(gameData.Landlord) )
            {
                score1 -= ljifenB[0];//庄家被开
                score2 -= ljifenC[0];//散家被敲
                score3 -= ljifenC[0];//散家被敲
            }
            else if (gameData.Player3.Name.Equals(gameData.Landlord))
            {
                score2 -= ljifenB[0];//庄家被开
                score1 -= ljifenC[0];//散家被敲
                score3 -= ljifenC[0];//散家被敲
            }
            else if (gameData.Player4.Name.Equals(gameData.Landlord))
            {
                score3 -= ljifenB[0];//庄家被开
                score1 -= ljifenC[0];//散家被敲
                score2 -= ljifenC[0];//散家被敲
            }
        }
        #endregion

        #region 一五九家最终得分结算

        ljifenB[1] += ldiao[1];

        if (ljifen[1] != 0)//公贺 三家齐贺
        {
            score1 += ljifen[1] * 3;
            score0 -= ljifen[1];
            score2 -= ljifen[1];
            score3 -= ljifen[1];
        }

        //庄家大活及单项积分 赢三散家
        if (gameData.Player2.Name.Equals(gameData.Landlord))
        {
            score1 += ljifenB[1] * 3;
            score0 -= ljifenB[1];
            score2 -= ljifenB[1];
            score3 -= ljifenB[1];
        }
        else //散家大活 赢庄家 敲二散家
        {
            score1 += ljifenB[1];//四八家开庄家
            score1 += ljifenC[1] * 2;//敲二散家

            if (gameData.PlayerSelf.Name.Equals(gameData.Landlord))
            {
                score0 -= ljifenB[1];//庄家被开
                score2 -= ljifenC[1];//散家被敲
                score3 -= ljifenC[1];//散家被敲
            }
            else if (gameData.Player3.Name.Equals(gameData.Landlord))
            {
                score2 -= ljifenB[1];//庄家被开
                score0 -= ljifenC[1];//散家被敲
                score3 -= ljifenC[1];//散家被敲
            }
            else if (gameData.Player4.Name.Equals(gameData.Landlord))
            {
                score3 -= ljifenB[1];//庄家被开
                score0 -= ljifenC[1];//散家被敲
                score2 -= ljifenC[1];//散家被敲
            }
        }
        #endregion

        #region 二六家最终得分结算

        ljifenB[2] += ldiao[2];

        if (ljifen[2] != 0)//公贺 三家齐贺
        {
            score2 += ljifen[2] * 3;
            score1 -= ljifen[2];
            score0 -= ljifen[2];
            score3 -= ljifen[2];
        }

        //庄家大活及单项积分 赢三散家
        if (gameData.Player3.Name.Equals(gameData.Landlord))
        {
            score2 += ljifenB[2] * 3;
            score1 -= ljifenB[2];
            score0 -= ljifenB[2];
            score3 -= ljifenB[2];
        }
        else //散家大活 赢庄家 敲二散家
        {
            score2 += ljifenB[2];//四八家开庄家
            score2 += ljifenC[2] * 2;//敲二散家

            if (gameData.PlayerSelf.Name.Equals(gameData.Landlord))
            {
                score0 -= ljifenB[2];//庄家被开
                score1 -= ljifenC[2];//散家被敲
                score3 -= ljifenC[2];//散家被敲
            }
            else if (gameData.Player2.Name.Equals(gameData.Landlord))
            {
                score1 -= ljifenB[2];//庄家被开
                score0 -= ljifenC[2];//散家被敲
                score3 -= ljifenC[2];//散家被敲
            }
            else if (gameData.Player4.Name.Equals(gameData.Landlord))
            {
                score3 -= ljifenB[2];//庄家被开
                score1 -= ljifenC[2];//散家被敲
                score0 -= ljifenC[2];//散家被敲
            }
        }
        #endregion

        #region 三七家最终得分结算

        ljifenB[3] += ldiao[3];

        if (ljifen[3] != 0)//公贺 三家齐贺
        {
            score3 += ljifen[3] * 3;
            score1 -= ljifen[3];
            score0 -= ljifen[3];
            score2 -= ljifen[3];
        }

        //庄家大活及单项积分 赢三散家
        if (gameData.Player4.Name.Equals(gameData.Landlord))
        {
            score3 += ljifenB[3] * 3;
            score1 -= ljifenB[3];
            score0 -= ljifenB[3];
            score2 -= ljifenB[3];
        }
        else //散家大活 赢庄家 敲二散家
        {
            score3 += ljifenB[3];//四八家开庄家
            score3 += ljifenC[3] * 2;//敲二散家

            if (gameData.PlayerSelf.Name.Equals(gameData.Landlord))
            {
                score0 -= ljifenB[3];//庄家被开
                score1 -= ljifenC[3];//散家被敲
                score2 -= ljifenC[3];//散家被敲
            }
            else if (gameData.Player2.Name.Equals(gameData.Landlord))
            {
                score1 -= ljifenB[3];//庄家被开
                score0 -= ljifenC[3];//散家被敲
                score2 -= ljifenC[3];//散家被敲
            }
            else if (gameData.Player3.Name.Equals(gameData.Landlord))
            {
                score2 -= ljifenB[3];//庄家被开
                score1 -= ljifenC[3];//散家被敲
                score0 -= ljifenC[3];//散家被敲
            }
        }
        #endregion

        
        string slord = gameData.Landlord;
        MdMode mode = gameData.CurrentMode;
        
        gameData.Init();//初始化数据
        gameData.Landlord = slord;//庄家
        gameData.CurrentMode = mode;//游戏模式：单机或联机
        
        gameData.PlayerSelf.Score += score0;
        gameData.Player2.Score += score1;
        gameData.Player3.Score += score2;
        gameData.Player4.Score += score3;
        

        int index = gameData.Players.FindIndex(
            (p) =>
            {
                return p.Name == gameData.PlayerSelf.Name;
            });//找到玩家本人所在的顺序

        int[] Numbers = new int[] { gameData.Players[index].thechong+score0,
                                    gameData.Players[(index + 1) % 4].thechong+ score1,
                                    gameData.Players[(index + 2) % 4].thechong+ score2,
                                    gameData.Players[(index + 3) % 4].thechong+ score3 };
        int maxPos = GameEngine.MaxValue(Numbers);
        result += "\r\n" + StringFanConst.TheWinner;
        switch (maxPos)
        {
            case -1:
                result += StringFanConst.MultiWinner + gameData.Landlord + StringFanConst.LianZhuang;
                break;
            case 0:
                result += gameData.PlayerSelf.Name;
                gameData.Landlord = gameData.PlayerSelf.Name;//赢筹最多家为庄
                break;
            case 1:
                result += gameData.Player2.Name;
                gameData.Landlord = gameData.Player2.Name;
                break;
            case 2:
                result += gameData.Player3.Name;
                gameData.Landlord = gameData.Player3.Name;
                break;
            case 3:
                result += gameData.Player4.Name;
                gameData.Landlord = gameData.Player4.Name;
                break;
        }
        result += "\r\n";
        gameData.Players[index].Score = gameData.PlayerSelf.Score;        
        gameData.Players[(index + 1) % 4].Score = gameData.Player2.Score;
        gameData.Players[(index + 2) % 4].Score = gameData.Player3.Score;
        gameData.Players[(index + 3) % 4].Score = gameData.Player4.Score;

        result += gameData.PlayerSelf.Name + StringFanConst.ThisJifen + score0 + StringFanConst.Zhu
            + "  "+ gameData.Players[index].thechong+ StringFanConst.Chong
            + "  " + StringFanConst.TotalJifen + gameData.PlayerSelf.Score + StringFanConst.Zhu + "\r\n";
        result += gameData.Player2.Name + StringFanConst.ThisJifen + score1 + StringFanConst.Zhu
            + "  " + gameData.Player2.thechong + StringFanConst.Chong 
            + "  " + StringFanConst.TotalJifen + gameData.Player2.Score + StringFanConst.Zhu + "\r\n"; 
        result += gameData.Player3.Name + StringFanConst.ThisJifen + score2 + StringFanConst.Zhu
            + gameData.Player3.thechong + StringFanConst.Chong
            +StringFanConst.TotalJifen + gameData.Player3.Score + StringFanConst.Zhu + "\r\n";
        result += gameData.Player4.Name + StringFanConst.ThisJifen + score3 + StringFanConst.Zhu
            + gameData.Player4.thechong + StringFanConst.Chong
            + StringFanConst.TotalJifen + gameData.Player4.Score + StringFanConst.Zhu + "\r\n";

        Debug.Log("ShowGameOverUICommand: The Winner is:" + recData.player.Name);
        Transform canvas = ContextView.transform.Find("Canvas");
        GameObject GameOverUI = canvas.Find("GameOverUI(Clone)").gameObject;
        GameOverUI.transform.SetSiblingIndex(canvas.childCount - 1);//显示在最前面
        GameOverUI.SetActive(true);
        //显示输赢信息
        dispatcher.Dispatch(ViewConst.UpdateGameOverResult, result);
        
        
        //关掉所有计时器
        dispatcher.Dispatch(ViewConst.SwitchTimer_Player1,Player1TimerStatus.GameIn_Off );
        dispatcher.Dispatch(ViewConst.SwitchTimer_Player2, false);
        dispatcher.Dispatch(ViewConst.SwitchTimer_Player3, false);
        dispatcher.Dispatch(ViewConst.SwitchTimer_Player4, false);


        gameData.PlayerSelf.ResetStatus();

        //GameObject.Find("NetClient").GetComponent<ClientService>().Init();
        if (slord == gameData.PlayerSelf.Name)
            recData.cmd = RemoteCMD_Const.Replay;  
        else
            recData.cmd = RemoteCMD_Const.CalcScore;
       
        recData.player = gameData.PlayerSelf;
        recData.player = new PlayerInfo() { Name = gameData.PlayerSelf.Name, Score = gameData.PlayerSelf.Score };
        clientService.SendDataToServer(new RemoteMsg(recData));
        //if (slord == gameData.PlayerSelf.Name)

        //{
        //    cmd = RemoteCMD_Const.Replay,
        //    Winner = slord,
        //    player = new PlayerInfo()
        //    { Name = gameData.PlayerSelf.Name }
        //}));//游戏结束  

    }
}