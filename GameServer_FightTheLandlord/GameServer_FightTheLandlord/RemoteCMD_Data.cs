using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace GameServer_FightTheLandlord
{
    [Serializable]
    public class RemoteCMD_Data
    {
        public RemoteCMD_Const cmd;
        public PlayerInfo player;
        public Card[] cards;
        public int MatchCounts;
        public RemoteCMD_Data()
        {
            player = new PlayerInfo();
        }
    }
    [Serializable]
    public class PlayerInfo
    {
        public string Name;
        public string Pwd;
        public int Score;
    }
    [Serializable]
    public class Card
    {

        public string Suit;
        public int Value;
        public Card() { }
        public Card(string suit, int value)
        {
            Suit = suit;
            Value = value;
        }

       
        public Card(string svstr)
        {
            string[] sArray = svstr.Split('*');
            Suit = sArray[0];
            Value = int.Parse(sArray[1]);
        }

        public bool GreatThan(Card card)
        {
            if (Suit == card.Suit &&
                Value > card.Value)
                return true;
            else
                return false;
        }

        public string SValue()
        {
            return Suit + "*" + Value.ToString();
        }
        public static string GetSpriteName(Card card)
        {
            string name = card.Suit;
            switch (card.Value)
            {

                case 1: name += PokerConst.Ace; break;
                case 2: name += PokerConst.Two; break;
                case 3: name += PokerConst.Three; break;
                case 4: name += PokerConst.Four; break;
                case 5: name += PokerConst.Five; break;
                case 6: name += PokerConst.Six; break;
                case 7: name += PokerConst.Seven; break;
                case 8: name += PokerConst.Eight; break;
                case 9: name += PokerConst.Nine; break;
                case 10: name += PokerConst.Ten; break;
                case 11: name += PokerConst.Jack; break;
                //case 10: name += PokerConst.Queen; break;
                //case 11: name += PokerConst.King; break;
                //case 12: name += PokerConst.Ace; break;
                //case 13: name += PokerConst.Two; break;
                //case 14: name += PokerConst.BlackJoker; break;
                //case 15: name += PokerConst.RedJoker; break;
            }
            return name;
        }


        public static string MdName(Card card)
        {
            string name = null;
            if (card.Suit.Equals(PokerConst.Club))
            {
                switch (card.Value)
                {
                    case 1: name = "九文"; break;
                    case 2: name = "八文"; break;
                    case 3: name = "七文"; break;
                    case 4: name = "六文"; break;
                    case 5: name = "五文"; break;
                    case 6: name = "四文"; break;
                    case 7: name = "三文"; break;
                    case 8: name = "二文"; break;
                    case 9: name = "一文"; break;
                    case 10: name = "枝花"; break;
                    case 11: name = "尊空文"; break;
                }
                return name;
            }
            else if (card.Suit.Equals(PokerConst.Spade))
            {
                switch (card.Value)
                {
                    case 1: name = "二十万"; break;
                    case 2: name = "三十万"; break;
                    case 3: name = "四十万"; break;
                    case 4: name = "五十万"; break;
                    case 5: name = "六十万"; break;
                    case 6: name = "七十万"; break;
                    case 7: name = "八十万"; break;
                    case 8: name = "九十万"; break;
                    case 9: name = "百万"; break;
                    case 10: name = "千万"; break;
                    case 11: name = "尊万万"; break;
                }
                return name;
            }
            else if (card.Suit.Equals(PokerConst.Diamond))
                name = "索";
            else if (card.Suit.Equals(PokerConst.Heart))
                name = "贯";


            switch (card.Value)
            {
                case 1: name = "一" + name; break;
                case 2: name = "二" + name; break;
                case 3: name = "三" + name; break;
                case 4: name = "四" + name; break;
                case 5: name = "五" + name; break;
                case 6: name = "六" + name; break;
                case 7: name = "七" + name; break;
                case 8: name = "八" + name; break;
                case 9: name = "尊九" + name; break;
            }
            return name;
        }
    }
    public class PokerConst
    {
        public const string Spade = "Spade";
        public const string Heart = "Heart";
        public const string Club = "Club";
        public const string Diamond = "Diamond";
        public const string Ace = "Ace";
        public const string Two = "Two";
        public const string Three = "Three";
        public const string Four = "Four";
        public const string Five = "Five";
        public const string Six = "Six";
        public const string Seven = "Seven";
        public const string Eight = "Eight";
        public const string Nine = "Nine";
        public const string Ten = "Ten";
        public const string Jack = "Jack";
        public const string Queen = "Queen";
        public const string King = "King";
        public const string BlackJoker = "SJoker";
        public const string RedJoker = "LJoker";
    }
    [Serializable]
    public class UserData
    {
        public int Id;
        public bool IsDeleted;
        public string Name;
        public string Pwd;
        public string QQEmail;
        public int Type;
        public int Gold;
        public string RegisterTime;
        public string EndLoginTime;
        public string UpdateTime;
    }

    public class ActionCode
    {
        public const int Default = 0;
        public const int Login = 1;
        public const int Register = 2;
        public const int GetResultInfo = 3;
        public const int CreateRoom = 4;
        public const int GetRoomList = 5;
        public const int JoinRoom = 6;
        public const int LeaveRoom = 7;
        public const int GetRoomInfo = 8;
        public const int StartGame = 9;
    }


    public class RequestCode
    {
        public const int Default = 0;
        public const int User = 1;
        public const int Result = 2;
        public const int Room = 3;
        public const int Game = 4;
    }


    [Serializable]
    public enum RemoteCMD_Const : int
    {
        Match = 0,//匹配
        MatchSuccess,//匹配成功
        MatchWait,//等待匹配完成             
        GenerateCards,//生成牌
        DealCards,//发牌
        BaseCards,//底牌
        Player2,//指定玩家2
        Player3,//指定玩家3
        Player4,//指定玩家4
        StartPlayer,//游戏开始的玩家
        CallLandlord,//叫地主
        NotCall,//不叫
        Claim,//抢地主
        NotClaim,//不抢
        Pass,//跳过
        Discards,//出牌
        Chong,//冲
        GamerOver,//游戏结束
        GameTurn,//行动回合
        CancelMatch,//取消匹配
        CalcScore,//计算积分
        Replay,//赢家
        ReturnLobby,//返回大厅
        ReturnRoom,//返回房间
        Register,//注册
        RegSuccess,//注册成功
        RegError,//格式错误
        RegAlready,//该用户名已注册
        Login,    //登录
        LoginSuccess,//登录成功
        LoginPwdErr, //密码错误
        LoginUserErr, //无此用户
        CreateRoom,//创建房间
        GetRoomList,//获取房间列表
        JoinRoom ,//加入房间
        LeaveRoom,//离开房间
        GetRoomInfo,//获取房间信息
        Quit          //某客户端退出游戏       
    }
}
