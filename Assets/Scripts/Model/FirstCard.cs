using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class FirstCard
    {
        #region 将所有牌按花色加入各自列表
        /*
         * private static List<List<Card>> GetAllSuits(List<Card> cards)
        {
            List<Card> lst = new List<Card>(cards);
            lst.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });//升序排序
            List<List<Card>> lSuits = new();
            foreach (Card c in lst)
            {
                if (c.Suit == PokerConst.Club)
                    lSuits[0].Add(c);
                else if (c.Suit == PokerConst.Diamond)
                    lSuits[1].Add(c);
                else if (c.Suit == PokerConst.Heart)
                    lSuits[2].Add(c);
                if (c.Suit == PokerConst.Spade)
                    lSuits[3].Add(c);

            }
            return lSuits;
        }
        */
        #endregion

        #region 获取所有牌共包含几种花色
        private static int GetSuitsCount(List<List<Card>> lSuits)
        {
            int icount = 0;
            foreach (List<Card> lc in lSuits)
            {
                if (lc.Count > 0)
                    icount++;
            }
            return icount;
        }
        #endregion

        public static int GetShangValve(string suit)
        {
            int iShang = PokerConst.ShangValve;
            if (suit != PokerConst.Spade && suit != PokerConst.Club)
                iShang -= 2;
            return iShang;
        }

        #region 從指定的某一門花色中選取合適出牌
        private static Card FirstSelSuit(List<Card> lcards, List<Card> allDis, bool bLord = false)
        {
            if (lcards == null || lcards.Count < 1)
                return null;
            if (lcards.Count == 1)
                return lcards[0];//僅一張直接返回

            List<Card> handXalldis = new List<Card>(lcards);
            handXalldis.AddRange(allDis);
            //赏肩双持
            int nqings = EngineTool.GetQingCount(lcards, lcards[0].Suit);//青張數量
            int nji = lcards.Count(c => c.Value == PokerConst.JiValve),
                njian = lcards.Count(c => c.Value == GetShangValve(c.Suit) - 1),
                nshang = lcards.Count(c => c.Value == GetShangValve(c.Suit));

            if (lcards.Count(c => c.Value >= GetShangValve(c.Suit) - 1) >= 2 &&   //  非十字门赏肩双持
                lcards.Count(c => c.Suit == PokerConst.Spade) == 0)
            {   //或十字门只剩万千百之二
                return lcards[lcards.Count - 1];//提赏

            }
            
            if (lcards.Exists(c => c.Suit == PokerConst.Spade)) //十字门
            {
                //万千 双持
                if (lcards.Count(c => c.Value > PokerConst.ShangValve - 1) == 2)
                    return lcards[lcards.Count - 1];//提万              

                //手有萬千 或萬千已現 直接獻百
                if (handXalldis.Count(c => c.Value > PokerConst.ShangValve - 1) == 2 &&
                    lcards.Exists(c => c.Value == PokerConst.ShangValve - 2))
                    return lcards.Find(c => c.Value == PokerConst.ShangValve - 2);//獻百

                //手持千百 或萬百
                if (lcards.Count(c => c.Value >= PokerConst.ShangValve - 2) >= 2 &&//有万或千
                    lcards.Exists(c => c.Value == PokerConst.ShangValve - 2))       //有百
                {
                    if (allDis.Exists(c => c.Value == PokerConst.ShangValve ||  //  場外唯一一張萬千已現身
                                         c.Value == PokerConst.JianValve))
                        return lcards.Find(c => c.Value == PokerConst.ShangValve - 2);//獻百
                    else
                    {
                        if (lcards.Count > 2)
                            return lcards[lcards.Count - 3];//發青張最大一張頂下紅千
                        else
                            return lcards[lcards.Count - 1];//發最大一張
                    }

                }

                if (nqings > 0)
                {
                    if (lcards.Exists(c => c.Value == PokerConst.JiValve))  //有趣 有青                        
                        return lcards[nqings];
                    else if (EngineTool.GetQingCount(lcards, lcards[0].Suit) > 0)
                        return lcards[nqings - 1];

                }
                return lcards[lcards.Count - 1];
            }
            /*    else if (lcards.Exists(c => c.Value == EngineTool.GetShangValve(c.Suit)) &&//有賞無肩 且肩未現身
                        !lcards.Exists(c => c.Value == EngineTool.GetShangValve(c.Suit)-1) &&
                        allDis.Exists(c=>c.Value== EngineTool.GetShangValve(lcards[0].Suit) - 1) &&
                        EngineTool.GetQingCount(lcards, lcards[0].Suit) > 0)//孤賞下

                    return lcards[bLord ? lcards.Count - 2:0];//發賞下牌
            */

           
               
            if (bLord)//非十子  莊家出最大
            {
                if (nshang <= 0 && njian == 1) //非十字門有肩無賞
                {
                    if (allDis.Exists(cx => cx.Suit == lcards[0].Suit &&
                                          cx.Value == GetShangValve(cx.Suit)))
                        return lcards[lcards.Count - 1];
                   

                    return lcards[nji];

                }

                if (EngineTool.isTrueCard(handXalldis, lcards[lcards.Count - 1]))
                    return lcards[lcards.Count - 1];
            }
                
            
            return lcards[nji];


        }
        #endregion

        #region 只剩三門和百十子  有百發百 有十無百開三
        private static Card SpadeAndThreeDoor( List<Card> lcards, List<Card> allDis, string suit1, string suit2,bool bLord = false)
        {
            int nsuits = lcards.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;
            if (nsuits == 2 &&
                !lcards.Exists(c => c.Suit == suit1) &&
                !lcards.Exists(c => c.Suit == suit2))
            {
                var vspades = lcards.Where(c => c.Suit == PokerConst.Spade);

                if (lcards.Exists(c => c.Suit == PokerConst.Spade &&
                                     c.Value == PokerConst.ShangValve - 2)&&
                    EngineTool.GetQingCount(lcards,PokerConst.Spade) ==0   )//有百 無青十
                    return FirstSelSuit(vspades.ToList(), allDis);
                
               return FirstSelSuit(lcards.Except( vspades).ToList(), allDis,bLord);
            }
            return null;
        }
        #endregion
        private static List<string> MakeTmpsuits(List<string> fourDoor)
        {
            List<string> allsuits = new List<string> { PokerConst.Club,
                                                    PokerConst.Diamond,
                                                    PokerConst.Heart,
                                                    PokerConst.Spade};
            List<string> newsuits = new List<string>();
            if (fourDoor.Count < 1)
            {
                newsuits.AddRange(allsuits);
                return newsuits;
            }

            else if (fourDoor.Count == 1)
            {
                allsuits.Remove(fourDoor[0]);
                newsuits.AddRange(allsuits);
                return newsuits;
            }
            else
                return fourDoor;

        }

        public static Card GetFirstCard(List<Card> cards, List<Card> allDis, bool bLord, List<string> FourDoor, bool flowerFallen = false)
        {
            if (cards.Count == 1) //只声一张牌 直接返回
                return cards[0];

            string suit1, suit2;
            if (FourDoor.Count < 2)
            {
                suit1 = MakeTmpsuits(FourDoor)[0];
                suit2 = MakeTmpsuits(FourDoor)[1];
            }
            else
            {
                suit1 = FourDoor[0];
                suit2 = FourDoor[1];
            }

            //實發過的花色數量 莊家在兩路正門未發即可

            int nsuits = allDis.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;
            List<List<Card>> lSort = EngineTool.Sort1230(cards, bLord, suit1, suit2, flowerFallen);
            //關賞鬥十
            Card selCard = GuanShangDouShi( allDis, suit1, suit2, cards, bLord);
            //告百發十
            if (cards.Exists(c => c.Value == EngineTool.GetShangValve(c.Suit, true)) &&//有賞才能報百
               ((bLord && nsuits < 2) || (!bLord && FourDoor.Count < 2)))
                selCard = GaoBaiFaShi(cards, allDis, FourDoor,bLord);     //告百發十

            //關真鬥十
            if (selCard == null && GetSuitsCount(lSort) == 2)    //不超过两种花色
                selCard = GuanZhenDouShi( allDis, cards,bLord);
            //只剩三門和十子
            if (selCard == null)
                selCard = SpadeAndThreeDoor( cards, allDis, suit1, suit2,bLord);
            //尽手提赏
            if (selCard == null)
                selCard = JinShou1Suit(lSort, allDis, suit1, suit2);
            //正門不盡
            if (selCard == null)
                selCard = KeepLastSuit1( cards, allDis, FourDoor,bLord);
            //正門優先
            if (selCard == null && (cards.Exists(c => c.Suit == suit1)
                                 || cards.Exists(c => c.Suit == suit2)))
                selCard = FirstZhengMen(lSort, allDis, suit1, suit2, bLord);

            //留張製尾
            if (selCard == null && GetSuitsCount(lSort) > 2)    //超过两种花色
                selCard = LiuZhangZhiWei(lSort, allDis, suit1, suit2);


            //千萬留守
            if (selCard != null && !QianWanLiuShou(cards, selCard))
                selCard = null;
            if (selCard == null)
                selCard = FirstSelSuit(lSort[0], allDis, bLord);
           // if (selCard == null || !QianWanLiuShou(cards, selCard))
           //     selCard = cards[0];
            if (selCard == null)
                selCard = cards[0];
            return selCard;
        }
        private static Card KeepLastSuit1( List<Card> cards,   List<Card> allDis, List<string> FourDoor,bool bLord)
        {
            if (FourDoor.Count < 2)
                return null;

            int counts0 = cards.Count(c => c.Suit == FourDoor[0]),
                counts1 = cards.Count(c => c.Suit == FourDoor[1]);
               
            if (counts0>0&& counts0 > 0&& (counts0 == 1 || counts1 == 1))
            {
                int idx = counts0 > 1 ? 0 : 1;
                return FirstSelSuit(cards.Where(c=>c.Suit == FourDoor[idx]).ToList(),
                                    allDis, bLord);
            }
            return null;
        }

        private static bool QianWanLiuShou(List<Card> lc, Card card)
        {

            int nspadeKQ = lc.Where(c => c.Suit == PokerConst.Spade &&
                                c.Value >= PokerConst.ShangValve - 1).Count();

            if (lc.Count == 1 || card.Suit != PokerConst.Spade || card.Value != PokerConst.ShangValve - 1)
                return true;
            else
            {
                if (nspadeKQ == 1 &&
                       !lc.Exists(c => c.Suit == PokerConst.Spade &&
                                     c.Value == PokerConst.ShangValve - 2))
                    return false;
                else
                    return true;

            }

        }


        private static Card FirstZhengMen(List<List<Card>> lSuits, List<Card> allDis, string suit1, string suit2, bool bLord = false)
        {
            List<List<Card>> ltmp = new List<List<Card>>();
            if (lSuits.Count == 1)
                return FirstSelSuit(lSuits[0], allDis, bLord);//只剩一門
                                                              //
            foreach (var lcards in lSuits)
            {
                if (lcards.Exists(c => c.Value == GetShangValve(c.Suit)) && //赏肩双持
                    lcards.Exists(c => c.Value == GetShangValve(c.Suit) - 1))

                    return FirstSelSuit(lcards, allDis, bLord);//发赏        
            }

            int nzhengSuit = lSuits.Count(l => l[0].Suit == suit1 ||
                                       l[0].Suit == suit2);

            
             for (int i=0;i<nzhengSuit; i++)
                    ltmp.Add(lSuits[i]);
            if (bLord && lSuits.Count > nzhengSuit)
                ltmp.Add(lSuits[nzhengSuit]);

            if (nzhengSuit == 1 && lSuits[0].Count == 1 &&
                  EngineTool.isTrueCard(allDis, lSuits[0][0]))
                return FirstSelSuit(lSuits[lSuits.Count - 1], allDis, bLord);
            if (!bLord && nzhengSuit == 1)
                return FirstSelSuit(ltmp[0], allDis, bLord);

            List<int> nqings = new List<int>();
            foreach (var vcards in ltmp)
            {
                if( !vcards.Exists(c => c.Value == GetShangValve(c.Suit)) &&    //有肩 有青  无赏
                    !vcards.Exists(c => EngineTool.isJinZhang(c)) &&
                    vcards.Exists(c => c.Value == GetShangValve(c.Suit) - 1) &&
                    !allDis.Exists(c=> c.Value == GetShangValve(c.Suit)&&   //  賞未現身
                                       c.Suit == vcards[0].Suit) )
                    return FirstSelSuit(vcards, allDis, bLord);

                if (bLord)
                {
                    if (vcards.Exists(c => c.Value == GetShangValve(c.Suit)) &&
                        allDis.Count(c=>c.Suit == vcards[0].Suit &&
                                        EngineTool.isJinZhang(c) ) == 2)
                        return FirstSelSuit(vcards, allDis, bLord);
                    if (vcards.Exists(c => c.Value == GetShangValve(c.Suit)-1) && //有肩
                        vcards.Exists(c => c.Value < GetShangValve(c.Suit) - 1)&& //有青或趣
                        allDis.Exists(c => c.Suit == vcards[0].Suit &&          //賞已現身
                                        c.Value == GetShangValve(c.Suit)) )
                        return FirstSelSuit(vcards, allDis, bLord);
                    if(vcards.Exists(c => c.Value < GetShangValve(c.Suit) - 1 &&   //有青張為真
                                          EngineTool.isTrueCard(allDis,c) ) )
                        return FirstSelSuit(vcards, allDis, bLord);
                }

                nqings.Add(EngineTool.GetQingCount(vcards, vcards[0].Suit));
            }

            int imax = nqings.Max();
            int maxIdx = nqings.FindIndex(idx => idx == imax);
            return FirstSelSuit(ltmp[maxIdx], allDis, bLord); 
        }

        //关真斗十
        private static Card GuanZhenDouShi( List<Card> allDis, List<Card> lc,bool bLord = false)
        {
            int isuits = lc.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;

            List<Card> cardSpades = lc.Where(c => c.Suit == PokerConst.Spade).ToList();
            int nSpades = cardSpades.Count;
            int nOtherSuit = lc.Count - nSpades;
           
            Card card = lc.Find(c => c.Suit != PokerConst.Spade);

            if (isuits == 2 && nOtherSuit == 1 &&
                nSpades == lc.Count - 1 &&
                EngineTool.isTrueCard(allDis, card))
            {
                return FirstSelSuit(cardSpades, allDis,bLord);
            }
            else
                return FirstSelSuit(lc.Except(cardSpades).ToList(),allDis, bLord);
        }
        //关赏斗十
        #region 关赏斗十
        private static Card GuanShangDouShi( List<Card> allDis, string suit1, string suit2, List<Card> lcards, bool blord)
        {

            int nsuits = lcards.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;

            List<Card> spadeCards = lcards.Where(c => c.Suit == PokerConst.Spade).ToList();

            if (nsuits == 2 && spadeCards.Count > 0)
            {

                List<Card> otherCards = lcards.Except(spadeCards).ToList();
                //  只有两门  十子和另一门
                if (otherCards.Exists(c => c.Value == GetShangValve(c.Suit)) &&      //有賞
                    !otherCards.Exists(c => c.Value == GetShangValve(c.Suit) - 1) && //無肩
                    otherCards.Count > 1 &&                                         //有青或趣
                    !allDis.Exists(c => c.Value == GetShangValve(otherCards[0].Suit) - 1 &&
                                        c.Suit == otherCards[0].Suit) && //賞下本肩未打出
                    spadeCards.Count(c => c.Value != PokerConst.JianValve) > 0)  //有千僧以外十子
                    return FirstSelSuit(spadeCards, allDis);
            }
            //    if (lcards.Count(c=>c.Suit==suit1) == 0||
            //       lcards.Count(c => c.Suit == suit2) == 0)


            if (nsuits == 3 && spadeCards.Count > 0 &&
                (!lcards.Exists(c => c.Suit == suit1) ||
                 !lcards.Exists(c => c.Suit == suit2)))
            {
                List<Card> threeCards = lcards.Where(c => c.Suit != PokerConst.Spade &&
                                                          c.Suit != suit1 &&
                                                          c.Suit != suit2).ToList();
                List<Card> zhengCards = lcards.Where(c => c.Suit == suit1 ||
                                                          c.Suit == suit2).ToList();
                if (zhengCards.Exists(c => c.Value == GetShangValve(c.Suit)) &&      //有賞
               !zhengCards.Exists(c => c.Value == GetShangValve(c.Suit) - 1) && //無肩
               zhengCards.Count > 1 &&                                         //有青或趣
               !allDis.Exists(c => c.Value == GetShangValve(zhengCards[0].Suit) - 1 &&
                                   c.Suit == zhengCards[0].Suit) ) //賞下本肩未打出                                                                             
                {

                    return SpadeAndThreeDoor(lcards.Where(c => c.Suit != suit1 &&
                                                         c.Suit != suit2).ToList(),
                                                         allDis, suit1, suit2,blord);
                }
                return null;

            }
            return null;
            #region 暫時不用
            /*      bSpadeandZheng1 = true;


              if (bSpadeandOther1 || bSpadeandZheng1)  
          {
              int top = lSuits[0].Count;//剩下第一種花色張數


              if (top >= 2
                  && lSuits[0][top - 1].Value >= iShang  //有赏有青 无肩
                  && lSuits[0][top - 2].Value < iShang - 1)
              {
                  return FirstSelSuit(lSuits[lSuits.Count - 1], allDis);

              }               
              else
                  return FirstSelSuit(lSuits[0], allDis);

          }
          else if (lSuits.Count >= 3
              && lSuits[0].Count >= 2
              && lSuits[1].Count >= 2
              && lSuits[2].Count > 0)  //  有三门
          {

              int nShangs = 0;
              foreach (var v in lSuits)
              {
                  if (v.Exists(c => c.Value == GetShangValve(c.Suit) && c.Suit != PokerConst.Spade))
                      nShangs++;
              }
              if (nShangs >= 2 || nShangs < 1)
                  return null;


              for (int i = 0; i < lSuits.Count; i++)
              {
                  if (lSuits[i].Count >= 2
                      && lSuits[i].Exists(c => c.Value == GetShangValve(c.Suit) &&c.Suit !=PokerConst.Spade)   //有赏有青 无肩
                      && EngineTool.GetQingCount(lSuits[i], lSuits[i][0].Suit) > 0
                      && !lSuits[i].Exists(c => c.Value == GetShangValve(c.Suit) - 1))
                  {

                      if (i == 0)
                          return   FirstSelSuit(lSuits[1], allDis);
                      else if (i == 1)
                          return FirstSelSuit(lSuits[0], allDis);
                      else
                      {
                          int nqing0 = EngineTool.GetQingCount(lSuits[0], lSuits[0][0].Suit),
                              nqing1 = EngineTool.GetQingCount(lSuits[1], lSuits[1][0].Suit);
                          int x = nqing0 > nqing1 ? 0 : 1;

                          return FirstSelSuit(lSuits[x], allDis);
                      }
                  }
                  else
                      return null;
              }
              return null;
          }
          else
              return null;*/
            #endregion
        }
        #endregion

        //留张制尾
        #region 留张制尾
        private static Card LiuZhangZhiWei(List<List<Card>> lNew, List<Card> allDis, string suit1, string suit2)
        {
            // List<List<Card>> lNew = Sort1230(lSuitsAll, suit1, suit2);

            if (lNew.Count <= 1)//少于2门牌
                return FirstSelSuit(lNew[0], allDis);

            int iShang = GetShangValve(lNew[0][0].Suit);
            int nqing1 = EngineTool.GetQingCount(lNew[0], lNew[0][0].Suit),
                nji1 = lNew[0].Count(c => c.Value == PokerConst.JiValve),
                nhong1 = lNew[0].Count - nqing1 - nji1;

            if (lNew.Count == 2) //只有2门
            {
                if (nqing1 > 1 || (nji1 > 0 && nqing1 >= 1))
                    return FirstSelSuit(lNew[0], allDis);
                else
                    return FirstSelSuit(lNew[1], allDis);
            }
            else//三门以上
            {
                int nqing2 = EngineTool.GetQingCount(lNew[1], lNew[1][0].Suit),//第二門青張數
                    nji2 = lNew[1].Count(c => c.Value == PokerConst.JiValve),//第二門是否有趣
                    nhong2 = lNew[1].Count - nqing1 - nji1;//第二門紅張數量

                if (nqing1 > 1 || (nji1 > 0 && nqing1 >= 1))//第一門有青有趣
                    return FirstSelSuit(lNew[0], allDis);
                else if (nqing2 > 1 || (nji2 > 0 && nqing2 >= 1))//第二門有青有趣
                    return FirstSelSuit(lNew[1], allDis);
                else if ((nji1 < 1 && nqing1 <= 1) && //第一門無趣 1青或無青
                      (nji2 < 1 && nqing2 <= 1)) //第二門也無趣 1青或無青
                    return FirstSelSuit(lNew[0], allDis);//第一門有青無趣
                else if (nji1 < 1 && nqing1 <= 1)
                    return FirstSelSuit(lNew[1], allDis);
                else if (nji2 < 1 && nqing2 <= 1)
                    return FirstSelSuit(lNew[0], allDis);
                else
                    return null;

            }
            #region 丢弃不用
            /* bool isJi1 = lNew[0].Exists(c => c.Value == PokerConst.JiValve) ? true : false;

             if (lNew.Count == 2) x = 1;
             if (lNew[1].Exists(c => c.Value == PokerConst.JiValve))//有趣
             {
                 if (nqing1 > 1)//有青
                 {
                     x = 1; 
                 }

                 else if (nqing1 <= 1 && lNew.Count >= 3)
                 {
                     x = 2;                   
                 }
             }

             if (lNew.Exists(p => p.Exists(q => q.Suit == suit1 || q.Suit == suit2)))   //存在正门
             {
                 //正门 无赏肩 无趣 
                 if (lNew[0].Exists(c => c.Suit == suit1 || c.Suit == suit2)
                     && !lNew[0].Exists(c => c.Value >= iShang - 1)
                     && !lNew[0].Exists(c => c.Value == PokerConst.JiValve)
                      )
                 {
                     if (lNew[0].Count > 1) //青张大于1
                         return FirstSelSuit(lNew[0]);
                     else
                         return FirstSelSuit(lNew[x]);
                 }
                 //正门  无赏肩 有趣
                 else if (lNew[0].Exists(c => c.Suit == suit1 || c.Suit == suit2)
                          && lNew[0].Exists(c => c.Value == PokerConst.JiValve)
                          && !lNew[0].Exists(c => c.Value >= iShang - 1))
                 {
                     if (lNew[0].Count > 1) //有青张 
                         return FirstSelSuit(lNew[0]);
                     else//无青
                         return FirstSelSuit(lNew[x]);
                 }
                 return null;
             }
             return null;
            */
            #endregion
        }

        #endregion

        //尽手提赏
        #region 尽手提賞
        private static Card JinShou1Suit(List<List<Card>> lNew, List<Card> allDis, string suit1, string suit2)
        {
            // List<List<Card>> lNew = Sort1230(lSuitsAll, suit1, suit2);
            if (lNew.Count > 1)
                return null;
            if (lNew[0].Exists(c => c.Value == GetShangValve(lNew[0][0].Suit) &&
                                    c.Suit != PokerConst.Spade))
                return lNew[0][lNew[0].Count - 1];
            else
                return FirstSelSuit(lNew[0], allDis);

        }
        #endregion

        //告百发十
        #region 告百发十
        private static Card GaoBaiFaShi(List<Card> lc, List<Card> allDis,List<string> Door4, bool bLord)
        {
            // List<List<Card>> lNew = Sort1230(lSuitsAll, suit1, suit2);
         
            int nsuits = allDis.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList().Count;
            List<Card> spadeCards = lc.Where(c => c.Suit == PokerConst.Spade).ToList(),
                       otherCards = lc.Except(spadeCards).ToList();
            List<string> doors = new List<string> (Door4);
            doors.Remove(PokerConst.Spade);
            if (spadeCards.Exists(c=>c.Value == PokerConst.ShangValve -2) &&
                otherCards.Exists(c=>c.Value == GetShangValve(c.Suit) ) &&
                ( (bLord && (nsuits <= 2 && doors.Count <2)||nsuits <=1 )||
                  (!bLord && nsuits <= 2 && doors.Count < 2)   )  )
                    return FirstSelSuit(spadeCards, allDis,bLord);
            return null;

        }
        #endregion


        //有百无赏
        #region 有百无赏
        private static Card YouBaiWuShnag(List<List<Card>> lNew, List<Card> allDis, string suit1, string suit2)
        {
            if (!lNew.Exists(lc => lc.Exists(c => c.Suit == PokerConst.Spade && c.Value == GetShangValve(c.Suit))))//无百
                return null;
            int totalCount = 0;
            foreach (List<Card> lc in lNew)
            {
                totalCount += lc.Count;
            }


            //List<List<Card>> lNew = Sort1230(lSuitsAll, suit1, suit2);
            if (!lNew.Exists(lc => lc.Exists(c => c.Suit != PokerConst.Spade && c.Value == GetShangValve(c.Suit))))    //有百无赏        
            {
                return FirstSelSuit(lNew[0], allDis);
            }
            else if (totalCount <= 5)
            {
                return FirstSelSuit(lNew[lNew.Count - 1], allDis);
            }
            else
                return null;

        }
        #endregion



    }
}
