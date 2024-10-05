using UnityEngine;

using System.Collections.Generic;
using System;
using Assets.Scripts.Model;
using System.Linq;
public enum MdMode
{
    MutiPlayer,
    SinglePlayer
}

public enum FirstStatus
{
    FirstOK,
    Flower3Not,
    JinShouShizi,
    WuShangGaoBai,
    LordFirst,
    TwoDoorOpen,
    ChuangQian,
    DaoTiQianWan,
    TiWan5Zhuo,
    TiJinWanQian,
    JiZhuoTiWan,
    KaiSan,
    LastOneSuit
}
public enum DisRole
{
    UnKownRole = -1,
    FirstCard,
    CatchCard,
    DingCard,
    MidCatchCard,
    BiCard,
    FreeCard,
    MieCard
}
public interface IMdData
{
    List<PlayerInfo> Players { get; set; }
    List<Card> SelectedCards { get; set; }
    List<Card> HandCards { get; set; }
    PlayerInfo PlayerSelf { get; set; }
    PlayerInfo Player2 { get; set; }
    PlayerInfo Player3 { get; set; }
    PlayerInfo Player4 { get; set; }
    List<Card> BaseCards { get; set; }
    List<Card> CurrentDiscards { get; set; }
    List<Card> CurrentDiscards4 { get; set; }

    List<Card> FirstDiscards { get; set; }
    List<Card> AllDiscards { get; set; }

    List<string> FourDoor { get; set; }//头门二门三门十子
    GameStatus CurrentStatus { get; set; }

    FirstStatus firstStatus { get; set; }
    int TheFirstTurn { get; set; }
    int CurrentTurn { get; set; }
    bool bFightTen { get; set; }
    int BaseScore { get; set; }
    int Mutiple { get; set; }
    int ClaimCount { get; set; }
    string Landlord { get; set; }
    int Baijia { get; set; }
    int Flower3 { get; set; }
    bool Zhuoji { get; set; }
    int RestCardNum { get; set; }
    string LastPlayer { get; set; }//上次出牌玩家
    MdMode CurrentMode { get; set; }
    //
    List<Card> GenerateCards();
    bool CardsCompareToDicards(Card[] cards);
    string GetCardsType(Card[] cards);
    void Init();
    List<Card> GetEnabledDisCards(List<Card> cards, string name, DisRole iRole = DisRole.UnKownRole);
    List<Card> SelectMieCards(List<Card> cards);
    int CurrentCards4Top();
    DisRole getRoleCard(int firstTurn, int CurTurn);
    List<PlayerInfo> ResortPlayers(int i);
    bool IsZhuoji(int Turn);
    List<Card> SortFightCards(List<Card> cards);
    bool IsFightOver();
    void Full4Door(string Suit, string name);

    string TipSeyang();
    int CheckMieCard(Card card);
    bool CheckCatchRush(Card card);//闲家急捉闲家
    FirstStatus CheckFirstCard(Card card, List<Card> lc, List<Card> lcwin = null, bool isBaijia = false);

}
public class MdDataCenter : IMdData
{
    public MdMode CurrentMode { get; set; }
    public string LastPlayer { get; set; }
    public int ClaimCount { get; set; }
    public int CurrentTurn { get; set; }

    public int TheFirstTurn { get; set; }

    public bool bFightTen { get; set; }
    public List<PlayerInfo> Players { get; set; }
    public PlayerInfo PlayerSelf { get; set; }
    public PlayerInfo Player2 { get; set; }
    public PlayerInfo Player3 { get; set; }
    public PlayerInfo Player4 { get; set; }
    public string Landlord { get; set; }

    public List<string> FourDoor { get; set; }//头门二门三门十子
    public GameStatus CurrentStatus { get; set; }
    public FirstStatus firstStatus { get; set; }
    public List<Card> SelectedCards { get; set; }
    public List<Card> BaseCards { get; set; }
    public List<Card> HandCards { get; set; }

    public int Baijia { get; set; }
    public int Flower3 { get; set; }
    public bool Zhuoji { get; set; }
    public List<Card> CurrentDiscards { get; set; }
    public List<Card> FirstDiscards { get; set; }
    public List<Card> AllDiscards { get; set; }
    public List<Card> CurrentDiscards4 { get; set; }
    public int Mutiple { get; set; }
    public int BaseScore { get; set; }
    public int RestCardNum { get; set; }
    public MdDataCenter()
    {
        PlayerSelf = new PlayerInfo();
        Player2 = new PlayerInfo();
        Player3 = new PlayerInfo();
        Player4 = new PlayerInfo();
        Players = new List<PlayerInfo>();
        SelectedCards = new List<Card>();
        HandCards = new List<Card>();
        CurrentDiscards = new List<Card>();
        FirstDiscards = new List<Card>();
        AllDiscards = new List<Card>();
        CurrentDiscards4 = new List<Card>();
        BaseCards = new List<Card>();
        FourDoor = new List<string>();
        CurrentStatus = GameStatus.CallLandlord;
        firstStatus = FirstStatus.FirstOK;
        bFightTen = false;
        ClaimCount = 1;
        CurrentTurn = -1;
        TheFirstTurn = 0;
        Baijia = -1;
        Flower3 = 0;
        BaseScore = 1000;
        RestCardNum = 8;
        Mutiple = 1;//默认一倍
        //CurrentMode = MdMode.SinglePlayer;//默认单机
        CurrentMode = MdMode.MutiPlayer;//默认联网
        //Landlord = "";
    }
    public void Init()
    {
        //Players.Clear();
        SelectedCards.Clear();
        HandCards.Clear();

        CurrentDiscards.Clear();
        FirstDiscards.Clear();
        AllDiscards.Clear();
        CurrentDiscards4.Clear();
        BaseCards.Clear();
        CurrentStatus = GameStatus.CallLandlord;
        firstStatus = FirstStatus.FirstOK;
        ClaimCount = 1;
        CurrentTurn = -1;
        RestCardNum = 8;
        Mutiple = 1;
        CurrentMode = MdMode.MutiPlayer;
        LastPlayer = "";
        Landlord = "";
        Baijia = -1;
        Flower3 = 0;
        BaseScore = 1000;

        PlayerSelf.Cards.Clear();
        PlayerSelf.winCards.Clear();
        Player2.Cards.Clear();
        Player2.winCards.Clear();
        Player3.Cards.Clear();
        Player3.winCards.Clear();
        Player4.Cards.Clear();
        Player4.winCards.Clear();

        FourDoor.Clear();
    }

    public void Full4Door(string Suit, string name)
    {
        if (Suit == PokerConst.Spade || FourDoor.IndexOf(Suit) >= 0 || FourDoor.Count >= 2)
            return;
        if (name == Landlord)
        {
            FourDoor.Clear();
            switch (Suit)
            {
                case PokerConst.Club:
                    FourDoor.Add(PokerConst.Diamond);
                    FourDoor.Add(PokerConst.Heart);
                    break;
                case PokerConst.Diamond:
                    FourDoor.Add(PokerConst.Club);
                    FourDoor.Add(PokerConst.Heart);
                    break;
                case PokerConst.Heart:
                    FourDoor.Add(PokerConst.Club);
                    FourDoor.Add(PokerConst.Diamond);
                    break;
            }
            FourDoor.Add(Suit);
            FourDoor.Add(PokerConst.Spade);
        }
        else
        {
            if (FourDoor.IndexOf(Suit) < 0      //花色未加入
              && FourDoor.Count < 2
              && Suit != PokerConst.Spade)
                FourDoor.Add(Suit);

            if (FourDoor.Count == 2)
            {
                if (FourDoor.Exists(suit => suit == PokerConst.Club) && FourDoor.Exists(suit => suit == PokerConst.Diamond))
                    FourDoor.Add(PokerConst.Heart);
                else if (FourDoor.Exists(suit => suit == PokerConst.Heart) && FourDoor.Exists(suit => suit == PokerConst.Diamond))
                    FourDoor.Add(PokerConst.Club);
                else
                    FourDoor.Add(PokerConst.Diamond);
                FourDoor.Add(PokerConst.Spade);
            }
        }
    }

    private bool IsJianJi(Card c)
    {
        if (c.Value == PokerConst.JiValve)
            return true;
        else if (c.Suit == PokerConst.Spade &&
                 c.Value >= EngineTool.GetShangValve(c.Suit) &&
                  c.Value < PokerConst.ShangValve)
            return true;
        else if (c.Suit == PokerConst.Spade &&
                 (c.Value == EngineTool.GetShangValve(c.Suit) - 1))
            return true;
        else
            return false;
    }


    public DisRole getRoleCard(int firstTurn, int CurTurn)
    {
        DisRole rol1 = DisRole.UnKownRole, rol2 = DisRole.UnKownRole;
        int iBoss = Players.FindIndex((p) => { return p.Name == Landlord; });//找到地主所在的顺序;        


        if (Baijia < 0 && CurrentDiscards4.Count > 0 &&
            CurrentDiscards4[0].Suit == PokerConst.Spade &&
                CurrentDiscards4[0].Value < EngineTool.GetShangValve(PokerConst.Spade))
        {

            int disSuits = FirstDiscards.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;
            bool blord = false;
            if (iBoss == CurTurn)
                blord = true;
            if (blord && FourDoor.Count < 2 || disSuits <= 2) //除了闲家正门两路
                Baijia = firstTurn;
            if (!blord && FourDoor.Count < 2) //除了庄发  闲家正门两路
                Baijia = firstTurn;

        }
        rol1 = getXRoleCard(firstTurn, CurTurn, iBoss);
        if (Baijia < 0)
            return rol1;
        rol2 = getXRoleCard(firstTurn, CurTurn, Baijia);
        if (rol2 < 0)
            return rol1;

        if ((CurTurn + 4) % 4 == (firstTurn + 4) % 4)
            return DisRole.FirstCard;     
        if (rol1 == DisRole.DingCard || rol2 == DisRole.DingCard)
            return DisRole.DingCard;
        if (rol1 == DisRole.CatchCard || rol2 == DisRole.CatchCard)
            return DisRole.CatchCard;
        return rol1;
    }
    //0、发牌 1、捉牌 2、顶牌 3、灭牌  有队友在后可自由行动
    private DisRole getXRoleCard(int firstTurn, int CurTurn, int iLord)
    {

        int iTurn = (CurTurn + 4) % 4;
        int iFirst = (firstTurn + 4) % 4;
        int imax = CurrentCards4Top();
        int idxMax = (iFirst + imax) % 4;

        if (iTurn != iFirst && iTurn != iLord )
        {
            if (CurrentDiscards4[0].Value == PokerConst.JiValve &&
                CurrentDiscards4[imax].Value == PokerConst.JiValve)
            {
                if (iTurn == iFirst + 3 || iTurn == iFirst - 1)
                    return DisRole.CatchCard;
                else
                    return DisRole.MidCatchCard;
            }
            if (CurrentDiscards4[0].Suit == PokerConst.Spade &&
                CurrentDiscards4[imax].Suit == PokerConst.Spade)//发十 发极 家家打
            {
                if (iTurn == iFirst + 3 || iTurn == iFirst - 1)
                    return DisRole.CatchCard;
                else
                    return DisRole.MidCatchCard;
            }
            if (Players[idxMax].winCards.Count >= 2 &&idxMax!= iLord)
            {
                var vjin = Players[idxMax].winCards.Where(c => EngineTool.isJinZhang(c));
                int  nSuits = vjin.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;

                //提防香爐腳
                if (Players[(idxMax + 1) % 4].winCards.Count == 1 &&
                    Players[(idxMax + 2) % 4].winCards.Count == 1 &&
                    Players[(idxMax + 3) % 4].winCards.Count == 1)
                    return DisRole.CatchCard;
                if (nSuits >= 2)                //佔大者已有二門錦張上桌必捉
                    return DisRole.CatchCard;


                if( Players[iTurn].winCards.Count < 2)
                {
                    if (RestCardNum < 3)   //第6桌還未正本可盡力上桌                
                        return DisRole.CatchCard;
                    return DisRole.FreeCard;
                }
                if (RestCardNum < 3)
                    return DisRole.FreeCard;
            }


            if (EngineTool.GetJianOrder(CurrentDiscards4[imax], AllDiscards) == 1 && //最大为二肩  孤赏必打 
                EngineTool.IsSingleShang(Players[iTurn].Cards))
                return DisRole.CatchCard;
            if (EngineTool.GetJianOrder(CurrentDiscards4[imax], AllDiscards) >= 2) //最大为二肩三肩  有赏必打                
                return DisRole.CatchCard;
        }

        if (iLord == iFirst) //庄家发牌
        {
            if (iTurn == iFirst)//庄家发牌
                return DisRole.FirstCard;

            if (iTurn == iLord + 1 || iTurn == iLord - 3)//压家捉牌
            {
                return DisRole.MidCatchCard;
            }
            else if (iTurn == iLord + 2 || iTurn == iLord - 2)  //庄家大就捉牌，否则灭牌
            {
                if (imax == 0)
                    return DisRole.MidCatchCard;
                return DisRole.MieCard;
            }
            else if (iTurn == iLord + 3 || iTurn == iLord - 1) //庄家大就捉牌，否则灭牌
            {
                if (imax == 0)
                    return DisRole.CatchCard;

                return DisRole.MieCard;
            }

            return DisRole.MieCard;
        }
        else if (iLord == iFirst + 1 || iLord == iFirst - 3)//庄在第二家
        {
            if (iTurn == iFirst)   //头家发牌
                return DisRole.FirstCard;
            if (iTurn == iLord)    //庄家頂大
                return DisRole.DingCard;
            if (iTurn == iLord + 1 || iTurn == iLord - 3)//压家
            {
                if (imax == 1)
                    return DisRole.MidCatchCard;
                else                                //庄小灭牌
                    return DisRole.MieCard;
            }
            else if (iTurn == iLord + 2 || iTurn == iLord - 2)//底家
            {
                if (imax == 1)//庄大压庄
                    return DisRole.CatchCard;
                else                                 //庄小灭牌
                    return DisRole.MieCard;
            }

            return DisRole.MieCard;
        }
        else if (iLord == iFirst + 2 || iLord == iFirst - 2)//庄在第3家
        {
            if (iTurn == iFirst)//头家
                return DisRole.FirstCard;
            else if (iTurn == iLord - 1 || iTurn == iLord + 3)//逼家  但有压家
                return DisRole.BiCard;
            else if (iTurn == iLord)//庄家-三家
                return DisRole.DingCard;
            else if (CurrentCards4Top() == 2 &&        //压家
                     (iTurn == iLord + 1 || iTurn == iLord - 3))
                return DisRole.CatchCard;
            else
                return DisRole.MieCard;

        }
        else if (iLord == iFirst + 3 || iLord == iFirst - 1)//庄在底家
        {
            if (iTurn == iFirst) //头家
                return DisRole.FirstCard;
            else if (iTurn == iLord - 2 || iTurn == iLord + 2) //二逼家
                return DisRole.BiCard;
            else if (iTurn == iLord - 1 || iTurn == iLord + 3
                     && CurrentCards4Top() == 0)//大逼家
                return DisRole.DingCard;
            else if (iTurn == iLord)    //底家- 庄家
                return DisRole.CatchCard;
            else
                return DisRole.MieCard;
        }

        return DisRole.MieCard;
    }
    public List<Card> SelectMieCards(List<Card> cards)
    {
        List<Card> Result = new List<Card>();

        List<Card> tenCards = new List<Card>();
        List<Card> threeCards = new List<Card>();
        List<Card> jiCards = new List<Card>();
        List<Card> bigCards = new List<Card>();
        List<Card> twoCards = new List<Card>();
        List<Card> oneCards = new List<Card>();

        Card[] temp = (Card[])cards.ToArray().Clone();
        List<Card> yourCards = new List<Card>(temp);

        foreach (Card c in cards)
        {
            if (c.Value == 1)
                jiCards.Add(c);
            else if ((c.Value > 8 && c.Suit.Equals(PokerConst.Spade))
                || (c.Value > 9 && c.Suit.Equals(PokerConst.Club))
                || (c.Value > 7 && (c.Suit.Equals(PokerConst.Heart) || c.Suit.Equals(PokerConst.Diamond))))
                bigCards.Add(c);
            else if (c.Suit.Equals(PokerConst.Spade) && c.Value > 1 && c.Value < 9)
                tenCards.Add(c);
            else if (FourDoor.Count > 2 && c.Suit.Equals(FourDoor[2]))
                threeCards.Add(c);
            else if (FourDoor.Count > 1 && c.Suit.Equals(FourDoor[1]))
                twoCards.Add(c);
            else
                oneCards.Add(c);
        }
        Result.AddRange(tenCards);
        Result.AddRange(threeCards);
        Result.AddRange(jiCards);
        Result.AddRange(twoCards);
        Result.AddRange(oneCards);
        Result.AddRange(bigCards);

        return Result;
    }
    public List<Card> GetEnabledDisCards(List<Card> cards, string name, DisRole iRole = DisRole.FirstCard)
    {
        //Discards4各家出的牌集合
        //if (CurrentDiscards4.Count >= 4)
        //    CurrentDiscards4.Clear();
        //if (CurrentDiscards4.Count <= 0 || CurrentDiscards4 == null)
        //{
        //    CurrentDiscards4.AddRange(CurrentDiscards);//第一个出牌
        //}

        List<Card> Result = new List<Card>();

        Card[] temp = (Card[])cards.ToArray().Clone();
        List<Card> yourCards = new List<Card>(temp);
        //
        yourCards.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });//升序排序
        CurrentDiscards.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });
        string sourceCardType = GetCardsType(CurrentDiscards.ToArray());//得到出的牌型
                                                                        //List<List<Card>> repeats_YourCards = CountRepeats(yourCards.ToArray());//按值重复次数分类
                                                                        //List<List<Card>> repeats_SourceCards = CountRepeats(CurrentDiscards.ToArray());//按值重复次数分类

        Card selCard = new Card();
        if (name.Equals(LastPlayer) || string.IsNullOrEmpty(LastPlayer) && iRole == 0)//上次是自己
        {
            bool bLord = name == Landlord ? true : false,
                 bFlower = Flower3 > 2 ? true : false;


            selCard = FirstCard.GetFirstCard(cards, AllDiscards, bLord, FourDoor, bFlower);




            Result.Add(selCard);
            if (!FourDoor.Exists(suit => suit == selCard.Suit) && FourDoor.Count < 2)
                Full4Door(selCard.Suit, name);
            //移除
            yourCards.Remove(selCard);

            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌
            return Result;
            //for (int i = repeats_YourCards.Count - 2; i >= 0; i--)
            //{
            //    if (repeats_YourCards[i].Count > 0)
            //    {
            //        List<Card> range = yourCards.FindAll(
            //        (c) => {
            //            return c.Value == repeats_YourCards[i][repeats_YourCards[i].Count - 1].Value;
            //        });//从3连对开始找,找最大的
            //        Result.AddRange(range);
            //        //移除
            //        for (int k = 0; k < range.Count; k++)
            //        {
            //            yourCards.Remove(range[k]);
            //        }
            //        cards.Clear();
            //        cards.AddRange(yourCards);//替换当前手牌
            //        return Result;
            //    }
            //}
        }
        switch (sourceCardType)
        {
            #region 地主牌型
            //case CardsType.Bomb:
            //    {
            //        //炸弹，查找第一个重复次数为4的大于上家的重复次数为4的牌
            //        for (int i = 0; i < repeats_YourCards[3].Count; i++)
            //        {
            //            if (repeats_YourCards[3][i].Value > repeats_SourceCards[3][0].Value)
            //            {
            //                for (int j = 0; j < yourCards.Count; j++)
            //                {
            //                    if (yourCards[j].Value == repeats_YourCards[3][i].Value)
            //                    {
            //                        Result.Add(yourCards[j]);//找到不同花色的重复的牌
            //                    }
            //                }
            //                yourCards.RemoveAll((c) =>
            //                {
            //                    return c.Value == repeats_YourCards[3][i].Value;
            //                });//删除所有要出的牌
            //                break;//跳出
            //            }
            //        }
            //    }
            //    break;
            //case CardsType.Double:
            //    {
            //        //对子，查找第一个重复次数为2的大于上家的重复次数为3的牌
            //        for (int i = 0; i < repeats_YourCards[1].Count; i++)
            //        {
            //            if (repeats_YourCards[1][i].Value > repeats_SourceCards[1][0].Value)
            //            {
            //                for (int j = 0; j < yourCards.Count; j++)
            //                {
            //                    if (yourCards[j].Value == repeats_YourCards[1][i].Value)
            //                    {
            //                        Result.Add(yourCards[j]);//找到不同花色的重复的牌
            //                    }
            //                }
            //                yourCards.RemoveAll((c) =>
            //                {
            //                    return c.Value == repeats_YourCards[1][i].Value;
            //                });//删除所有要出的牌
            //                break;//跳出
            //            }
            //        }
            //    }
            //    break;
            //case CardsType.Triple:
            //    {
            //        //三张，查找第一个重复次数为3的大于上家的重复次数为3的牌
            //        for (int i = 0; i < repeats_YourCards[2].Count; i++)
            //        {
            //            if (repeats_YourCards[2][i].Value > repeats_SourceCards[2][0].Value)
            //            {
            //                for (int j = 0; j < yourCards.Count; j++)
            //                {
            //                    if (yourCards[j].Value == repeats_YourCards[2][i].Value)
            //                    {
            //                        Result.Add(yourCards[j]);//找到不同花色的重复的牌
            //                    }
            //                }
            //                yourCards.RemoveAll((c) =>
            //                {
            //                    return c.Value == repeats_YourCards[2][i].Value;
            //                });//删除所有要出的牌
            //                break;//跳出
            //            }
            //        }
            //    }
            //    break;

            case CardsTypeMdFan.Single:
                {

                }
                break;
                #region 地主牌型
                //case CardsType.Straight:
                //    {
                //        //单顺子
                //        bool flag = true;
                //        for (int i = CurrentDiscards.Count - 1; i >= 0; i--)
                //        {
                //            Card card = null;
                //            if ((card = yourCards.Find((c) =>
                //            {
                //                return c.Value == CurrentDiscards[i].Value + 1;
                //            })) != null)//必须要有比出的顺子的每一张牌都大1的牌
                //            {
                //                Result.Add(card);//结果添加
                //                yourCards.Remove(card);//移除
                //            }
                //            else
                //            {
                //                flag = false;//没有找到就退出
                //                break;
                //            }
                //        }
                //        if (!flag)
                //        {
                //            Result.Clear();//清除结果
                //        }
                //    }
                //    break;
                //case CardsType.DoubleStraight:
                //    {
                //        //双顺子
                //        bool flag = true;
                //        for (int i = repeats_SourceCards[1].Count - 1; i >= 0; i--)
                //        {
                //            Card card = null;
                //            if ((card = repeats_YourCards[1].Find((c) =>
                //            {
                //                return c.Value == repeats_SourceCards[1][i].Value + 1;
                //            })) != null)//必须要有比出的顺子的每一张牌都大1的牌
                //            {
                //                List<Card> range = yourCards.FindAll(
                //                    (c) => { return c.Value == card.Value; });//找到所有重复的牌
                //                if (range.Count > 2)
                //                {
                //                    range.RemoveRange(2, range.Count - 1);//只留两张牌
                //                }
                //                Result.AddRange(range);//结果添加
                //                //移除
                //                for (int k = 0; k < range.Count; k++)
                //                {
                //                    yourCards.Remove(range[k]);
                //                }
                //            }
                //            else
                //            {
                //                flag = false;//没有找到就退出
                //                break;
                //            }
                //        }
                //        if (!flag)
                //        {
                //            Result.Clear();//清除结果
                //        }
                //    }
                //    break;
                //case CardsType.TripleStraight:
                //    {
                //        //三顺子
                //        bool flag = true;
                //        for (int i = repeats_SourceCards[2].Count - 1; i >= 0; i--)
                //        {
                //            Card card = null;
                //            if ((card = repeats_YourCards[2].Find((c) =>
                //            {
                //                return c.Value == repeats_SourceCards[2][i].Value + 1;
                //            })) != null)//必须要有比出的顺子的每一张牌都大1的牌
                //            {
                //                List<Card> range = yourCards.FindAll(
                //                    (c) => { return c.Value == card.Value; });//找到所有重复的牌
                //                if (range.Count > 3)
                //                {
                //                    range.RemoveRange(3, range.Count - 1);//只留三张牌
                //                }
                //                Result.AddRange(range);//结果添加
                //                //移除
                //                for (int k = 0; k < range.Count; k++)
                //                {
                //                    yourCards.Remove(range[k]);
                //                }
                //            }
                //            else
                //            {
                //                flag = false;//没有找到就退出
                //                break;
                //            }
                //        }
                //        if (!flag)
                //        {
                //            Result.Clear();//清除结果
                //        }
                //    }
                //    break;
                //case CardsType.TriplePlusOne:
                //    {
                //        //三带一，只比较第三张牌
                //        int value = -1;
                //        for (int i = 0; i < repeats_YourCards[2].Count; i++)
                //        {
                //            if (repeats_YourCards[2][i].Value > repeats_SourceCards[2][0].Value)
                //            {
                //                value = repeats_YourCards[2][i].Value;
                //                break;
                //            }
                //        }
                //        if (value != -1)
                //        {
                //            List<Card> range = yourCards.FindAll((c) => { return c.Value == value; });
                //            if (range.Count > 3)
                //            {
                //                range.RemoveAt(3);//只要三个
                //            }
                //            Result.AddRange(range);//添加进结果
                //            //移除
                //            for (int k = 0; k < range.Count; k++)
                //            {
                //                yourCards.Remove(range[k]);
                //            }
                //            Result.Add(yourCards[0]);//带一张
                //            yourCards.RemoveAt(0);//移除
                //        }
                //    }
                //    break;
                //case CardsType.TriplePlusDouble:
                //    {
                //        //三带一对
                //        int value = -1;
                //        if (repeats_YourCards[1].Count <= 0) break;//没有对子
                //        for (int i = 0; i < repeats_YourCards[2].Count; i++)
                //        {
                //            if (repeats_YourCards[2][i].Value > repeats_SourceCards[2][0].Value)
                //            {
                //                value = repeats_YourCards[2][i].Value;
                //                break;
                //            }
                //        }
                //        if (value != -1)//有比上家大的3连
                //        {
                //            List<Card> range = yourCards.FindAll((c) => { return c.Value == value; });
                //            if (range.Count > 3)
                //            {
                //                range.RemoveAt(3);//只要三个
                //            }
                //            Result.AddRange(range);//添加进结果
                //            //移除
                //            for (int k = 0; k < range.Count; k++)
                //            {
                //                yourCards.Remove(range[k]);
                //            }
                //            range = yourCards.FindAll((c) =>
                //            { return c.Value == repeats_YourCards[1][0].Value; });//找到第一个对子
                //            Result.AddRange(range);//添加进结果
                //            //移除
                //            for (int k = 0; k < range.Count; k++)
                //            {
                //                yourCards.Remove(range[k]);
                //            }
                //        }
                //    }
                //    break;
                //case CardsType.QuartePlusTwo:
                //    {
                //        //四带2单
                //        int value = -1;
                //        for (int i = 0; i < repeats_YourCards[3].Count; i++)
                //        {
                //            if (repeats_YourCards[3][i].Value > repeats_SourceCards[3][0].Value)
                //            {
                //                value = repeats_YourCards[3][i].Value;
                //                break;
                //            }
                //        }
                //        if (value != -1)//有比上家大的4连
                //        {
                //            List<Card> range = yourCards.FindAll((c) => { return c.Value == value; });
                //            Result.AddRange(range);//添加进结果
                //            //移除
                //            for (int k = 0; k < range.Count; k++)
                //            {
                //                yourCards.Remove(range[k]);
                //            }
                //            //加两个单牌
                //            Result.Add(yourCards[0]);
                //            yourCards.RemoveAt(0);
                //            Result.Add(yourCards[0]);
                //            yourCards.RemoveAt(0);
                //        }
                //    }
                //    break;
                //case CardsType.QuartePlusDouble:
                //    {
                //        //四带2对
                //        int value = -1;
                //        for (int i = 0; i < repeats_YourCards[3].Count; i++)
                //        {
                //            if (repeats_YourCards[3][i].Value > repeats_SourceCards[3][0].Value)
                //            {
                //                value = repeats_YourCards[3][i].Value;
                //                break;
                //            }
                //        }
                //        if (value != -1)//有比上家大的4连
                //        {
                //            List<Card> range = yourCards.FindAll((c) => { return c.Value == value; });
                //            Result.AddRange(range);//添加进结果
                //            //移除
                //            for (int k = 0; k < range.Count; k++)
                //            {
                //                yourCards.Remove(range[k]);
                //            }
                //            //加两个对牌
                //            for (int i = 0; i < repeats_YourCards[1].Count; i++)
                //            {
                //                range = yourCards.FindAll((c) =>
                //                {
                //                    return c.Value == repeats_YourCards[1][i].Value;
                //                });
                //                Result.AddRange(range);
                //                //移除
                //                for (int j = 0; j < range.Count; j++)
                //                {
                //                    yourCards.Remove(range[j]);
                //                }
                //            }
                //        }
                //    }
                //    break;
                //case CardsType.PlanePlusWings:
                //    {
                //        //飞机带翅膀，比较重复次数为3的最大的一张牌
                //        //三顺子
                //        bool flag = true;
                //        for (int i = repeats_SourceCards[2].Count - 1; i >= 0; i--)
                //        {
                //            Card card = null;
                //            if ((card = repeats_YourCards[2].Find((c) =>
                //            {
                //                return c.Value == repeats_SourceCards[2][i].Value + 1;
                //            })) != null)//必须要有比出的顺子的每一张牌都大1的牌
                //            {
                //                List<Card> range = yourCards.FindAll(
                //                    (c) => { return c.Value == card.Value; });//找到所有重复的牌
                //                if (range.Count > 2)
                //                {
                //                    range.RemoveRange(2, range.Count - 1);//只留两张牌
                //                }
                //                Result.AddRange(range);//结果添加
                //                //移除
                //                for (int k = 0; k < range.Count; k++)
                //                {
                //                    yourCards.Remove(range[k]);
                //                }
                //            }
                //            else
                //            {
                //                flag = false;//没有找到就退出
                //                break;
                //            }
                //        }
                //        //
                //        if (!flag)
                //        {
                //            Result.Clear();//清除结果
                //        }
                //        else
                //        {
                //            //带的是单牌
                //            if (repeats_SourceCards[2].Count == repeats_SourceCards[0].Count)
                //            {
                //                for (int i = 0; i < repeats_SourceCards[2].Count; i++)
                //                {
                //                    Result.Add(yourCards[0]);
                //                    yourCards.RemoveAt(0);
                //                }
                //            }
                //            //带的是对牌
                //            if (repeats_SourceCards[2].Count == repeats_SourceCards[1].Count)
                //            {
                //                for (int i = 0; i < repeats_YourCards[1].Count; i++)
                //                {
                //                    List<Card> range = yourCards.FindAll(
                //                        (c) => { return c.Value == repeats_YourCards[1][i].Value; });//找到对牌
                //                    Result.AddRange(range);
                //                    //移除
                //                    for (int k = 0; k < range.Count; k++)
                //                    {
                //                        yourCards.Remove(range[k]);
                //                    }
                //                }

                //            }
                //        }
                //    }
                //    break;
                #endregion
                #endregion
        }

        int imax = CurrentCards4Top();
        imax = imax >= 0 ? imax : 0;
        if (iRole == DisRole.FirstCard)
        {
            bool bLord = name == Landlord ? true : false,
                 bFlower = Flower3 > 2 ? true : false;
            selCard = FirstCard.GetFirstCard(cards, AllDiscards, bLord, FourDoor, bFlower);


            Result.Add(selCard);

            if (!FourDoor.Exists(suit => suit == selCard.Suit) && FourDoor.Count < 2)
                Full4Door(selCard.Suit, name);
            //移除
            yourCards.Remove(selCard);

            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌


            return Result;
        }
        else if (iRole == DisRole.MieCard) //灭牌
        {
            if (CurrentDiscards4.Count > 0 &&
                CurrentDiscards4[0].Suit == PokerConst.Spade)
                selCard = MieCard.GetMieCard2(yourCards, FourDoor, true);
            else
                selCard = MieCard.GetMieCard2(yourCards, FourDoor, false);
            Result.Add(selCard);
            yourCards.Remove(selCard);

            if (Result.Count <= 0)
            {
                Result.Add(yourCards[0]);//添加到待出牌                       
                yourCards.RemoveAt(0);//移除出的牌
            }
            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌
            return Result;
        }
        else if (CurrentDiscards4.Count > 0 &&
                    (iRole == DisRole.CatchCard ||
                    iRole == DisRole.MidCatchCard ||
                    iRole == DisRole.BiCard||
                    iRole == DisRole.FreeCard))
        { //单张，查找第一个大于上家的牌


            if (iRole == DisRole.BiCard &&
                 CurrentDiscards4[imax].Value < EngineTool.GetShangValve(CurrentDiscards4[imax].Suit) - 3)  //头逼 遇到小张)
            {
                selCard = EngineTool.GetBiCard(CurrentDiscards4[imax], yourCards);                             
            }

            if (iRole == DisRole.CatchCard ||
                  iRole == DisRole.MidCatchCard)
            {
                selCard = EngineTool.GetCatchCard(CurrentDiscards4[imax], yourCards);                          
            }

            if (iRole == DisRole.FreeCard &&
                 CurrentDiscards4[imax].Value < EngineTool.GetShangValve(CurrentDiscards4[imax].Suit) - 3)
            {
                selCard = EngineTool.GetCatchCard(CurrentDiscards4[imax], yourCards);
            }


            if (selCard != null && iRole != DisRole.CatchCard &&
                 CurrentDiscards4[imax].Value < EngineTool.GetShangValve(CurrentDiscards4[imax].Suit) - 3 &&   //非底家 只有赏 遇非大张
                 selCard.Value == EngineTool.GetShangValve(selCard.Suit))  //出牌为赏
            {
                if (yourCards.Count(c => !EngineTool.isJinZhang(c) &&
                                       c.Suit != CurrentDiscards4[imax].Suit) > 0) //还有青张
                {
                    if (CurrentDiscards4.Count > 0 &&
                        CurrentDiscards4[0].Suit == PokerConst.Spade)
                        selCard = MieCard.GetMieCard2(yourCards, FourDoor, true);
                    else
                        selCard = MieCard.GetMieCard2(yourCards, FourDoor, false);
                                   
                }

            }
           

            if (selCard == null || selCard.Value <= 0)
            {
                if (CurrentDiscards4.Count > 0 &&
                        CurrentDiscards4[0].Suit == PokerConst.Spade)
                    selCard = MieCard.GetMieCard2(yourCards, FourDoor, true);
                else
                    selCard = MieCard.GetMieCard2(yourCards, FourDoor, false);
            }
            Result.Add(selCard);
            yourCards.Remove(selCard);

            if (Result.Count <= 0)
            {
                Result.Add(yourCards[0]);//添加到待出牌                       
                yourCards.RemoveAt(0);//移除出的牌
            }
            
            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌
            return Result;
        }
        else if (iRole == DisRole.DingCard)
        {
            for (int i = yourCards.Count - 1; i >= 0; i--)
            {
                if (CurrentDiscards4.Count < 1 || yourCards[i].GreatThan(CurrentDiscards4[imax]))
                {
                    Result.Add(yourCards[i]);//添加到待出牌                         
                    yourCards.RemoveAt(i);//移除出的牌
                    break;//跳出
                }
            }


            if (Result.Count <= 0)
            {
                if (CurrentDiscards4.Count > 0 &&
           CurrentDiscards4[0].Suit == PokerConst.Spade)
                    selCard = MieCard.GetMieCard2(yourCards, FourDoor, true);
                else
                    selCard = MieCard.GetMieCard2(yourCards, FourDoor, false);
                Result.Add(selCard);
                yourCards.Remove(selCard);


            }
            if (Result.Count <= 0)
            {
                Result.Add(yourCards[0]);//添加到待出牌                       
                yourCards.RemoveAt(0);//移除出的牌
            }
            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌
            return Result;
        }
        else if (Result.Count <= 0)
        {
            Result.Add(yourCards[0]);//添加到待出牌                       
            yourCards.RemoveAt(0);//移除出的牌
        }
        if (Result.Count == CurrentDiscards.Count)
        {
            cards.Clear();
            cards.AddRange(yourCards);//替换当前手牌
        }
        #region 地主牌型
        //else
        //{
        //    Result.Clear();
        //    for (int j = 0; j < yourCards.Count; j++)
        //    {
        //        if (yourCards[j].Value == (int)MdValue.BlackJoker ||
        //            yourCards[j].Value == (int)MdValue.RedJoker)
        //        {
        //            Result.Add(yourCards[j]);
        //        }
        //    }
        //    #region 地主牌型
        //    //if (Result.Count == 2)//大小王都在，王炸
        //    //{
        //    //    yourCards.RemoveAll((c) =>
        //    //    {
        //    //        return (c.Value == (int)MdValue.BlackJoker ||
        //    //        c.Value == (int)MdValue.RedJoker);
        //    //    });//删除出牌
        //    //    cards.Clear();
        //    //    cards.AddRange(yourCards);//替换当前手牌
        //    //}
        //    //else
        //    //{
        //    //    Result.Clear();//清除单王
        //    //}
        //    ////有炸弹,并且上家不是炸弹
        //    //if (repeats_YourCards[3].Count > 1 && sourceCardType != CardsType.Bomb)
        //    //{
        //    //    List<Card> range = yourCards.FindAll(
        //    //    (c) => { return c.Value == repeats_YourCards[3][0].Value; });//找到炸弹
        //    //    Result.AddRange(range);
        //    //    //移除
        //    //    for (int k = 0; k < range.Count; k++)
        //    //    {
        //    //        yourCards.Remove(range[k]);
        //    //    }
        //    //    cards.Clear();
        //    //    cards.AddRange(yourCards);//替换当前手牌
        //    //}
        //   
        //}
        #endregion

        return Result;//返回出的牌
    }
    /// <summary>
    /// 随机生成一副马吊牌
    /// </summary>
    /// <param ></param>
    /// <returns>List<Card></returns>
    public List<Card> GenerateCards()
    {
        List<Card> sourceCards = new List<Card>();
        System.Random rand = new System.Random();
        string[] suit = {
            PokerConst.Club,//梅花
            PokerConst.Diamond,//方块
            PokerConst.Heart,//红桃
            PokerConst.Spade};//黑桃
        int count = 11;
        for (int i = 0; i < 4; i++)
        {
            if (i == 1 || i == 2) count = 9;//方块 黑桃11张 梅花 红桃 9张
            else count = 11;
            for (int j = 1; j <= count; j++)
            {
                sourceCards.Add(new Card(suit[i], j));//每种花色生成13张
            }
        }
        //sourceCards.Add(new Card("", 14));//小王
        //sourceCards.Add(new Card("", 15));//大王
        List<Card> randCards = new List<Card>();
        int index = 0;
        for (int i = 0; i < 40; i++)
        {
            index = rand.Next(0, sourceCards.Count);
            randCards.Add(sourceCards[index]);//随机添加
            sourceCards.RemoveAt(index);//加完以后删除防止重复
        }
        return randCards;
    }

    /// <summary>
    /// 是否灭牌
    /// </summary>
    /// SortCards
    public int CurrentCards4Top()
    {
        if (CurrentDiscards4.Count < 1)
            return -1;
        Card maxC = CurrentDiscards4[0];
        int pos = 0;
        for (int i = 1; i < CurrentDiscards4.Count; i++)
        {
            if (CurrentDiscards4[i].GreatThan(maxC))
            {
                maxC = CurrentDiscards4[i];
                pos = i;
            }
        }
        return pos;

    }

    public List<PlayerInfo> ResortPlayers(int i)
    {
        List<PlayerInfo> newPlayers = new List<PlayerInfo>();
        for (int j = 0; j < 4; j++)
        {
            int k = (i + j + 1) % 4;
            newPlayers.Add(Players[k]);
        }
        return newPlayers;
    }

    public bool IsZhuoji(int Turn)
    {
        int imax = CurrentCards4Top();
        if (CurrentDiscards4.Count == 4
            && CurrentDiscards4[0].Value == 1 &&
            CurrentDiscards4[imax].Value == 2)
        {
            Turn -= 3 - imax;
            switch (Turn)
            {
                case 0:
                    PlayerSelf.Zhuoji = PlayerSelf.winCards.Count;
                    break;
                case 1:
                    Player2.Zhuoji = Player2.winCards.Count;
                    break;
                case 2:
                    Player3.Zhuoji = Player3.winCards.Count;
                    break;
                case 3:
                    Player4.Zhuoji = Player4.winCards.Count;
                    break;
            }
            return true;
        }
        return false;
    }
    public List<Card> SortFightCards(List<Card> cards)
    {
        List<Card> csnew = new List<Card>();
        foreach (Card cs in cards)
        {
            if (csnew.Count == 0)
                csnew.Add(cs);
            else
            {
                bool inserted = false;
                for (int i = 0; i < csnew.Count; ++i)
                {
                    if (cs.Suit.Equals(cards[0].Suit) &&
                            cs.GreatThan(cards[i]))
                    {
                        csnew.Insert(i, cs);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                    csnew.Add(cs);
            }
        }
        return csnew;
    }

    public bool IsFightOver()
    {
        int count = PlayerSelf.winCards.Count + Player2.winCards.Count + Player3.winCards.Count + Player4.winCards.Count;
        if (count >= 8)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 和上家出的牌进行比较
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public bool CardsCompareToDicards(Card[] cards)
    {

        string discardType = GetCardsType(CurrentDiscards.ToArray());//上家牌型
        string cardType = GetCardsType(cards);  //
        #region 地主牌型
        //if (cardType.Equals(CardsType.Mussy))
        //{
        //    return false;//杂牌不能出
        //}
        //List<List<Card>> repeats_Cards = null;
        //List<List<Card>> repeats_Discards = null;
        //if (discardType.Equals(CardsType.Rocket)) return false;//上家是王炸
        //if (cardType.Equals(CardsType.Rocket))
        //{
        //    return true;//自己是王炸
        //}
        //if (cardType.Equals(CardsType.Bomb) && !discardType.Equals(CardsType.Bomb))
        //{
        //    return true;//自己是炸弹，上家不是
        //}
        //if (!cardType.Equals(CardsType.Bomb) && discardType.Equals(CardsType.Bomb))
        //{
        //    return false;//上家是炸弹，自己不是
        //}
        #endregion
        if (cardType.Equals(discardType))//牌型相等才能比较
        {
            switch (cardType)
            {
                //case CardsType.Bomb:
                //case CardsType.Double:
                //case CardsType.Triple:
                case CardsTypeMdFan.Single:
                    {
                        //单张或重复，只比较第一张
                        return cards[0].Suit.Equals(CurrentDiscards[0].Suit) ? cards[0].Value > CurrentDiscards[0].Value : false;
                    }
                    #region 地主牌型
                    //case CardsType.Straight:
                    //case CardsType.DoubleStraight:
                    //case CardsType.TripleStraight:
                    //    {
                    //        //顺子，比较最后一张
                    //        return cards[cards.Length - 1].Value >
                    //            CurrentDiscards[CurrentDiscards.Count - 1].Value;
                    //    }
                    //case CardsType.TriplePlusOne:
                    //case CardsType.TriplePlusDouble:
                    //    {
                    //        //三带一，只比较第三张牌
                    //        return cards[2].Value >
                    //            CurrentDiscards[2].Value;
                    //    }
                    //case CardsType.QuartePlusTwo:
                    //    {
                    //        //四带二单张，比较第3张牌
                    //        return cards[2].Value >
                    //            CurrentDiscards[2].Value;
                    //    }
                    //case CardsType.QuartePlusDouble:
                    //    {
                    //        //四带二对，比较重复次数为4的牌
                    //        repeats_Cards = CountRepeats(cards);
                    //        repeats_Discards = CountRepeats(cards);
                    //        return repeats_Cards[3][0].Value >
                    //            repeats_Discards[3][0].Value;
                    //    }
                    //case CardsType.PlanePlusWings:
                    //    {
                    //        //飞机带翅膀，比较重复次数为3的最大的一张牌
                    //        if (repeats_Cards == null)
                    //        {
                    //            repeats_Cards = CountRepeats(cards);
                    //            repeats_Discards = CountRepeats(cards);
                    //        }
                    //        return repeats_Cards[2][repeats_Cards[2].Count - 1].Value >
                    //            repeats_Discards[2][repeats_Cards[2].Count - 1].Value;
                    //    }
                    #endregion

            }
        }
        return false;//牌型和上家不同，无法出牌
    }


    /// <summary>
    /// 获取牌型，马吊只用单张
    /// </summary>
    public string GetCardsType(Card[] cards)
    {
        List<Card> lst = new List<Card>(cards);
        lst.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });  //升序排序
        if (lst.Count <= 4)
        {
            if (lst.Count == 1)
            {
                return CardsTypeMdFan.Single;
            }
            #region 地主牌型
            //if (lst.Count == 2)
            //{
            //    if (lst[0].Value == lst[1].Value)
            //    {
            //        return CardsType.Double;//对子
            //    }
            //    if (lst[0].Value == (int)MdValue.BlackJoker || lst[0].Value == (int)MdValue.RedJoker &&
            //       lst[1].Value == (int)MdValue.BlackJoker || lst[1].Value == (int)MdValue.RedJoker)
            //    {
            //        return CardsType.Rocket;
            //    }
            //}
            //if (lst.Count == 3)
            //{
            //    if (lst[0].Value == lst[2].Value)
            //    {
            //        return CardsType.Triple;
            //    }
            //}
            //if (lst.Count == 4)
            //{
            //    //四张都相等
            //    if (lst[0].Value == lst[3].Value)
            //    {
            //        return CardsType.Bomb;
            //    }
            //    //1到3或2到4相等
            //    if (lst[0].Value == lst[2].Value || lst[1].Value == lst[3].Value)
            //    {
            //        return CardsType.TriplePlusOne;
            //    }
            //}
            //}
            //else
            //{
            //    List<List<Card>> repeats = new List<List<Card>>() {
            //        new List<Card>(),new List<Card>(),new List<Card>(),new List<Card>()};
            //    bool hasTwoOrBJokerOrRJoker = false;
            //    for (int i = 0; i < lst.Count; i++)
            //    {
            //        int count = 0;
            //        if (lst[i].Value == (int)MdValue.Two || lst[i].Value == (int)MdValue.BlackJoker ||
            //            lst[i].Value == (int)MdValue.RedJoker)
            //        {
            //            hasTwoOrBJokerOrRJoker = true;//有大王或小王或2
            //        }
            //        for (int j = 0; j < lst.Count; j++)
            //        {
            //            if (lst[j].Value == lst[i].Value)
            //            {
            //                count++;//重复次数+1
            //            }
            //        }
            //        //重复数据的值只记录一次，如333中的3只记录一次
            //        if (!repeats[count - 1].Exists((c) => { return c.Value == lst[i].Value; }))
            //            repeats[count - 1].Add(lst[i]);//根据重复次数分别记录（1,2,3,4）
            //    }
            //    //5张牌，重复3张一次，重复两张一次，如 444+99
            //    if (lst.Count == 5 && repeats[2].Count == 1 && repeats[1].Count == 1)
            //    {
            //        return CardsType.TriplePlusDouble;
            //    }
            //    //6张牌，重复4张一次，如4444+3+8 或 4444+33)
            //    if (lst.Count == 6 && repeats[3].Count == 1)
            //    {
            //        return CardsType.QuartePlusTwo;
            //    }
            //    //8张牌，重复4张一次，如4444＋55＋77)
            //    if (lst.Count == 8 && repeats[3].Count == 1 && repeats[1].Count == 2)
            //    {
            //        return CardsType.QuartePlusDouble;
            //    }
            //    //三顺＋同数量的对牌，如：333444555+7799JJ
            //    if (repeats[2].Count >= 2 && repeats[2].Count == lst.Count / 5 &&
            //        repeats[2][repeats[2].Count - 1].Value - repeats[2][0].Value ==
            //        lst.Count / 5 - 1)
            //    {
            //        return CardsType.PlanePlusWings;
            //    }
            //    //三顺＋同数量的单牌，如：444555+79
            //    if (repeats[2].Count >= 2 && repeats[2].Count == lst.Count / 4 &&
            //        repeats[2][repeats[2].Count - 1].Value - repeats[2][0].Value ==
            //        lst.Count / 4 - 1)
            //    {
            //        return CardsType.PlanePlusWings;
            //    }

            //    if (!hasTwoOrBJokerOrRJoker)//没有大小王2
            //    {
            //        //任意五张或五张以上点数相连的牌，如：45678或78910JQK。不包括 2和双王
            //        if (lst.Count == repeats[0].Count && (lst[lst.Count - 1].Value - lst[0].Value) == lst.Count - 1)
            //        {
            //            return CardsType.Straight;
            //        }
            //        //三对或更多的连续对牌，如：334455、7788991010JJ。不包括 2 点和双王
            //        if (lst.Count / 2 >= 3 && lst.Count % 2 == 0 && repeats[1].Count == lst.Count / 2 &&
            //            (lst[lst.Count - 1].Value - lst[0].Value) == lst.Count / 2 - 1)
            //        {
            //            return CardsType.DoubleStraight;
            //        }
            //        //二个或更多的连续三张牌，如：333444 、555666777888。不包括 2 点和双王:
            //        if (lst.Count / 3 == repeats[2].Count && lst.Count % 3 == 0 &&
            //            (lst[lst.Count - 1].Value - lst[0].Value) == lst.Count / 3 - 1)
            //        {
            //            if (repeats[2].Count == repeats[0].Count ||
            //                repeats[2].Count == repeats[1].Count)
            //            {
            //                return CardsType.PlanePlusWings;//三顺+同样数量的单牌或对子
            //            }
            //            return CardsType.TripleStraight;
            //        }
            //    }
            #endregion
        }
        return CardsTypeMdFan.Mussy;
    }

    public string TipSeyang()
    {
        //可组合色样提示
        string stip = "";
        List<CardType> lt = new List<CardType>();
        if (RestCardNum == 8)
        {
            lt = GameEngine.GetSeyangTip(HandCards, PlayerSelf.winCards);
        }
        else if (RestCardNum <= 0)
            return "";
        else
        {
            List<Card> ltipCards = new List<Card>(PlayerSelf.winCards);
            //if (SelectedCards.Count > 0)
            //    ltipCards.AddRange(SelectedCards);
            //else
            ltipCards.AddRange(HandCards);

            if (ltipCards != null)
            {
                lt = GameEngine.GetSeyangTip(ltipCards, PlayerSelf.winCards);
                if (RestCardNum < 8 && !CardSeyang.Ji.Keys.ToString().Contains(ltipCards[0].SValue()))
                    lt.RemoveAll(it => it.Name == CardsTypeMdFan.NvDiDengJi
                                    || it.Name == CardsTypeMdFan.FeiLaiQu
                                    || it.Name == CardsTypeMdFan.TianRanQu);

                lt.RemoveAll(it => it.Name == CardsTypeMdFan.BaQuan
                                || it.Name == CardsTypeMdFan.QiDiao
                                || it.Name == CardsTypeMdFan.LiuDiao
                                || it.Name == CardsTypeMdFan.QingBaiRenJia);
            }
        }

        foreach (var ct in lt)//计入各色样名称 牌组 与开注
        {
            stip += GameEngine.ConvertMdName(ct.CardKey) + "  " + ct.Name + "  " + ct.Jifen.ToString() + StringFanConst.Zhu + "\r\n";
        }
        return stip;
    }


    public FirstStatus CheckFirstCard(Card card, List<Card> lc, List<Card> lcwin = null, bool isBaijia = false)
    {

        List<Card> lnew = new List<Card>(lcwin);

        lnew.AddRange(lc);
        bool bHaveShangBai = false,//有賞有百  才可以告百
            bLordFirst = false,      //莊家發過牌
            bTwoDoorOPen = false,   //兩門已正
            bCloseShang = false,           //關賞鬥十
            bJinshouShi = false,    //盡手十子
            bWanQian = false,          //闖千雙持
            bLiuWei = false,
            bCloseZhen = false;              //關真鬥十

        // bTiWanQian=false;           //提盡萬千
        Card faceCard = BaseCards[BaseCards.Count - 1];
        List<Card> handXDisall = new List<Card>(HandCards);
        handXDisall.AddRange(AllDiscards);

        int suits = EngineTool.GetSuitsCount(lc); //在手花色數量
        if (lnew.Exists(c => c.Value == EngineTool.GetShangValve(c.Suit, true)) &&
            isBaijia)
            bHaveShangBai = true;

        int iLord = Players.FindIndex(p => p.Name == Landlord);
        int iSelf = Players.FindIndex(p => p.Name == PlayerSelf.Name);
        int iFirst = BaseCards[BaseCards.Count - 1].Value % 4;
        if (Players[iLord].winCards.Count > 0 || (iLord == iFirst && iLord != iSelf))
            bLordFirst = true;

        if (FourDoor.Count >= 2 &&
            AllDiscards.Exists(c => c.Suit == FourDoor[0]) &&
            AllDiscards.Exists(c => c.Suit == FourDoor[1]))
            bTwoDoorOPen = true;



        bool bSpadeandOther1 = false;
        bool bSpadeandZheng1 = false;
        //  只有两门  十子和另一门
        if (suits == 2 &&
             lc.Exists(c => c.Suit == PokerConst.Spade) &&
            lc.Exists(c => c.Suit != PokerConst.Spade && c.Value == EngineTool.GetShangValve(c.Suit)))
            bSpadeandOther1 = true;

        bool bLord = false, bFlowerFallen = false;
        if (PlayerSelf.Name == Landlord) bLord = true;
        if (Flower3 >= 3) bFlowerFallen = true;
        List<List<Card>> llcsort = new List<List<Card>>();
        if (FourDoor.Count < 2)
            llcsort.AddRange(EngineTool.Sort1230(lc, bLord));
        else
            llcsort.AddRange(EngineTool.Sort1230(lc, bLord, FourDoor[0], FourDoor[1], bFlowerFallen));
        int counts1Card = 0, //只存在一张牌的非十花色数量
            countsAnyCard;//存在2张以上牌的非十花色数量
        foreach (var lsuit in llcsort)
        {
            if (lsuit.Count == 1 &&
                !lsuit.Exists(c => c.Suit == PokerConst.Spade))//&&
                                                               //c.Value == PokerConst.JiValve) )
                counts1Card++;
        }
        int havespade = lc.Exists(c => c.Suit == PokerConst.Spade) ? 1 : 0;
        countsAnyCard = llcsort.Count - havespade - counts1Card;
        if (countsAnyCard == 1 &&
             lc.Exists(c => c.Suit != PokerConst.Spade && c.Value == EngineTool.GetShangValve(c.Suit)))
            bSpadeandOther1 = true;

        //最多只剩一個正門
        if (suits >= 2 && FourDoor.Count > 1 &&
            (lc.Count(c => c.Suit == FourDoor[0]) == 0 ||
             lc.Count(c => c.Suit == FourDoor[1]) == 0) &&
            lc.Count(c => c.Suit == PokerConst.Spade && c.Value != PokerConst.JianValve) > 0)
            bSpadeandZheng1 = true;


        if (lc.Exists(cx => cx.Suit != PokerConst.Spade && cx.Value == EngineTool.GetShangValve(cx.Suit)))

            if (bSpadeandOther1 || bSpadeandZheng1 || countsAnyCard == 0)
                bCloseShang = true;

        if (lc.Count(c => c.Suit == PokerConst.Spade) == lc.Count - 1 &&
            lc.Exists(c => c.Suit != PokerConst.Spade))//&&
                                                       // EngineTool.isTrueCard(AllDiscards, c, faceCard)))
            bCloseZhen = true;  //關真斗十+留張製尾


        if (suits == 1 && lc.Exists(c => c.Suit == PokerConst.Spade))
            bJinshouShi = true;

        if (lc.Exists(c => c.Suit == PokerConst.Spade) &&
            lc.Count(c => c.Value >= PokerConst.ShangValve - 1) == 2)
            bWanQian = true;

        if (FourDoor.Count == 4 &&
            lc.Count(c => c.Suit == FourDoor[0]) <= 1 &&
            lc.Count(c => c.Suit == FourDoor[1]) <= 1)
            bLiuWei = true;

        //原門不可出盡
        if (!bLord && EngineTool.isLastSuit1(card, HandCards, PlayerSelf.Cards, FirstDiscards, AllDiscards, FourDoor))
            return FirstStatus.LastOneSuit;

        if (FourDoor.Count == 4 && card.Suit == FourDoor[2])
        {
            if (bCloseShang || bCloseZhen || bJinshouShi || bLiuWei ||
                Landlord == PlayerSelf.Name ||
                 EngineTool.isCloseShang(AllDiscards, lc, card.Suit, bLord))
                return FirstStatus.FirstOK;
            else if (EngineTool.isTrueCard(AllDiscards, card,
                                           faceCard,
                                           HandCards))
                return FirstStatus.FirstOK;
            else
                return FirstStatus.KaiSan;


        }
        else if (card.Suit == PokerConst.Spade)  //出十
        {
            if (card.Value < PokerConst.ShangValve - 2)  //十子青張或二十
            {
                //不管是否三花落盡 有沒有百老            

                //關賞/關真/盡手 留尾  鬭十
                if (bCloseZhen || bJinshouShi ||
                    EngineTool.isCloseShang(AllDiscards, lc, card.Suit, bLord))
                    return FirstStatus.FirstOK;

                if (Flower3 < 3)//三花未落
                {

                    if (!isBaijia)//非百老之家 && 
                        //!PlayerSelf.Cards.Exists(c=>c.Value==PokerConst.ShangValve-2&&
                        //                          c.Suit == PokerConst.Spade) )//非百老之家

                        return FirstStatus.Flower3Not;
                    else //百老之家
                    {
                        //無賞告百
                        if (!bHaveShangBai)
                            return FirstStatus.WuShangGaoBai;
                        //莊家已發
                        if (bLordFirst && PlayerSelf.Name != Landlord)
                            return FirstStatus.LordFirst;
                        //兩門已正
                        if (bTwoDoorOPen)
                            return FirstStatus.TwoDoorOpen;

                        return FirstStatus.FirstOK;
                    }
                }
                else   //三花落盡 隨意發十
                    return FirstStatus.FirstOK;
            }
            else if (card.Value == PokerConst.JianValve) //千
            {
                //最後一張
                if (lc.Count <= 1)
                    return FirstStatus.FirstOK;
                //有百可以按告百規則出千
                if (bHaveShangBai && !(bTwoDoorOPen || bLordFirst || PlayerSelf.isBai))
                    return FirstStatus.FirstOK;
                if (PlayerSelf.isBai)
                    return FirstStatus.FirstOK;
                //最後2張 手上無十 場上無萬 可以提千
                if (lc.Count == 2)
                {
                    if (lc.Count(c => c.Suit == PokerConst.Spade) < 2 &&
                       EngineTool.isTrueCard(AllDiscards, card, faceCard))
                        return FirstStatus.FirstOK;
                }

                //急捉不能提千
                if (lc.Count > 1 && PlayerSelf.isJiGua &&
                     PlayerSelf.Name != Landlord &&
                    lc.Count(c => c.Value >= PokerConst.JianValve) < 2)
                    return FirstStatus.JiZhuoTiWan;

                //提盡萬千
                if (lcwin.Exists(c => c.Value == PokerConst.ShangValve &&
                                         c.Suit == PokerConst.Spade) &&
                    card.Value == PokerConst.JianValve &&
                    lc.Count > 1)
                    return FirstStatus.TiJinWanQian;

                //紅萬未出 先提千僧
                if (lc.Exists(c => c.Value == PokerConst.ShangValve &&
                                       c.Suit == PokerConst.Spade))
                    return FirstStatus.DaoTiQianWan;


                return FirstStatus.ChuangQian;


            }
            else if (card.Value == PokerConst.ShangValve)//万
            {
                if (PlayerSelf.isJiGua && lc.Count > 1 &&
                     PlayerSelf.Name != Landlord &&
                    lc.Count(c => c.Value >= PokerConst.JianValve) < 2)
                    return FirstStatus.JiZhuoTiWan;
                if (lc.Count == 4 && card.Value == PokerConst.ShangValve &&
                    handXDisall.Count(c => c.Value >= PokerConst.ShangValve - 2 && //千百已現 加紅萬3
                                         c.Suit == PokerConst.Spade) < 3)
                    return FirstStatus.TiWan5Zhuo;
                return FirstStatus.FirstOK;
            }
            else
                return FirstStatus.FirstOK;

        }
        else
            return FirstStatus.FirstOK;

    }

    public bool CheckCatchRush(Card card)//闲家急捉闲家
    {
        if (PlayerSelf.Name == Landlord)
            return false;

        if (CurrentDiscards4.Count > 0 && CurrentDiscards4.Count < 4)
        {
            bool bCatch = false;
            Card topCard = CurrentDiscards4[CurrentCards4Top()];
            if (card.GreatThan(topCard) &&
                !EngineTool.isJinZhang(topCard) &&
                topCard.Suit != PokerConst.Spade)  //灭牌
                bCatch = true;
            /*
            if (bCatch &&
                CurrentDiscards4.Count == 3 &&  //到底家出牌
                card.Value >= PokerConst.ShangValve - 1 &&
                (topCard.Value != PokerConst.JiValve ||
                 topCard.Value != PokerConst.ShangValve - 2))
                return false;*/

            int idxLord = Players.FindIndex((p) => { return p.Name == Landlord; });
            int idxSelf = Players.FindIndex((p) => { return p.Name == PlayerSelf.Name; });
            int idxtop = (4 + idxSelf - (CurrentDiscards4.Count - CurrentCards4Top())) % 4;
            int idx0 = (4 + idxSelf - CurrentDiscards4.Count) % 4;
            int lordPos = (4 + idxLord - idx0) % 4;
            if (bCatch &&                           //捉牌
                Players[idxtop].Name != Landlord && //捉闲家占大牌
                Players[idxtop].winCards.Count < 2 &&   //该闲家还未正本
                lordPos <= CurrentDiscards4.Count - 1 &&    //庄家已出牌，未占大
                HandCards.Count > 3 &&                  //第6桌以前
                HandCards.Exists(c => !EngineTool.isJinZhang(c) &&
                                      !c.GreatThan(topCard)  )   )//手牌有该门以外青张
            {

                if (HandCards.Exists(c => c.Suit == PokerConst.Spade &&
                                   c.Value >= PokerConst.JianValve))
                {
                    PlayerSelf.isJiGua = true;
                    Players[idxSelf].isJiGua = true;
                    return false;
                }

                List<Card> jinCards = HandCards.Union(PlayerSelf.winCards).Where(c => EngineTool.isJinZhang(c)).ToList();
                int nsuits = jinCards.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;
                if (nsuits == 4)//存在四門色樣
                    return false;

                if (FourDoor.Count == 4)
                {
                    /*int nqings = EngineTool.GetQingCount(HandCards, FourDoor[FourDoor.Count - 1]) ,
                        nqing3 = EngineTool.GetQingCount(HandCards, FourDoor[FourDoor.Count - 2]),
                        njis = HandCards.Count(c => c.Value == PokerConst.JiValve &&                                                  
                                                  c.Suit == FourDoor[FourDoor.Count - 1]),
                        nji3 = HandCards.Count(c => c.Value == PokerConst.JiValve &&
                                                  c.Suit == FourDoor[FourDoor.Count - 2]);
                    nqing3 += nji3;
                    nqings += njis;
                    if (nqing3 <= 1 && nqings <= 1)
                        return false;*/                   

                    var vcards = HandCards.Where(c => !EngineTool.isJinZhang(c));

                        if (!vcards.ToList().Exists(c => topCard.GreatThan(c))  &&
                        !vcards.ToList().Exists(c => c.Suit != topCard.Suit)   ) 
                        
                        return false;
                }

                return true;
            }
        }
        return false;
    }

    public int CheckMieCard(Card card)
    {
        if (CurrentDiscards4.Count > 0)
        {
            bool bMie = false, bKeepShi = false;
            int itop = CurrentCards4Top();
            if (card.Value < CurrentDiscards4[itop].Value ||
                card.Suit != CurrentDiscards4[itop].Suit)  //灭牌
                bMie = true;

            List<Card> lctmp = new List<Card>(HandCards);
            lctmp.AddRange(AllDiscards);
            if (HandCards.Count(c => c.Suit == PokerConst.Spade) == 1 &&      //手上僅有一張十子
                HandCards.Exists(c => c.Suit != PokerConst.Spade &&
                                    c.Value < EngineTool.GetShangValve(c.Suit) - 1) &&//有非十子青張
                AllDiscards.Exists(c => c.Suit == PokerConst.Spade &&
                                    (c.Value == PokerConst.JianValve)) &&     //千僧已經現身
                AllDiscards.Exists(c => c.Suit == PokerConst.Spade &&
                                     (c.Value == PokerConst.JianValve - 1)) && //百老已經現身
                !AllDiscards.Exists(c => c.Suit == PokerConst.Spade &&
                                     (c.Value == PokerConst.JiValve)))
                bKeepShi = true;

            if (bMie && bKeepShi)
            {
                if (card.Suit == PokerConst.Spade && !EngineTool.isJinZhang(card) )
                    return 6;
                return 0;
            }
            if (bMie && CurrentDiscards4[itop].Suit != PokerConst.Spade) //二十未現身
            {
                //千僧  ——————防百老二十

                if (HandCards.Count > 1 &&
                    HandCards.Count(c => c.Suit == PokerConst.Spade && c.Value >= PokerConst.JianValve) < 2 &&
                    (card.Value == PokerConst.JianValve && card.Suit == PokerConst.Spade) &&
                    (lctmp.Exists(c => c.Value == PokerConst.JiValve &&
                                                c.Suit == PokerConst.Spade) &&
                     lctmp.Exists(c => c.Value == PokerConst.JianValve - 1 &&
                                                c.Suit == PokerConst.Spade)))//万千应留守
                    return 5;
                //紅萬——————防千僧百老
                if (HandCards.Count > 1 &&
                     HandCards.Count(c => c.Suit == PokerConst.Spade && c.Value >= PokerConst.JianValve) < 2 &&
                    (card.Value == PokerConst.ShangValve && card.Suit == PokerConst.Spade) &&
                   (lctmp.Exists(c => c.Value == PokerConst.JianValve &&
                                               c.Suit == PokerConst.Spade) &&
                    lctmp.Exists(c => c.Value == PokerConst.JianValve - 1 &&
                                               c.Suit == PokerConst.Spade)))//万千应留守
                    return 5;

                if (EngineTool.GetQingCount(HandCards, PokerConst.Spade) > 0 &&    //有十字青张而未灭
                 card.Suit != PokerConst.Spade)

                    return 1;
            }

            if (bMie && CurrentDiscards4[itop].Suit == PokerConst.Spade && //上家出十子
                     HandCards.Exists(c => c.Suit != PokerConst.Spade && //有十门以外青张 
                                  c.Value > PokerConst.JiValve && c.Value < EngineTool.GetShangValve(c.Suit) - 1) &&
                     !lctmp.Exists(c => c.Value == PokerConst.JiValve &&
                                               c.Suit == PokerConst.Spade))
            {
                bool bSpadeA = false;
                if (HandCards.Exists(c => c.Suit == PokerConst.Spade &&
                                                c.Value == PokerConst.JiValve))
                    bSpadeA = true;

                if (card.Suit != PokerConst.Spade)  //  要灭的不是十子
                {
                    if (EngineTool.GetQingCount(HandCards, PokerConst.Spade) >= 2
                        && !bSpadeA) //有超过一个青张,无二十
                        return 7;
                    else if (EngineTool.GetQingCount(HandCards, PokerConst.Spade) > 0
                            && bSpadeA) //有二十万不可留青张                          
                        return 4;
                    else
                        return 0;
                }
                else if (card.Value != PokerConst.JiValve)//要灭的牌是十子,但不是二十
                {

                    if (EngineTool.GetQingCount(HandCards, PokerConst.Spade) == 1 &&
                        card.Value < PokerConst.ShangValve - 2 &&
                        !bSpadeA)//青张
                        return 3;
                    else
                        return 0;
                }
                else
                    return 0;
            }

            if (bMie && FourDoor.Count > 4 &&
                EngineTool.GetQingCount(HandCards, PokerConst.Spade) <= 1)//三門已開 無青十
                return 0;
            if (bMie && FourDoor.Count > 2 && PlayerSelf.Name != Landlord)  //有2張以上三门青张而未出
            {
                int nqing3 = EngineTool.GetQingCount(HandCards, FourDoor[2]);
                var vcards = HandCards.Where(c => !EngineTool.isJinZhang(c));

                //出牌为三门时，三门已经没有小于最大者的青张，可以灭正门
                if (CurrentDiscards4[itop].Suit == FourDoor[2]&&
                      (card.Suit == FourDoor[0]||
                       card.Suit == FourDoor[1]  )  )
                {
                    if (
                        !vcards.ToList().Exists(c => CurrentDiscards4[itop].GreatThan(c)) )
                        return 0;
                }

                if (nqing3 > 0 && card.Suit != FourDoor[2] &&   //三門超過一個青張
                     HandCards.Count(c => c.Suit == FourDoor[2]) > nqing3 &&
                     EngineTool.GetQingCount(HandCards, PokerConst.Spade) == 0)

                    //闲家优先灭三门 庄家无视
                    return 2;


            }
            if (bMie && FourDoor.Count <= 2 &&
                     HandCards.Exists(c => c.Suit != CurrentDiscards4[itop].Suit && //有本门以外青张 
                                  c.Value > PokerConst.JiValve && c.Value < EngineTool.GetShangValve(c.Suit) - 1) &&
                                  FourDoor.Contains(card.Suit))
            {
                List<string> lstmp = new List<string>();
                if (FourDoor.Count < 1)
                    lstmp.AddRange(MieCard.Get3Suits(CurrentDiscards4[itop].Suit));
                else if (FourDoor.Count == 1)
                    lstmp.AddRange(MieCard.Get3Suits(FourDoor[0]));
                else
                    lstmp.AddRange(FourDoor);

                if (EngineTool.GetQingCount(HandCards, PokerConst.Spade) > 1)//有2張以上黑桃青张
                    return 1;
                if (lstmp.Count > 2 && (
                          EngineTool.GetQingCount(HandCards, lstmp[lstmp.Count - 1]) > 1 ||
                          EngineTool.GetQingCount(HandCards, lstmp[lstmp.Count - 2]) > 1))

                    return 2;


            }


        }
        return 0;
    }

    #region  用不上
    private List<List<Card>> CountRepeats(Card[] cards)
    {
        List<Card> lst = new List<Card>(cards);
        lst.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });//升序排序
        List<List<Card>> repeats = new List<List<Card>>() {
                new List<Card>(),new List<Card>(),new List<Card>(),new List<Card>()};
        for (int i = 0; i < lst.Count; i++)
        {
            int count = 0;
            for (int j = 0; j < lst.Count; j++)
            {
                if (cards[j].Value == cards[i].Value)
                {
                    count++;//重复次数+1
                }
            }
            //重复数据的值只记录一次，如333中的3只记录一次
            if (!repeats[count - 1].Exists((c) => { return c.Value == lst[i].Value; }))
                repeats[count - 1].Add(lst[i]);//根据重复次数分别记录（1,2,3,4）
        }
        return repeats;
    }
    #endregion
}
public enum MdValue : int
{
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack
}

public enum GameStatus
{
    CallLandlord = 0,
    Claim = 1,
    FightLandlord = 2,
    ViewReslult = 3,
    Chong
}

public enum EpicCards
{
    NoEpic = 0,
    FourJian = 1,
    FourQu = 2,
    Bailaojia = 3,
    ShunFengQi = 4,
    ShunFeng4Qu = 5,
    ShunFeng4Jian = 6
}

public enum TwinsFlower
{
    NoTwins = 0,
    WinQian = 1,
    WinWan = 2,
    KingWan = 3
}


[Serializable]
public class PlayerInfo
{
    public string Name;
    public string Pwd;
    public EpicCards EpicStatus = EpicCards.NoEpic;
    public TwinsFlower twinsF = TwinsFlower.NoTwins;
    public int Score = 1000;
    public int Zhuoji = -1;
    public int thechong = 0;
    public bool isBai = false;
    public bool isJiGua = false;
    public List<Card> Cards = new List<Card>();
    public List<Card> winCards = new List<Card>();

    public void ResetStatus()
    {
        EpicStatus = EpicCards.NoEpic;
        twinsF = TwinsFlower.NoTwins;
        Zhuoji = -1;
        thechong = 0;
        isBai = false;
        isJiGua = false;
        Cards.Clear();
        winCards.Clear();
    }
}