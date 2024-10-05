using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class MieCard
    {
        public static List<string> Get3Suits(string suit1=null)
        {
            List<string> suits = new List< string > ();
            List<string> suitsAll = new List<string> {PokerConst.Club,PokerConst.Diamond,
                                                      PokerConst.Heart };
            if (suit1 != null && suit1 != PokerConst.Spade)
            { 
                suitsAll.Remove(suit1);
                suits.Add(suit1);
            }

           

            suits.AddRange(suitsAll);

            return suits;
;        }

        public static List<Card> SortMieCards(List<Card>lc, string suit3)
        { 
            List<Card> miecards = new List<Card> ();
            List<Card> qingSpadeX3 = lc.Where(c => (c.Suit == PokerConst.Spade||
                                                        c.Suit == suit3 )&&
                                         c.Value < PokerConst.ShangValve - 2 &&
                                         c.Value > PokerConst.JiValve).ToList();
            List<Card> jicards = lc.Where(c => c.Value == PokerConst.JiValve).ToList();
            List<Card> qingZhengs = lc.Where(c => c.Suit != PokerConst.Spade &&
                                                c.Suit != suit3 &&
                                                c.Value != PokerConst.JiValve &&
                                                c.Value < EngineTool.GetShangValve(c.Suit) - 1).ToList();

            qingSpadeX3.Sort((a, b) => { return a.Suit == b.Suit ? b.Suit.CompareTo(a.Suit): a.Value.CompareTo(b.Value); });//升序排序
            jicards.Sort((a, b) => { return b.Suit.CompareTo(a.Suit); });
            qingZhengs.Sort((a, b) => { return a.Value.CompareTo(b.Value); });
            miecards.AddRange(qingSpadeX3);
            miecards.AddRange(jicards);
            miecards.AddRange(qingZhengs);

            List<Card> lhong = lc.Except(miecards).ToList();
            lhong.Sort((a, b) => { return a.Value == b.Value?a.Value.CompareTo(b.Value): b.Suit.CompareTo(a.Suit); });
            miecards.AddRange(lhong);

            return miecards;
        }

        public static Card GetMieCard2(List<Card> cards, List<string> lDoor4, bool isSpade = false)
        {
            string suit3 = null;
            if (lDoor4.Count ==0 )
                suit3 = Get3Suits()[2];
            else if (lDoor4.Count == 1)
                suit3 = Get3Suits(lDoor4[0])[2];
            List<Card> ltmp = SortMieCards(cards, suit3);
            if (isSpade && 
                ltmp.Count(c=>c.Suit==PokerConst.Spade)==1 &&
                ltmp.Count(c => c.Suit != PokerConst.Spade) > 0)
            {
                return ltmp.Where(c => c.Suit != PokerConst.Spade).ToList()[0];
            }
            return ltmp[0];

        }
            public static Card GetMieCard(List<Card> cards, List<string> lDoor4,bool isSpade = false)
        {
            if (cards.Count == 1)
                return cards[0];
            if (cards.Count == 2)//剩两张灭趣
            {
                if (cards.Exists(c => c.Value == PokerConst.JiValve))
                {
                    if (cards[0].Value == PokerConst.JiValve)
                        return cards[0];
                    else
                        return cards[1];
                }

                else if ( (EngineTool.GetShangValve(cards[0].Suit) - cards[0].Value) >=
                    EngineTool.GetShangValve(cards[1].Suit) - cards[1].Value)    //剩2張，滅較小牌
                    return cards[0];
                else
                    return cards[1];
            }

            bool bAceS = false, bAce3 = false, bAce2 = false, bAce1 = false;
            int countQings = 0, countQing3 = 0, countQing2 = 0, countQing1 = 0; // 青张数量
            int countHongs = 0, countHong3 = 0, countHong2 = 0, countHong1 = 0; // 红张数量

            string threeDoor = "", 
                    twoDoor ="",
                    oneDoor = "";

            if (lDoor4.Count == 1) { //只开一门时，暂定其余正门和生门顺序
                oneDoor = Get3Suits(lDoor4[0])[0];
                twoDoor = Get3Suits(lDoor4[0])[1];
                threeDoor = Get3Suits(lDoor4[0])[2];
            }
            if (lDoor4.Count >= 3) {
                oneDoor = lDoor4[0];
                twoDoor = lDoor4[1];
                threeDoor = lDoor4[2]; 
            }


            List<Card> lSpade = cards.Where(c => c.Suit == PokerConst.Spade).ToList();
            List<Card> lThree = cards.Where(c => c.Suit == threeDoor).ToList();
            List<Card> lTwo = cards.Where(c => c.Suit == twoDoor).ToList();
            List<Card> lOne = cards.Where(c => c.Suit == oneDoor).ToList();
            
            countQings = lSpade.Where(c => c.Value > 1 && c.Value <= EngineTool.GetShangValve(c.Suit) - 1).Count();//十子青张数量
            countHongs = lSpade.Where(c => c.Value >= EngineTool.GetShangValve(c.Suit) ).Count();//十子红张数量
            bAceS = lSpade.Exists(c => c.Value == PokerConst.JiValve) ? true : false;//是否有二十万

            countQing1 = lOne.Where(c => c.Value > 1 && c.Value < EngineTool.GetShangValve(c.Suit) - 1).Count();//头门青张数量
            countHong1 = lOne.Where(c => c.Value >= EngineTool.GetShangValve(c.Suit)-1).Count();//头门红张数量
            bAce1 = lOne.Exists(c => c.Value == PokerConst.JiValve) ? true : false;

            countQing2 = lTwo.Where(c => c.Value > 1 && c.Value < EngineTool.GetShangValve(c.Suit) - 1).Count();//二门青张数量
            countHong2 = lTwo.Where(c => c.Value >= EngineTool.GetShangValve(c.Suit) - 1).Count();//二门红张数量
            bAce2 = lTwo.Exists(c => c.Value == PokerConst.JiValve) ? true : false;

            countQing3 = lThree.Where(c => c.Value > 1 && c.Value < EngineTool.GetShangValve(c.Suit) - 1).Count();//三门青张数量
            countHong3 = lThree.Where(c => c.Value >= EngineTool.GetShangValve(c.Suit) - 1).Count();//三门红张数量
            bAce3 = lThree.Exists(c => c.Value == PokerConst.JiValve) ? true : false;


            
            lSpade.Sort((a, b) => { return a.Value.CompareTo(b.Value); });//升序排序
            lThree.Sort((a, b) => { return a.Value.CompareTo(b.Value); });//升序排序
            lTwo.Sort((a, b) => { return a.Value.CompareTo(b.Value); });//升序排序
            lOne.Sort((a, b) => { return a.Value.CompareTo(b.Value); });//升序排序

            
            
            //大于2张
            if (bAceS && countQings >= 1)        //二十 + 1个以上青张
                return lSpade[1];
            else if (!bAceS && (countQings > 1 || (countQings > 0 && countHongs > 0)))  //无二十 + 2个以上青张 或 有青有红
                return lSpade[0];
            else if ( (!isSpade && countQings >= 1)||//出非十时候灭尽十门青张
                      (isSpade && (!bAceS && countQings > 1)||//斗十时不能将十子灭尽
                                  (bAceS && countQings >= 1)  ))//只有一个青张
                return lSpade[0].Value==PokerConst.JiValve? lSpade[1]: lSpade[0];
            else if (bAce3 && countQing3 >= 1)  //三门有趣 有青
                return lThree[1];
            else if (!bAce3 && (countQing3 > 1 || (countQing3 > 0 && countHong3 > 0)))//无A + 2个以上青张 或 有青有红
                return lThree[0];
            else if (countQing2 > countQing1)//二门青张多
            {
                if (bAce2 && countQing2 >= 1)          //有A 有青张
                    return lTwo[1];
                else if (!bAce2 && (countQing2 > 1 || (countQing2 > 0 && countHong2 > 0)))//无A + 2个以上青张 或 有青有红
                    return lTwo[0];
                else
                    return lTwo[0];
            }
            else if (countQing2 < countQing1)//头门青张多
            {
                if (bAce1 && countQing1 >= 1)    //有A 有青张
                    return lOne[1];
                else if (!bAce1 && (countQing1 > 1 || (countQing1 > 0 && countHong1 > 0))) //无A + 2个以上青张 或 有青有红
                    return lOne[0];
                else
                    return lOne[0];
            }
            else if (countQing2 == countQing1)//头门 二门 青张数量相等 且不为0
            {
                if (countQing2 > 0 && countHong2 > countHong1)   //二门红张多
                {
                    if (bAce2 && bAce1)
                        return lTwo[1];
                    else if (bAce2)
                        return lOne[0];
                    else if (bAce1)
                        return lTwo[0];
                    else if (countHong2 == 1) //两门都没有A
                        return lOne[0];
                    else if (countHong2 > 1)
                        return lTwo[0];

                    else return cards[0];

                }
                else if (countQing2 > 0 && countHong2 <= countHong1)//头门红张多
                {
                    if (bAce2 && bAce1)
                        return lOne[1];
                    else if (bAce2)
                        return lTwo[0];
                    else if (bAce1)
                        return lOne[0];
                    else if (countHong2 == 1) //两门都没有A
                        return lTwo[0];
                    else if (countHong2 > 1)
                        return lOne[0];

                    else return cards[0];
                }
                else if (countQing1 < 1 && countQing2 < 1)  //  头门 二门 均无青张
                {
                    if (bAce3 && bAceS)//十子和三门 有双A
                    {
                        if (countHong3 > countHongs)
                            return lSpade[0];
                        else return lThree[0];
                     
                    }
                    else if (!bAce3)
                    {
                        if (countQing3 > 0)
                            return lThree[0];
                        else if (countQings > 0 && bAceS)
                            return lSpade[1];
                        else if (countQings > 0 && !bAceS)
                            return lSpade[0];

                        else return cards[0];

                    }
                    else if (!bAceS)
                    {
                        if (countQing3 > 0 && !bAce3)
                            return lThree[0];
                        else if (countQing3 > 0 && bAce3)
                            return lThree[1];
                        else if (countQings > 0)
                            return lSpade[0];

                        else return cards[0];
                    }
                    else if (!bAce3 && !bAceS)
                    {
                        if (countQing3 > 0)
                            return lThree[0];
                        else if (countQings > 0)
                            return lSpade[0];
                        else if (countQing2 > 0)
                            return bAce2 ? lTwo[1] : lTwo[0];
                        else if (countQing1 > 0)
                            return bAce1 ? lOne[1] : lOne[0];

                        else return cards[0];

                    }
                    else return cards[0];

                }
                else return cards[0];
            }
            else
                return cards[0];
        }


    }



}
