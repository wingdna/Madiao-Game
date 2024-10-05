using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]



public class Card {

    public string Suit;
    public int Value;
    public Card()
    {

    }
    public Card(string suit,int value)
    {
        Suit = suit;
        Value = value;
    }
    public Card(string svstr)
    {
        string[] sArray = svstr.Split('*');
        
        Debug.Log("正在转换"+svstr);
        Suit = sArray[0].Replace("-","");
        Value = int.Parse(sArray[1].Replace("-", ""));
    }

    public bool GreatThan(Card card)
    {
        if (Suit == card.Suit &&
            Value > card.Value)
            return true;
        else
            return false;
    }

    public bool Equals(Card card)
    {
        if (Suit == card.Suit &&
            Value == card.Value)
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
            
            //case 14: name += PokerConst.BlackJoker; break;
            //case 15: name += PokerConst.RedJoker; break;
        }
        return name;
    }
     

    public string MdName()
    {
        string name = null;
        if (Suit.Equals(PokerConst.Club))
        {
            switch (Value)
            {
                case 1: name = MDNameFanConst.Wen9; break;
                case 2: name = MDNameFanConst.Wen8; break;
                case 3: name = MDNameFanConst.Wen7; break;
                case 4: name = MDNameFanConst.Wen6; break;
                case 5: name = MDNameFanConst.Wen5; break;
                case 6: name = MDNameFanConst.Wen4; break;
                case 7: name = MDNameFanConst.Wen3; break;
                case 8: name = MDNameFanConst.Wen2; break;
                case 9: name = MDNameFanConst.Wen1; break;
                case 10: name = MDNameFanConst.WenHua; break;
                case 11: name = MDNameFanConst.WenKong; break;
            }
            return name;
        }
        else if (Suit.Equals(PokerConst.Spade))
        {
            switch (Value)
            {
                case 1: name = MDNameFanConst.ErShi; break;
                case 2: name = MDNameFanConst.SanShi; break;
                case 3: name = MDNameFanConst.SiShi; break;
                case 4: name = MDNameFanConst.WuShi; break;
                case 5: name = MDNameFanConst.LiuShi; break;
                case 6: name = MDNameFanConst.QiShi; break;
                case 7: name = MDNameFanConst.BaShi; break;
                case 8: name = MDNameFanConst.JiuShi; break;
                case 9: name = MDNameFanConst.BaiWan;   break;
                case 10: name = MDNameFanConst.QianWan;  break;
                case 11: name = MDNameFanConst.WanWan; break;
            }
            return name;
        }
        else if (Suit.Equals(PokerConst.Diamond))
            name = MDNameFanConst.Suo;
        else if (Suit.Equals(PokerConst.Heart))
            name = MDNameFanConst.Guan;
        

        switch (Value)
        {
            case 1: name = MDNameFanConst.One + name; break;
            case 2: name = MDNameFanConst.Two + name; break;
            case 3: name = MDNameFanConst.Three + name; break;
            case 4: name = MDNameFanConst.Four + name; break;
            case 5: name = MDNameFanConst.Five + name; break;
            case 6: name = MDNameFanConst.Six + name; break;
            case 7: name = MDNameFanConst.Seven + name; break;
            case 8: name = MDNameFanConst.Eight + name; break;
            case 9: name = MDNameFanConst.Nine + name; break;                        
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
    public const int ShangValve = 11;
    public const int JianValve = 10;
    public const int JiValve = 1;
}
